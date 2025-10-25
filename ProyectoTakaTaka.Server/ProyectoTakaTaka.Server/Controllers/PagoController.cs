using Microsoft.AspNetCore.Mvc;
using ProyectoTakaTaka.Repositorio.Repositorios;
using ProyectoTakaTaka.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos.Entity;

namespace ProyectoTakaTaka.Server.Controllers
{
    [ApiController]
    [Route("api/Pagos")]
    public class PagoController : ControllerBase
    {
        private readonly IRepositorioPago repositorio;

        public PagoController(IRepositorioPago repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoPagos")]
        public async Task<ActionResult<List<PagoListadoDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoPagos();
            return Ok(lista);
        }

        [HttpPost("CrearPago")]
        public async Task<ActionResult<string>> Post(CrearPagoDTO dto)
        {
            await repositorio.InsertPago(dto);
            return Ok(new { mensaje = "Pago registrado correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CrearPagoDTO dto)
        {
            await repositorio.UpdatePago(id, dto);
            return Ok("Pago actualizado correctamente.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await repositorio.DeletePago(id);
            return Ok("Pago eliminado correctamente.");
        }
    }
}
