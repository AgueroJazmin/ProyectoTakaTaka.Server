using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos;
using ProyectoTakaTaka.BD.Datos.Entity;
using ProyectoTakaTaka.Shared.DTO;

namespace ProyectoTakaTaka.Server.Controllers
{
    [ApiController]
    [Route("api/Mes")]
    public class MesController : ControllerBase
    {
        private readonly MiDbContext context;

        public MesController(MiDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<MesListadoDTO>>> GetMes()
        {
            var meses = await context.Meses
          .Select(m => new MesListadoDTO
          {
             Id = m.Id,
             MesHabilitado = m.MesHabilitado,
             Año = m.Año,
             EstadoRegistro = (int)m.EstadoRegistro
          })
             .ToListAsync();

            if (meses == null || meses.Count == 0)
                return NotFound("Meses no disponibles.");

            return Ok(meses);
        }

        [HttpGet("Activos")]
        public async Task<ActionResult<List<MesListadoDTO>>> GetMesesActivos()
        {
            var meses = await context.Meses
                .Where(m => m.EstadoRegistro == ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo)
                .Select(m => new MesListadoDTO
                {
                    Id = m.Id,
                    MesHabilitado = m.MesHabilitado,
                    Año = m.Año,
                    EstadoRegistro = (int)m.EstadoRegistro
                })
                .ToListAsync();

            if (meses == null || meses.Count == 0)
                return NotFound("No hay meses activos.");

            return Ok(meses);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(MesCrearDTO DTO)
        {
            var mes = new Mes
            {
                MesHabilitado = DTO.MesHabilitado,
                Año = DTO.Año,
                EstadoRegistro = (Shared.ENUM.EnumEstadoRegistro)DTO.EstadoRegistro
            };

            await context.Meses.AddAsync(mes);
            await context.SaveChangesAsync();
            return Ok(mes.Id);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var mes = await context.Meses.FirstOrDefaultAsync(x => x.Id == id);
            if (mes == null)
            {
                return NotFound($"No se encontró el mes {id} o ya fue eliminado");
            }
            context.Meses.Remove(mes);
            await context.SaveChangesAsync();
            return Ok($"El Mes {id} se eliminó correctamente");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, MesCrearDTO DTO)
        {
            if (id != DTO.Id)
                return BadRequest("Datos no válidos");

            var mes = await context.Meses.FirstOrDefaultAsync(x => x.Id == id);
            if (mes == null)
                return NotFound($"No se encontró el Mes {id}");

            mes.MesHabilitado = DTO.MesHabilitado;
            mes.Año = DTO.Año;
            mes.EstadoRegistro = (Shared.ENUM.EnumEstadoRegistro)DTO.EstadoRegistro;

            context.Meses.Update(mes);
            await context.SaveChangesAsync();

            return Ok($"El Mes {id} se actualizó correctamente");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Mes>> GetMesId(int id)
        {
            var meses = await context.Meses.FirstOrDefaultAsync(x => x.Id == id);
            if (meses == null)
            {
                return NotFound("Mes no disponible.");
            }
            return Ok(meses);
        }
    }
}
