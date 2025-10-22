using Microsoft.AspNetCore.Mvc;
using ProyectoTakaTaka.Repositorio.Repositorios;
using ProyectoTakaTaka.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos.Entity;

namespace ProyectoTakaTaka.Server.Controllers
{
    [ApiController]
    [Route("api/Opcionales")]
    public class OpcionalController : ControllerBase
    {
        private readonly IRepositorioOpcional repositorio;

        public OpcionalController(IRepositorioOpcional repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoOpcional")]
        public async Task<ActionResult<List<ListadoOpcionalDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoOpcionales();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListadoOpcionalDTO>> Get(int id)
        {
            var opcional = await repositorio.SelectOpcionalById(id);
            if (opcional == null) return NotFound();
            return Ok(opcional);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CrearOpcionalDTO dto)
        {
            await repositorio.InsertOpcional(dto);
            return Ok("Opcional creado correctamente");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CrearOpcionalDTO dto)
        {
            await repositorio.UpdateOpcional(id, dto);
            return Ok("Opcional actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await repositorio.DeleteOpcional(id);
            return Ok("Opcional eliminado correctamente");
        }
    }
}
