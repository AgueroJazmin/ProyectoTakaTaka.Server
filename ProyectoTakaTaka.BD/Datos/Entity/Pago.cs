using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Pago : EntityBase
    {
        [Required(ErrorMessage = "El evento asociado al pago es obligatorio")]
        public int EventoId { get; set; }
        public Evento? Evento { get; set; }
        // 

        [Required(ErrorMessage = "El Estado de Pago es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Estado de Pago excede la cantidad de caracteres")]
        public required string EstadoPago { get; set; }

        //El Estado de Pago es donde se aclara so esta Seña, Pendiente o Pagado.

        [Required(ErrorMessage = "El Monto de Pago es oblgatorio")]
        [Precision(10, 2)]         //El MaxLength se usa para los strings, para decimales se usa Precision
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")] //El monto no puede ser 0 o negativo
        [MaxLength(100, ErrorMessage = "El Monto excede la cantidad de caracteres permitidos")]
        public required decimal Monto { get; set; }

        [Required(ErrorMessage = "El Metodo de Pago es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Metodo excede la cantidad de caracteres")]
        public required string Metodo { get; set; }

        //Para el frontend ver de poner un Subir Archivos para que ahi se suban los comprobantes
        //Entonces nomas en Metodo se aclara que tipo de comprobante es

        [Required(ErrorMessage = "La Fecha del pago es oblgatorio")]
        public required DateTime FechaPago { get; set; }
    }
}
