using com.cyberinternauts.csharp.Database.MigrationOperations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace com.cyberinternauts.csharp.Database
{
    public static class MigrationsCorrections
    {
        private static OperationBuilder<TOperation> MigrationCorrection<TOperation>(this MigrationBuilder migrationBuilder, TOperation operation) where TOperation : MigrationOperation
        {
            migrationBuilder.Operations.Add(operation);
            return new OperationBuilder<TOperation>(operation);
        }

        public static OperationBuilder<ArbitrarySqlBefore> ArbitrarySqlBefore(this MigrationBuilder migrationBuilder, string sql)
            => MigrationCorrection<ArbitrarySqlBefore>(migrationBuilder, new ArbitrarySqlBefore(sql));

        public static OperationBuilder<ArbitrarySqlAfter> ArbitrarySqlAfter(this MigrationBuilder migrationBuilder, string sql)
            => MigrationCorrection<ArbitrarySqlAfter>(migrationBuilder, new ArbitrarySqlAfter(sql));

        public static OperationBuilder<ChangeDateToNullable> ChangeDateToNullable(this MigrationBuilder migrationBuilder, string table, string column)
            => MigrationCorrection<ChangeDateToNullable>(migrationBuilder, new ChangeDateToNullable(table, column));

        public static OperationBuilder<ChangeDateToNotNullable> ChangeDateToNotNullable(this MigrationBuilder migrationBuilder, string table, string column)
            => MigrationCorrection<ChangeDateToNotNullable>(migrationBuilder, new ChangeDateToNotNullable(table, column));
    }
}
