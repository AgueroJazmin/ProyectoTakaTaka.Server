using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.DTO
{
    public class ClienteListadoDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = "";
        public required string Telefono { get; set; } = "";
    }
}
