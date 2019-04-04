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

let appPath = "./src/PTK/" |> Path.getFullName

let dotnetcliVersion = DotNet.getSDKVersionFromGlobalJson()

Target.create "Clean" (fun _ -> 
  ignore()
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
    let setParams (options : DotNet.BuildOptions) =
        { options with
            BuildBasePath = Some appPath }
    DotNet.build setParams |> ignore
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
    let setParams (options : DotNet.Options) =
        { options with
            WorkingDirectory = appPath }
    DotNet.exec setParams "watch" "run" |> ignore
  }
  let browser = async {
    Threading.Thread.Sleep 5000
    Diagnostics.Process.Start "http://localhost:8085" |> ignore
  }

  [ server; browser]
  |> Async.Parallel
  |> Async.RunSynchronously
  |> ignore
)

"Clean"
  ==> "InstallDotNetCore"
  ==> "Build"

"Clean"
  ==> "Restore"
  ==> "Run"

Target.runOrDefaultWithArguments "Build"