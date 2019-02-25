namespace Layout

open Database
open Npgsql
open System.Threading.Tasks
open Common.Models

module Database =
  let getAll (connectionString:string) : Task<Result<CategoryWithCount seq, exn>> =
    queueConnection connectionString (fun connection -> 
      query connection "
        SELECT c.id, c.title, count(m.id) as postCount FROM Categories c
        LEFT JOIN Mems m on c.id = m.categoryId
        GROUP BY c.id, c.title
        ORDER BY count(m.id) desc
        FETCH FIRST 5 rows only
      " None)
