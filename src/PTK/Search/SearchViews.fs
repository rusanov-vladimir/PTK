namespace Search

open Microsoft.AspNetCore.Http
open Giraffe.GiraffeViewEngine
open Saturn
open Mems

module Views =
  let index (ctx : HttpContext) (objs : Mem list) isAdmin categories =
    let cnt = [
      div [_class "container "] [
        h2 [ _class "title"] [rawText "Found memories:"]
        hr []
        ul [] [
          yield! 
            match objs.Length with
            | 0 -> [ rawText "Nothing was found, try some other search..."]
            | _ -> [
              for o in objs do
                yield li [] [
                  div [] [
                      h3 [] [
                          a [_class "entry-title"; _href ("/mems/"+string o.id)] [rawText o.title]
                          ]
                      h6 [] [rawText (string o.content)]
                      hr []
                  ]
                ]
              ]
        ]
      ]
    ]
    App.layout ([section [_class "section"] cnt]) [script [_src "/Search.js"] []] isAdmin categories
