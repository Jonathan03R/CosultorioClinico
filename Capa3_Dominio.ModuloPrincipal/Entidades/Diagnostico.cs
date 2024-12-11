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
        private string diagnosticoDescripcion;
        private string diagnosticoCie11;

        private Consulta consulta;

        public string DiagnosticoCodigo { get => diagnosticoCodigo; set => diagnosticoCodigo = value; }
        public string DiagnosticoDescripcion { get => diagnosticoDescripcion; set => diagnosticoDescripcion = value; }
        public string DiagnosticoCie11 { get => diagnosticoCie11; set => diagnosticoCie11 = value; }
        public Consulta Consulta { get => consulta; set => consulta = value; }
    }
}

