using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.TransferenciaDatos
{
    public class DiagnosticoDTO
    {
        private string diagnosticoconsultaCodigo;
        private string diagnosticoDescripcion;
        private string diagnosticoCie11;

        public string DiagnosticoconsultaCodigo { get => diagnosticoconsultaCodigo; set => diagnosticoconsultaCodigo = value; }
        public string DiagnosticoDescripcion { get => diagnosticoDescripcion; set => diagnosticoDescripcion = value; }
        public string DiagnosticoCie11 { get => diagnosticoCie11; set => diagnosticoCie11 = value; }
    }
}
