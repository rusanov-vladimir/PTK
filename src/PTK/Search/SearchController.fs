namespace Search

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Giraffe.Core
open Saturn
open Common
open Index

module Controller =

  let homeIndex (ctx : HttpContext) =
    task {      
      let page = 
        match ctx.TryGetQueryStringValue "page" with
        | None   -> 1
        | Some p -> int p
      let searchString = 
        match ctx.TryGetQueryStringValue "searchString" with
        | None   -> ""
        | Some p -> p
      
      let cnf = Controller.getConfig ctx
      let! result = Database.getMemsBySearchString cnf.connectionString searchString page
      match result with
      | Ok result ->
          return! Common.MyRender ctx (Views.index ctx (List.ofSeq result))
      | Error ex ->
          return raise ex
    }

  let search = controller {
    index homeIndex
  }