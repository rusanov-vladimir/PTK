module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open Giraffe
open System.Security.Claims


let googleUserIdForRmunn = "test"

let matchUpUsers : HttpHandler = fun next ctx ->
    // A real implementation would match up user identities with something stored in a database, not hardcoded in Users.fs like this example
    let isRmunn =
        ctx.User.Claims |> Seq.exists (fun claim ->
            claim.Issuer = "GitHub" && claim.Type = ClaimTypes.NameIdentifier && claim.Value = googleUserIdForRmunn)
    if isRmunn then
        printfn "User rmunn is an admin of this demo app, adding admin role to user claims"
        ctx.User.AddIdentity(new ClaimsIdentity([Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String, "MyApplication")]))
    next ctx

let loggedIn = pipeline {
    requires_authentication (Giraffe.Auth.challenge "GitHub")
    plug matchUpUsers
}

let isAdmin = pipeline {
    plug loggedIn
    requires_role "Admin" (RequestErrors.FORBIDDEN "Must be admin")
}

let browser = pipeline {
    plug acceptHtml
    plug putSecureBrowserHeaders
    plug fetchSession
    set_header "x-pipeline-type" "Browser"
}

let defaultView = router {
    get "/" (htmlView Index.layout)
    get "/index.html" (redirectTo false "/")
    get "/default.html" (redirectTo false "/")
}

let browserRouter = router {
    not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
    pipe_through browser //Use the default browser pipeline

    forward "" defaultView //Use the default view
    forward "/mems" Mems.Controller.resource
    forward "/cats" (isAdmin >=> Categories.Controller.resource)
}




//Other scopes may use different pipelines and error handlers

// let api = pipeline {
//     plug acceptJson
//     set_header "x-pipeline-type" "Api"
// }

// let apiRouter = scope {
//     error_handler (text "Api 404")
//     pipe_through api
//
//     forward "/someApi" someScopeOrController
// }

let router = scope {
    // forward "/api" apiRouter
    forward "" browserRouter
}