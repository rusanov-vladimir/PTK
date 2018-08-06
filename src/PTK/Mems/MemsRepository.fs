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

  let getAll connectionString : Task<Result<Mem seq, exn>> =
    use connection = new NpgsqlConnection(connectionString)
    connection.Open()
    queryJoin2<Mem, Category> connection memsJoinCats combine None

  let getById connectionString id : Task<Result<Mem option, exn>> =
    use connection = new NpgsqlConnection(connectionString)
    connection.Open()
    singleJoin2<Mem, Category> connection (memsJoinCats + " WHERE Mems.id=@id") combine (Some <| dict ["id" => id])

  let update connectionString v : Task<Result<int,exn>> =
    use connection = new NpgsqlConnection(connectionString)
    connection.Open()
    execute connection "UPDATE Mems SET id = @id, title = @title, content = @content, author = @author, categoryId = @categoryId WHERE id=@id" v

  let insert connectionString v : Task<Result<int,exn>> =
    use connection = new NpgsqlConnection(connectionString)
    connection.Open()
    execute connection "INSERT INTO Mems(id, title, content, author, categoryId) VALUES (@id, @title, @content, @author, @categoryId)" v

  let delete connectionString id : Task<Result<int,exn>> =
    use connection = new NpgsqlConnection(connectionString)
    connection.Open()
    execute connection "DELETE FROM Mems WHERE id=@id" (dict ["id" => id])
