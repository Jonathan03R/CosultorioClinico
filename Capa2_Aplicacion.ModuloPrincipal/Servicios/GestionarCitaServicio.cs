using System;
using System.Collections.Generic;
using System.Linq;
using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa3_Dominio.ModuloPrincipal.Entidad;

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

        public GestionarCitaServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            citaSQL = new CitaSQL(accesoSQLServer);
            especialidadSQL = new EspecialidadSQL(accesoSQLServer);
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            medicoSQL = new MedicoSQL(accesoSQLServer);
            codigoSQL = new CodigoSQL(accesoSQLServer);
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


        public void RegistrarCita(Cita cita)
        {
            accesoSQLServer.IniciarTransaccion();
            try
            {
                cita.CitaCodigo = codigoSQL.GenerarCodigoUnico("CA", "Gestion.cita", "citaCodigo");
                citaSQL.CrearCita(cita);
                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }

        public void ActualizarCita(Cita cita)
        {


            if (!cita.EsValida())
            {
                throw new Exception("Los datos no son validos");
            }

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

        public Cita ObtenerCitaPorId(string citaCodigo)
        {

            var citas = citaSQL.MostrarCitasPaciente(citaCodigo);
            return citas.FirstOrDefault(c => c.CitaCodigo == citaCodigo);
        }

        public List<Cita> ObtenerCitasPorPaciente(string pacienteCodigo)
        {
            return citaSQL.MostrarCitasPaciente(pacienteCodigo);
        }

        public void CancelarCita(string citaCodigo)
        {

            var cita = ObtenerCitaPorId(citaCodigo);
            if (cita == null)
            {
                throw new ArgumentException("La cita no existe");
            }
            if (cita.EsCancelacionValida())
            {
                throw new ArgumentException("La cita es urgente");
            }


            cita.CitaEstado = "Cancelada";

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
        public List<Cita> ObtenerTodasCitas()
        {
            try
            {
                accesoSQLServer.IniciarTransaccion();
                List<Cita> todasLasCitas = citaSQL.MostrarCitas();
                accesoSQLServer.TerminarTransaccion();
                return todasLasCitas;   
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }


    }
}

