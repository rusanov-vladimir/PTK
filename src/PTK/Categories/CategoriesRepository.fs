namespace Categories

open Database
open Npgsql
open System.Threading.Tasks

module Database =
  let getAll connectionString : Task<Result<Category seq, exn>> =
    use connection = new NpgsqlConnection(connectionString)
    query connection "SELECT id, title FROM Categories" None

  let getById connectionString id : Task<Result<Category option, exn>> =
    use connection = new NpgsqlConnection(connectionString)
    querySingle connection "SELECT id, title FROM Categories WHERE id=@id" (Some <| dict ["id" => id])

  let update connectionString v : Task<Result<int,exn>> =
    use connection = new NpgsqlConnection(connectionString)
    execute connection "UPDATE Categories SET id = @id, title = @title WHERE id=@id" v

  let insert connectionString v : Task<Result<int,exn>> =
    use connection = new NpgsqlConnection(connectionString)
    execute connection "INSERT INTO Categories(id, title) VALUES (@id, @title)" v

  let delete connectionString id : Task<Result<int,exn>> =
    use connection = new NpgsqlConnection(connectionString)
    execute connection "DELETE FROM Categories WHERE id=@id" (dict ["id" => id])

