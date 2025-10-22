using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Opcional : EntityBase
    {
        public List<EventoOpcional> EventoOpcionales { get; set; } = new();

        [Required(ErrorMessage = "El Opcional  es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El nombre del Opcional excede la cantidad de caracteres")]
        public required string NomOpcional { get; set; }

        [Precision(10, 2)]
        public decimal Precio { get; set; } = 0;
    }
}
