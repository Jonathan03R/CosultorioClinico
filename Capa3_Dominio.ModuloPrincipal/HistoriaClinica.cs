using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public  class HistoriaClinica
    {

        private string historialClinicoCodigo;
        private string antecedentesMedicos;
        private string alergias;
        private DateTime fechaCreacion;
        private DateTime fechaActualizacion;
        private Paciente paciente;

        public string HistorialClinicoCodigo { get => historialClinicoCodigo; set => historialClinicoCodigo = value; }
        public string AntecedentesMedicos { get => antecedentesMedicos; set => antecedentesMedicos = value; }
        public string Alergias { get => alergias; set => alergias = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public DateTime FechaActualizacion { get => fechaActualizacion; set => fechaActualizacion = value; }
        public Paciente Paciente { get => paciente; set => paciente = value; }


        // Método para verificar si ya existe un paciente asignado
        public bool EstaAsignadaAPaciente()
        {
            return paciente != null;
        }

        // Método para verificar si el paciente ya tiene una historia clínica
        public bool EsPacienteConHistoriaClinica(Paciente paciente)
        {
            return paciente != null && paciente.PacienteCodigo == this.paciente?.PacienteCodigo;
        }

        // Método para asignar un paciente a la historia clínica, con validación
        public void AsignarPaciente(Paciente nuevoPaciente)
        {
            if (nuevoPaciente == null)
            {
                throw new ArgumentNullException(nameof(nuevoPaciente), "El paciente no puede ser nulo.");
            }

            if (EstaAsignadaAPaciente())
            {
                throw new InvalidOperationException("Esta historia clínica ya está asignada a otro paciente.");
            }

            paciente = nuevoPaciente;
        }
    }
}
