module Server

open Saturn
open Config

let endpointPipe = pipeline {
    plug head
    plug requestId
}

let app = application {
    pipe_through endpointPipe

    error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
    router Router.router
    url "http://0.0.0.0:8085/"
    memory_cache 
    disable_diagnostics
    use_static "static"
    use_gzip
    use_config (fun _ -> {connectionString = "User ID=postgres;Password=ptk;Host=127.0.0.1;Port=5432;Database=ptk;Pooling=true;"} ) //TODO: Set development time configuration
}

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code