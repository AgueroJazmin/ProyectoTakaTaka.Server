using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Empleado : EntityBase
    {
        [Required(ErrorMessage = "El Nombre del Empleado es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre excede la cant")]
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El Cargo del Empleado es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre excede la cant")]
        public required string Cargo { get; set; }
        public List<Evento>? Eventos { get; set; }
    }
}
