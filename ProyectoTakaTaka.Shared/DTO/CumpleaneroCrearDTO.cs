using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class CumpleaneroCrearDTO
    {
        [Required(ErrorMessage = "El Nombre del cliente es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre excede la cant")]
        public required string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La Fecha de Nacimiento del cumpleañero/a es oblgatorio")]
        public required DateOnly FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El ClienteId es oblgatorio")]
        public required int ClienteId { get; set; }
    }
}
