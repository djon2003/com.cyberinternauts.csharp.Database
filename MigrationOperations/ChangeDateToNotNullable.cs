using com.cyberinternauts.csharp.Database.MigrationAttributes;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace com.cyberinternauts.csharp.Database.MigrationOperations
{
    [ExecuteBefore]
    public class ChangeDateToNotNullable : BaseMigrationOperation
    {
        private string Table { get; set; }
        private string Column { get; set; }

        public ChangeDateToNotNullable(string table, string column)
        {
            Table = table;
            Column = column;
        }

        public override void Generate(MigrationsSqlGeneratorDependencies dependencies, IModel? model, MigrationCommandListBuilder builder)
        {
            var helper = dependencies.SqlGenerationHelper;
            builder
                .Append("UPDATE ")
                .Append(helper.DelimitIdentifier(Table))
                .Append(" SET ")
                .Append(helper.DelimitIdentifier(Column))
                .Append(" = '0001-01-01 00:00:00' WHERE ")
                .Append(helper.DelimitIdentifier(Column))
                .Append(" = null")
                .AppendLine(helper.StatementTerminator)
                .EndCommand();
        }
    }
}
