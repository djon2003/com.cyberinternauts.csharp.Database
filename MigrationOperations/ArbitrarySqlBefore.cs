using com.cyberinternauts.csharp.Database.MigrationAttributes;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace com.cyberinternauts.csharp.Database.MigrationOperations
{
    [ExecuteBefore]
    public class ArbitrarySqlBefore : BaseMigrationOperation
    {
        private string Sql { get; set; }

        public ArbitrarySqlBefore(string sql) => Sql = sql;

        public override void Generate(MigrationsSqlGeneratorDependencies dependencies, IModel? model, MigrationCommandListBuilder builder) 
            => builder.AppendLine(Sql).EndCommand();
    }
}
