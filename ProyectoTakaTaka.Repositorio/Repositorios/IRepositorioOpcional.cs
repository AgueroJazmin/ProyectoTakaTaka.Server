using ProyectoTakaTaka.Shared.DTO;

namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public interface IRepositorioOpcional
    {
        Task DeleteOpcional(int id);
        Task InsertOpcional(CrearOpcionalDTO dto);
        Task<List<ListadoOpcionalDTO>> SelectListadoOpcionales();
        Task<ListadoOpcionalDTO?> SelectOpcionalById(int id);
        Task UpdateOpcional(int id, CrearOpcionalDTO dto);
    }
}