namespace Common

open Microsoft.AspNetCore.Http
open Saturn

module Common =

  let IsAdmin (ctx : HttpContext) =
    ctx.User.IsInRole("Admin")

  let MyRender (ctx : HttpContext) template =
    Controller.renderHtml ctx (ctx |> (IsAdmin >> template))