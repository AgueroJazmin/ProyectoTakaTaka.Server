using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Horario : EntityBase
    {
        [Required(ErrorMessage = "Es necesaria la hora de inicio")]
        public required TimeOnly HInicio { get; set; }

        [Required(ErrorMessage = "Es necesaria la hora de fin")]
        public required TimeOnly HFin { get; set; }
        public bool MediaHoraExtra { get; set; } = false;
        public bool Disponible { get; set; } = true;
        public List<Evento>? Eventos { get; set; } = new();
    }
}
