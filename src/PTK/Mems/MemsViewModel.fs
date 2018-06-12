namespace Mems

[<CLIMutable>]
type MemViewModel = {
  id: int
  title: string
  content: string
  author: string
  categoryId: int
}