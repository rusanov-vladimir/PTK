namespace Layout

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Saturn

module Controller =
  let getCategoriesWithCount (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig<Config.Config> ctx
      let! catsResult = Database.getAll cnf.connectionString
      let cats = 
        match catsResult with
        | Ok result ->
            List.ofSeq result
        | Error ex -> 
            raise ex
      let catsview = View.categoriesPartial ctx cats
      return catsview
    }