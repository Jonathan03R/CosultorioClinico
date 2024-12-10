using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public class Diagnostico
    {
        private string diagnosticoCodigo;
        private string diagnosticoconsultaCodigo;
        private string diagnosticoDescripcion;

        public string DiagnosticoCodigo { get => diagnosticoCodigo; set => diagnosticoCodigo = value; }
        public string DiagnosticoconsultaCodigo { get => diagnosticoconsultaCodigo; set => diagnosticoconsultaCodigo = value; }
        public string DiagnosticoDescripcion { get => diagnosticoDescripcion; set => diagnosticoDescripcion = value; }


    }
}

