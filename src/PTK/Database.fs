module Database

open Dapper
open System.Data.Common
open System.Collections.Generic
open FSharp.Control.Tasks.ContextInsensitive

let inline (=>) k v = k, box v

let execute (connection:#DbConnection) (sql:string) (data:_) =
    task {
        try
            let! res = connection.ExecuteAsync(sql, data)
            return Ok res
        with
        | ex -> return Error ex
    }

let query (connection:#DbConnection) (sql:string) (parameters:IDictionary<string, obj> option) =
    task {
        try
            let! res =
                match parameters with
                | Some p -> connection.QueryAsync<'T>(sql, p)
                | None -> connection.QueryAsync<'T>(sql)
            return Ok res
        with
        | ex -> return Error ex
    }

let queryJoin2<'T1, 'T2> (connection:#DbConnection) (sql:string) (combi:System.Func<'T1, 'T2, 'T1>) (parameters:IDictionary<string, obj> option) =
    task {
        try
            let! res =
                match parameters with
                | Some p -> connection.QueryAsync<'T1, 'T2, 'T1>(sql, combi, p)
                | None -> connection.QueryAsync<'T1, 'T2, 'T1>(sql, combi)
            return Ok res
        with
        | ex -> return Error ex
    }    

let querySingle (connection:#DbConnection) (sql:string) (parameters:IDictionary<string, obj> option) =
    task {
        try
            let! res =
                match parameters with
                | Some p -> connection.QuerySingleOrDefaultAsync<'T>(sql, p)
                | None -> connection.QuerySingleOrDefaultAsync<'T>(sql)
            return
                if isNull (box res) then Ok None
                else Ok (Some res)

        with
        | ex -> return Error ex
    }

let singleJoin2<'T1, 'T2> (connection:#DbConnection) (sql:string) (combi:System.Func<'T1, 'T2, 'T1>) (parameters:IDictionary<string, obj> option) =
    task {
        try
            let! res =
                match parameters with
                | Some p -> connection.QueryAsync<'T1, 'T2, 'T1>(sql, combi, p)
                | None -> connection.QueryAsync<'T1, 'T2, 'T1>(sql, combi)
            let res2 = 
                match res with
                | x when (List.ofSeq x |> List.length = 1) -> Some (Seq.find (fun _->true) x)
                | _ -> None
            return Ok res2
        with
        | ex -> return Error ex
    }    