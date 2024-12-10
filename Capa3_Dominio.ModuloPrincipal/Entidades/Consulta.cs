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
        private Medico medico;
        private Paciente paciente;
        private TipoConsulta tipoConsulta;
        private HistoriaClinica historiaClinica;

        private List<Diagnostico> Diagnosticos;
        private List<RecetaMedica> RecetasMedicas;
        public string ConsultaCodigo { get => consultaCodigo; set => consultaCodigo = value; }
        public DateTime? ConsultaFechaHoraFinal { get => consultaFechaHoraFinal; set => consultaFechaHoraFinal = value; }
        public string ConsultaMotivo { get => consultaMotivo; set => consultaMotivo = value; }
        public Cita Cita { get => cita; set => cita = value; }
        public Medico Medico { get => medico; set => medico = value; }
        public Paciente Paciente { get => paciente; set => paciente = value; }
        public TipoConsulta TipoConsulta { get => tipoConsulta; set => tipoConsulta = value; }
        public List<Diagnostico> Diagnosticos1 { get => Diagnosticos; set => Diagnosticos = value; }
        public List<RecetaMedica> RecetasMedicas1 { get => RecetasMedicas; set => RecetasMedicas = value; }
        public HistoriaClinica HistoriaClinica { get => historiaClinica; set => historiaClinica = value; }
    }
    
}
