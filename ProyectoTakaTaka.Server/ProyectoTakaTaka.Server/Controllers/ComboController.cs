using Microsoft.AspNetCore.Mvc;
using ProyectoTakaTaka.Repositorio.Repositorios;
using ProyectoTakaTaka.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos.Entity;

namespace ProyectoTakaTaka.Server.Controllers
{
    [ApiController]
    [Route("api/Combos")]
    public class ComboController : ControllerBase
    {
        private readonly IRepositorioCombo repositorio;

        public ComboController(IRepositorioCombo repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoCombos")]
        public async Task<ActionResult<List<ListadoComboDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoCombos();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListadoComboDTO>> Get(int id)
        {
            var combo = await repositorio.SelectComboById(id);
            if (combo == null) return NotFound();
            return Ok(combo);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CrearComboDTO dto)
        {
            await repositorio.InsertCombo(dto);
            return Ok("Combo creado correctamente");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CrearComboDTO dto)
        {
            await repositorio.UpdateCombo(id, dto);
            return Ok("Combo actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await repositorio.DeleteCombo(id);
            return Ok("Combo eliminado correctamente");
        }
    }
}
