using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Cita
    {
        private string citaCodigo;
        private string citaEstado;
        private DateTime citaFechaHora;
        private Notificacion citaNotificacion;
        private Paciente citaPaciente;
        private Medico citaMedico;
        private TipoConsulta citaTipoConsulta;



        public string CitaCodigo { get => citaCodigo; set => citaCodigo = value; }
        public string CitaEstado { get => citaEstado; set => citaEstado = value; }
        public DateTime CitaFechaHora { get => citaFechaHora; set => citaFechaHora = value; }
        public Paciente CitaPaciente { get => citaPaciente; set => citaPaciente = value; }
        public Medico CitaMedico { get => citaMedico; set => citaMedico = value; }
        public TipoConsulta CitaTipoConsulta { get => citaTipoConsulta; set => citaTipoConsulta = value; }
        public Notificacion CitaNotificacion { get => citaNotificacion; set => citaNotificacion = value; }

        //comprobar si la cita es valida del dia de hoy
        public bool EsCitaPasada() {
            return citaFechaHora.Date < DateTime.Today && (citaEstado == "P" || citaEstado == "N");
        }

    }
}



