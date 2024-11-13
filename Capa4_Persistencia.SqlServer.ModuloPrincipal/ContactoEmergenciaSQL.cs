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
                    ContactoEmergencia contacto = ObtenerContactoEmergencia(resultadoSQL);
                    listaContactos.Add(contacto);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionContactoEmergenciaInvalido(ExcepcionContactoEmergenciaInvalido.ERROR_DE_CONSULTA);
            }
            return listaContactos;
        }

        private ContactoEmergencia ObtenerContactoEmergencia(SqlDataReader resultadoSQL)
        {
            ContactoEmergencia contactoEmergencia = new ContactoEmergencia
            {
                ContactoEmergenciaCodigo = resultadoSQL.GetString(0),
                ContactoEmergenciaNombre = resultadoSQL.GetString(1),
                ContactoEmergenciaRelacion = resultadoSQL.GetString(2),
                ContactoEmergenciaTelefono = resultadoSQL.GetString(3)
            };
            return contactoEmergencia;
        }
    }
}
