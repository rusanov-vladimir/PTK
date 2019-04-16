namespace Migrations
open SimpleMigrations

[<Migration(201904151326L, "Add modifieddate To Mems")>]
type AddModifiedDateToMems() =
  inherit Migration()

  override __.Up() =
    base.Execute("""

ALTER TABLE Mems
ADD COLUMN modifieddate timestamp without time zone NULL;

""")

  override __.Down() =
      base.Execute("""

ALTER TABLE Mems 
DROP CCOLUMN modifieddate;

""")
