using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class CodigoSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public CodigoSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }
        public string GenerarCodigoUnico(string prefijo, string tabla, string columnaCodigo)
        {
            string codigoGenerado = null;

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento("spGenerarCodigoUnico");

                // Agregar los parámetros necesarios
                comandoSQL.Parameters.Add(new SqlParameter("@prefijo", SqlDbType.NVarChar, 3)).Value = prefijo;
                comandoSQL.Parameters.Add(new SqlParameter("@tabla", SqlDbType.NVarChar, 128)).Value = tabla;
                comandoSQL.Parameters.Add(new SqlParameter("@columnaCodigo", SqlDbType.NVarChar, 128)).Value = columnaCodigo;

                // Ejecutar el comando y obtener el código generado
                using (SqlDataReader reader = comandoSQL.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        codigoGenerado = reader["CodigoUnico"].ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al generar el código único: " + ex.Message);
            }

            return codigoGenerado;
        }
    }
}