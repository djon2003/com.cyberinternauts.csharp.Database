using com.cyberinternauts.csharp.Database.MigrationOperations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace com.cyberinternauts.csharp.Database
{
    public static class MigrationsCorrections
    {
        public static OperationBuilder<ChangeDateToNullable> ChangeDateToNullable(this MigrationBuilder migrationBuilder, string table, string column)
        {
            var operation = new ChangeDateToNullable(table, column);
            migrationBuilder.Operations.Add(operation);
            return new OperationBuilder<ChangeDateToNullable>(operation);
        }

        public static OperationBuilder<ChangeDateToNotNullable> ChangeDateToNotNullable(this MigrationBuilder migrationBuilder, string table, string column)
        {
            var operation = new ChangeDateToNotNullable(table, column);
            migrationBuilder.Operations.Add(operation);
            return new OperationBuilder<ChangeDateToNotNullable>(operation);
        }
    }
}
