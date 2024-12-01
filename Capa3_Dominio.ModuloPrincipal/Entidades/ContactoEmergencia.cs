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


        public bool EsNumeroTelefonoValido()
        {
            if (string.IsNullOrEmpty(contactoEmergenciaTelefono))
                return false;

            if (!contactoEmergenciaTelefono.All(char.IsDigit))
                return false;

            if (contactoEmergenciaTelefono.Length == 9 && contactoEmergenciaTelefono.StartsWith("9"))
                return true;

            return false; 
        }
    }
}
