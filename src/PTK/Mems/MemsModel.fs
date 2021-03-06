namespace Mems
open System
open FSharpVSPowerTools
open Categories


[<CLIMutable>]
type Mem = {
  id: int
  title: string
  content: string
  author: string
  tstamp : DateTime
  modifieddate: DateTime option
  category: Category 
}

module Validation =
  let validate v =
    let validators = [
      //fun u -> if u.id = 0 then Some ("id", "Id shouldn't be empty")
               //else None
      ]

    validators
    |> List.fold (fun acc e ->
      match e v with
      | Some (k,v) -> Map.add k v acc
      | None -> acc
    ) Map.empty
