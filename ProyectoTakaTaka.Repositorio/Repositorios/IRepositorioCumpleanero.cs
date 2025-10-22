using ProyectoTakaTaka.Shared.DTO;

namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public interface IRepositorioCumpleanero
    {
        Task DeleteCumpleanero(int id);
        Task InsertCumpleanero(CumpleaneroCrearDTO dto);
        Task<CumpleaneroListadoDTO?> SelectCumpleaneroById(int id);
        Task<List<CumpleaneroListadoDTO>> SelectListadoCumpleaneros();
        Task UpdateCumpleanero(int id, CumpleaneroCrearDTO dto);
    }
}