using Capa3_Dominio.ModuloPrincipal;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class CitaSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public CitaSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }

        public void CrearCita(Cita cita)
        {
            string procedimientoSQL = "pro_Insertar_Cita";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@citaCodigo", cita.CitaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@citaEstado", cita.CitaEstado));
                comandoSQL.Parameters.Add(new SqlParameter("@citaFechaHora", cita.CitaFechaHora));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException EX)
            {
                throw EX;
            }
        }

        public void CancelarCita(string citaCodigo)
        {
            string procedimientoSQL = "pro_Cancelar_Cita";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@citaCodigo", citaCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_CANCELACION);
            }
        }

        public List<Consulta> MostrarCitasPaciente(string pacienteCodigo)
        {
            List<Consulta> listaCitas = new List<Consulta>();
            string procedimientoSQL = "pro_VisualizarCitasPaciente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", pacienteCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    Consulta consulta = new Consulta()
                    {
                        Cita = new Cita()
                        {
                            CitaCodigo = resultadoSQL.IsDBNull(0) ? string.Empty : resultadoSQL.GetString(0),
                            CitaEstado = resultadoSQL.IsDBNull(1) ? string.Empty : resultadoSQL.GetString(1),
                            CitaFechaHora = resultadoSQL.IsDBNull(2) ? DateTime.MinValue : resultadoSQL.GetDateTime(2),
                        },
                        TipoConsulta = new TipoConsulta()
                        {
                            TipoConsultaDescripcion = resultadoSQL.IsDBNull(3) ? string.Empty : resultadoSQL.GetString(3)
                        },
                        Medico = new Medico()
                        {
                            MedicoNombre = resultadoSQL.IsDBNull(4) ? string.Empty : resultadoSQL.GetString(4),
                            MedicoApellido = resultadoSQL.IsDBNull(5) ? string.Empty : resultadoSQL.GetString(5),
                        },
                        ConsultaMotivo = resultadoSQL.IsDBNull(6) ? string.Empty : resultadoSQL.GetString(6),

                        ConsultaFechaHoraFinal = resultadoSQL.IsDBNull(7) ? (DateTime?)null : resultadoSQL.GetDateTime(7),
                    };

                    listaCitas.Add(consulta);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return listaCitas;
        }

      

        public List<TipoConsulta> ListarTiposDeConsulta()
        {
            List<TipoConsulta> listaTiposConsulta = new List<TipoConsulta>();
            string procedimientoSQL = "Pro_Listar_TipoConsulta";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                while (resultadoSQL.Read())
                {
                    TipoConsulta tipoConsulta = new TipoConsulta
                    {
                        TipoConsultaCodigo = resultadoSQL.GetString(0), 
                        TipoConsultaDescripcion = resultadoSQL.GetString(1) 
                    };
                    listaTiposConsulta.Add(tipoConsulta);
                }
                resultadoSQL.Close();
            }
            catch (SqlException)
            {
                throw new Exception("Error al listar los tipos de consulta.");
            }

            return listaTiposConsulta;
        }


        public List<Consulta> MostrarCitas()
        {
            List<Consulta> listaCitas = new List<Consulta>();
            string procedimientoSQL = "pro_Mostrar_Citas";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                while (resultadoSQL.Read())
                {
                    Consulta consulta = new Consulta()
                    {
                        Cita = new Cita()
                        {
                            CitaCodigo = resultadoSQL.GetString(0),
                            CitaEstado = resultadoSQL.GetString(1),
                            CitaFechaHora = resultadoSQL.GetDateTime(2),
                        },
                        TipoConsulta = new TipoConsulta() 
                        {
                            TipoConsultaDescripcion = resultadoSQL.GetString(3),    
                        },
                        Medico = new Medico() { 
                            MedicoCodigo = resultadoSQL.GetString(4),
                            MedicoNombre = resultadoSQL.GetString(5),
                            MedicoApellido = resultadoSQL.GetString(6),
                            Especialidad = new Especialidad()
                            {
                                EspecialidadCodigo = resultadoSQL.GetString(7),
                                EspecialidadNombre = resultadoSQL.GetString(8), 
                            },
                        },
                        Paciente = new Paciente() 
                        {
                            PacienteCodigo = resultadoSQL .GetString(9),
                            PacienteNombreCompleto = resultadoSQL .GetString(10),   
                        }
                    };
                    
                    listaCitas.Add(consulta);
                }
                resultadoSQL.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener las citas: " + ex.Message);
            }

            return listaCitas;
        }

        // Cambio de estado Pendiente

        public void CambiarEstadoPendiente(string consultaCodigo)
        {
            string procedimientoSQL = "pro_Actualizar_Estado_CitaPendiente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@citaCodigo", consultaCodigo));
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
            string procedimientoSQL = "pro_Actualizar_Estado_CitaNoAsistio";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@citaCodigo", consultaCodigo));
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
            string procedimientoSQL = "pro_Actualizar_Estado_CitaAtendido";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@citaCodigo", consultaCodigo));
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
            string procedimientoSQL = "pro_Actualizar_Estado_CitaCancelado";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@citaCodigo", consultaCodigo));
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


        //estado Cuando se esta atendendo un paciente

        public void CambiarEstadoAtencionProceso(string consultaCodigo)
        {
            string procedimientoSQL = "pro_Actualizar_Estado_CitaAtendiendose";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@citaCodigo", consultaCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                //throw new Exception($"Error al cambiar el estado a pendiente: {sqlEx.Message}");
                throw sqlEx;
            }
            catch (Exception ex)
            {
                //throw new Exception($"Error inesperado: {ex.Message}");
                throw ex;
            }
        }



    }
}