using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCoreEmpleadosSession.Models
{
    [Table("EMPLEADO")]
    public class Empleado
    {
        [Key]
        [Column("EMP_NO")]
        public int IdEmpleado { get; set; }        
        [Column("APELLIDO")]
        public int Apellido { get; set; }        
        [Column("OFICIO")]
        public int Oficio { get; set; }        
        [Column("SALARIO")]
        public int Salario { get; set; }
    }
}
