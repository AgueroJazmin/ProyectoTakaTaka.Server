using ProyectoTakaTaka.Shared.DTO;

namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public interface IRepositorioCombo
    {
        Task DeleteCombo(int id);
        Task InsertCombo(CrearComboDTO dto);
        Task<ListadoComboDTO?> SelectComboById(int id);
        Task<List<ListadoComboDTO>> SelectListadoCombos();
        Task UpdateCombo(int id, CrearComboDTO dto);
    }
}