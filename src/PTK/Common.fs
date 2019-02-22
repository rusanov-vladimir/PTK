namespace Common

open Microsoft.AspNetCore.Http
open Saturn
open Layout

module Common =

  let IsAdmin (ctx : HttpContext) =
    ctx.User.IsInRole("Admin")

  let MyRender (ctx : HttpContext) template =
    let categoriesWithCount = (Layout.Controller.getCategoriesWithCount ctx).Result
    Controller.renderHtml ctx ((IsAdmin >> template) ctx categoriesWithCount)