using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    internal class Diagnostico
    {
        private string diagnosticoCodigo;
        private string diagnosticoconsultaCodigo;
        private string diagnosticoDescripcion;
        private DateTime diagnosticoFecha;

        public string DiagnosticoCodigo { get => diagnosticoCodigo; set => diagnosticoCodigo = value; }
        public string DiagnosticoconsultaCodigo { get => diagnosticoconsultaCodigo; set => diagnosticoconsultaCodigo = value; }
        public string DiagnosticoDescripcion { get => diagnosticoDescripcion; set => diagnosticoDescripcion = value; }
        public DateTime DiagnosticoFecha { get => diagnosticoFecha; set => diagnosticoFecha = value; }

        public bool EsDatosValidosDiagnostico()
        {
            bool esDescripcionValida = !string.IsNullOrEmpty(diagnosticoDescripcion);
            bool esFechaValida = diagnosticoFecha != DateTime.MinValue;
            bool contieneCie11Referencia = ContieneReferenciaCie11(); 

            return esDescripcionValida && esFechaValida && contieneCie11Referencia;
        }

        /*Regla 03:*/

        // Método que verifica si la descripción menciona una referencia al CIE-11
        private bool ContieneReferenciaCie11()
        {
            // Usamos directamente el atributo diagnosticoDescripcion
            string patronCie11 = @"CIE-11\s*[:\-]?\s*[A-Z0-9]{3,5}(\.\d{1,2})?";

            return System.Text.RegularExpressions.Regex.IsMatch(diagnosticoDescripcion, patronCie11, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

    }
}

