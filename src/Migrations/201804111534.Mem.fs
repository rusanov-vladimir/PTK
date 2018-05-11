namespace Migrations
open SimpleMigrations

[<Migration(201804111534L, "Create Mems")>]
type CreateMems() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE `Categories` (
    	`id`	INTEGER NOT NULL,
    	`title`	TEXT NOT NULL,
    	PRIMARY KEY(`id`)
    );

    CREATE TABLE `Mems` (
    	`id`	INTEGER NOT NULL,
    	`title`	TEXT NOT NULL,
    	`content`	TEXT NOT NULL,
    	`author`	TEXT NOT NULL,
    	`categoryId`	INT DEFAULT NULL,
    	PRIMARY KEY(`id`),
    	FOREIGN KEY(`categoryId`) REFERENCES `Categories`(`id`)
    );")

  override __.Down() =
    base.Execute(@"DROP TABLE Mems; DROP TABLE Categories;")
