namespace Migrations
open SimpleMigrations

[<Migration(201804111534L, "Create Mems")>]
type CreateMems() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Categories (
    id         serial primary key,
    title       varchar(40) NOT NULL
);

CREATE TABLE Mems (
    	id serial primary key,
    	title       varchar(40) NOT NULL,
    	content   varchar(40) NOT NULL,
    	author       varchar(40) NOT NULL,
        categoryId SERIAL REFERENCES Categories
);")

  override __.Down() =
    base.Execute(@"DROP TABLE Mems; DROP TABLE Categories;")
