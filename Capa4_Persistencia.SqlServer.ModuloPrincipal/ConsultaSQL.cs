using Capa3_Dominio.ModuloPrincipal;
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
                            ConsultaEstado = resultadoSQL["consultaEstado"].ToString().Trim(),
                            Cita = new Cita
                            {
                                //CitaCodigo = resultadoSQL["consultacitaCodigo"].ToString().Trim(),
                                CitaFechaHora = resultadoSQL["HoraCitaInicio"] != DBNull.Value
                                    ? Convert.ToDateTime(resultadoSQL["HoraCitaInicio"])
                                    : DateTime.MinValue,
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

        public void GuardarCambiosConsulta(Consulta consulta, string cambioDescripcion, string medicoCodigo)
        {
            string procedimientoSQL = "pro_GuardarCambios_Consulta";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consulta.ConsultaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@nuevoMotivo", consulta.ConsultaMotivo));
                comandoSQL.Parameters.Add(new SqlParameter("@nuevoEstado", consulta.ConsultaEstado));
                comandoSQL.Parameters.Add(new SqlParameter("@cambiomedicoCodigo", medicoCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@cambioDescripcion", cambioDescripcion));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al guardar cambios en la consulta: {ex.Message}");
            }
        }


        public void CambiarEstadoConsulta(string consultaCodigo, string nuevoEstado, string medicoCodigo, string descripcionCambio)
        {
            string procedimientoSQL = "pro_Cambiar_Estado_Consulta";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consultaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@nuevoEstado", nuevoEstado));
                comandoSQL.Parameters.Add(new SqlParameter("@cambiomedicoCodigo", medicoCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@cambioDescripcion", descripcionCambio));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al cambiar el estado de la consulta: {ex.Message}");
            }
        }

        //los verdaderos cambios de estados 

        // Cambio de estado Pendiente

        public void CambiarEstadoPendiente(string consultaCodigo) 
        {
            string procedimientoSQL = "pro_Actualizar_Estado_ConsultaPendiente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consultaCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Error al cambiar el estado a pendiente: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }

        //estado No Asistieron
        public void CambiarEstadoNoAsistieron(string consultaCodigo)
        {
            string procedimientoSQL = "pro_Actualizar_Estado_ConsultaPendiente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consultaCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Error al cambiar el estado a pendiente: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }

        //estado Atendido
        public void CambiarEstadoAtendido(string consultaCodigo)
        {
            string procedimientoSQL = "pro_Actualizar_Estado_ConsultaPendiente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consultaCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Error al cambiar el estado a pendiente: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }
        //estado Cancelada

        public void CambiarEstadoCancelado(string consultaCodigo)
        {
            string procedimientoSQL = "pro_Actualizar_Estado_ConsultaPendiente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@consultaCodigo", consultaCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Error al cambiar el estado a pendiente: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }





    }
}
