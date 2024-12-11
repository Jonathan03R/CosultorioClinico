 using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class ContactoEmergenciaSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public ContactoEmergenciaSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }
        public List<ContactoEmergencia> MostrarContactosPorPaciente(string pacienteCodigo)
        {
            List<ContactoEmergencia> listaContactos = new List<ContactoEmergencia>();
            string procedimientoSQL = "pro_Mostrar_ContactosEmergencia";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", pacienteCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {

                    ContactoEmergencia contacto = new ContactoEmergencia
                    {
                        ContactoEmergenciaCodigo = resultadoSQL.GetString(0), // contactoEmergenciaCodigo
                        ContactoEmergenciaNombre = resultadoSQL.GetString(1), // contactoEmergenciaNombre
                        ContactoEmergenciaRelacion = resultadoSQL.GetString(2), // contactoEmergenciaRelacion
                        ContactoEmergenciaTelefono = resultadoSQL.GetString(3), // contactoEmergenciaTelefono
                    };

                    listaContactos.Add(contacto);
                }
                resultadoSQL.Close();
            }
            catch (SqlException)
            {
                throw new ExcepcionContactoEmergenciaInvalido(ExcepcionContactoEmergenciaInvalido.ERROR_DE_CONSULTA);
            }
            return listaContactos;
        }

        public void AgregarContactoEmergencia(ContactoEmergencia contactoEmergencia)
        {
            string procedimientoSQL = "pro_Agregar_ContactoEmergencia";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);

                comandoSQL.Parameters.Add(new SqlParameter("@contactoEmergenciaCodigo", contactoEmergencia.ContactoEmergenciaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@contactoEmergenciaNombre", contactoEmergencia.ContactoEmergenciaNombre));
                comandoSQL.Parameters.Add(new SqlParameter("@contactoEmergenciaRelacion", contactoEmergencia.ContactoEmergenciaRelacion));
                comandoSQL.Parameters.Add(new SqlParameter("@contactoEmergenciaTelefono", contactoEmergencia.ContactoEmergenciaTelefono));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", contactoEmergencia.Paciente.PacienteCodigo));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                //throw new ExcepcionContactoEmergenciaInvalido(ExcepcionContactoEmergenciaInvalido.ERROR_DE_CONSULTA);

                throw ex;
            }
            
        }


        public void ActualizarContactoEmergencia(ContactoEmergencia contactoEmergencia)
        {
            string procedimientoSQL = "pro_Actualizar_ContactoEmergencia";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);

                comandoSQL.Parameters.Add(new SqlParameter("@contactoEmergenciaCodigo", contactoEmergencia.ContactoEmergenciaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@contactoEmergenciaNombre", contactoEmergencia.ContactoEmergenciaNombre));
                comandoSQL.Parameters.Add(new SqlParameter("@contactoEmergenciaRelacion", contactoEmergencia.ContactoEmergenciaRelacion));
                comandoSQL.Parameters.Add(new SqlParameter("@contactoEmergenciaTelefono", contactoEmergencia.ContactoEmergenciaTelefono));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar el contacto de emergencia.", ex);
            }
        }

    }


}
