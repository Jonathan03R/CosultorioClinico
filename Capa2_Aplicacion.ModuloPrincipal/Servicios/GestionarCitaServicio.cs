using System;
using System.Collections.Generic;
using System.Linq;
using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using static Capa4_Persistencia.SqlServer.ModuloPrincipal.HorariosSQL;
using Capa3_Dominio.ModuloPrincipal.TransferenciaDatos;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicio
{
    public class GestionarCitaServicio
    {
        private readonly AccesoSQLServer accesoSQLServer;
        private readonly CitaSQL citaSQL;
        private readonly EspecialidadSQL especialidadSQL;
        private readonly PacienteSQL pacienteSQL;
        private readonly MedicoSQL medicoSQL;
        private readonly CodigoSQL codigoSQL;
        private readonly ConsultaSQL consultaSQL;
        private readonly HorariosSQL horariosSQL;

        public GestionarCitaServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            citaSQL = new CitaSQL(accesoSQLServer);
            especialidadSQL = new EspecialidadSQL(accesoSQLServer);
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            medicoSQL = new MedicoSQL(accesoSQLServer);
            codigoSQL = new CodigoSQL(accesoSQLServer);
            consultaSQL = new ConsultaSQL(accesoSQLServer);
            horariosSQL = new HorariosSQL(accesoSQLServer);
        }

        // Mostrar horarios con citas para una especialidad y fecha específica
        public List<HorarioConCita> MostrarHorariosConCitas(string especialidadCodigo, DateTime fecha)
        {
            try
            {
                accesoSQLServer.AbrirConexion();
                List<HorarioConCita> horariosConCitas = horariosSQL.ListarHorariosConCitas(especialidadCodigo, fecha);
                accesoSQLServer.CerrarConexion();
                return horariosConCitas;
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw new Exception($"Error al mostrar horarios con citas: {ex.Message}", ex);
            }
        }

        public List<Medico> ObtenerMedicosConEspecialidad()
        {
            try
            {
                accesoSQLServer.IniciarTransaccion();
                List<Medico> listaMedicos = medicoSQL.MostrarMedicosConEspecialidad();
                accesoSQLServer.TerminarTransaccion();

                return listaMedicos;
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }


        public void RegistrarCita(Consulta consulta)
        {
            accesoSQLServer.IniciarTransaccion();
            try
            {
                // Generar códigos únicos para la cita y la consulta
                consulta.ConsultaCodigo = codigoSQL.GenerarCodigoUnico("CON", "Gestion.Consulta", "consultaCodigo");
                consulta.ConsultaFechaHoraFinal = null;
                consulta.Cita.CitaCodigo = codigoSQL.GenerarCodigoUnico("CIT", "Gestion.cita", "citaCodigo");
                
               
                citaSQL.CrearCita(consulta.Cita);
                consultaSQL.CrearConsulta(consulta);

                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw new Exception($"Error al registrar la cita y la consulta: {ex.Message}", ex);
            }
        }


        public void ActualizarCita(Cita cita)
        {

            accesoSQLServer.IniciarTransaccion();
            try
            {
                citaSQL.CrearCita(cita);
                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }

        public Consulta ObtenerCitaPorId(string citaCodigo)
        {

            var citas = citaSQL.MostrarCitasPaciente(citaCodigo);
            return citas.FirstOrDefault(c => c.Cita.CitaCodigo == citaCodigo);
        }

        public List<Consulta> ObtenerCitasPorPaciente(string pacienteCodigo)
        {
            return citaSQL.MostrarCitasPaciente(pacienteCodigo);
        }

        public void cambiarEstadoActivoConsultaCancelada(string condigoCita)
        {

            accesoSQLServer.AbrirConexion();
            citaSQL.CambiarEstadoCancelado(condigoCita);
            accesoSQLServer.CerrarConexion();
        }


        public List<Especialidad> ListarEspecialidades()
        {
            try
            {
                accesoSQLServer.IniciarTransaccion();

                List<Especialidad> listaEspecialidad = especialidadSQL.Pro_Listar_Especialidad();


                accesoSQLServer.TerminarTransaccion();

                return listaEspecialidad;
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }

        }

        public List<TipoConsulta> TipoConsulta()
        {
            try
            {
                accesoSQLServer.IniciarTransaccion();
                List<TipoConsulta> tipoConsultas = citaSQL.ListarTiposDeConsulta();
                accesoSQLServer.TerminarTransaccion();
                return tipoConsultas;
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }

        public Paciente obtenerPacientePorDni(Paciente paciente)
        {
            accesoSQLServer.AbrirConexion();
            Paciente pacienteResult = pacienteSQL.MostarPacienteDni(paciente.PacienteDNI);
            accesoSQLServer.CerrarConexion();
            return pacienteResult;
        }

        //Segun las reglas solo debo obtener las citas de dia de hoy como indico el profesor ,
        //no nos sirve de nada conocer las citas de ayer o de mañana, nos interesa saber pero este metodo retorna todas las citas 
        public List<Consulta> ObtenerTodasCitas()
        {
            try
            {
                accesoSQLServer.IniciarTransaccion();
                List<Consulta> todasLasCitas = citaSQL.MostrarCitas();
                accesoSQLServer.TerminarTransaccion();
                return todasLasCitas;   
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }


        xd en desarrollo aun


    }
}

