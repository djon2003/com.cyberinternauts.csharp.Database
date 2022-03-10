using com.cyberinternauts.csharp.Database.MigrationAttributes;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace com.cyberinternauts.csharp.Database.MigrationOperations
{
    [ExecuteAfter]
    public class ArbitrarySqlAfter : BaseMigrationOperation
    {
        private string Sql { get; set; }

        public ArbitrarySqlAfter(string sql) => Sql = sql;

        public override void Generate(MigrationsSqlGeneratorDependencies dependencies, IModel? model, MigrationCommandListBuilder builder) 
            => builder.AppendLine(Sql).EndCommand();
    }
}
