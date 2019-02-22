namespace Layout

open Microsoft.AspNetCore.Http
open Giraffe.GiraffeViewEngine
open Saturn
open Common.Models

module View =

  let categoriesPartial (ctx : HttpContext) (objs : CategoryWithCount list) = 
    div [] [
      h3 [][encodedText "Categories."]    
      ul [] [
        for o in objs do
          yield li [][
              a [_href "#"][encodedText o.title] 
              encodedText (sprintf "(%i)" o.postCount)
          ]
      ]
    ]