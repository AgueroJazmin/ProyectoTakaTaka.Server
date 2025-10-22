using Microsoft.AspNetCore.Mvc;
using ProyectoTakaTaka.Repositorio.Repositorios;
using ProyectoTakaTaka.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos.Entity;

namespace ProyectoTakaTaka.Server.Controllers
{
    [ApiController]
    [Route("api/Horarios")]
    public class HorarioController : ControllerBase
    {
        private readonly IRepositorioHorario repositorio;

        public HorarioController(IRepositorioHorario repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoHorarios")]
        public async Task<ActionResult<List<HorarioListadoDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoHorarios();
            if (lista.Count == 0)
                return NotFound("No hay horarios registrados.");
            return Ok(lista);
        }

        [HttpGet("HorariosDisponibles")]
        public async Task<ActionResult<List<HorarioListadoDTO>>> GetDisponibles()
        {
            var lista = await repositorio.SelectHorariosDisponibles();
            if (lista.Count == 0)
                return NotFound("No hay horarios disponibles.");
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult> Post(HorarioCrearDTO dto)
        {
            try
            {
                var id = await repositorio.InsertHorario(dto);
                return Ok($"Horario creado correctamente con ID {id}.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, HorarioCrearDTO dto)
        {
            try
            {
                bool actualizado = await repositorio.UpdateHorario(id, dto);
                if (!actualizado)
                    return NotFound($"No se encontró el horario con ID {id}");

                return Ok($"Horario {id} actualizado correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await repositorio.DeleteHorario(id);
            if (!ok) return NotFound($"No se encontró el horario {id}.");
            return Ok("Horario eliminado correctamente.");
        }
    }
}
