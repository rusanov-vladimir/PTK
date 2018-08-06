module Program

open System.Reflection
open SimpleMigrations
open Npgsql
open SimpleMigrations.DatabaseProvider
open SimpleMigrations.Console
open System.Text.RegularExpressions


let connectionString ()= 
    let environmentConnectionString = System.Environment.GetEnvironmentVariable("DATABASE_URL")
    let connectionString =
        if not <| System.String.IsNullOrEmpty environmentConnectionString then
            let m = Regex.Match(environmentConnectionString, "postgres://(.*):(.*)@(.*):(.*)/(.*)")
            let cstring =
              "Server="   + m.Groups.[3].Value + ";" +
              "Port="     + m.Groups.[4].Value + ";" + 
              "Database=" + m.Groups.[5].Value + ";" +
              "User Id="  + m.Groups.[1].Value + ";" +
              "Password=" + m.Groups.[2].Value + ";" +
              "Pooling=true;SSL Mode=Require;Trust Server Certificate=true;"
            cstring
        else
            "User ID=postgres;Password=ptk;Host=127.0.0.1;Port=5432;Database=ptk;Pooling=true;"
    connectionString

[<EntryPoint>]
let main argv =
    let assembly = Assembly.GetExecutingAssembly()
    let cn = connectionString()
    use db = new NpgsqlConnection(cn)
    db.Open()
    let provider = PostgresqlDatabaseProvider(db)
    let migrator = SimpleMigrator(assembly, provider)
    let consoleRunner = ConsoleRunner(migrator)
    consoleRunner.Run(argv)
    0