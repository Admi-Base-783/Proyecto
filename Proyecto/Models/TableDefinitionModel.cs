using Proyecto.Data;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Proyecto.Models
{
    public class TableDefinitionModel
    {
        public string TableName { get; set; }
        public List<ColumnDefinition> Columns { get; set; }
        public ForeignKeyDefinition ForeignKey { get; set; } // Llave foránea
    }

    public class ColumnDefinition
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool AllowNull { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string ReferencedTable { get; set; }
        public string ReferencedColumn { get; set; }
        public string Constraint { get; set; } // Ejemplo: DEFAULT 0
    }
    public class ForeignKeyDefinition
    {
        public string SourceColumn { get; set; }
        public string TargetTable { get; set; }
        public string TargetColumn { get; set; }
        public bool CascadeOnDelete { get; set; }
    }

}


