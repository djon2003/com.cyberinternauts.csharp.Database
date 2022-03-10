using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace com.cyberinternauts.csharp.Database
{
    public class MigrationsSqlGenerator<GeneratorType> : IMigrationsSqlGenerator where GeneratorType : MigrationsSqlGenerator
    {
        public GeneratorType BaseGenerator { get; private set; }
        protected MigrationsSqlGeneratorDependencies Dependencies { get; private set; }

        public MigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider migrationsAnnotations)
        {
            if (Activator.CreateInstance(typeof(GeneratorType), new object[] { dependencies, migrationsAnnotations }) is GeneratorType generator)
            {
                BaseGenerator = generator;
                Dependencies = dependencies;
            }
            else
            {
                throw new MissingMethodException(typeof(GeneratorType) + " is missing a constructor (" + typeof(MigrationsSqlGeneratorDependencies) + ", " + typeof(IRelationalAnnotationProvider) + ")");
            }
        }

        protected IReadOnlyList<MigrationCommand> Generate(List<MigrationOperation> operations, IModel? model)
        {
            MigrationCommandListBuilder migrationCommandListBuilder = new(Dependencies);
            try
            {
                foreach (BaseMigrationOperation operation in operations)
                {
                    operation.Generate(Dependencies, model, migrationCommandListBuilder);
                }
            }
            catch 
            {
                //Nothing to do                    
            }

            return migrationCommandListBuilder.GetCommandList();
        }

        public IReadOnlyList<MigrationCommand> Generate(IReadOnlyList<MigrationOperation> operations, IModel? model = null, MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
        {
            var middleOperations = operations.ToList();

            // Take operations to execute before and remove them from "middle" operations
            var operationsToExecuteBefore = middleOperations
                .Where(o => o.GetType().CustomAttributes.Any(a => a.AttributeType.Equals(typeof(MigrationAttributes.ExecuteBeforeAttribute))))
                .ToList();
            operationsToExecuteBefore.ForEach(o => middleOperations.Remove(o));

            // Take operations to execute after and remove them from "middle" operations
            var operationsToExecuteAfter = middleOperations
                .Where(o => o.GetType().CustomAttributes.Any(a => a.AttributeType.Equals(typeof(MigrationAttributes.ExecuteAfterAttribute))))
                .ToList();
            operationsToExecuteAfter.ForEach(o => middleOperations.Remove(o));

            // Generate operations by group (before, middle, after)
            var before = Generate(operationsToExecuteBefore, model);
            var middle = BaseGenerator.Generate(middleOperations, model, options);
            var after = Generate(operationsToExecuteAfter, model);

            // Combine generations
            var combined = new List<MigrationCommand>();
            combined.AddRange(before);
            combined.AddRange(middle);
            combined.AddRange(after);

            // Return combined generations
            return combined;
        }
    }
}
