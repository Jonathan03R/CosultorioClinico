using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.TransferenciaDatos
{
    public class HorarioConCita
    {
        public string MedicoCodigo { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string ConsultaCodigo { get; set; }
        public string CitaCodigo { get; set; }
        public string PacienteCodigo { get; set; }
        public string CitaEstado { get; set; }

        public string MedicoNombre { get; set; } // Añadido para el nombre del médico
        public string PacienteNombre { get; set; }
    }
}
