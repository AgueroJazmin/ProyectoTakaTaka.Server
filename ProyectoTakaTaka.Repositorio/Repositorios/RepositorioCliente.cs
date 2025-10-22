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

        public async Task DeleteCliente(int id)
        {
            var entidad = await context.Clientes.FindAsync(id);
            if (entidad == null) return;

            context.Clientes.Remove(entidad);
            await context.SaveChangesAsync();
        }
    }
}
