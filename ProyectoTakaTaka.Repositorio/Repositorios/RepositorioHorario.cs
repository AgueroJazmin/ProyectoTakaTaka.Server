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
    public class RepositorioHorario : Repositorio<Horario>, IRepositorioHorario
    {
        private readonly MiDbContext context;

        public RepositorioHorario(MiDbContext context) : base(context)
        {
            this.context = context;
        }


        public async Task<List<HorarioListadoDTO>> SelectListadoHorarios()
        {
            return await context.Horarios
                .Select(h => new HorarioListadoDTO
                {
                    Id = h.Id,
                    HInicio = h.HInicio,
                    HFin = h.HFin,
                    MediaHoraExtra = h.MediaHoraExtra,
                    Disponible = h.Disponible,
                    EstadoRegistro = (int)h.EstadoRegistro
                })

                .ToListAsync();
        }

        public async Task<int> InsertHorario(HorarioCrearDTO dto)
        {
            bool existe = await context.Horarios.AnyAsync(h => h.HInicio == dto.HInicio && h.HFin == dto.HFin);

            if (existe)
                throw new InvalidOperationException("Ya existe un horario con ese rango de horas.");

            var entidad = new Horario
            {
                HInicio = dto.HInicio,
                HFin = dto.HFin,
                MediaHoraExtra = dto.MediaHoraExtra,
                Disponible = dto.Disponible
            };

            await context.Horarios.AddAsync(entidad);
            await context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<List<HorarioListadoDTO>> SelectHorariosDisponibles()
        {
            return await context.Horarios
                .Where(h => h.Disponible)
                .Select(h => new HorarioListadoDTO
                {
                    Id = h.Id,
                    HInicio = h.HInicio,
                    HFin = h.HFin,
                    MediaHoraExtra = h.MediaHoraExtra,
                    Disponible = h.Disponible,
                    EstadoRegistro = (int)h.EstadoRegistro
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateHorario(int id, HorarioCrearDTO dto)
        {
            var horario = await context.Horarios.FirstOrDefaultAsync(h => h.Id == id);
            if (horario == null) return false;

            bool existeDuplicado = await context.Horarios.AnyAsync(h => h.Id != id && h.HInicio == dto.HInicio && h.HFin == dto.HFin);

            if (existeDuplicado)
                throw new InvalidOperationException("Ya existe un horario con ese rango de horas.");


            horario.HInicio = dto.HInicio;
            horario.HFin = dto.HFin;
            horario.MediaHoraExtra = dto.MediaHoraExtra;
            horario.Disponible = dto.Disponible;

            context.Horarios.Update(horario);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHorario(int id)
        {
            var horario = await context.Horarios.FirstOrDefaultAsync(h => h.Id == id);
            if (horario == null) return false;

            context.Horarios.Remove(horario);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
