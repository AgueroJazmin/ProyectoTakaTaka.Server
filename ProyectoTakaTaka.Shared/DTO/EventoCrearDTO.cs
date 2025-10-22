using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class EventoCrearDTO
    {
        [Required(ErrorMessage = "Error")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Error")]
        public int CumpleaneroId { get; set; }

        [Required(ErrorMessage = "Error")]
        public int Combo { get; set; }
        public List<int> OpcionalesId { get; set; } = new();

        [Required(ErrorMessage = "La Fecha del evento es oblgatorio")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un horario disponible")]
        public int HorarioId { get; set; }

        [Required(ErrorMessage = "Es necesario especificar la Tematica del Evento")]
        [MaxLength(100, ErrorMessage = "Error")]
        public string Tematica { get; set; } = "";
    }
}
