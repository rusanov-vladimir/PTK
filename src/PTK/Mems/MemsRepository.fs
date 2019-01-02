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
        Mems.id, Mems.title, Mems.content, Mems.author, Categories.id, Categories.title 
      FROM Mems
      JOIN Categories ON Mems.categoryId = Categories.id" 
  let pageSize  = 2  

  let getAll connectionString page: Task<Result<Mem seq, exn>> =
    let paginationQuery = sprintf " OFFSET @skip ROWS FETCH NEXT %i ROWS ONLY" pageSize
    let truncatedQuery = memsJoinCats.Replace("Mems.content", "LEFT(Mems.content, 100) as content")
    queueConnection connectionString (fun connection -> 
      queryJoin2<Mem, Category> connection (truncatedQuery + paginationQuery) combine (Some <| dict ["skip" => (page-1)*pageSize]) )

  let getById connectionString id : Task<Result<Mem option, exn>> =
    queueConnection connectionString (fun connection -> 
      singleJoin2<Mem, Category> connection (memsJoinCats + " WHERE Mems.id=@id") combine (Some <| dict ["id" => id]) )

  let update connectionString v : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
      execute connection "UPDATE Mems SET id = @id, title = @title, content = @content, author = @author, categoryId = @categoryId WHERE id=@id" v )

  let insert connectionString v : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
      execute connection "INSERT INTO Mems(title, content, author, categoryId) VALUES (@title, @content, @author, @categoryId)" v )

  let delete connectionString id : Task<Result<int,exn>> =
    queueConnection connectionString (fun connection -> 
      execute connection "DELETE FROM Mems WHERE id=@id" (dict ["id" => id]) )
