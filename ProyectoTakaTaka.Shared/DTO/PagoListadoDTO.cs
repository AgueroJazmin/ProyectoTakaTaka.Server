using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class PagoListadoDTO
    {
        public int Id { get; set; }

        public int EventoId { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; } = "";
        public string EstadoPago { get; set; } = "";
        public DateTime FechaPago { get; set; }
        public string Evento { get; set; } = "";
    }
}
