using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Horario
    {
        private string horarioCodigo, horarioDia;
        private bool horarioDisponibilidad;
        private TimeSpan horarioHoraInicio, horarioHoraFin;

        public Horario(string horarioCodigo, string horarioDia, bool horarioDisponibilidad, TimeSpan horarioHoraInicio, TimeSpan horarioHoraFin)
        {
            this.horarioCodigo = horarioCodigo;
            this.horarioDia = horarioDia;
            this.horarioDisponibilidad = horarioDisponibilidad;
            this.horarioHoraInicio = horarioHoraInicio;
            this.horarioHoraFin = horarioHoraFin;
        }

        public string HorarioCodigo { get => horarioCodigo; set => horarioCodigo = value; }
        public string HorarioDia { get => horarioDia; set => horarioDia = value; }
        public bool HorarioDisponibilidad { get => horarioDisponibilidad; set => horarioDisponibilidad = value; }
        public TimeSpan HorarioHoraInicio { get => horarioHoraInicio; set => horarioHoraInicio = value; }
        public TimeSpan HorarioHoraFin { get => horarioHoraFin; set => horarioHoraFin = value; }


        
        public bool EsDisponible(DateTime fechaHora)
        {
            var diaSemana = fechaHora.ToString("dddd");
            var hora = fechaHora.TimeOfDay;

            return HorarioDisponibilidad && HorarioDia.Equals(diaSemana, StringComparison.OrdinalIgnoreCase) &&
                   hora >= HorarioHoraInicio && hora <= HorarioHoraFin;
        }
    }

}

