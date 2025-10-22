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
    public class RepositorioCumpleanero : Repositorio<Cumpleanero>, IRepositorioCumpleanero
    {
        private readonly MiDbContext context;

        public RepositorioCumpleanero(MiDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<CumpleaneroListadoDTO>> SelectListadoCumpleaneros()
        {
            return await context.Cumpleaneros
                .Select(c => new CumpleaneroListadoDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    FechaNacimiento = c.FechaNacimiento,
                    Edad = DateTime.Now.Year - c.FechaNacimiento.Year
                })
                .ToListAsync();
        }

        public async Task<CumpleaneroListadoDTO?> SelectCumpleaneroById(int id)
        {
            return await context.Cumpleaneros
                .Where(c => c.Id == id)
                .Select(c => new CumpleaneroListadoDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    FechaNacimiento = c.FechaNacimiento
                })
                .FirstOrDefaultAsync();
        }

        public async Task InsertCumpleanero(CumpleaneroCrearDTO dto)
        {
            var clienteExiste = await context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            if (!clienteExiste)
            {
                throw new Exception($"No existe un cliente con Id {dto.ClienteId}");
            }

            var cumplanero = new Cumpleanero
            {
                Nombre = dto.Nombre,
                FechaNacimiento = dto.FechaNacimiento,
                ClienteId = dto.ClienteId
            };

            context.Cumpleaneros.Add(cumplanero);
            await context.SaveChangesAsync();
        }



        public async Task UpdateCumpleanero(int id, CumpleaneroCrearDTO dto)
        {
            var entidad = await context.Cumpleaneros.FindAsync(id);
            if (entidad == null) return;

            entidad.Nombre = dto.Nombre;
            entidad.FechaNacimiento = dto.FechaNacimiento;

            await context.SaveChangesAsync();
        }

        public async Task DeleteCumpleanero(int id)
        {
            var entidad = await context.Cumpleaneros.FindAsync(id);
            if (entidad == null) return;

            context.Cumpleaneros.Remove(entidad);
            await context.SaveChangesAsync();
        }
    }
}
