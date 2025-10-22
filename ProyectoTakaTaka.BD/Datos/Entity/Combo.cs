using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Combo : EntityBase
    {
        [Required(ErrorMessage = "El Combo es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El nombre del Combo excede la cantidad de caracteres")]
        public required string NomCombo { get; set; }

        [Precision(10, 2)]
        public decimal Precio { get; set; } = 0;
    }
}
