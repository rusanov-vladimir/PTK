#if !FAKE
  #r "netstandard" // windows
  #r "Facades/netstandard" // mono
  #r "./packages/NETStandard.Library/build/netstandard2.0/ref/netstandard.dll"
#endif
#r "paket: 
  nuget FSharp.Core 
  nuget Fake.DotNet.Cli
  nuget Fake.DotNet.Paket
  nuget Fake.IO.FileSystem
  nuget Fake.Core.Target //"
#load "./.fake/build.fsx/intellisense.fsx"
open System

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.Core.TargetOperators
open System.Threading

let appPath = "./src" |> Path.getFullName
let ptkProjectPath = Path.combine appPath "PTK/PTK.fsproj"
let migrationProjectPath = Path.combine appPath "Migrations/Migrations.fsproj"

let dotnetcliVersion = DotNet.getSDKVersionFromGlobalJson()

let directoriesToClean () =
    DirectoryInfo.getSubDirectories (DirectoryInfo.ofPath appPath)
    |> Array.collect (fun x -> [|
      Path.combine x.FullName "bin";
      Path.combine x.FullName "obj";
      Path.combine x.FullName "out";
      |])

Target.create "Clean" (fun _ ->
    Trace.log "--- Cleaning stuff ---"
    Shell.cleanDirs (directoriesToClean())
)

Target.create "InstallDotNetCore" (fun _ ->
    let setParams (options : DotNet.CliInstallOptions) =
        { options with
            Version = DotNet.Version dotnetcliVersion }
    DotNet.install setParams |> ignore
    ()
)

Target.create "Restore" (fun _ ->
    let setParams (options : Paket.PaketRestoreParams) =
        { options with
            WorkingDir = appPath }
    Paket.restore setParams |> ignore
)

Target.create "Build" (fun _ ->
     DotNet.build id ptkProjectPath
     DotNet.build id migrationProjectPath
)

Target.create "MigrateDb" (fun _ ->
    DotNet.exec id "./src/Migrations/bin/Release/netcoreapp2.0/Migrations.dll" "" |> ignore
)

Target.create "Publish" (fun _ ->
    let setParams (options : DotNet.PublishOptions) =
        { options with
            BuildBasePath = Some appPath }
    DotNet.publish setParams |> ignore
    let prc = new Diagnostics.Process()
    prc.StartInfo.FileName <- "docker"
    prc.StartInfo.Arguments <- "restart ptk"
    prc.StartInfo.CreateNoWindow <- true
    prc.StartInfo.UseShellExecute  <- false
    prc.StartInfo.RedirectStandardOutput <- true
    prc.StartInfo.RedirectStandardError <- true
    prc.Start() |> ignore
)

Target.create "Run" (fun _ ->
  let server = async {
    DotNet.exec (fun p -> { p with WorkingDirectory = Path.combine appPath "PTK" } ) "watch" "run" |> ignore
  }
  let browser = async {
    Thread.Sleep 5000
    Process.start (fun i -> { i with FileName = "http://localhost:8085" }) |> ignore
  }

  [ server; browser]
  |> Async.Parallel
  |> Async.RunSynchronously
  |> ignore
)

"Clean"
  ==> "InstallDotNetCore"
  ==> "Restore"
  ==> "Build"

"Build"
  ==> "MigrateDb"
  ==> "Run"

Target.runOrDefaultWithArguments "Build"