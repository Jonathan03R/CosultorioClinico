using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Cita
    {
        private string citaCodigo, citaEstado;
        private DateTime citaFechaHora;
        private Notificacion citaNotificacion;
        private Paciente citaPaciente;
        private Medico citaMedico;
        private TipoConsulta citaTipoConsulta;

        public Cita(string citaCodigo, string citaEstado, DateTime citaFechaHora)
        {
            this.citaCodigo = citaCodigo;
            this.citaEstado = citaEstado;
            this.citaFechaHora = citaFechaHora;
        }

        public string CitaCodigo { get => citaCodigo; set => citaCodigo = value; }
        public string CitaEstado { get => citaEstado; set => citaEstado = value; }
        public DateTime CitaFechaHora { get => citaFechaHora; set => citaFechaHora = value; }
        public Paciente CitaPaciente { get => citaPaciente; set => citaPaciente = value; }
        public Medico CitaMedico { get => citaMedico; set => citaMedico = value; }
        public TipoConsulta CitaTipoConsulta { get => citaTipoConsulta; set => citaTipoConsulta = value; }
        public Notificacion CitaNotificacion { get => citaNotificacion; set => citaNotificacion = value; }

        // Este método sirve para verificar si todos los datos necesarios y medico disponible
        public bool EsValida()
        {
            return CitaPaciente != null && CitaMedico != null && CitaTipoConsulta != null &&
                   CitaMedico.TieneHorarioDisponible(CitaFechaHora);
        }


        //Este método verifica si es posible modificar la cita a una nueva fecha y hora,
        //basándose en la disponibilidad del médico.
        public bool PermiteModificacion(DateTime nuevaFechaHora)
        {
            return CitaMedico.TieneHorarioDisponible(nuevaFechaHora);
        }

        // Este método verifica si la cita se puede cancelar (por ejemplo,
        // las citas urgentes no deberían cancelarse).
        public bool EsCancelacionValida()
        {
            return CitaEstado != "Urgente";
        }


        // Este metodo envía una confirmación de la cita al paciente.
        public void EnviarConfirmacion()
        {
            var notificacion = new Notificacion
            {
                NotificacionFechaDeEnvio = DateTime.Now,
                NotificacionCodigo = Guid.NewGuid().ToString(),
                NotificacionDestinatario = CitaPaciente.PacienteCorreoElectronico,
                NotificacionMensaje = $"Su cita ha sido programada para {CitaFechaHora}."
            };
            notificacion.Enviar();
        }

        // Este Método estático sirve para filtrar citas según criterios específicos.
        public static List<Cita> FiltrarCitas(List<Cita> citas, Medico medico = null, Especialidad especialidad = null, DateTime? fecha = null, string estado = null, bool? urgencia = null)
        {
            return citas.Where(c => (medico == null || c.CitaMedico == medico) &&
                                    (especialidad == null || c.CitaMedico.Especialidad == especialidad) &&
                                    (fecha == null || c.CitaFechaHora.Date == fecha.Value.Date) &&
                                    (estado == null || c.CitaEstado == estado) &&
                                    (!urgencia.HasValue || (urgencia.Value && c.CitaEstado == "Urgente") ||
                                                          (!urgencia.Value && c.CitaEstado != "Urgente"))
                            ).ToList();
        }

        // Este metodo Reasigna la cita a un médico sustituto de la misma especialidad y con disponibilidad.
        public bool ReasignarCita(Medico sustituto)
        {
            if (sustituto.Especialidad == CitaMedico.Especialidad && sustituto.TieneHorarioDisponible(CitaFechaHora))
            {
                CitaMedico = sustituto;
                return true;
            }
            return false;
        }
    }
}



