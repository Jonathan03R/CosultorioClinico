using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.TransferenciaDatos
{
    public class RecetaMedicaDTO
    { 
        public string recetaDescripcion { get; set; }
        public string recetaTratamiento { get; set; }
        public string recetaRecomendaciones { get; set; }
        public string codigoConsulta { get; set; }
    }
}
