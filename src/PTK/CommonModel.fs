namespace Common.Models

[<CLIMutable>]
type CategoryWithCount = {
    id: int
    title: string
    postCount: int
  }