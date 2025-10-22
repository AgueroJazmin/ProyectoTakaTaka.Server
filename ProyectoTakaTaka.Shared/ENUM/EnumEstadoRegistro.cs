using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.ENUM
{
    public enum EnumEstadoRegistro
    {
        activo = 1,
        inactivo = 2,
        borrado = 3,
        EnGrabacion = 4// cuando algo fallo porque se estaba grabando 
    }
}
