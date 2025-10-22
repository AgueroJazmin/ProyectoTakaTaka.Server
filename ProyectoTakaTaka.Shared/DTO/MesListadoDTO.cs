using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class MesListadoDTO
    {
        public int Id { get; set; }
        public string MesHabilitado { get; set; } = "";
        public string Año { get; set; } = "";
        public int EstadoRegistro { get; set; }

    }
}
