using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace com.cyberinternauts.csharp.Database
{
    public abstract class BaseMigrationOperation : Microsoft.EntityFrameworkCore.Migrations.Operations.MigrationOperation
    {
        public abstract void Generate(MigrationsSqlGeneratorDependencies dependencies, IModel? model, MigrationCommandListBuilder builder);
    }
}
