using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public class ContactoEmergencia
    {
        private string contactoEmergenciaCodigo;
        private string contactoEmergenciaNombre;
        private string contactoEmergenciaTelefono;
        private string contactoEmergenciaRelacion;

        private Paciente paciente;

        public string ContactoEmergenciaCodigo { get => contactoEmergenciaCodigo; set => contactoEmergenciaCodigo = value; }
        public string ContactoEmergenciaNombre { get => contactoEmergenciaNombre; set => contactoEmergenciaNombre = value; }
        public string ContactoEmergenciaTelefono { get => contactoEmergenciaTelefono; set => contactoEmergenciaTelefono = value; }
        public string ContactoEmergenciaRelacion { get => contactoEmergenciaRelacion; set => contactoEmergenciaRelacion = value; }
        public Paciente Paciente { get => paciente; set => paciente = value; }

        public bool EsNumeroTelefonoValido()
        {
            if (string.IsNullOrEmpty(ContactoEmergenciaTelefono))
                return false;

            if (!ContactoEmergenciaTelefono.All(char.IsDigit))
                return false;

            if (ContactoEmergenciaTelefono.Length == 9 && ContactoEmergenciaTelefono.StartsWith("9"))
                return true;

            return false; 
        }
    }
}
