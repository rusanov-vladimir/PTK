namespace Mems
open System

[<CLIMutable>]
type MemViewModel = {
  id: int
  title: string
  content: string
  author: string
  tstamp : DateTime
  categoryId: int
}