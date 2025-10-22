using Microsoft.AspNetCore.Mvc;
using ProyectoTakaTaka.Repositorio.Repositorios;
using ProyectoTakaTaka.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos.Entity;


namespace ProyectoTakaTaka.Server.Controllers
{
    [ApiController]
    [Route("api/Clientes")]

    public class ClienteController : ControllerBase
    {
        private readonly IRepositorioCliente repositorio;

        public ClienteController(IRepositorioCliente repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoClientes")]
        public async Task<ActionResult<List<ClienteListadoDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoClientes();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteListadoDTO>> Get(int id)
        {
            var cliente = await repositorio.SelectClienteById(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);

        }

        [HttpPost]
        public async Task<ActionResult> Post(ClienteCrearDTO dto)
        {
            await repositorio.InsertCliente(dto);
            return Ok("Cliente creado correctamente");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ClienteCrearDTO dto)
        {
            await repositorio.UpdateCliente(id, dto);
            return Ok("Cliente actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await repositorio.DeleteCliente(id);
            return Ok("Cliente eliminado correctamente");
        }

    }
}
