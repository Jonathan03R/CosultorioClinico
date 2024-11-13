using System;
using System.Collections.Generic;
using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System.Linq;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicio
{
    public class GestionarPacienteServicio
    {
        private readonly AccesoSQLServer accesoSQLServer;
        private readonly PacienteSQL pacienteSQL;
        private readonly CodigoSQL codigoSQL;
        private readonly HistoriaClinicaSQL historiaClinicaSQL;

        public GestionarPacienteServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            codigoSQL = new CodigoSQL(accesoSQLServer);
            historiaClinicaSQL = new HistoriaClinicaSQL(accesoSQLServer);
        }

        public void RegistrarPacienteConHistoria(Paciente paciente)
        {
            accesoSQLServer.IniciarTransaccion();
            paciente.PacienteCodigo = codigoSQL.GenerarCodigoUnico("PA", "Salud.Pacientes", "pacienteCodigo");
            try
            {
                pacienteSQL.CrearPaciente(paciente);
                
                HistoriaClinica nuevaHistoriaClinica = new HistoriaClinica
                {
                    HistorialClinicoCodigo = codigoSQL.GenerarCodigoUnico("HC", "Salud.HistoriaClinica", "historialClinicoCodigo"),
                    Paciente = paciente,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    AntecedentesMedicos = null,
                    Alergias = null
                };

                historiaClinicaSQL.AgregarHistoriaClinica(nuevaHistoriaClinica);
                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }

        public void ActualizarPaciente(Paciente paciente)
        {
            // Validación inicial del paciente
            if (paciente == null)
            {
                throw new ArgumentNullException(nameof(paciente), "Llene todo los campos.");
            }

            if (!paciente.EsDatosValidos())
            {
                throw new ArgumentException("Los datos del paciente son incompletos o inválidos.");
            }

            accesoSQLServer.IniciarTransaccion();
            try
            {
                pacienteSQL.ActualizarPaciente(paciente);
                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }


        public List<Paciente> ObtenerHistorialPacientes()
        {
            List<Paciente> result = new List<Paciente>();

            try
            {
                accesoSQLServer.IniciarTransaccion();

                result = pacienteSQL.BuscarPaciente(null, null, null);

                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }

            List<Paciente> pacientesActivos = result.Where(p => p.esPacienteActivo()).ToList();
            return pacientesActivos;
        }

        


        //public List<Paciente> listarPacientesActivos()
        //{
        //    try
        //    {
        //        accesoSQLServer.IniciarTransaccion();

        //        List<Paciente> listaPacientes = pacienteSQL.ListarPacientes();
        //        List<Paciente> listaPacientesActivos = listaPacientes.Where(p => p.esPacienteActivo()).ToList();

        //        accesoSQLServer.TerminarTransaccion();

        //        return listaPacientesActivos;
        //    }
        //    catch (Exception ex)
        //    {
        //        accesoSQLServer.CancelarTransaccion();
        //        throw ex;
        //    }
        //}

        public List<HistoriaClinica> listarPacientesActivos()
        {
            try
            {
                accesoSQLServer.IniciarTransaccion();

                List<HistoriaClinica> listaPacientes = pacienteSQL.ListarPacientes();
               

                accesoSQLServer.TerminarTransaccion();

                return listaPacientes;
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }
    }
}
