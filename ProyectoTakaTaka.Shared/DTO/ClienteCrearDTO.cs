using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class ClienteCrearDTO
    {
        [Required(ErrorMessage = "El Nombre del cliente es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre excede la cant")]
        public required string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El Nombre del cliente es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Apellido excede la cant")]
        public required string Apellido { get; set; } = "";

        [Required(ErrorMessage = "El numero de telefono del cliente es oblgatorio")]
        public required string Telefono { get; set; } = "";

    }
}
