namespace Search

open Database
open Npgsql
open System.Threading.Tasks
open Mems
open Database
open Categories

module Database =

  let getMemsBySearchString connectionString searchString page : Task<Result<seq<Mem>, exn>> =
    let wherepredicate = "WHERE Mems.title LIKE '%' || @searchString || '%' OR Mems.Content LIKE '%' || @searchString || '%' OR Categories.title LIKE '%' || @searchString || '%'"
    let query =  memsJoinCats + System.Environment.NewLine + wherepredicate
    let paginatedQuery = paginateAndTruncateContentGetQuery query
    queueConnection connectionString (fun connection -> 
      Database.queryJoin2<Mem, Category> connection paginatedQuery combine 
            (Some <| dict [
                "skip" => (page-1)*pageSize;
                "pagesize"=> pageSize;
                "searchString"=> searchString;
                ]
            )
        )