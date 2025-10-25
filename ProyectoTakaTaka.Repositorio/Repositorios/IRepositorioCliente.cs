using ProyectoTakaTaka.Shared.DTO;

namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public interface IRepositorioCliente
    {
        //Task DeleteCliente(int id);
        Task<bool> DeleteCliente(int id);
        Task InsertCliente(ClienteCrearDTO dto);
        Task<ClienteListadoDTO?> SelectClienteById(int id);
        Task<List<ClienteListadoDTO>> SelectListadoClientes();
        Task UpdateCliente(int id, ClienteCrearDTO dto);
    }
}