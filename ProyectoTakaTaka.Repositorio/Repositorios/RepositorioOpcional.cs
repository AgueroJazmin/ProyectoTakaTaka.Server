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
    public class RepositorioOpcional : Repositorio<Opcional>, IRepositorioOpcional
    {
        private readonly MiDbContext context;

        public RepositorioOpcional(MiDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<ListadoOpcionalDTO>> SelectListadoOpcionales()
        {
            return await context.Opcionales
                .Select(o => new ListadoOpcionalDTO
                {
                    Id = o.Id,
                    NomOpcional = o.NomOpcional,
                    Precio = o.Precio
                })
                .ToListAsync();
        }

        public async Task<ListadoOpcionalDTO?> SelectOpcionalById(int id)
        {
            return await context.Opcionales
                .Where(o => o.Id == id)
                .Select(o => new ListadoOpcionalDTO
                {
                    Id = o.Id,
                    NomOpcional = o.NomOpcional,
                    Precio = o.Precio
                })
                .FirstOrDefaultAsync();
        }

        public async Task InsertOpcional(CrearOpcionalDTO dto)
        {
            var entidad = new Opcional
            {
                NomOpcional = dto.NomOpcional,
                Precio = dto.Precio
            };

            context.Opcionales.Add(entidad);
            await context.SaveChangesAsync();
        }

        public async Task UpdateOpcional(int id, CrearOpcionalDTO dto)
        {
            var entidad = await context.Opcionales.FindAsync(id);
            if (entidad == null) return;

            entidad.NomOpcional = dto.NomOpcional;
            entidad.Precio = dto.Precio;

            await context.SaveChangesAsync();
        }

        public async Task DeleteOpcional(int id)
        {
            var entidad = await context.Opcionales.FindAsync(id);
            if (entidad == null) return;

            context.Opcionales.Remove(entidad);
            await context.SaveChangesAsync();
        }
    }
}
