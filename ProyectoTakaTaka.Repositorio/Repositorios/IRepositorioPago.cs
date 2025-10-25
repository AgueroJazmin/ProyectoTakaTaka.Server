using ProyectoTakaTaka.Shared.DTO;

namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public interface IRepositorioPago
    {
        Task DeletePago(int id);
        Task InsertPago(CrearPagoDTO dto);
        //Task InsertPago(int id, CrearPagoDTO dto);
        Task<List<PagoListadoDTO>> SelectListadoPagos();
        Task UpdatePago(int id, CrearPagoDTO dto);
    }
}