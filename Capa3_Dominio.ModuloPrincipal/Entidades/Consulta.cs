using Capa3_Dominio.ModuloPrincipal.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public class Consulta
    {
        private string consultaCodigo;
        private DateTime? consultaFechaHoraFinal;
        private string consultaMotivo;

        private Cita cita;

        public string ConsultaCodigo { get => consultaCodigo; set => consultaCodigo = value; }
        public DateTime? ConsultaFechaHoraFinal { get => consultaFechaHoraFinal; set => consultaFechaHoraFinal = value; } // Nullable
        public string ConsultaMotivo { get => consultaMotivo; set => consultaMotivo = value; }
        public Cita Cita { get => cita; set => cita = value; }

        
    }
    
}
