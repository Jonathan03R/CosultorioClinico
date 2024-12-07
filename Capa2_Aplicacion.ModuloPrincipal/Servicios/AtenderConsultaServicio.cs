using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Capa2_Aplicacion.ModuloPrincipal.Servicios
{
    public class AtenderConsultaServicio
    {

        private readonly AccesoSQLServer accesoSQLServer;
        private readonly CodigoSQL codigoSQL;
        private readonly ConsultaSQL consultaSQL;
        private readonly PacienteSQL pacienteSQL;
        private readonly MedicoSQL medicoSQL;
        private readonly CitaSQL citaSQL;


        public AtenderConsultaServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            codigoSQL = new CodigoSQL(accesoSQLServer);
            consultaSQL = new ConsultaSQL(accesoSQLServer);
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            medicoSQL = new MedicoSQL(accesoSQLServer);
            citaSQL = new CitaSQL(accesoSQLServer);
        }


        //Listar las consultas de los  pacientes de la fecha de hoy
        public List<Consulta> MostrarConsultasDelDia()
        {
            List<Consulta> consultasDeHoy = new List<Consulta>();
            try
            {
                accesoSQLServer.IniciarTransaccion();
                List<Consulta> consultas = consultaSQL.ListarConsultas();

                foreach (var consulta in consultas) 
                {
                    if (consulta.Cita.EsCitaPasada())
                    {
                        citaSQL.CambiarEstadoNoAsistieron(consulta.Cita.CitaCodigo);
                    }
                }

                consultas = consultaSQL.ListarConsultas();

                foreach (var consulta in consultas)
                {
                    //if (consulta.Cita.CitaFechaHora.Date == DateTime.Today) 
                    //{
                       Paciente paciente = pacienteSQL.MostrarPacientePorCodigo(consulta.Cita.CitaPaciente.PacienteCodigo);
                        consulta.Cita.CitaPaciente = paciente;

                        Medico medico = medicoSQL.ObtenerMedicoPorCodigo(consulta.Cita.CitaMedico.MedicoCodigo);
                        consulta.Cita.CitaMedico = medico;

                        consultasDeHoy.Add(consulta); 
                    //}
                }

                accesoSQLServer.TerminarTransaccion(); 
                return consultasDeHoy; 
            
            }
            catch (Exception ex) {
                throw ex;
            }
          
            
        }


        public void cambiarEstadoConsultaPendientree(string codigoConsulta)
        {
            accesoSQLServer.AbrirConexion();
            citaSQL.CambiarEstadoPendiente(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }



        //Cambiar estado Pendiente, No Asistieron, Atendido, Cancelado
        public void cambiarEstadoActivoConsultaNoAsistieron(string codigoConsulta)
        {

            accesoSQLServer.AbrirConexion();
            citaSQL.CambiarEstadoNoAsistieron(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }

        public void cambiarEstadoActivoConsultaAtendido(string codigoConsulta)
        {

            accesoSQLServer.AbrirConexion();
            citaSQL.CambiarEstadoAtendido(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }
        public void cambiarEstadoActivoConsultaCancelada(string codigoConsulta)
        {

            accesoSQLServer.AbrirConexion();
            citaSQL.CambiarEstadoCancelado(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }

        //Registrar Consulta

        //Registrar Diagnostico

        //Listar Diagnostico

        //Listar Recetas  medicas anteriores del paciente

        //Registrar Receta Medica
    }
}
