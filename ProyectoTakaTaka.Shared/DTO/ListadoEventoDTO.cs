using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class ListadoEventoDTO
    {
        public int Id { get; set; }
        public string Evento { get; set; } = "";
        public string Cliente { get; set; } = "";
        public required string Telefono { get; set; } = "";
        public string Cumpleanero { get; set; } = "";
        public int Edad { get; set; }
        public string Combo { get; set; } = "";
        public List<string> Opcionales { get; set; } = new();
        public List<PagoListadoDTO> Pagos { get; set; } = new();
        public DateOnly Fecha { get; set; }
        public int HorarioId { get; set; }
        public TimeOnly HInicio { get; set; }
        public TimeOnly HFin { get; set; }
        public string Tematica { get; set; } = "";
    }
}
