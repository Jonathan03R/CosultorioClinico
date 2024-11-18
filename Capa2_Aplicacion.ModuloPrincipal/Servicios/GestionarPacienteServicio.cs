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
        private readonly ContactoEmergenciaSQL contactoEmergenciaSQL;

        public GestionarPacienteServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            codigoSQL = new CodigoSQL(accesoSQLServer);
            historiaClinicaSQL = new HistoriaClinicaSQL(accesoSQLServer);
            contactoEmergenciaSQL = new ContactoEmergenciaSQL(accesoSQLServer);
        }

        public void RegistrarPacienteConHistoria(Paciente paciente, List<ContactoEmergencia> contactosEmergencia)
        {

            if (contactosEmergencia == null || !contactosEmergencia.Any())
            {
                throw new ArgumentException("Debe agregar al menos un contacto de emergencia.");
            }

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

                foreach (var contacto in contactosEmergencia)
                {
                    contacto.ContactoEmergenciaCodigo = codigoSQL.GenerarCodigoUnico("CE", "Salud.ContactosEmergencia", "contactoEmergenciaCodigo");
                    contactoEmergenciaSQL.AgregarContactoEmergencia(contacto, paciente.PacienteCodigo);
                }

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


        public void cambiarEstadoInactivoPaciente(Paciente paciente) 
        {
            //if (!paciente.esPacienteActivo()) 
            //{
            //    throw new ArgumentException("Este paciente ya esta incativo");
            //}
            if (paciente.PacienteCodigo == null) 
            {
                throw new ArgumentException("erro al selecionar el paciente");
            }

            accesoSQLServer.IniciarTransaccion();
            try
            {
                pacienteSQL.EliminarPaciente(paciente);
                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;   
            }

        }

        public void cambiarEstadoActivosPaciente(Paciente paciente)
        {
            //if (!paciente.esPacienteActivo()) 
            //{
            //    throw new ArgumentException("Este paciente ya esta incativo");
            //}
            if (paciente.PacienteCodigo == null)
            {
                throw new ArgumentException("erro al selecionar el paciente");
            }

            accesoSQLServer.IniciarTransaccion();
            try
            {
                pacienteSQL.RecuperarPaciente(paciente);
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

        public List<HistoriaClinica> listarPacientes()
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
