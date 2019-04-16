namespace Mems

open Microsoft.AspNetCore.Http
open Giraffe.GiraffeViewEngine
open Saturn
open FSharp.Markdown
open Categories
open System

type ComboViewModel = { Id: int; Name: string }
type EditorType = 
  | SingleLine
  | MultiLine

module Views =
  let readIndex (ctx : HttpContext) (objs : Mem list) =
    let cnt = [
      div [_class "container "] [
        for o in objs do
          yield article [_class "entry"] [
            header [_class "entry-header"] [
              h2 [_class "entry-title"][
                a [_href (Links.withId ctx (string o.id) ); _title o.title][
                  rawText o.title
                ]
              ]
              div [_class "entry-meta"][
                ul [][
                  li [] [rawText (string (if o.modifieddate.IsNone  then o.tstamp else o.modifieddate.Value))]
                  span [_class "meta-sep"][rawText "&bull;"]
                  li [] [
                    a [_href "#"; _title o.category.title; _rel "category tag"] [rawText o.category.title]
                  ]
                  span [_class "meta-sep"][rawText "&bull;"]
                  li [] [rawText o.author]
                ]
              ]
            ]
            div [_class "entry-content"][
              p [][o.content |> string |>  Markdown.Parse |> Markdown.WriteHtml |> rawText]
            ]
          ]
        ]
      ]
    App.layout ([section [_class "section"] cnt]) [script [_src "/memoriesRead.js"] []]

  let index (ctx : HttpContext) (objs : Mem list) =
    let cnt = [
      div [] [
        h2 [ _class "title"] [rawText "Listing Mems"]

        table [_class "table is-hoverable is-fullwidth container"] [
          thead [] [
            tr [] [
              th [] [rawText "Title"]
              th [] [rawText "Content"]
              th [] [rawText "Author"]
              th [] [rawText "Category"]
              th [] []
            ]
          ]
          tbody [] [
            for o in objs do
              yield tr [_class "entry"] [
                td [] [rawText (string o.title)]
                td [] [o.content |> string |>  Markdown.Parse |> Markdown.WriteHtml |> rawText]
                td [] [rawText (string o.author)]
                td [] [rawText (string o.category.title)]
                td [] [
                  a [_class "button is-text"; _href (Links.withId ctx (string o.id) )] [rawText "Show"]
                  a [_class "button is-text"; _href (Links.edit ctx (string o.id) )] [rawText "Edit"]
                  a [_class "button is-text is-delete"; attr "data-href" (Links.withId ctx (string o.id) ); _onclick "deleteMem(this)" ] [rawText "Delete"]
                ]
              ]
          ]
        ]

        a [_class "button is-text"; _href (Links.add ctx )] [rawText "New Mem"]
      ]
    ]
    App.layout ([section [_class "section"] cnt]) [script [_src "/mems-scroll.js"] []]


  let show (ctx : HttpContext) (o : Mem) =
    let cnt = [
      div [_class "container "] [
        h2 [ _class "title"] [rawText o.title]
        ul [] [
          li [] [ o.content |> string |>  Markdown.Parse |> Markdown.WriteHtml |> rawText ]
          li [] [ strong [] [rawText "Author: "]; rawText (string o.author) ]
          li [] [ strong [] [rawText "Category: "]; rawText (string o.category.title) ]
        ]
        a [_class "button is-text"; _href (Links.edit ctx (string o.id))] [rawText "Edit"]
        a [_class "button is-text"; _href (Links.index ctx )] [rawText "Back"]
      ]
    ]
    App.layout ([section [_class "section"] cnt]) []

  let readShow (ctx : HttpContext) (o : Mem) =
    let cnt = [
      div [_class "container "] [
        h2 [ _class "title"] [rawText o.title]
        ul [] [
          li [] [ o.content |> string |>  Markdown.Parse |> Markdown.WriteHtml |> rawText ]
          li [] [ strong [] [rawText "Author: "]; rawText (string o.author) ]
          li [] [ strong [] [rawText "Category: "]; rawText (string o.category.title) ]
        ]
        a [_class "button is-text"; _href (Links.index ctx )] [rawText "Back"]
      ]
    ]
    App.layout ([section [_class "section"] cnt]) []

  let private form (ctx: HttpContext) (o: Mem option) (cats: Category seq)(validationResult : Map<string, string>) isUpdate =
    let validationMessage =
      div [_class "notification is-danger"] [
        a [_class "delete"; attr "aria-label" "delete"] []
        rawText "Oops, something went wrong! Please check the errors below."
      ]

    let generatedInput selector key inputType= 
      match inputType with
      | SingleLine ->
        input [_class (if validationResult.ContainsKey key then "input is-danger" else "input"); _value (defaultArg (o |> Option.map selector) ""); _name key ; _type "text" ]
      | MultiLine ->           
        textarea [_class (if validationResult.ContainsKey key then "input is-danger" else "input"); _name key ; ] [
          rawText (defaultArg (o |> Option.map selector) "")
        ]     
        
    let field selector lbl key inputType=
      div [_class "field"] [
        yield label [_class "label"] [rawText (string lbl)]
        yield div [_class "control has-icons-right"] [
          yield generatedInput selector key inputType
          if validationResult.ContainsKey key then
            yield span [_class "icon is-small is-right"] [
              i [_class "fas fa-exclamation-triangle"] []
            ]
        ]
        if validationResult.ContainsKey key then
          yield p [_class "help is-danger"] [rawText validationResult.[key]]
      ]
      
    let dropdown selector lbl key options = 
       div [_class "field"] [
        yield label [_class "label"] [rawText (string lbl)]
        yield div [_class "control has-icons-right"] [
          yield select [_class (if validationResult.ContainsKey key then "input is-danger" else "input"); _value (defaultArg (o |> Option.map selector) ""); _name key ; _type "text" ] 
                    (options |> Seq.map (fun x-> option [_value (string x.Id)][rawText x.Name]) |> List.ofSeq)
          if validationResult.ContainsKey key then
            yield span [_class "icon is-small is-right"] [
              i [_class "fas fa-exclamation-triangle"] []
            ]
        ]
        if validationResult.ContainsKey key then
          yield p [_class "help is-danger"] [rawText validationResult.[key]]
      ]

    let buttons =
      div [_class "field is-grouped"] [
        div [_class "control"] [
          input [_type "submit"; _class "button is-link"; _value "Submit"]
        ]
        div [_class "control"] [
          a [_class "button is-text"; _href (Links.index ctx)] [rawText "Cancel"]
        ]
      ]

    let cnt = [
      div [_class "container "] [
        form [ _action (if isUpdate then Links.withId ctx (string o.Value.id) else Links.index ctx ); _method "post"] [
          if not validationResult.IsEmpty then
            yield validationMessage
          yield input [_value (defaultArg (o |> (Option.map (fun i -> string i.id))) ""); _name "Id" ; _type "hidden" ]
          yield field (fun i -> (string i.title)) "Title" "title" SingleLine
          yield field (fun i -> (string i.content)) "Content" "content"  MultiLine
          yield field (fun i -> (string i.author)) "Author" "author" SingleLine
          yield dropdown (fun i -> (string i.category.id)) "CategoryId" "categoryId" (cats |> Seq.map (fun x-> {Id = x.id; Name= x.title}))
          yield buttons
        ]
      ]
    ]
    App.layout ([section [_class "section"] cnt]) []

  let add (ctx: HttpContext) (o: Mem option) (cats: Category seq) (validationResult : Map<string, string>)=
    form ctx o cats validationResult false

  let edit (ctx: HttpContext) (o: Mem) (cats: Category seq) (validationResult : Map<string, string>) =
    form ctx (Some o) cats validationResult true