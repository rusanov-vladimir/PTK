namespace Mems

open Database
open System.Threading.Tasks
open Mems
open Categories
open Npgsql

module Database =
  let combine  =
    System.Func<Mem, Category, Mem>(fun (m: Mem) (c:Category) -> {m with category=c} )
  
  let memsJoinCats ="
      SELECT 
        Mems.id, Mems.title, Mems.content, Mems.author, Mems.tstamp, Mems.modifieddate, Categories.id, Categories.title
      FROM Mems
      JOIN Categories ON Mems.categoryId = Categories.id" 
  let pageSize  = 2  
  let orderingQuery = " ORDER BY Mems.modifieddate DESC, Mems.tstamp DESC"
  let paginationQuery = " OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY"
  let truncateContentQuery (originalQuery:string) = originalQuery.Replace("Mems.content", "LEFT(Mems.content, 100) as content")
  let paginateAndTruncateContentGetQuery (originalQuery:string) = truncateContentQuery originalQuery + orderingQuery + paginationQuery

  let getAll connectionString page: Task<Result<seq<Mem>, exn>> =    
    let finalQuery = paginateAndTruncateContentGetQuery memsJoinCats
    queueConnection connectionString (fun connection -> 
      queryJoin2<Mem, Category> connection finalQuery combine (Some <| dict ["skip" => (page-1)*pageSize; "pagesize"=> pageSize]) )

  let getById connectionString id : Task<Result<Mem option, exn>> =
    queueConnection connectionString (fun connection -> 
      singleJoin2<Mem, Category> connection (memsJoinCats + " WHERE Mems.id=@id") combine (Some <| dict ["id" => id]) )

  let update connectionString v : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
      execute connection "UPDATE Mems SET id = @id, title = @title, content = @content, author = @author, categoryId = @categoryId, modifieddate=current_timestamp WHERE id=@id" v )

  let insert connectionString v : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
      execute connection "INSERT INTO Mems(title, content, author, categoryId, tstamp) VALUES (@title, @content, @author, @categoryId, current_timestamp)" v )

  let delete connectionString id : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
      execute connection "DELETE FROM Mems WHERE id=@id" (dict ["id" => id]) )
