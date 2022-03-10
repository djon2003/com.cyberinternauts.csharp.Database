# com.cyberinternauts.csharp.Database
Database Tooling having now only one purpose: being able to call SQL code within a migration in correct order.

The problem was happening when using Sqlite. Maybe it would not otherwise.

## Example of the issue:
```
migrationBuilder.AlterColumn<DateTime>(
    name: "BirthDay",
    table: "MetaPersons",
    type: "TEXT",
    nullable: true,
    oldClrType: typeof(DateTime),
    oldType: "TEXT");
migrationBuilder.Sql(@"UPDATE MetaPersons SET BirthDay = null WHERE BirthDay = '0001-01-01 00:00:00';");
```

```
BEGIN TRANSACTION;

//FIXME: Next line is called before table modification and crashes
UPDATE MetaPersons SET BirthDay = null WHERE BirthDay = '0001-01-01 00:00:00'; 

CREATE TABLE "ef_temp_MetaPersons" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_MetaPersons" PRIMARY KEY AUTOINCREMENT,
    "BirthDay" TEXT NULL,
    "DeathDay" TEXT NULL,
    "ExternalId" TEXT NULL,
    "MetaSource" TEXT NULL,
    "Name" TEXT COLLATE NOCASE NULL,
    "Professions" TEXT NULL
);

INSERT INTO "ef_temp_MetaPersons" ("Id", "BirthDay", "DeathDay", "ExternalId", "MetaSource", "Name", "Professions")
SELECT "Id", "BirthDay", "DeathDay", "ExternalId", "MetaSource", "Name", "Professions"
FROM "MetaPersons";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "MetaPersons";

ALTER TABLE "ef_temp_MetaPersons" RENAME TO "MetaPersons";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE UNIQUE INDEX "IX_MetaPersons_ExternalId_MetaSource" ON "MetaPersons" ("ExternalId", "MetaSource");

CREATE INDEX "IX_MetaPersons_Name" ON "MetaPersons" ("Name");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220309145323_MetaPerson.BirthDate Nullable', '6.0.2');

COMMIT;
```
    
## Example of the result:
```
migrationBuilder.AlterColumn<DateTime>(
    name: "BirthDay",
    table: "MetaPersons",
    type: "TEXT",
    nullable: true,
    oldClrType: typeof(DateTime),
    oldType: "TEXT");
migrationBuilder.ChangeDateToNullable("MetaPersons", "BirthDay");
```

```
BEGIN TRANSACTION;

CREATE TABLE "ef_temp_MetaPersons" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_MetaPersons" PRIMARY KEY AUTOINCREMENT,
    "BirthDay" TEXT NULL,
    "DeathDay" TEXT NULL,
    "ExternalId" TEXT NULL,
    "MetaSource" TEXT NULL,
    "Name" TEXT COLLATE NOCASE NULL,
    "Professions" TEXT NULL
);

INSERT INTO "ef_temp_MetaPersons" ("Id", "BirthDay", "DeathDay", "ExternalId", "MetaSource", "Name", "Professions")
SELECT "Id", "BirthDay", "DeathDay", "ExternalId", "MetaSource", "Name", "Professions"
FROM "MetaPersons";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "MetaPersons";

ALTER TABLE "ef_temp_MetaPersons" RENAME TO "MetaPersons";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE UNIQUE INDEX "IX_MetaPersons_ExternalId_MetaSource" ON "MetaPersons" ("ExternalId", "MetaSource");

CREATE INDEX "IX_MetaPersons_Name" ON "MetaPersons" ("Name");

UPDATE "MetaPersons" SET "BirthDay" = null WHERE "BirthDay" = '0001-01-01 00:00:00';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220309145323_MetaPerson.BirthDate Nullable', '6.0.2');

COMMIT;
```
