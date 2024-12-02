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
    public class HistoriaClinicaSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public HistoriaClinicaSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }


        // Método para agregar una historia clínica a la base de datos
        public void AgregarHistoriaClinica(HistoriaClinica historiaClinica)
        {
            string procedimientoSQL = "pro_Agregar_HistoriaClinica";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@historialClinicoCodigo", historiaClinica.HistorialClinicoCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@antecedentesMedicos", (object)historiaClinica.AntecedentesMedicos ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@alergias", (object)historiaClinica.Alergias ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@fechaCreacion", historiaClinica.FechaCreacion));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public HistoriaClinica MostrarHistoriaClinica(string pacienteCodigo)
        {
            HistoriaClinica historiaClinica = null;
            string procedimientoSQL = "pro_Mostrar_HistoriaClinica";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", pacienteCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                if (resultadoSQL.Read())
                {
                    historiaClinica = new HistoriaClinica
                    {
                        HistorialClinicoCodigo = resultadoSQL.GetString(0),
                        AntecedentesMedicos = resultadoSQL.IsDBNull(2) ? null : resultadoSQL.GetString(2),
                        Alergias = resultadoSQL.IsDBNull(3) ? null : resultadoSQL.GetString(3),
                        FechaCreacion = resultadoSQL.GetDateTime(4)
                    };
                }
                else
                {
                    throw new ExcepcionHistoriaClinicaInvalida(ExcepcionHistoriaClinicaInvalida.NO_EXISTE_REGISTRO);
                }

                resultadoSQL.Close();
            }
            catch (SqlException)
            {
                throw new ExcepcionHistoriaClinicaInvalida(ExcepcionHistoriaClinicaInvalida.ERROR_DE_CONSULTA);
            }

            return historiaClinica;
        }

    }
}
