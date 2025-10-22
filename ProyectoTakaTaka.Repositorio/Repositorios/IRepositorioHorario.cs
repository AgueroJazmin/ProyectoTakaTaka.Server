using ProyectoTakaTaka.Shared.DTO;

namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public interface IRepositorioHorario
    {
        Task<bool> DeleteHorario(int id);
        Task<int> InsertHorario(HorarioCrearDTO dto);
        Task<List<HorarioListadoDTO>> SelectHorariosDisponibles();
        Task<List<HorarioListadoDTO>> SelectListadoHorarios();
        Task<bool> UpdateHorario(int id, HorarioCrearDTO dto);
    }
}