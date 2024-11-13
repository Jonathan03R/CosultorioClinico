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

        public string PacienteCodigo { get => pacienteCodigo; set => pacienteCodigo = value; }
        public string PacienteDNI { get => pacienteDNI; set => pacienteDNI = value; }
        public string PacienteNombreCompleto { get => pacienteNombreCompleto; set => pacienteNombreCompleto = value; }
        public DateTime PacienteFechaNacimiento { get => pacienteFechaNacimiento; set => pacienteFechaNacimiento = value; }
        public string PacienteDireccion { get => pacienteDireccion; set => pacienteDireccion = value; }
        public string PacienteTelefono { get => pacienteTelefono; set => pacienteTelefono = value; }
        public string PacienteCorreoElectronico { get => pacienteCorreoElectronico; set => pacienteCorreoElectronico = value; }
        public string PacienteEstado { get => pacienteEstado; set => pacienteEstado = value; }

        

        public bool esPacienteActivo()
        {
            return pacienteEstado == "A";
        }

        public bool EsDatosValidos()
        {
            bool esNombreValido = !string.IsNullOrEmpty(pacienteNombreCompleto);

            bool esDniValido = !string.IsNullOrEmpty(pacienteDNI) && pacienteDNI.Length == 8 && pacienteDNI.All(char.IsDigit);
            return esNombreValido && esDniValido;
        }
    }
}

