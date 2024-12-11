using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public class Paciente
    {

        private string pacienteCodigo;
        private string pacienteDNI;
        private string pacienteNombreCompleto;
        private DateTime pacienteFechaNacimiento;
        private string pacienteDireccion;
        private string pacienteTelefono;
        private string pacienteCorreoElectronico;
        private string pacienteEstado;

        private HistoriaClinica historiaClinica;

        public string PacienteCodigo { get => pacienteCodigo; set => pacienteCodigo = value; }
        public string PacienteDNI { get => pacienteDNI; set => pacienteDNI = value; }
        public string PacienteNombreCompleto { get => pacienteNombreCompleto; set => pacienteNombreCompleto = value; }
        public DateTime PacienteFechaNacimiento { get => pacienteFechaNacimiento; set => pacienteFechaNacimiento = value; }
        public string PacienteDireccion { get => pacienteDireccion; set => pacienteDireccion = value; }
        public string PacienteTelefono { get => pacienteTelefono; set => pacienteTelefono = value; }
        public string PacienteCorreoElectronico { get => pacienteCorreoElectronico; set => pacienteCorreoElectronico = value; }
        public string PacienteEstado { get => pacienteEstado; set => pacienteEstado = value; }
        public HistoriaClinica HistoriaClinica { get => historiaClinica; set => historiaClinica = value; }

        public bool EsValidoElNumeroDelPaciente() 
        {
            if (string.IsNullOrEmpty(PacienteTelefono))
                return false;

            if (!PacienteTelefono.All(char.IsDigit))
                return false;

            if (PacienteTelefono.Length == 9 && PacienteTelefono.StartsWith("9"))
                return true;

            return false;
        }

        public bool EsFechaNacimientoValida()
        {
            // La fecha de nacimiento no puede ser en el futuro
            if (PacienteFechaNacimiento > DateTime.Now)
            {
                return false;
            }
            // El paciente debe tener al menos un día de nacido
            TimeSpan edad = DateTime.Now - PacienteFechaNacimiento;
            if (edad.TotalDays < 1)
            {
                return false;
            }

            return true;
        }

        public bool esPacienteActivo()
        {
            return PacienteEstado == "A";
        }

    }
}

