using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public class RecetaMedica
    {
        private string recetaCodigo;
        private string recetaDescripcion;
        private string recetaTratamiento;
        private string recetaRecomendaciones;

        private Consulta consulta;

        public string RecetaCodigo { get => recetaCodigo; set => recetaCodigo = value; }
        public string RecetaDescripcion { get => recetaDescripcion; set => recetaDescripcion = value; }
        public string RecetaTratamiento { get => recetaTratamiento; set => recetaTratamiento = value; }
        public string RecetaRecomendaciones { get => recetaRecomendaciones; set => recetaRecomendaciones = value; }
        public Consulta Consulta { get => consulta; set => consulta = value; }
    }
}
