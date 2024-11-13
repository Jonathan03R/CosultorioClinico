using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class TipoConsulta
    {
        private string tipoConsultaCodigo, tipoConsultaDescripcion;

  

        public string TipoConsultaCodigo { get => tipoConsultaCodigo; set => tipoConsultaCodigo = value; }
        public string TipoConsultaDescripcion { get => tipoConsultaDescripcion; set => tipoConsultaDescripcion = value; }
    }
}
