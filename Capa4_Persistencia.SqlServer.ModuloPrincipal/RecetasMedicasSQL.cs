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
    public class RecetasMedicasSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public RecetasMedicasSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }
        public List<RecetaMedica> MostrasRecetasMedicasPorConsulta(string consultaCodigo)
        {

            List<RecetaMedica> listaRecetasMedicas = new List<RecetaMedica>();
            string procedimientoSql = "pro_listar_RecetasPorConsulta";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSql);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consultaCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    RecetaMedica recetaMedica = new RecetaMedica()
                    {
                        RecetaCodigo = resultadoSQL.IsDBNull(0) ? null : resultadoSQL.GetString(0),
                        RecetaDescripcion = resultadoSQL.IsDBNull(1) ? null : resultadoSQL.GetString(1),
                        RecetaTratamiento = resultadoSQL.IsDBNull(2) ? null : resultadoSQL.GetString(2),
                        RecetaRecomendaciones = resultadoSQL.IsDBNull(3) ? null : resultadoSQL.GetString(3),
                    };

                    listaRecetasMedicas.Add(recetaMedica);
                }
                resultadoSQL.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return listaRecetasMedicas;
        }

        public void CrearRecetaMedica(RecetaMedica recetaMedica)
        {
            string procedimientoSQL = "pro_registrar_RecetaMedica";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);

                // Agregar parámetros al procedimiento almacenado
                comandoSQL.Parameters.Add(new SqlParameter("@RecetaCodigo", recetaMedica.RecetaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@RecetaConsultaCodigo", recetaMedica.Consulta.ConsultaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@RecetaDescripcion", recetaMedica.RecetaDescripcion));
                comandoSQL.Parameters.Add(new SqlParameter("@RecetaTratamiento", recetaMedica.RecetaTratamiento));
                comandoSQL.Parameters.Add(new SqlParameter("@RecetaRecomendaciones", recetaMedica.RecetaTratamiento));

                // Ejecutar el comando
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al registrar la receta médica: {ex.Message}");
            }
        }


    }
}
