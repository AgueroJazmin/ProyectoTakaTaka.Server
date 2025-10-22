using ProyectoTakaTaka.Shared.DTO;

namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public interface IRepositorioEvento
    {
        Task<bool> BorrarEvento(int id);
        Task<int> InsertarEvento(EventoCrearDTO dto);
        Task<int> InsertarEventoCompleto(EventoCrearCompletoDTO dto);
        Task<List<EventoCantidadDTO>> SelectCantidadEventos();
        Task<List<ListadoEventoDTO>> SelectListadoEventos();
    }
}