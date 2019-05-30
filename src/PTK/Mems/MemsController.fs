namespace Mems

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Giraffe.Core
open Saturn
open Common
open Saturn.ControllerHelpers.Controller

module Controller =

  let readIndexActionApi (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let page =
        match ctx.TryGetQueryStringValue "page" with
        | None   -> 1
        | Some p -> int p

      let! result = Database.getAll cnf.connectionString page
      match result with
      | Ok result ->
          return! json ctx (List.ofSeq result)
      | Error ex ->
          return raise ex
    }

  let readShowActionApi (ctx: HttpContext) (id: int) =
    task {

      let cnf = Controller.getConfig ctx
      let! mems =Database.getById cnf.connectionString id
      match mems with
      | Ok (Some result) ->
          return! json ctx result
      | Ok None ->
          return! json ctx []
      | Error ex ->
          return raise ex
    }

  let readIndexAction (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let page =
        match ctx.TryGetQueryStringValue "page" with
        | None   -> 1
        | Some p -> int p

      let! result = Database.getAll cnf.connectionString page
      match result with
      | Ok result ->
          return! Common.MyRender ctx (Views.readIndex ctx (List.ofSeq result))
      | Error ex ->
          return raise ex
    }

  let indexAction (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let page = 
        match ctx.TryGetQueryStringValue "page" with
        | None   -> 1
        | Some p -> int p
      let! result = Database.getAll cnf.connectionString page
      match result with
      | Ok result ->
          return! Common.MyRender ctx (Views.index ctx (List.ofSeq result))
      | Error ex ->
          return raise ex
    }

  let showAction (ctx: HttpContext) (id: int) =
    task {

      let cnf = Controller.getConfig ctx
      let! mems =Database.getById cnf.connectionString id
      match mems with
      | Ok (Some result) ->
          return! Common.MyRender ctx (Mems.Views.show ctx result)
      | Ok None ->
          return! Controller.renderHtml ctx NotFound.layout
      | Error ex ->
          return raise ex
    }

  let readShowAction (ctx: HttpContext) (id: int) =
    task {

      let cnf = Controller.getConfig ctx
      let! mems =Database.getById cnf.connectionString id
      match mems with
      | Ok (Some result) ->
          return! Common.MyRender ctx (Mems.Views.readShow ctx result)
      | Ok None ->
          return! Controller.renderHtml ctx NotFound.layout
      | Error ex ->
          return raise ex
    }

  let addAction (ctx: HttpContext) =
    task{
      let cnf = Controller.getConfig ctx        
      let! catsRes = Categories.Database.getAll cnf.connectionString
      let cats = 
        match catsRes with
        | Ok x -> x
        | Error ex -> raise ex
      return! Common.MyRender ctx (Mems.Views.add ctx None cats Map.empty )
    }

  let editAction (ctx: HttpContext) (id: int) =
    task {
      let cnf = Controller.getConfig ctx
      let! result =Database.getById cnf.connectionString id
      let! catsRes = Categories.Database.getAll cnf.connectionString
      let cats = 
        match catsRes with
        | Ok x -> x
        | Error ex -> raise ex
      match result with
      | Ok (Some result) ->
          return! Common.MyRender ctx (Mems.Views.edit ctx result cats Map.empty)
      | Ok None ->
          return! Controller.renderHtml ctx NotFound.layout
      | Error ex ->
          return raise ex
    }

  let mapToDomain (input:MemViewModel) catRes =
    let maybeCat = 
        match catRes with
        | Ok x ->x
        | Error ex -> raise ex
    let cat = 
        match maybeCat with
        | Some cat ->  cat
        | None -> raise (System.ArgumentException("Category not found!"))

    {id = input.id; title = input.title; category = cat; author = input.author; content = input.content; tstamp = input.tstamp ; modifieddate = input.modifieddate}

  let createAction (ctx: HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let! input = Controller.getModel<MemViewModel> ctx
      let! catRes = Categories.Database.getById  cnf.connectionString input.categoryId
      let mem = mapToDomain input catRes
      let validateResult =Validation.validate mem
      if validateResult.IsEmpty then
        let! result = Database.insert cnf.connectionString input
        match result with
        | Ok _ ->
            return! Controller.redirect ctx (Links.index ctx)
        | Error ex ->
            return raise ex
      else
        let! catsRes = Categories.Database.getAll cnf.connectionString
        let cats = 
          match catsRes with
          | Ok x -> x
          | Error ex -> raise ex
        return! Common.MyRender ctx (Mems.Views.add ctx (Some mem) cats validateResult)
    }

  let updateAction (ctx: HttpContext) (id: int) =
    task {
      let cnf = Controller.getConfig ctx
      let! input = Controller.getModel<MemViewModel> ctx
      let! catRes = Categories.Database.getById  cnf.connectionString input.categoryId
      let mem = mapToDomain input catRes
      let validateResult =Validation.validate mem
      if validateResult.IsEmpty then
        let! result = Database.update cnf.connectionString input
        match result with
        | Ok _ ->
            return! Controller.redirect ctx (Links.index ctx)
        | Error ex ->
            return raise ex
      else

        let! catsRes = Categories.Database.getAll cnf.connectionString
        let cats = 
          match catsRes with
          | Ok x -> x
          | Error ex -> raise ex
        return! Common.MyRender ctx (Mems.Views.edit ctx mem cats validateResult)
    }

  let deleteAction (ctx: HttpContext) (id: int) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.delete cnf.connectionString id
      match result with
      | Ok _ ->
          return! Controller.redirect ctx (Links.index ctx)
      | Error ex ->
          return raise ex
    }

  let crud = controller {
    index indexAction
    show showAction
    add addAction
    edit editAction
    create createAction
    update updateAction
    delete deleteAction
  }

  let read = controller {
    index readIndexAction
    show readShowAction
  }

  let api = controller {
    index readIndexActionApi
    show readShowActionApi
  }

