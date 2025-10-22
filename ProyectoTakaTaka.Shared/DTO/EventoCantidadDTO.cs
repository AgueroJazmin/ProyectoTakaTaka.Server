using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class EventoCantidadDTO
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HInicio { get; set; }
        public TimeOnly HFin { get; set; }
    }
}
