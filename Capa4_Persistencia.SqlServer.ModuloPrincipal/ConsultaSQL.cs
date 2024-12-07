﻿using Capa3_Dominio.ModuloPrincipal;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class ConsultaSQL
    {

        private AccesoSQLServer accesoSQLServer;

        public ConsultaSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }

        public void CrearConsulta(Consulta consulta)
        {
            string procedimientoSQL = "pro_Crear_Consulta";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consulta.ConsultaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@consultacitaCodigo", consulta.Cita.CitaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@consultaFechaHoraFinal", consulta.ConsultaFechaHoraFinal));
                comandoSQL.Parameters.Add(new SqlParameter("@consultaMedicoCodigo", consulta.Cita.CitaMedico.MedicoCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@consultaPacienteCodigo", consulta.Cita.CitaPaciente.PacienteCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@consultaMotivo", consulta.ConsultaMotivo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al guardar la consulta: {ex.Message}"); 
            }
        }

        public List<Consulta> ListarConsultas()
        {
            List<Consulta> consultas = new List<Consulta>();
            string procedimientoSQL = "pro_Listar_Consulta";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);

                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    while (resultadoSQL.Read())
                    {
                        Consulta consulta = new Consulta
                        {
                            ConsultaCodigo = resultadoSQL["consultaCodigo"].ToString().Trim(),
                            ConsultaFechaHoraFinal = resultadoSQL["consultaFechaHoraFinal"] != DBNull.Value
                                ? Convert.ToDateTime(resultadoSQL["consultaFechaHoraFinal"])
                                : DateTime.MinValue,
                            ConsultaMotivo = resultadoSQL["consultaMotivo"]?.ToString().Trim(),
                            Cita = new Cita
                            {
                                CitaFechaHora = resultadoSQL["HoraCitaInicio"] != DBNull.Value
                                    ? Convert.ToDateTime(resultadoSQL["HoraCitaInicio"])
                                    : DateTime.MinValue,
                                CitaEstado = resultadoSQL["EstadoCita"]?.ToString(),
                                CitaPaciente = new Paciente
                                {
                                    PacienteCodigo = resultadoSQL["PacienteCodigo"].ToString().Trim()
                                },
                                CitaMedico = new Medico
                                {
                                    MedicoCodigo = resultadoSQL["MedicoCodigo"].ToString().Trim()
                                }
                            }
                        };

                        consultas.Add(consulta);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar consultas: {ex.Message}", ex);
            }

            return consultas;
        }


        



    }
}
