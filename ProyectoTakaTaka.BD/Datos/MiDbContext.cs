using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProyectoTakaTaka.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace ProyectoTakaTaka.BD.Datos
{
    public class MiDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cumpleanero> Cumpleaneros { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Opcional> Opcionales { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Mes> Meses { get; set; }
        public DbSet<EventoOpcional> EventosOpcionales { get; set; }
        public DbSet<Horario> Horarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Aquí puedes configurar tus entidades y relaciones

            var cascadeFKs = modelBuilder.Model
                .G­etEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Casca­de);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restr­ict;
            }
        }

        public MiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected MiDbContext()
        {
        }
    }
}
