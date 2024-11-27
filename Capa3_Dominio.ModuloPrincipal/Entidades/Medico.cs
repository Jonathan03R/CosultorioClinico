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
        private List<Horario> medicoHorarios;

        public string MedicoCodigo { get => medicoCodigo; set => medicoCodigo = value; }
        public string MedicoApellido { get => medicoApellido; set => medicoApellido = value; }
        public string MedicoNombre { get => medicoNombre; set => medicoNombre = value; }
        public string MedicoCorreo { get => medicoCorreo; set => medicoCorreo = value; }
        public string MedicoDNI { get => medicoDNI; set => medicoDNI = value; }
        public string MedicoTelefono { get => medicoTelefono; set => medicoTelefono = value; }
        public string MedicoEstado { get => medicoEstado; set => medicoEstado = value; }
        public Especialidad Especialidad { get => especialidad; set => especialidad = value; }
        public List<Horario> MedicoHorarios { get => medicoHorarios; set => medicoHorarios = value; }

        public bool TieneHorarioDisponible(DateTime fechaHora)
        {
            foreach (var horario in MedicoHorarios)
            {
                if (horario.EsDisponible(fechaHora))
                {
                    return true;
                }
            }
            return false;
        }

        
        public bool EsEspecialistaCompatible(Especialidad especialidad)
        {
            return Especialidad == especialidad;
        }
    }
}
