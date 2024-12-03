using Capa3_Dominio.ModuloPrincipal.Entidad;
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
                comandoSQL.Parameters.Add(new SqlParameter("@citaNotificacionCodigo", cita.CitaNotificacion?.NotificacionCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@citaPacienteCodigo", cita.CitaPaciente.PacienteCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@citaTipoConsultaCodigo", cita.CitaTipoConsulta.TipoConsultaCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@citaMedicoCodigo", cita.CitaMedico.MedicoCodigo));
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

        public List<Cita> MostrarCitasPaciente(string pacienteCodigo)
        {
            List<Cita> listaCitas = new List<Cita>();
            string procedimientoSQL = "pro_VisualizarCitasPaciente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", pacienteCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    Cita cita = ObtenerCita(resultadoSQL);
                    listaCitas.Add(cita);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_CONSULTA);
            }
            return listaCitas;
        }

        private Cita ObtenerCita(SqlDataReader resultadoSQL)
        {
            Cita cita = new Cita();

            cita.CitaCodigo = resultadoSQL.GetString(0);  
            cita.CitaEstado = resultadoSQL.GetString(1);   
            cita.CitaFechaHora = resultadoSQL.GetDateTime(2);
            cita.CitaTipoConsulta = new TipoConsulta
            {
                TipoConsultaCodigo = resultadoSQL.GetString(3) 
            };

            if (!resultadoSQL.IsDBNull(4))
            {
                cita.CitaNotificacion = new Notificacion
                {
                    NotificacionCodigo = resultadoSQL.GetString(4)
                };
            }
            cita.CitaMedico = new Medico
            {
                MedicoNombre = resultadoSQL.GetString(5), 
                MedicoApellido = resultadoSQL.GetString(6) 
            };

            return cita;
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


        public List<Cita> MostrarCitas()
        {
            List<Cita> listaCitas = new List<Cita>();
            string procedimientoSQL = "pro_Mostrar_Citas";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                while (resultadoSQL.Read())
                {
                    Cita cita = new Cita();

                    // Asignar valores a las propiedades de la cita
                    cita.CitaCodigo = resultadoSQL.GetString(0);   // CodigoCita
                    cita.CitaEstado = resultadoSQL.GetString(1);   // EstadoCita
                    cita.CitaFechaHora = resultadoSQL.GetDateTime(2);
                    // Asignar TipoConsulta
                    cita.CitaTipoConsulta = new TipoConsulta
                    {
                        TipoConsultaCodigo = resultadoSQL.GetString(3) 
                    };
                    if (!resultadoSQL.IsDBNull(4))
                    {
                        cita.CitaNotificacion = new Notificacion
                        {
                            NotificacionCodigo = resultadoSQL.GetString(4) 
                        };
                    }
                    cita.CitaMedico = new Medico
                    {
                        MedicoNombre = resultadoSQL.GetString(5),   
                        MedicoApellido = resultadoSQL.GetString(6)  // ApellidoMedico
                    };
                    cita.CitaPaciente = new Paciente
                    {
                        PacienteCodigo = resultadoSQL.GetString(7),           // PacienteCodigo
                        PacienteNombreCompleto = resultadoSQL.GetString(8)   // NombrePaciente
                    };

                    listaCitas.Add(cita);
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
            string procedimientoSQL = "pro_Actualizar_Estado_CitaNoAsistio";
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
            string procedimientoSQL = "pro_Actualizar_Estado_CitaAtendido";
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
            string procedimientoSQL = "pro_Actualizar_Estado_CitaCancelado";
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