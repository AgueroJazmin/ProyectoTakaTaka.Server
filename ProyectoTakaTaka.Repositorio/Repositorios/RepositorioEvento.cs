using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos;
using ProyectoTakaTaka.BD.Datos.Entity;
using ProyectoTakaTaka.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
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
         .ToListAsync();

            // 🔹 Generar DTO ajustando los 30 minutos extra si corresponde
            var resultado = lista.Select(e =>
            {
                // detectar si tiene el opcional extra
                bool tieneExtra = e.EventoOpcionales!
                    .Any(o => o.Opcional!.NomOpcional == "30 Minutos Adicionales");

                var hInicio = e.Horario!.HInicio;
                var hFin = tieneExtra ? e.Horario.HFin.AddMinutes(30) : e.Horario.HFin;

                return new ListadoEventoDTO
                {
                    Id = e.Id,
                    Evento = $"{e.Cumpleanero!.Nombre} - {e.Cliente!.Nombre} {e.Cliente!.Apellido} - {e.Fecha:dd/MM/yyyy} ({hInicio} a {hFin})",
                    Cliente = $"{e.Cliente!.Nombre} {e.Cliente!.Apellido}",
                    Cumpleanero = e.Cumpleanero!.Nombre,
                    Edad = e.Cumpleanero.Edad,
                    Telefono = e.Cliente!.Telefono,
                    Combo = e.Combo!.NomCombo,
                    Opcionales = e.EventoOpcionales!.Select(eo => eo.Opcional!.NomOpcional).ToList(),
                    Fecha = e.Fecha,
                    HorarioId = e.HorarioId,
                    HInicio = hInicio,
                    HFin = hFin,
                    Tematica = e.Tematica,
                    Pagos = e.Pagos!.Select(p => new PagoListadoDTO
                    {
                        Id = p.Id,
                        EstadoPago = p.EstadoPago,
                        Monto = p.Monto,
                        Metodo = p.Metodo,
                        FechaPago = p.FechaPago
                    }).ToList()
                };
            }).ToList();

            return resultado;
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

                //Validar que la fecha no sea pasada
                if (fechaEvento < DateTime.Today)
                    throw new Exception("No se pueden agendar eventos en fechas pasadas.");

                //Obtener mes y año
                int mesNumero = fechaEvento.Month;
                int añoEvento = fechaEvento.Year;

                //Convertir número a nombre en español (ej: "marzo")
                string nombreMes = new DateTime(añoEvento, mesNumero, 1)
                    .ToString("MMMM", new CultureInfo("es-ES"))
                    .ToLower();

                //Buscar si ese mes está habilitado en la tabla Meses
                var mesHabilitado = await context.Meses
                    .FirstOrDefaultAsync(m =>
                        m.MesHabilitado.ToLower() == nombreMes &&
                        m.Año == añoEvento.ToString());

                //Si el mes no existe o está inactivo no se permite crear evento
                if (mesHabilitado == null)
                    throw new Exception($"El mes '{nombreMes}' del año {añoEvento} no está registrado en el sistema.");

                if (mesHabilitado.EstadoRegistro != ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo)
                    throw new Exception($"El mes '{nombreMes}' del año {añoEvento} está inactivo y no se pueden agendar eventos.");

                //Verificar horario válido y disponible
                var horario = await context.Horarios.FirstOrDefaultAsync(h => h.Id == dto.HorarioId);
                if (horario == null)
                    throw new Exception("El horario seleccionado no existe.");

                // Verificar si ya hay un evento con la misma fecha y horario
                bool existeEventoMismoHorario = await context.Eventos
                    .AnyAsync(e => e.Fecha == dto.Fecha && e.HorarioId == dto.HorarioId);

                if (existeEventoMismoHorario)
                    throw new Exception("El horario seleccionado ya está ocupado para esa fecha.");
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
            var evento = await context.Eventos
                .Include(e => e.Horario)
                .Include(e => e.EventoOpcionales)
                .Include(e => e.Pagos)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null)
                return false;

            //Libera el horario antes de eliminar nada
            if (evento.Horario != null)
            {
                evento.Horario.Disponible = true;
                context.Horarios.Update(evento.Horario);
            }

            // eliminar pagos
            if (evento.Pagos != null && evento.Pagos.Any())
                context.Pagos.RemoveRange(evento.Pagos);

            //eliminar evento-opcionales
            if (evento.EventoOpcionales != null && evento.EventoOpcionales.Any())
                context.EventosOpcionales.RemoveRange(evento.EventoOpcionales);

            //elimina el evento
            context.Eventos.Remove(evento);

            //guarda todo de una vez
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<int> InsertarEventoCompleto(EventoCrearCompletoDTO dto)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                //Validar que el mes esté activo
                DateTime fechaEvento = dto.Fecha.ToDateTime(TimeOnly.MinValue);
                int mesNumero = fechaEvento.Month;
                int añoEvento = fechaEvento.Year;

                string nombreMes = new DateTime(añoEvento, mesNumero, 1)
                    .ToString("MMMM", new System.Globalization.CultureInfo("es-ES"))
                    .ToLower();

                var mesHabilitado = await context.Meses
                    .FirstOrDefaultAsync(m =>
                        m.MesHabilitado.ToLower() == nombreMes &&
                        m.Año == añoEvento.ToString());

                if (mesHabilitado == null)
                    throw new Exception($"El mes '{nombreMes}' del año {añoEvento} no está registrado en el sistema.");

                if (mesHabilitado.EstadoRegistro != ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo)
                    throw new Exception($"El mes '{nombreMes}' del año {añoEvento} está inactivo y no se pueden agendar eventos.");


                //Crear Cliente
                var cliente = new Cliente
                {
                    Nombre = dto.ClienteNombre,
                    Apellido = dto.ClienteApellido,
                    Telefono = dto.ClienteTelefono,
                    EstadoRegistro = ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo
                };
                await context.Clientes.AddAsync(cliente);
                await context.SaveChangesAsync();

                //Crear Cumpleañero vinculado a ese cliente
                var cumple = new Cumpleanero
                {
                    Nombre = dto.CumpleaneroNombre,
                    FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-dto.CumpleaneroEdad)),
                    ClienteId = cliente.Id,
                    EstadoRegistro = ProyectoTakaTaka.Shared.ENUM.EnumEstadoRegistro.activo
                };
                await context.Cumpleaneros.AddAsync(cumple);
                await context.SaveChangesAsync();

                //Validar horario existente
                var horario = await context.Horarios.FirstOrDefaultAsync(h => h.Id == dto.HorarioId);
                if (horario == null)
                    throw new Exception("El horario seleccionado no existe.");

                //Verificar si ya hay un evento en esa fecha y horario
                bool existeEventoMismoHorario = await context.Eventos
                    .AnyAsync(e => e.Fecha == dto.Fecha && e.HorarioId == dto.HorarioId);

                if (existeEventoMismoHorario)
                    throw new Exception("El horario seleccionado ya está ocupado para esa fecha.");

                //Crear Evento 
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

                // Agregar opcionales 
                if (dto.OpcionalesId != null && dto.OpcionalesId.Count > 0)
                {
                    evento.EventoOpcionales = dto.OpcionalesId
                        .Select(o => new EventoOpcional { OpcionalId = o })
                        .ToList();
                }

                await context.Eventos.AddAsync(evento);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                return evento.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear el evento: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<List<HorarioDiaDTO>> SelectHorariosPorFecha(DateOnly fecha)
        {
            try
            {
                var horarios = await context.Horarios
                    .OrderBy(h => h.HInicio)
                    .ToListAsync();

                var eventosDelDia = await context.Eventos
                    .Include(e => e.Horario)
                    .Include(e => e.EventoOpcionales)
                        .ThenInclude(eo => eo.Opcional)
                    .Where(e => e.Fecha == fecha)
                    .ToListAsync();

                var lista = horarios.Select(h => new HorarioDiaDTO
                {
                    HorarioId = h.Id,
                    HInicio = h.HInicio,
                    HFin = h.HFin,
                    Disponible = true,
                    Nota = null
                }).ToList();

                foreach (var ev in eventosDelDia)
                {
                    if (ev.Horario == null) continue;

                    var slot = lista.FirstOrDefault(s => s.HorarioId == ev.HorarioId);
                    if (slot == null) continue; //evita el error si no encuentra coincidencia

                    slot.Disponible = false;

                    bool tieneExtra = ev.EventoOpcionales.Any(o =>
                        o.Opcional != null && o.Opcional.NomOpcional == "30 Minutos Adicionales");

                    if (tieneExtra)
                    {
                        slot.HFin = slot.HFin.AddMinutes(30);
                        slot.Nota = "30 min extra";

                        var siguiente = lista
                            .Where(s => s.HInicio > slot.HInicio)
                            .OrderBy(s => s.HInicio)
                            .FirstOrDefault();

                        if (siguiente != null)
                        {
                            siguiente.HInicio = siguiente.HInicio.AddMinutes(30);
                            siguiente.HFin = siguiente.HFin.AddMinutes(30);
                        }
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en SelectHorariosPorFecha: {ex.Message}");
            }
        }
        public async Task<List<EventoCantidadDTO>> SelectEventosPorMes(int mes, int año)
        {
            //trae eventos del mes y añadimos HFin extendido si tiene el opcional
            var lista = await context.Eventos
                .Include(e => e.Horario)
                .Include(e => e.EventoOpcionales)
                    .ThenInclude(eo => eo.Opcional)
                .Where(e => e.Fecha.Month == mes && e.Fecha.Year == año)
                .ToListAsync();

            var resultado = new List<EventoCantidadDTO>();

            foreach (var e in lista)
            {
                // Extender horario si tiene "30 Minutos Adicionales"
                var fin = e.Horario!.HFin;
                bool tieneExtra = e.EventoOpcionales!
                    .Any(o => o.Opcional!.NomOpcional == "30 Minutos Adicionales");

                if (tieneExtra)
                    fin = fin.AddMinutes(30); //TimeOnly.AddMinutes

                resultado.Add(new EventoCantidadDTO
                {
                    Id = e.Id,
                    Fecha = e.Fecha,
                    HInicio = e.Horario.HInicio,
                    HFin = fin
                });
            }

            return resultado;
        }
    }
}
