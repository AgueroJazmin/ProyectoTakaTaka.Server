using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.BD.Datos.Entity
{
    public class Cliente : EntityBase
    {
        /*Por el momento se requiere que sea obligatorio el Nombre y Apellido
         de los clientes, esto debido a que la charla con la dueña no 
        menciona que les solicite su DNI o CUIL.
        Sin embargo esta pensado que unicamente el CUIL sea 
        el solamente el campo requerido y unico para el cliente
        
        [Required(ErrorMessage = "El Nombre del cliente es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre excede la cant")]

        public required string CUIL { get; set; }
         */
        [Required(ErrorMessage = "El Nombre del cliente es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre excede la cant")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El Nombre del cliente es oblgatorio")]
        [MaxLength(100, ErrorMessage = "El Apellido excede la cant")]
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El numero de telefono del cliente es oblgatorio")]
        public required string Telefono { get; set; }
    }
}
