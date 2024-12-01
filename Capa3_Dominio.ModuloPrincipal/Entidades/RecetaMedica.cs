using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    internal class RecetaMedica
    {
        private string recetaCodigo;
        private string recetaConsultaCodigo;
        private string recetaDescripcion;
        private DateTime recetaFecha;
        private string recetaTratamiento;
        private string recetaRecomendaciones;

        public string RecetaCodigo { get => recetaCodigo; set => recetaCodigo = value; }
        public string RecetaConsultaCodigo { get => recetaConsultaCodigo; set => recetaConsultaCodigo = value; }
        public string RecetaDescripcion { get => recetaDescripcion; set => recetaDescripcion = value; }
        public DateTime RecetaFecha { get => recetaFecha; set => recetaFecha = value; }
        public string RecetaTratamiento { get => recetaTratamiento; set => recetaTratamiento = value; }
        public string RecetaRecomendaciones { get => recetaRecomendaciones; set => recetaRecomendaciones = value; }

        public bool EsDatosValidosReceta()
        {
            bool esDescripcionValida = !string.IsNullOrEmpty(recetaDescripcion);
            bool esFechaValida = RecetaFecha != DateTime.MinValue;
            bool esTratamientoValido = !string.IsNullOrEmpty(recetaTratamiento);
            bool esRecomendacionesValidas = !string.IsNullOrEmpty(recetaRecomendaciones);

            return esDescripcionValida && esTratamientoValido && esRecomendacionesValidas;
        }

        /*Regla 6:*/
        public bool ValidarEmisionRecetaMedica()
        {
            // Validar si la receta corresponde a la consulta realizada
            if (RecetaCodigo == RecetaConsultaCodigo)
            {
                return true;
            }
            return false;
        }


    }
}
