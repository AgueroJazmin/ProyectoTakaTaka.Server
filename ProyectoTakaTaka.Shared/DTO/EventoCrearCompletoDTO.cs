using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class EventoCrearCompletoDTO
    {
        // Cliente
        public string ClienteNombre { get; set; } = "";
        public string ClienteApellido { get; set; } = "";
        public string ClienteTelefono { get; set; } = "";

        // Cumpleañero
        public string CumpleaneroNombre { get; set; } = "";
        public int CumpleaneroEdad { get; set; }

        // Evento
        public DateOnly Fecha { get; set; }
        public int ComboId { get; set; }
        public int HorarioId { get; set; }
        public string Tematica { get; set; } = "";
        public List<int> OpcionalesId { get; set; } = new();
    }
}
