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
    public class RepositorioCliente : Repositorio<Cliente>, IRepositorioCliente
    {
        private readonly MiDbContext context;

        public RepositorioCliente(MiDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<ClienteListadoDTO>> SelectListadoClientes()
        {
            return await context.Clientes
                .Select(c => new ClienteListadoDTO
                {
                    Id = c.Id,
                    NombreCompleto = $"{c.Nombre} - {c.Apellido}",
                    Telefono = $"{c.Telefono}"
                })
                .ToListAsync();
        }
        public async Task<ClienteListadoDTO?> SelectClienteById(int id)
        {
            return await context.Clientes
                .Where(c => c.Id == id)
                .Select(c => new ClienteListadoDTO
                {
                    Id = c.Id,
                    NombreCompleto = $"{c.Nombre} - {c.Apellido}",
                    Telefono = $"{c.Telefono}"
        })
                .FirstOrDefaultAsync();
        }

        public async Task InsertCliente(ClienteCrearDTO dto)
        {
            var entidad = new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Telefono = dto.Telefono
            };

            context.Clientes.Add(entidad);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCliente(int id, ClienteCrearDTO dto)
        {
            var entidad = await context.Clientes.FindAsync(id);
            if (entidad == null) return;

            entidad.Nombre = dto.Nombre;
            entidad.Apellido = dto.Apellido;
            entidad.Telefono = dto.Telefono;

            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCliente(int id)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var cliente = await context.Clientes
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                    return false;

                //Obtener cumpleañeros del cliente
                var cumpleaneros = await context.Cumpleaneros
                    .Where(c => c.ClienteId == id)
                    .ToListAsync();

                foreach (var cumple in cumpleaneros)
                {
                    //Eventos del cumpleañero
                    var eventos = await context.Eventos
                        .Include(e => e.EventoOpcionales)
                        .Include(e => e.Pagos)
                        .Where(e => e.CumpleaneroId == cumple.Id)
                        .ToListAsync();

                    foreach (var ev in eventos)
                    {
                        // eliminar pagos
                        if (ev.Pagos != null && ev.Pagos.Any())
                            context.Pagos.RemoveRange(ev.Pagos);

                        // eliminar evento-opcionales
                        if (ev.EventoOpcionales != null && ev.EventoOpcionales.Any())
                            context.EventosOpcionales.RemoveRange(ev.EventoOpcionales);

                        // liberar horario si viene cargado
                        if (ev.HorarioId != 0)
                        {
                            var h = await context.Horarios.FindAsync(ev.HorarioId);
                            if (h != null) h.Disponible = true;
                        }

                        // eliminar evento
                        context.Eventos.Remove(ev);
                    }

                    // eliminar el cumpleañero
                    context.Cumpleaneros.Remove(cumple);
                }

                // finalmente eliminar el cliente
                context.Clientes.Remove(cliente);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
