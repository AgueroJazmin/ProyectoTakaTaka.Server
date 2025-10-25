using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos;
using ProyectoTakaTaka.BD.Datos.Entity;
using ProyectoTakaTaka.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public class RepositorioPago : Repositorio<Pago>, IRepositorioPago
    {
        private readonly MiDbContext context;

        public RepositorioPago(MiDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<PagoListadoDTO>> SelectListadoPagos()
        {
            return await context.Pagos
                .Include(p => p.Evento)
                .ThenInclude(p => p.Cumpleanero)
                .Include(p => p.Evento!.Cliente)
                .Select(p => new PagoListadoDTO
                {
                    Id = p.Id,
                    Monto = p.Monto,
                    Metodo = p.Metodo,
                    EstadoPago = p.EstadoPago,
                    FechaPago = p.FechaPago,
                    EventoId = p.EventoId,
                    Evento = $"{p.Evento!.Cumpleanero!.Nombre} - {p.Evento!.Cliente!.Nombre} {p.Evento!.Cliente!.Apellido}"
                })
                .ToListAsync();
        }

        public async Task InsertPago(CrearPagoDTO dto)
        {
            var pago = new Pago
            {
                EventoId = dto.EventoId,
                Monto = dto.Monto,
                Metodo = dto.Metodo,
                EstadoPago = dto.EstadoPago,
                FechaPago = dto.FechaPago
            };

            context.Pagos.Add(pago);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePago(int id, CrearPagoDTO dto)
        {
            var entidad = await context.Pagos.FindAsync(id);
            if (entidad == null) return;

            entidad.Monto = dto.Monto;
            entidad.Metodo = dto.Metodo;
            entidad.EstadoPago = dto.EstadoPago;
            entidad.FechaPago = dto.FechaPago;

            await context.SaveChangesAsync();
        }

        public async Task DeletePago(int id)
        {
            var entidad = await context.Pagos.FindAsync(id);
            if (entidad == null) return;

            context.Pagos.Remove(entidad);
            await context.SaveChangesAsync();
        }
    }
}
