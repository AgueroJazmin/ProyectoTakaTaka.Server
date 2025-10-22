using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Mes : EntityBase
    {
        [Required(ErrorMessage = "Los Meses habilitados son obligatorios")]
        [MaxLength(100, ErrorMessage = "Error")]
        public required string MesHabilitado { get; set; }

        [Required(ErrorMessage = "El Año es obligatorio")]
        [MaxLength(100, ErrorMessage = "Error")]
        public required string Año { get; set; }
       // public List<Horario>? Horarios { get; set; } = new();
    }
}
