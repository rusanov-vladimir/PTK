namespace Migrations
open SimpleMigrations

[<Migration(201804111534L, "Create Mems")>]
type CreateMems() =
  inherit Migration()

  override __.Up() =
    base.Execute("""

CREATE TABLE Categories (
    id          SERIAL PRIMARY KEY,
    title       VARCHAR(128) NOT NULL,
    description VARCHAR(512) NOT NULL
);

CREATE TABLE Faviorites (
    id           SERIAL PRIMARY KEY,
    title        VARCHAR(128) NOT NULL,
    comment      VARCHAR(512) NULL,
    url          VARCHAR(512) NOT NULL
);

CREATE TABLE Mems (
  	id           SERIAL PRIMARY KEY,
  	title        VARCHAR(256) NOT NULL,
  	content      text NOT NULL,
  	author       VARCHAR(128) NOT NULL,
    categoryId   SERIAL REFERENCES Categories
);

CREATE TABLE Tags (
    id          SERIAL PRIMARY KEY,
    title       VARCHAR(128) NOT NULL,
    description VARCHAR(512) NOT NULL
);

CREATE TABLE TagsMems (
    id      SERIAL PRIMARY KEY,
    memId   SERIAL REFERENCES Mems,
    tagId   SERIAL REFERENCES Tags,
    CONSTRAINT uq_mem_tag_ UNIQUE (memId, tagId)
);

""")

  override __.Down() =
    base.Execute(@"DROP TABLE Mems; DROP TABLE Categories; DROP TABLE Faviorites;")
