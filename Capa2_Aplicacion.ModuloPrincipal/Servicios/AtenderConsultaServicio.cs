using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicios
{
    public class AtenderConsultaServicio
    {

        private readonly AccesoSQLServer accesoSQLServer;
        private readonly CodigoSQL codigoSQL;
        private readonly ConsultaSQL consultaSQL;
        private readonly PacienteSQL pacienteSQL;
        private readonly MedicoSQL medicoSQL;


        public AtenderConsultaServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            codigoSQL = new CodigoSQL(accesoSQLServer);
            consultaSQL = new ConsultaSQL(accesoSQLServer);
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            medicoSQL = new MedicoSQL(accesoSQLServer);
        }


        //Listar las consultas de los  pacientes de la fecha de hoy
        public List<Consulta> MostrarConsultasDelDia()
        {
            List<Consulta> consultasDeHoy = new List<Consulta>();

          
                accesoSQLServer.AbrirConexion(); 
                List<Consulta> consultas = consultaSQL.ListarConsultas();

                foreach (var consulta in consultas)
                {
                    if (consulta.Cita.CitaFechaHora == DateTime.Today) 
                    {
                        Paciente paciente = pacienteSQL.MostrarPacientePorCodigo(consulta.ConsultaPacienteCodigo);
                        consulta.Paciente = paciente;

                        Medico medico = medicoSQL.ObtenerMedicoPorCodigo(consulta.ConsultaMedicoCodigo);
                        consulta.Medicos = medico;

                        consultasDeHoy.Add(consulta); 
                    }
                }

                accesoSQLServer.CerrarConexion(); 
            
            
            return consultasDeHoy; 
        }


        public void cambiarEstadoConsultaPendientree(string codigoConsulta)
        {
            accesoSQLServer.AbrirConexion();
            consultaSQL.CambiarEstadoPendiente(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }



        //Cambiar estado Pendiente, No Asistieron, Atendido, Cancelado
        public void cambiarEstadoActivoConsultaNoAsistieron(string codigoConsulta)
        {
           
            accesoSQLServer.AbrirConexion();
            consultaSQL.CambiarEstadoNoAsistieron(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }

        public void cambiarEstadoActivoConsultaAtendido(string codigoConsulta)
        {

            accesoSQLServer.AbrirConexion();
            consultaSQL.CambiarEstadoAtendido(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }
        public void cambiarEstadoActivoConsultaCancelada(string codigoConsulta)
        {

            accesoSQLServer.AbrirConexion();
            consultaSQL.CambiarEstadoCancelado(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }

        //Registrar Consulta

        //Registrar Diagnostico

        //Listar Diagnostico

        //Listar Recetas  medicas anteriores del paciente

        //Registrar Receta Medica
    }
}
