namespace Migrations
open SimpleMigrations

[<Migration(201805051727L, "Create categories to Mems")>]
type AddCategoriesToMems() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"
    CREATE TABLE Categories(
      id INTEGER NOT NULL,
      title TEXT NOT NULL
    );

    ALTER TABLE Mems
    ADD COLUMN CategoryId INT NULL;")

  override __.Down() =
    base.Execute(@"DROP TABLE Categories;
    DROP TABLE Mems;
    CREATE TABLE Mems(
      id INTEGER NOT NULL,
      title TEXT NOT NULL,
      content TEXT NOT NULL,
      author TEXT NOT NULL
    );")
