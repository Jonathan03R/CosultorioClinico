using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Capa3_Dominio.ModuloPrincipal.Entidades;
using System.Globalization;
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
        private readonly HistoriaClinicaSQL historiaClinicaSQL;
        private readonly DiagnosticoSQL diagnosticoSQL; 
        private readonly RecetasMedicasSQL recetasMedicasSQL;
        private readonly DetallesConsultaSQL detallesConsultaSQL;

        public AtenderConsultaServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            codigoSQL = new CodigoSQL(accesoSQLServer);
            consultaSQL = new ConsultaSQL(accesoSQLServer);
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            medicoSQL = new MedicoSQL(accesoSQLServer);
            citaSQL = new CitaSQL(accesoSQLServer);
            historiaClinicaSQL = new HistoriaClinicaSQL(accesoSQLServer);   
            recetasMedicasSQL = new RecetasMedicasSQL(accesoSQLServer);
            diagnosticoSQL = new DiagnosticoSQL(accesoSQLServer);
            detallesConsultaSQL = new DetallesConsultaSQL(accesoSQLServer);
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
                       Paciente paciente = pacienteSQL.MostrarPacientePorCodigo(consulta.Paciente.PacienteCodigo);
                        consulta.Paciente = paciente;

                        Medico medico = medicoSQL.ObtenerMedicoPorCodigo(consulta.Medico.MedicoCodigo);
                        consulta.Medico = medico;

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
        //Obtener informacion de su historia clinica

        public List<Consulta> historiaClinicaDetalles(string HistorialClinicaCodigo ) 
        {
            accesoSQLServer.AbrirConexion();
            List<Consulta> listaHistorial  = historiaClinicaSQL.MostrasDetallesHistoriaClinica(HistorialClinicaCodigo);

            foreach (var consulta in listaHistorial)
            {
                consulta.Diagnosticos1 = diagnosticoSQL.MostrasDiagnosticosPorConsulta(consulta.ConsultaCodigo);

                consulta.RecetasMedicas1 = recetasMedicasSQL.MostrasRecetasMedicasPorConsulta(consulta.ConsultaCodigo);
            }
            accesoSQLServer.CerrarConexion();
            return listaHistorial;  
        }

        public Paciente DatosPaciente(string PacienteCodigo) {

            accesoSQLServer.AbrirConexion();
            Paciente DataPaciente = pacienteSQL.MostrarPacientePorCodigo(PacienteCodigo);
            accesoSQLServer.CerrarConexion();
            return DataPaciente;
        }

        public List<Consulta> ConsultasPrevias(string PacienteCodigo)
        {
            accesoSQLServer.AbrirConexion();
            List<Consulta> consulta = citaSQL.MostrarCitasPaciente(PacienteCodigo);
            accesoSQLServer.CerrarConexion();

            // Filtrar las citas donde CitaEstado != "P"
            consulta = consulta.Where(c => c.Cita.CitaEstado != "P" && c.Cita.CitaEstado != "T").ToList();
            return consulta;
        }


        //Registrar Detalle Consulta

        public void RegistrarDetallesConsulta(DetallesConsulta detallesConsulta)
        {
            accesoSQLServer.IniciarTransaccion();
            try
            {
                detallesConsulta.DetallesConsultaCodigo1 = codigoSQL.GenerarCodigoUnico("DET", "Gestion.DetallesConsulta", "detallesConsultaCodigo");

                detallesConsultaSQL.RegistrarDetallesConsulta(detallesConsulta);

                accesoSQLServer.TerminarTransaccion(); 
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion(); 
                throw new Exception($"Error al registrar los detalles de la consulta: {ex.Message}", ex);
            }
        }


        public void RegistrarDiagnostico(Diagnostico diagnostico)
        {
            accesoSQLServer.IniciarTransaccion();
            try
            {
                diagnostico.DiagnosticoCodigo = codigoSQL.GenerarCodigoUnico("DIG", "Salud.Diagnostico", "diagnosticoCodigo");

                diagnosticoSQL.CrearDiagnostico(diagnostico);

                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                //throw new Exception($"Error al registrar los detalles de la consulta: {ex.Message}", ex);
                throw ex;
            }
        }

        public void RegistrarRecetasMedicas(RecetaMedica recetaMedicas)
        {
            accesoSQLServer.IniciarTransaccion();
            try
            {
                recetaMedicas.RecetaCodigo = codigoSQL.GenerarCodigoUnico("REC", "Salud.RecetaMedica", "recetaCodigo");

                recetasMedicasSQL.CrearRecetaMedica(recetaMedicas); 

                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                //throw new Exception($"Error al registrar los detalles de la consulta: {ex.Message}", ex);
                throw ex;
            }
        }



        public void ActualizarHoraFinalConsulta(string codigoConsulta)
        {
            accesoSQLServer.AbrirConexion();
            citaSQL.ActualizarFechaFinalCita(codigoConsulta);
            accesoSQLServer.CerrarConexion();
        }


  
        public void cambiarEstadoAtencionEnProceso(string citaCodigo)
        {
             accesoSQLServer.AbrirConexion();
             var todasLasCitas = citaSQL.MostrarCitas();
             var consultaEnAtencion = todasLasCitas.FirstOrDefault(c => c.Cita.CitaEstado == "T");
             if (consultaEnAtencion != null)
             {
                 throw new ArgumentException("Hay una cita que esta siendo atendida.");
             }
             citaSQL.CambiarEstadoAtencionProceso(citaCodigo);
             accesoSQLServer.CerrarConexion();
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

        public void cambiarEstadoActivoConsultaAtendido(string citaCodigo)
        {

            accesoSQLServer.AbrirConexion();
            citaSQL.CambiarEstadoAtendido(citaCodigo);
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
