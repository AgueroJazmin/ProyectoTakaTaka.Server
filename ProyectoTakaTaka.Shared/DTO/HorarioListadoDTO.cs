using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class HorarioListadoDTO
    {
        public int Id { get; set; }
        public TimeOnly HInicio { get; set; }
        public TimeOnly HFin { get; set; }
        public bool MediaHoraExtra { get; set; }
        public bool Disponible { get; set; }
        public int EstadoRegistro { get; set; }
    }
}
