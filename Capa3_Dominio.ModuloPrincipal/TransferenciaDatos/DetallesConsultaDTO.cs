using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.TransferenciaDatos
{
    public class DetallesConsultaDTO
    {
        public string DetallesConsultaMotivoConsulta { get; set; }
        public string DetallesConsultaHistoriaEnfermedad { get; set; }
        public string DetallesConsultaRevisiones { get; set; }
        public string DetallesConsultaEvaluacionPsico { get; set; }
        public string CodigoConsulta { get; set; }
    }
}
