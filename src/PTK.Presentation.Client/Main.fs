module PTK.Presentation.Client.Main

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Json
open Bolero.Templating.Client

/// Routing endpoints definition.
type Page =
    | [<EndPoint "/">] Home
    // | [<EndPoint "/counter">] Counter
    // | [<EndPoint "/data">] Data

/// The Elmish application's model.
type Model =
    {
        page: Page
        counter: int
        mems: Mem[] option
        error: string option
        username: string
        password: string
        signedInAs: option<string>
        signInFailed: bool
    }

and Book =
    {
        title: string
        author: string
        [<DateTimeFormat "yyyy-MM-dd">]
        publishDate: DateTime
        isbn: string
    }

and Category = 
    {
        id: int
        title: string
        description: string
    }

and Mem = 
    {
      id: int
      title: string
      content: string
      author: string
      tstamp : DateTime
      modifieddate: DateTime option
      category: Category 
    }

let initModel =
    {
        page = Home
        counter = 0
        mems = None
        error = None
        username = ""
        password = ""
        signedInAs = None
        signInFailed = false
    }


/// Remote service definition.
type MemService =
    {
        /// Get the list of all books in the collection.
        getMems: unit -> Async<Mem[]>

        /// Add a book in the collection.
        // addBook: Book -> Async<unit>

        // /// Remove a book from the collection, identified by its ISBN.
        // removeBookByIsbn: string -> Async<unit>

        // /// Sign into the application.
        // signIn : string * string -> Async<option<string>>

        // /// Get the user's name, or None if they are not authenticated.
        // getUsername : unit -> Async<string>

        // /// Sign out from the application.
        // signOut : unit -> Async<unit>
    }

/// The Elmish application's update messages.
type Message =
    | SetPage of Page
    | Increment
    | Decrement
    | SetCounter of int
    | GetMems
    | GotMems of Mem[]
    | SetUsername of string
    | SetPassword of string
    | GetSignedInAs
    // | RecvSignedInAs of RemoteResponse<string>
    | SendSignIn
    | RecvSignIn of option<string>
    | SendSignOut
    | RecvSignOut
    | Error of exn
    | ClearError

let update remote message model =
    let onSignIn = function
        | Some _ -> Cmd.ofMsg GetMems
        | None -> Cmd.none
    match message with
    | SetPage page ->
        { model with page = page }, Cmd.none

    | Increment ->
        { model with counter = model.counter + 1 }, Cmd.none
    | Decrement ->
        { model with counter = model.counter - 1 }, Cmd.none
    | SetCounter value ->
        { model with counter = value }, Cmd.none

    | GetMems ->
        let cmd = Cmd.ofAsync remote.getMems () GotMems Error
        { model with mems = None }, cmd
    | GotMems mems ->
        { model with mems = Some mems }, Cmd.none

    | SetUsername s ->
        { model with username = s }, Cmd.none
    | SetPassword s ->
        { model with password = s }, Cmd.none
    // | GetSignedInAs ->
    //     model, Cmd.ofRemote remote.getUsername () RecvSignedInAs Error
    // | RecvSignedInAs resp ->
    //     let username = resp.TryGetResponse()
    //     { model with signedInAs = username }, onSignIn username
    // | SendSignIn ->
    //     model, Cmd.ofAsync remote.signIn (model.username, model.password) RecvSignIn Error
    | RecvSignIn username ->
        { model with signedInAs = username; signInFailed = Option.isNone username }, onSignIn username
    // | SendSignOut ->
    //     model, Cmd.ofAsync remote.signOut () (fun () -> RecvSignOut) Error
    | RecvSignOut ->
        { model with signedInAs = None; signInFailed = false }, Cmd.none

    | Error RemoteUnauthorizedException ->
        { model with error = Some "You have been logged out."; signedInAs = None }, Cmd.none
    | Error exn ->
        { model with error = Some exn.Message }, Cmd.none
    | ClearError ->
        { model with error = None }, Cmd.none

/// Connects the routing system to the Elmish application.
let router = Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/newMain.html">


let lastChanged mem =
    if mem.modifieddate.IsNone  then Some mem.tstamp else mem.modifieddate
let toDisplayDate (date:DateTime option) =
    match date with
    | Some x -> x.ToShortDateString()
    | None -> String.Empty
let lastChangedToDisplaydate = lastChanged >> toDisplayDate  

let readIndex (objs : Mem list) =
    let cnt = [
      div [attr.``class`` "container "] [
        for o in objs do
          yield article [attr.``class`` "entry"] [
            header [attr.``class`` "entry-header"] [
              h2 [attr.``class`` "entry-title"][
                //a [attr.href (Links.withId ctx (string o.id) ); attr.title o.title][
                a [attr.href "#"; attr.title o.title][
                  text o.title
                ]
              ]
              div [attr.``class`` "entry-meta"][
                ul [][
                  li [] [text (lastChangedToDisplaydate o)]
                  span [attr.``class`` "meta-sep"][text "&bull;"]
                  li [] [
                    a [attr.href "#"; attr.title o.category.title; attr.rel "category tag"] [text o.category.title]
                  ]
                  span [attr.``class`` "meta-sep"][text "&bull;"]
                  li [] [text o.author]
                ]
              ]
            ]
            div [attr.``class`` "entry-content"][
                p [][o.content |> text]
              //p [][o.content |> string |>  Markdown.Parse |> Markdown.WriteHtml |> text]
            ]
          ]
        ]
      ]
    App.layout ([section [attr.``class`` "section"] cnt]) [script [attr.src "/memoriesRead.js"] []]

let homePage model dispatch =
    Main.Home()
        .Data(cond model.mems <| function
            | None ->
                div [][text "nothing"]
            | Some mems ->
                forEach mems <| fun book ->
                    tr [] [
                        td [] [text book.title]
                        td [] [text book.author]
                    ]).Elt()

// let counterPage model dispatch =
//     Main.Counter()
//         .Decrement(fun _ -> dispatch Decrement)
//         .Increment(fun _ -> dispatch Increment)
//         .Value(model.counter, fun v -> dispatch (SetCounter v))
//         .Elt()

// let dataPage model (username: string) dispatch =
//     Main.Data()
//         .Reload(fun _ -> dispatch GetBooks)
//         .Username(username)
//         .SignOut(fun _ -> dispatch SendSignOut)
//         .Rows(cond model.books <| function
//             | None ->
//                 Main.EmptyData().Elt()
//             | Some books ->
//                 forEach books <| fun book ->
//                     tr [] [
//                         td [] [text book.title]
//                         td [] [text book.author]
//                         td [] [text (book.publishDate.ToString("yyyy-MM-dd"))]
//                         td [] [text book.isbn]
//                     ])
//         .Elt()

// let signIinPage model dispatch =
//     Main.SignIn()
//         .Username(model.username, fun s -> dispatch (SetUsername s))
//         .Password(model.password, fun s -> dispatch (SetPassword s))
//         .SignIn(fun _ -> dispatch SendSignIn)
//         .ErrorNotification(
//             cond model.signInFailed <| function
//             | false -> empty
//             | true ->
//                 Main.ErrorNotification()
//                     .HideClass("is-hidden")
//                     .Text("Sign in failed. Use any username and the password \"password\".")
//                     .Elt()
//         )
//         .Elt()

let menuItem (model: Model) (page: Page) (text: string) =
    Main.MenuItem()
        .Active(if model.page = page then "is-active" else "")
        .Url(router.Link page)
        .Text(text)
        .Elt()

let view model dispatch =
    Main()
        .Menu(concat [
            menuItem model Home "Home"
            // menuItem model Counter "Counter"
            // menuItem model Data "Download data"
        ])
        .Body(
            cond model.page <| function
            | Home -> homePage model dispatch
            // | Counter -> counterPage model dispatch
            // | Data ->
            //     cond model.signedInAs <| function
            //     | Some username -> dataPage model username dispatch
            //     | None -> signIinPage model dispatch
        )
        .Error(
            cond model.error <| function
            | None -> empty
            | Some err ->
                Main.ErrorNotification()
                    .Text(err)
                    .Hide(fun _ -> dispatch ClearError)
                    .Elt()
        )
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        let memService = {
                            getMems = 
                                fun x->  
                                async{
                                    let mem = {id = 1; title="test"; content = "content"; author= "a"; modifieddate=Option.None; tstamp = DateTime.Now; category = { id= 2; title= "cat"; description= "string"} }
                                    let list = [|mem|]
                                    return list
                                }
                         }
        let update = update memService
        let mems = memService.getMems () |> Async.RunSynchronously
        Program.mkProgram (fun _ -> {initModel with mems = Some mems}, Cmd.ofMsg GetSignedInAs) update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReloading
#endif
