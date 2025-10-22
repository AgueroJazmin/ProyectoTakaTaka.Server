using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Cumpleanero : EntityBase
    {
        [Required(ErrorMessage = "")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Required(ErrorMessage = "El Nombre del cliente es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre excede la cantidad")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento del cumpleañero/a es requerido")]
        public required DateOnly FechaNacimiento { get; set; }


        //No se si dejarlo aca, o bueno si
        public int Edad
        {
            get
            {
                var hoy = DateTime.Today;
                int edad = hoy.Year - FechaNacimiento.Year;
                if (FechaNacimiento > DateOnly.FromDateTime(hoy.AddYears(-edad))) edad--;
                return edad;
            }
        }
    }
}
