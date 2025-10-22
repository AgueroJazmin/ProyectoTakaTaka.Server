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
    public class RepositorioCombo : Repositorio<Combo>, IRepositorioCombo
    {
        private readonly MiDbContext context;

        public RepositorioCombo(MiDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<ListadoComboDTO>> SelectListadoCombos()
        {
            return await context.Combos
                .Select(c => new ListadoComboDTO
                {
                    Id = c.Id,
                    NomCombo = c.NomCombo,
                    Precio = c.Precio
                })
                .ToListAsync();
        }

        public async Task<ListadoComboDTO?> SelectComboById(int id)
        {
            return await context.Combos
                .Where(c => c.Id == id)
                .Select(c => new ListadoComboDTO
                {
                    Id = c.Id,
                    NomCombo = c.NomCombo,
                    Precio = c.Precio
                })
                .FirstOrDefaultAsync();
        }

        public async Task InsertCombo(CrearComboDTO dto)
        {
            var entidad = new Combo
            {
                NomCombo = dto.NomCombo,
                Precio = dto.Precio
            };

            context.Combos.Add(entidad);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCombo(int id, CrearComboDTO dto)
        {
            var entidad = await context.Combos.FindAsync(id);
            if (entidad == null) return;

            entidad.NomCombo = dto.NomCombo;
            entidad.Precio = dto.Precio;

            await context.SaveChangesAsync();
        }

        public async Task DeleteCombo(int id)
        {
            var entidad = await context.Combos.FindAsync(id);
            if (entidad == null) return;

            context.Combos.Remove(entidad);
            await context.SaveChangesAsync();
        }
    }
}
