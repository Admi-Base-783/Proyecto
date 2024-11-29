using Proyecto.Models;
using System;
using System.Linq;

namespace Proyecto.Data
{
    public static class SqlQueryBuilder
    {
        public static string GenerateCreateTableQuery(TableDefinitionModel model)
        {
            var columns = model.Columns.Select(c =>
                $"{c.ColumnName} {c.DataType} {(c.AllowNull ? "NULL" : "NOT NULL")} {c.Constraint}".Trim());

            var primaryKeys = model.Columns
                .Where(c => c.IsPrimaryKey)
                .Select(c => c.ColumnName)
                .ToList();

            string primaryKeyClause = primaryKeys.Any()
                ? $", PRIMARY KEY ({string.Join(", ", primaryKeys)})"
                : string.Empty;

            return $"CREATE TABLE {model.TableName} ({string.Join(", ", columns)}{primaryKeyClause});";
        }
        public static string GenerateRelationshipQuery(RelationshipDefinitionModel model)
        {
            string cascadeOption = model.CascadeOnDelete ? "ON DELETE CASCADE" : string.Empty;

            return $@"
        ALTER TABLE {model.SourceTable}
        ADD CONSTRAINT {model.ConstraintName ?? $"{model.SourceTable}_{model.TargetTable}_FK"}
        FOREIGN KEY ({model.SourceColumn})
        REFERENCES {model.TargetTable} ({model.TargetColumn})
        {cascadeOption};
    ";
        }
        public static string GenerateForeignKeyQuery(ForeignKeyDefinition foreignKey, string sourceTable)
        {
            string cascade = foreignKey.CascadeOnDelete ? "ON DELETE CASCADE" : string.Empty;

            return $@"
        ALTER TABLE {sourceTable}
        ADD CONSTRAINT FK_{sourceTable}_{foreignKey.TargetTable}
        FOREIGN KEY ({foreignKey.SourceColumn})
        REFERENCES {foreignKey.TargetTable} ({foreignKey.TargetColumn})
        {cascade};
        ";
        }


    }
}