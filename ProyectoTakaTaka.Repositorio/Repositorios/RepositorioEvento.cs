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
    public class RepositorioEvento : Repositorio<Evento>, IRepositorioEvento
    {
        private readonly MiDbContext context;

        public RepositorioEvento(MiDbContext context) : base(context)
        {
            this.context = context;
        }

        //HInicio
        //HFin

        public async Task<List<ListadoEventoDTO>> SelectListadoEventos()
        {
            var lista = await context.Eventos
                .Include(e => e.Cliente)
                .Include(e => e.Cumpleanero)
                .Include(e => e.Horario)
                .Include(e => e.Combo)
                .Include(e => e.EventoOpcionales)
                .ThenInclude(eo => eo.Opcional)
                .Include(e => e.Pagos)
                .Select(e => new ListadoEventoDTO
                {
                    Id = e.Id,
                    Evento = $"{e.Cumpleanero!.Nombre} - {e.Cliente!.Nombre} {e.Cliente!.Apellido} - {e.Fecha:dd/MM/yyyy} ({e.Horario!.HInicio} a {e.Horario!.HFin})",
                    Cliente = $"{e.Cliente!.Nombre} {e.Cliente!.Apellido}",
                    Cumpleanero = e.Cumpleanero!.Nombre,
                    Edad = e.Cumpleanero.Edad,
                    Telefono = e.Cliente!.Telefono,
                    Combo = e.Combo!.NomCombo,
                    Opcionales = e.EventoOpcionales!.Select(eo => eo.Opcional!.NomOpcional).ToList(),
                    Fecha = e.Fecha,
                    HorarioId = e.HorarioId,
                    HInicio = e.Horario!.HInicio,
                    HFin = e.Horario!.HFin,
                    Tematica = e.Tematica,
                    Pagos = e.Pagos!
                .Select(p => new PagoListadoDTO
                {
                    Id = p.Id,
                    EstadoPago = p.EstadoPago,
                    Monto = p.Monto,
                    Metodo = p.Metodo,
                    FechaPago = p.FechaPago
                })
                .ToList()

                })
                .ToListAsync();
            return lista;
        }

        public async Task<List<EventoCantidadDTO>> SelectCantidadEventos()
        {
            return await context.Eventos
                .Include(e => e.Horario)
                .Select(e => new EventoCantidadDTO
                {
                    Id = e.Id,
                    Fecha = e.Fecha,
                    HInicio = e.Horario!.HInicio,
                    HFin = e.Horario.HFin
                })
                .ToListAsync();
        }

        public async Task<int> InsertarEvento(EventoCrearDTO dto)
        {
            try
            {
                //Convertir la fecha de DateOnly a DateTime
                DateTime fechaEvento = dto.Fecha.ToDateTime(TimeOnly.MinValue);

                //Obtener mes y año
                int mesNumero = fechaEvento.Month;
                int añoEvento = fechaEvento.Year;

                //Convertir número a nombre en español (ej: "marzo")
                string nombreMes = new DateTime(añoEvento, mesNumero, 1)
                    .ToString("MMMM", new System.Globalization.CultureInfo("es-ES"))
                    .ToLower();

                //Buscar si ese mes está habilitado en la tabla Meses
                var mesHabilitado = await context.Meses
                    .FirstOrDefaultAsync(m =>
                        m.MesHabilitado.ToLower() == nombreMes &&
                        m.Año == añoEvento.ToString());

                //Si el mes no existe o está inactivo → no se permite crear evento
                if (mesHabilitado == null)
                    throw new Exception($"El mes '{nombreMes}' del año {añoEvento} no está registrado en el sistema.");

                if (mesHabilitado.EstadoRegistro != ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo)
                    throw new Exception($"El mes '{nombreMes}' del año {añoEvento} está inactivo y no se pueden agendar eventos.");

                //Verificar horario válido y disponible
                var horario = await context.Horarios.FirstOrDefaultAsync(h => h.Id == dto.HorarioId);
                if (horario == null)
                    throw new Exception("El horario seleccionado no existe.");

                if (!horario.Disponible)
                    throw new Exception("El horario seleccionado ya está ocupado.");

                //Crear la entidad Evento
                var entidad = new Evento
                {
                    ClienteId = dto.ClienteId,
                    CumpleaneroId = dto.CumpleaneroId,
                    ComboId = dto.Combo,
                    Fecha = DateOnly.FromDateTime(fechaEvento),
                    Tematica = dto.Tematica,
                    HorarioId = dto.HorarioId
                };

                //Agregar opcionales si existen
                if (dto.OpcionalesId != null && dto.OpcionalesId.Count > 0)
                {
                    entidad.EventoOpcionales = dto.OpcionalesId
                        .Select(id => new EventoOpcional { OpcionalId = id })
                        .ToList();
                }

                //Marcar el horario como no disponible
                horario.Disponible = false;

                //Guardar todo en la base
                await context.Eventos.AddAsync(entidad);
                await context.SaveChangesAsync();

                return entidad.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el evento: {ex.InnerException?.Message ?? ex.Message}");
            }

        }

        public async Task<bool> BorrarEvento(int id)
        {
            var evento = await context.Eventos.Include(e => e.Horario)
                .Include(e => e.EventoOpcionales).FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null) return false;

            // Libera el horario asociado
            if (evento.Horario != null)
                evento.Horario.Disponible = true;

            // Elimina primero los registros dependientes (EventoOpcionales)
            if (evento.EventoOpcionales != null && evento.EventoOpcionales.Any())
                context.EventosOpcionales.RemoveRange(evento.EventoOpcionales);

            // Luego elimina el evento principal
            context.Eventos.Remove(evento);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<int> InsertarEventoCompleto(EventoCrearCompletoDTO dto)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // 1️⃣ Crear Cliente
                var cliente = new Cliente
                {
                    Nombre = dto.ClienteNombre,
                    Apellido = dto.ClienteApellido,
                    Telefono = dto.ClienteTelefono,
                    EstadoRegistro = ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo
                };
                await context.Clientes.AddAsync(cliente);
                await context.SaveChangesAsync();

                // 2️⃣ Crear Cumpleañero vinculado a ese cliente
                var cumple = new Cumpleanero
                {
                    Nombre = dto.CumpleaneroNombre,
                    FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-dto.CumpleaneroEdad)),
                    ClienteId = cliente.Id,
                    EstadoRegistro = ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo
                };
                await context.Cumpleaneros.AddAsync(cumple);
                await context.SaveChangesAsync();

                // 3️⃣ Crear Evento vinculado al cliente y cumpleañero
                var evento = new Evento
                {
                    ClienteId = cliente.Id,
                    CumpleaneroId = cumple.Id,
                    ComboId = dto.ComboId,
                    HorarioId = dto.HorarioId,
                    Fecha = dto.Fecha,
                    Tematica = dto.Tematica,
                    EstadoRegistro = ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo
                };

                // Agregar opcionales (si hay)
                if (dto.OpcionalesId != null && dto.OpcionalesId.Count > 0)
                {
                    evento.EventoOpcionales = dto.OpcionalesId
                        .Select(o => new EventoOpcional { OpcionalId = o })
                        .ToList();
                }

                // Validar horario
                var horario = await context.Horarios.FirstOrDefaultAsync(h => h.Id == dto.HorarioId);
                if (horario == null)
                    throw new Exception("El horario seleccionado no existe.");
                if (!horario.Disponible)
                    throw new Exception("El horario seleccionado ya está ocupado.");

                horario.Disponible = false;

                await context.Eventos.AddAsync(evento);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                return evento.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
