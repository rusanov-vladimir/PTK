namespace Mems

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn
open Categories

module Controller =

  let indexAction (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getAll cnf.connectionString
      match result with
      | Ok result ->
        return! Controller.renderXml ctx (Views.index ctx (List.ofSeq result))
      | Error ex ->
        return raise ex
    }

  let showAction (ctx: HttpContext, id : string) =
    task {

      let cnf = Controller.getConfig ctx
      let! mems = Database.getById cnf.connectionString id
      match mems with
      | Ok (Some result) ->
        return! Controller.renderXml ctx (Views.show ctx result)
      | Ok None ->
        return! Controller.renderXml ctx NotFound.layout
      | Error ex ->
        return raise ex
    }

  let addAction (ctx: HttpContext) =
    task{
      let cnf = Controller.getConfig ctx        
      let! catsRes = Database.getAllCategories cnf.connectionString
      let cats = 
        match catsRes with
        | Ok x -> x
        | Error ex -> raise ex
      return! Controller.renderXml ctx (Views.add ctx None cats Map.empty )
    }

  let editAction (ctx: HttpContext, id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getById cnf.connectionString id
      let! catsRes = Database.getAllCategories cnf.connectionString
      let cats = 
        match catsRes with
        | Ok x -> x
        | Error ex -> raise ex
      match result with
      | Ok (Some result) ->
        return! Controller.renderXml ctx (Views.edit ctx result cats Map.empty)
      | Ok None ->
        return! Controller.renderXml ctx NotFound.layout
      | Error ex ->
        return raise ex
    }

  let mapToDomain (input:MemViewModel, catRes) =
    let maybeCat = 
        match catRes with
        | Ok x ->x
        | Error ex -> raise ex
    let cat = 
        match maybeCat with
        | Some cat ->  cat
        | None -> raise (System.ArgumentException("Category not found!"))

    {id = input.id; title = input.title; category = cat; author = input.author; content = input.content }

  let createAction (ctx: HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let! input = Controller.getModel<MemViewModel> ctx
      let! catRes = Database.getCategoryById  cnf.connectionString (string input.categoryId)
      let mem = mapToDomain (input, catRes)
      let validateResult = Validation.validate mem
      if validateResult.IsEmpty then
        let! result = Database.insert cnf.connectionString input
        match result with
        | Ok _ ->
          return! Controller.redirect ctx (Links.index ctx)
        | Error ex ->
          return raise ex
      else
        let! catsRes = Database.getAllCategories cnf.connectionString
        let cats = 
          match catsRes with
          | Ok x -> x
          | Error ex -> raise ex
        return! Controller.renderXml ctx (Views.add ctx (Some mem) cats validateResult)
    }

  let updateAction (ctx: HttpContext, id : string) =
    task {
      let! input = Controller.getModel<Mem> ctx
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then
        let cnf = Controller.getConfig ctx
        let! result = Database.update cnf.connectionString input
        match result with
        | Ok _ ->
          return! Controller.redirect ctx (Links.index ctx)
        | Error ex ->
          return raise ex
      else
        let cnf = Controller.getConfig ctx
        let! catsRes = Database.getAllCategories cnf.connectionString
        let cats = 
          match catsRes with
          | Ok x -> x
          | Error ex -> raise ex
        return! Controller.renderXml ctx (Views.edit ctx input cats validateResult)
    }

  let deleteAction (ctx: HttpContext, id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.delete cnf.connectionString id
      match result with
      | Ok _ ->
        return! Controller.redirect ctx (Links.index ctx)
      | Error ex ->
        return raise ex
    }

  let resource = controller {
    index indexAction
    show showAction
    add addAction
    edit editAction
    create createAction
    update updateAction
    delete deleteAction
  }

