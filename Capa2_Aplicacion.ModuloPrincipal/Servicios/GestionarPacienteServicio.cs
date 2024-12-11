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

            if (!paciente.EsValidoElNumeroDelPaciente())
            {
                throw new ArgumentException($"El número del paciente {paciente.PacienteTelefono} no es válido.");
            }
            if (!paciente.EsFechaNacimientoValida()) 
            {
                throw new ArgumentException($"Ingrese su fecha de nacimiento correcta");
            }

            foreach (var contacto in contactosEmergencia)
            {
                if (!contacto.EsNumeroTelefonoValido())
                {
                    throw new ArgumentException($"El número de teléfono {contacto.ContactoEmergenciaTelefono} no es válido.");
                }
            }

            accesoSQLServer.IniciarTransaccion();
            try
            {
                HistoriaClinica nuevaHistoriaClinica = new HistoriaClinica
                {
                    HistorialClinicoCodigo = codigoSQL.GenerarCodigoUnico("HIS", "Salud.HistoriaClinica", "historialClinicoCodigo"),
                };

                historiaClinicaSQL.AgregarHistoriaClinica(nuevaHistoriaClinica);

                paciente.HistoriaClinica = nuevaHistoriaClinica;
                paciente.PacienteCodigo = codigoSQL.GenerarCodigoUnico("PAC", "Salud.Pacientes", "pacienteCodigo");
                pacienteSQL.CrearPaciente(paciente);

                foreach (var contacto in contactosEmergencia)
                {
                    contacto.ContactoEmergenciaCodigo = codigoSQL.GenerarCodigoUnico("CEM", "Salud.ContactosEmergencia", "contactoEmergenciaCodigo");
                    contacto.Paciente = paciente;
                    contactoEmergenciaSQL.AgregarContactoEmergencia(contacto);
                }

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

            if (!paciente.EsValidoElNumeroDelPaciente())
            {
                throw new ArgumentException($"El número de teléfono {paciente.PacienteTelefono} no es válido.");
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

        public void ActualizarContactoEmergencia(ContactoEmergencia contactoEmergencia)
        {
            if (string.IsNullOrEmpty(contactoEmergencia.ContactoEmergenciaCodigo))
            {
                throw new ArgumentException("El código del contacto de emergencia no puede estar vacío.");
            }

            if (!contactoEmergencia.EsNumeroTelefonoValido())
            {
                throw new ArgumentException($"El número de teléfono {contactoEmergencia.ContactoEmergenciaTelefono} no es válido.");
            }

            accesoSQLServer.IniciarTransaccion();
            try
            {
                contactoEmergenciaSQL.ActualizarContactoEmergencia(contactoEmergencia);
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
            if (paciente.PacienteCodigo == null) 
            {
                throw new ArgumentException("erro al selecionar el paciente");
            }
            accesoSQLServer.AbrirConexion();
            pacienteSQL.EliminarPaciente(paciente);
            accesoSQLServer.CerrarConexion();
        }

        public void cambiarEstadoActivosPaciente(Paciente paciente)
        {
            if (paciente.PacienteCodigo == null)
            {
                throw new ArgumentException("erro al selecionar el paciente");
            }
            accesoSQLServer.AbrirConexion();
            pacienteSQL.RecuperarPaciente(paciente);
            accesoSQLServer.CerrarConexion();
        }

        public List<Paciente> listarPacientes()
        {
            accesoSQLServer.AbrirConexion();
            List<Paciente> listaPacientes = pacienteSQL.ListarPacientes();
            accesoSQLServer.CerrarConexion();
            return listaPacientes;
        }


        public List<ContactoEmergencia> ListarContactoDeEmergencias(string pacienteCodigo)
        {
            accesoSQLServer.AbrirConexion();
            List<ContactoEmergencia> contactosResult = contactoEmergenciaSQL.MostrarContactosPorPaciente(pacienteCodigo);
            accesoSQLServer.CerrarConexion();
            return contactosResult;
        }
    }
}
