using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa3_Dominio.ModuloPrincipal;
using Capa3_Dominio.ModuloPrincipal.Entidades;
using Capa4_Persistencia.SqlServer.ModuloBase;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class DetallesConsultaSQL
    {

        private AccesoSQLServer accesoSQLServer;

        public DetallesConsultaSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }

            public void RegistrarDetallesConsulta(DetallesConsulta detallesConsulta)
        {
            string procedimientoSQL = "pro_registrar_DetallesConsulta";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@DetallesConsultaCodigo", detallesConsulta.DetallesConsultaEvaluacionPsico1));
                comandoSQL.Parameters.Add(new SqlParameter("@DetallesConsultaHistoriaEnfermedad", detallesConsulta.DetallesConsultaHistoriaEnfermedad1));
                comandoSQL.Parameters.Add(new SqlParameter("@DetallesConsultaRevisiones", detallesConsulta.DetallesConsultaRevisiones1));
                comandoSQL.Parameters.Add(new SqlParameter("@DetallesConsultaEvaluacionPsico", detallesConsulta.DetallesConsultaEvaluacionPsico1));
                comandoSQL.Parameters.Add(new SqlParameter("@DetallesConsultaMotivoConsulta", detallesConsulta.DetallesConsultaMotivoConsulta1));
                comandoSQL.Parameters.Add(new SqlParameter("@ConsultaCodigo", detallesConsulta.Consulta.ConsultaCodigo));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al registrar los detalles de la consulta: {ex.Message}");
            }
        }

    }

}
