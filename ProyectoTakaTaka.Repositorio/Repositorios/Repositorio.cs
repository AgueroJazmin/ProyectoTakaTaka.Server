using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProyectoTakaTaka.Repositorio.Repositorios
{
    public class Repositorio<E> : IRepositorio<E> where E : class, IEntityBase
    {
        private readonly MiDbContext context;

        public Repositorio(MiDbContext context)
        {
            this.context = context;
        }

        public async Task<List<E>> Select()
        {
            var eventos = await context.Eventos.ToListAsync();
            return await context.Set<E>().ToListAsync();
        }

        public async Task<E?> SelectById(int id)
        {
            return await context.Set<E>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Insert(E entidad)
        {
            try
            {
                await context.Set<E>().AddAsync(entidad);
                await context.SaveChangesAsync();
                return entidad.Id;
            }
            catch (Exception errorr)
            {
                throw errorr;
            }
        }

        public async Task<bool> Existe(int id)
        {
            bool existe = await context.Set<E>().AnyAsync(x => x.Id == id);
            return existe;
        }

        public async Task<bool> Update(int id, E entidad)
        {
            if (id != entidad.Id)
            {
                return false;
            }
            var existe = await Existe(id);

            if (!existe)
            {
                return false;
            }

            try
            {
                context.Set<E>().Update(entidad);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception errorr)
            {
                throw errorr;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entidad = await SelectById(id);
            if (entidad is null) return false;
            try
            {
                context.Set<E>().Remove(entidad);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception err) { throw err; }
        }
    }
}
