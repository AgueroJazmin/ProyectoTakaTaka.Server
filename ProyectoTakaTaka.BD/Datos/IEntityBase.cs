using ProyectoTakaTaka.Shared.ENUM;

namespace ProyectoTakaTaka.BD.Datos
{
    public interface IEntityBase
    {
        EnumEstadoRegistro EstadoRegistro { get; set; }
        int Id { get; set; }
    }
}