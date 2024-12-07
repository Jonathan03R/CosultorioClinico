using Capa3_Dominio.ModuloPrincipal.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public class Medico
    {
        private string medicoCodigo;
        private string medicoApellido;
        private string medicoNombre;
        private string medicoCorreo;
        private string medicoDNI;
        private string medicoTelefono;
        private string medicoEstado;
        private Especialidad especialidad;
        private List<Horario> horarios;

        public string MedicoCodigo { get => medicoCodigo; set => medicoCodigo = value; }
        public string MedicoApellido { get => medicoApellido; set => medicoApellido = value; }
        public string MedicoNombre { get => medicoNombre; set => medicoNombre = value; }
        public string MedicoCorreo { get => medicoCorreo; set => medicoCorreo = value; }
        public string MedicoDNI { get => medicoDNI; set => medicoDNI = value; }
        public string MedicoTelefono { get => medicoTelefono; set => medicoTelefono = value; }
        public string MedicoEstado { get => medicoEstado; set => medicoEstado = value; }
        public Especialidad Especialidad { get => especialidad; set => especialidad = value; }
        public List<Horario> Horarios { get => horarios; set => horarios = value; }

        // Verifica si el médico tiene disponibilidad en la fecha y hora indicadas
        public bool TieneHorarioDisponible(DateTime fechaHora)
        {
            return Horarios.Any(h => h.EstaDisponible(fechaHora));
        }

    }
}
