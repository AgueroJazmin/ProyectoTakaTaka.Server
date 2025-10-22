using Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Evento : EntityBase
    {
        [Required(ErrorMessage = "Error")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        //Para saber que empleado esta a cargo del evento asi la dueña puede ver quien es el responsable
        public int? EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }

        [Required(ErrorMessage = "Error")]
        public int CumpleaneroId { get; set; }
        public Cumpleanero? Cumpleanero { get; set; }

        [Required(ErrorMessage = "Error")]
        public int ComboId { get; set; }
        public Combo? Combo { get; set; }

        [Required(ErrorMessage = "Debe seleccionarse un horario para el evento")]
        public int HorarioId { get; set; }
        public Horario? Horario { get; set; }

        //
        [Required(ErrorMessage = "Error")]
        public List<EventoOpcional> EventoOpcionales { get; set; } = new();
        public List<Pago>? Pagos { get; set; } = new List<Pago>();

        //
        [Required(ErrorMessage = "La Fecha del evento es oblgatorio")]
        public required DateOnly Fecha { get; set; }


        [Required(ErrorMessage = "Es necesario especificar la Tematica del Evento")]
        [MaxLength(100, ErrorMessage = "Error")]
        public required string Tematica { get; set; }
    }
}
