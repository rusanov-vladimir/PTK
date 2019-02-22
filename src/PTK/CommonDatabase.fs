namespace Layout

open Database
open Npgsql
open System.Threading.Tasks
open Common.Models

module Database =

  let getAll connectionString : Task<Result<seq<CategoryWithCount>, exn>> =
    queueConnection connectionString (fun connection -> 
      query connection "SELECT id, title, description FROM Categories" None)
    
