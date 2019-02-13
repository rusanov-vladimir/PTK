namespace Home

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Giraffe.Core
open Saturn
open Common

module Controller =

  let homeIndex (ctx : HttpContext) =
    task {
      return! Common.MyRender ctx Index.layout
    }


  let home = controller {
    index homeIndex
  }

