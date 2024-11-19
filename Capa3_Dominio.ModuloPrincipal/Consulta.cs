using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    internal class Consulta
    {
        private string consultaCodigo;
        private DateTime consultaFechaHora;
        private string consultaMedicoCodigo;
        private string consultaPacienteCodigo;
        private string consultaMotivo;
        private string consultaEstado;

        public string ConsultaCodigo { get => consultaCodigo; set => consultaCodigo = value; }
        public DateTime ConsultaFechaHora { get => consultaFechaHora; set => consultaFechaHora = value; }
        public string ConsultaMedicoCodigo { get => consultaMedicoCodigo; set => consultaMedicoCodigo = value; }
        public string ConsultaPacienteCodigo { get => consultaPacienteCodigo; set => consultaPacienteCodigo = value; }
        public string ConsultaMotivo { get => consultaMotivo; set => consultaMotivo = value; }
        public string ConsultaEstado { get => consultaEstado; set => consultaEstado = value; }
    }
}
