module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open Giraffe
open System.Security.Claims


let gitHubUsername = "rusanov-vladimir"

let matchUpUsers : HttpHandler = fun next ctx ->
    // A real implementation would match up user identities with something stored in a database, not hardcoded in Users.fs like this example
    let isSiteOwner =
        ctx.User.Claims |> Seq.exists (fun claim ->
            claim.Issuer = "GitHub" && claim.Type = "githubUsername" && claim.Value = gitHubUsername)
    if isSiteOwner then
        ctx.User.AddIdentity(new ClaimsIdentity([Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String, "PTK")]))
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
    forward "/memories" Mems.Controller.read
    getf "/memories/%i" Mems.Controller.readPageAction
    forward "/mems" (isAdmin >=> Mems.Controller.crud)
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