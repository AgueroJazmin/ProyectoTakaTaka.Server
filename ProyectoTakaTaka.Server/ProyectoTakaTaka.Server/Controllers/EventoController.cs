using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos.Entity;
using ProyectoTakaTaka.Repositorio.Repositorios;
using ProyectoTakaTaka.Shared.DTO;
using static ProyectoTakaTaka.Server.Client.Pages.Evento.EventoCrear;

namespace ProyectoTakaTaka.Server.Controllers
{
    [ApiController]
    [Route("api/Evento")]
    public class EventoController : ControllerBase
    {
        private readonly IRepositorioEvento repositorio;

        public EventoController(IRepositorioEvento repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoEvento")] //api/Evento/ListadoEvento
        //IaActionResult es el resultado del get que devuelve una colecccion o una lista de tipo string con 3 datos
        public async Task<ActionResult<List<ListadoEventoDTO>>> GetListadoE()
        {
            // var eventos = await context.Eventos.ToListAsync();
            var lista = await repositorio.SelectListadoEventos();

            if (lista == null)
            {
                return NotFound("No se encontraron eventos registrados.");
            }

            if (lista.Count == 0)
            {
                return Ok("No se encontraron eventos registrados.");
            }

            return Ok(lista);
        }

        [HttpGet("CantidadEvento")]
        public async Task<ActionResult<List<EventoCantidadDTO>>> GetCantidadE()
        {
            // var eventos = await context.Eventos.ToListAsync();
            var lista = await repositorio.SelectListadoEventos();

            if (lista == null)
            {
                return NotFound("No se encontraron eventos registrados.");
            }

            if (lista.Count == 0)
            {
                return Ok("No se encontraron eventos registrados.");
            }

            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostCrear(EventoCrearDTO DTO)
        {
            try
            {
                var id = await repositorio.InsertarEvento(DTO);
                return Ok(id);
            }
            catch (Exception e)
            {
                var explicate = e.InnerException?.Message ?? e.Message;
                return BadRequest($"Error al crear el evento: {explicate}");
            }

        }

        [HttpPost("CrearCompleto")]
        public async Task<ActionResult<int>> CrearCompleto(EventoCrearCompletoDTO dto)
        {
            try
            {
                var id = await repositorio.InsertarEventoCompleto(dto);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el evento completo: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminar = await repositorio.BorrarEvento(id);
            if (!eliminar)
            {
                return NotFound($"No se encontro el evento: {id}");
            }

            return Ok($"El evento: {id}  se elimino correctamente.");
        }
    }
}
