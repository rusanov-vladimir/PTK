module Server

open Saturn
open Config
open System.Text.RegularExpressions


let endpointPipe = pipeline {
    plug head
    plug requestId
}

let url_with_port ()=
    let environmentPort = System.Environment.GetEnvironmentVariable("PORT")
    let port =
        if not <| System.String.IsNullOrWhiteSpace environmentPort then
            System.Int32.Parse(environmentPort)
        else 
            8085
    let res = (sprintf "http://0.0.0.0:%i/" (port))    
    res

let connectionString ()= 
    let environmentConnectionString = System.Environment.GetEnvironmentVariable("DATABASE_URL")
    let connectionString =
        if not <| System.String.IsNullOrEmpty environmentConnectionString then
            let m = Regex.Match(environmentConnectionString, "postgres://(.*):(.*)@(.*):(.*)/(.*)")
            let cstring =
              "Server="   + m.Groups.[3].Value + ";" +
              "Port="     + m.Groups.[4].Value + ";" + 
              "Database=" + m.Groups.[5].Value + ";" +
              "User Id="  + m.Groups.[1].Value + ";" +
              "Password=" + m.Groups.[2].Value + ";" +
              "Pooling=true;SSL Mode=Require;Trust Server Certificate=true;"
            cstring
        else
            "User ID=postgres;Password=ptk;Host=127.0.0.1;Port=5432;Database=ptk;Pooling=true;"
    connectionString

let app = application {
    pipe_through endpointPipe
    
    error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
    use_router Router.browserRouter
    url (url_with_port())
    memory_cache 
    disable_diagnostics
    use_static "static"
    use_github_oauth (System.Environment.GetEnvironmentVariable("id")) (System.Environment.GetEnvironmentVariable("pass")) "/oauth_callback_github" ["login", "githubUsername"; "name", "fullName"]
    use_gzip
    use_config (fun _ -> {connectionString = connectionString()} ) //TODO: Set development time configuration
}

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code