using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.TransferenciaDatos
{
    public class CitaDTO
    {
        //public string CitaCodigo { get; set; }
        public DateTime CitaFechaHora { get; set; }
        public string PacienteCodigo { get; set; }
        public string MedicoCodigo { get; set; }
        public string TipoConsultaCodigo { get; set; }
    }
}
