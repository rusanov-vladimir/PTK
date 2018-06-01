namespace Categories

open Database
open Microsoft.Data.Sqlite
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Categories

module Database =

  
  let categoriesSql ="
      SELECT 
        Categories.id, Categories.title
      FROM Categories" 
  let getAllCategories connectionString : Task<Result<Category seq, exn>> =
    use connection = new SqliteConnection(connectionString)
    query connection categoriesSql None

  

  // let getById connectionString id : Task<Result<Mem option, exn>> =
  //   use connection = new SqliteConnection(connectionString)
  //   singleJoin2<Mem, Category> connection (memsJoinCats + " WHERE Mems.id=@id") combine (Some <| dict ["id" => id])

  // let update connectionString v : Task<Result<int,exn>> =
  //   use connection = new SqliteConnection(connectionString)
  //   execute connection "UPDATE Mems SET id = @id, title = @title, content = @content, author = @author, categoryId = @categoryId WHERE id=@id" v

  // let insert connectionString v : Task<Result<int,exn>> =
  //   use connection = new SqliteConnection(connectionString)
  //   execute connection "INSERT INTO Mems(id, title, content, author, categoryId) VALUES (@id, @title, @content, @author, @categoryId)" v

  // let delete connectionString id : Task<Result<int,exn>> =
  //   use connection = new SqliteConnection(connectionString)
  //   execute connection "DELETE FROM Mems WHERE id=@id" (dict ["id" => id])
