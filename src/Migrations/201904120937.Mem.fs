namespace Migrations
open SimpleMigrations

[<Migration(201904120937L, "Add timestamp To Mems")>]
type AddTimestampToMems() =
  inherit Migration()

  override __.Up() =
    base.Execute("""

ALTER TABLE Mems
ADD COLUMN tstamp timestamp without time zone NULL;

UPDATE MEMS SET tstamp = current_timestamp WHERE tstamp is null;

ALTER TABLE Mems ADD CONSTRAINT tstamp_not_null CHECK (tstamp IS NOT NULL) NOT VALID;
ALTER TABLE Mems VALIDATE CONSTRAINT tstamp_not_null;

""")

  override __.Down() =
      base.Execute("""

ALTER TABLE Mems 
DROP CCOLUMN tstamp;

""")
