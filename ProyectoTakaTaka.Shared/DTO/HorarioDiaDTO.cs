using ProyectoTakaTaka.Shared.Configuraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class HorarioDiaDTO
    {
        public int HorarioId { get; set; }
        public TimeOnly HInicio { get; set; }
        public TimeOnly HFin { get; set; }
        public bool Disponible { get; set; }
        public string? Nota { get; set; } // opcional para decir "30' extra aplicado"

    }
}
