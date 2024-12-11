using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.Entidades
{
    public class DetallesConsulta
    {
        private string DetallesConsultaCodigo;
        private string DetallesConsultaHistoriaEnfermedad;
        private string DetallesConsultaRevisiones;
        private string DetallesConsultaEvaluacionPsico;
        private string DetallesConsultaMotivoConsulta;

        private Consulta consulta;

        public string DetallesConsultaCodigo1 { get => DetallesConsultaCodigo; set => DetallesConsultaCodigo = value; }
        public string DetallesConsultaHistoriaEnfermedad1 { get => DetallesConsultaHistoriaEnfermedad; set => DetallesConsultaHistoriaEnfermedad = value; }
        public string DetallesConsultaRevisiones1 { get => DetallesConsultaRevisiones; set => DetallesConsultaRevisiones = value; }
        public string DetallesConsultaEvaluacionPsico1 { get => DetallesConsultaEvaluacionPsico; set => DetallesConsultaEvaluacionPsico = value; }
        public string DetallesConsultaMotivoConsulta1 { get => DetallesConsultaMotivoConsulta; set => DetallesConsultaMotivoConsulta = value; }
        public Consulta Consulta { get => consulta; set => consulta = value; }
    }
}
