using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class RelationshipDefinitionModel
    {
        [Required(ErrorMessage = "La tabla de origen es obligatoria.")]
        public string SourceTable { get; set; }

        [Required(ErrorMessage = "La columna de origen es obligatoria.")]
        [MinLength(1, ErrorMessage = "El nombre de la columna debe tener al menos un carácter.")]
        public string SourceColumn { get; set; }

        [Required(ErrorMessage = "La tabla de destino es obligatoria.")]
        public string TargetTable { get; set; }

        [Required(ErrorMessage = "La columna de destino es obligatoria.")]
        [MinLength(1, ErrorMessage = "El nombre de la columna debe tener al menos un carácter.")]
        public string TargetColumn { get; set; }

        [Required(ErrorMessage = "El tipo de relación es obligatorio.")]
        [RegularExpression("1-1|1-N|N-M", ErrorMessage = "El tipo de relación debe ser '1-1', '1-N' o 'N-M'.")]
        public string RelationshipType { get; set; }

        [MaxLength(128, ErrorMessage = "El nombre de la restricción no puede exceder los 128 caracteres.")]
        public string ConstraintName { get; set; } // Opcional

        public bool CascadeOnDelete { get; set; } = false; // Por defecto es false si no se envía
    }
}
