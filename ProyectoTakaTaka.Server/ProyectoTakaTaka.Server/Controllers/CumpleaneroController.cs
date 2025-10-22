using Microsoft.AspNetCore.Mvc;
using ProyectoTakaTaka.Repositorio.Repositorios;
using ProyectoTakaTaka.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos.Entity;

namespace ProyectoTakaTaka.Server.Controllers
{
    [ApiController]
    [Route("api/Cumpleaneros")]
    public class CumpleaneroController : ControllerBase
    {
        private readonly IRepositorioCumpleanero repositorio;

        public CumpleaneroController(IRepositorioCumpleanero repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoCumpleaneros")]
        public async Task<ActionResult<List<CumpleaneroListadoDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoCumpleaneros();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CumpleaneroListadoDTO>> Get(int id)
        {
            var cumpleanero = await repositorio.SelectCumpleaneroById(id);
            if (cumpleanero == null) return NotFound();
            return Ok(cumpleanero);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CumpleaneroCrearDTO dto)
        {
            await repositorio.InsertCumpleanero(dto);
            return Ok("Cumpleañero creado correctamente");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CumpleaneroCrearDTO dto)
        {
            await repositorio.UpdateCumpleanero(id, dto);
            return Ok("Cumpleañero actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await repositorio.DeleteCumpleanero(id);
            return Ok("Cumpleañero eliminado correctamente");
        }
    }
}
