using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Horario
    {
        private string horarioCodigo;
        private DiaSemana horarioDia;
        private TimeSpan horarioHoraInicio, horarioHoraFin;
        private Medico medico;

        public string HorarioCodigo { get => horarioCodigo; set => horarioCodigo = value; }
        public DiaSemana HorarioDia { get => horarioDia; set => horarioDia = value; }
        public TimeSpan HorarioHoraInicio { get => horarioHoraInicio; set => horarioHoraInicio = value; }
        public TimeSpan HorarioHoraFin { get => horarioHoraFin; set => horarioHoraFin = value; }
        public Medico Medico { get => medico; set => medico = value; }

        public bool EstaDisponible(DateTime fechaHora)
        {
            DayOfWeek diaDeSemana = (DayOfWeek)(int)horarioDia;  // Mapear DiaSemana a DayOfWeek

            return fechaHora.DayOfWeek == diaDeSemana &&
                   fechaHora.TimeOfDay >= horarioHoraInicio &&
                   fechaHora.TimeOfDay <= horarioHoraFin;
        }
    }

    // Enum para representar los días de la semana
    public enum DiaSemana
    {
        Lunes = 0,
        Martes = 1,
        Miércoles = 2,
        Jueves = 3,
        Viernes = 4,
        Sábado = 5,
        Domingo = 6
    }

}

