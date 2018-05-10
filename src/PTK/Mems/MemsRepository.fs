namespace Mems

open Database
open Microsoft.Data.Sqlite
open System.Threading.Tasks

module Database =
  let getAll connectionString : Task<Result<Mem seq, exn>> =
    use connection = new SqliteConnection(connectionString)
    query connection "SELECT id, title, content, author, categoryId FROM Mems" None

  let getById connectionString id : Task<Result<Mem option, exn>> =
    use connection = new SqliteConnection(connectionString)
    querySingle connection "SELECT id, title, content, author, categoryId FROM Mems WHERE id=@id" (Some <| dict ["id" => id])

  let update connectionString v : Task<Result<int,exn>> =
    use connection = new SqliteConnection(connectionString)
    execute connection "UPDATE Mems SET id = @id, title = @title, content = @content, author = @author, categoryId = @categoryId WHERE id=@id" v

  let insert connectionString v : Task<Result<int,exn>> =
    use connection = new SqliteConnection(connectionString)
    execute connection "INSERT INTO Mems(id, title, content, author, categoryId) VALUES (@id, @title, @content, @author, @categoryId)" v

  let delete connectionString id : Task<Result<int,exn>> =
    use connection = new SqliteConnection(connectionString)
    execute connection "DELETE FROM Mems WHERE id=@id" (dict ["id" => id])



  let insertCategory connectionString v : Task<Result<int,exn>> =
    use connection = new SqliteConnection(connectionString)
    execute connection "INSERT INTO Categories(id, title) VALUES (@id, @title)" v

  let deleteCategories connectionString : Task<Result<int,exn>> =
    use connection = new SqliteConnection(connectionString)
    execute connection "DELETE FROM Categories" null

