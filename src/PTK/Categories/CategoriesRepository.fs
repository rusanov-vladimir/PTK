namespace Categories

open Database
open Npgsql
open System.Threading.Tasks

module Database =

  let getAll connectionString : Task<Result<Category seq, exn>> =
    queueConnection connectionString (fun connection -> 
      query connection "SELECT id, title, description FROM Categories" None)
    

  let getById connectionString id : Task<Result<Category option, exn>> =
    queueConnection connectionString (fun connection -> 
      querySingle connection "SELECT id, title, description FROM Categories WHERE id=@id" (Some <| dict ["id" => id]) )

  let update connectionString v : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
      execute connection "UPDATE Categories SET id = @id, title = @title, description=@description WHERE id=@id" v )

  let insert connectionString v : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
      execute connection "INSERT INTO Categories(title, description) VALUES (@title, @description)" v )

  let delete connectionString id : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
       execute connection "DELETE FROM Categories WHERE id=@id" (dict ["id" => id]))

