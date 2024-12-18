﻿using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class DiagnosticoSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public DiagnosticoSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }
        public List<Diagnostico> MostrasDiagnosticosPorConsulta(string consultaCodigo)
        {

            List<Diagnostico> ListarDiagnosticos = new List<Diagnostico>();
            string procedimientoSql = "pro_listar_DiagnosticosPorConsulta";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSql);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consultaCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    Diagnostico diagnostico = new Diagnostico()
                    {
                        DiagnosticoCodigo = resultadoSQL.IsDBNull(0) ? null : resultadoSQL.GetString(0),
                        DiagnosticoDescripcion = resultadoSQL.IsDBNull(1) ? null : resultadoSQL.GetString(1),
                    };

                    ListarDiagnosticos.Add(diagnostico);
                }

                resultadoSQL.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return ListarDiagnosticos;
        }
        public void CrearDiagnostico(Diagnostico diagnostico)
        {
            string procedimientoSQL = "pro_registrar_Diagnostico";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);

                // Agregar parámetros al procedimiento almacenado
                comandoSQL.Parameters.Add(new SqlParameter("@DiagnosticoCodigo", diagnostico.DiagnosticoCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@DiagnosticoConsultaCodigo", diagnostico.Consulta.ConsultaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@DiagnosticoDescripcion", diagnostico.DiagnosticoDescripcion));
                comandoSQL.Parameters.Add(new SqlParameter("@DiagnosticoCodigoCie11",
                             diagnostico.DiagnosticoCie11 ?? (object)DBNull.Value));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al registrar el diagnóstico: {ex.Message}");
            }
        }

    }
}
