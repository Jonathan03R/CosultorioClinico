using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class PacienteSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public PacienteSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }


        public void CrearPaciente(Paciente paciente)
        {
            string procedimientoSQL = "pro_Crear_Paciente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", paciente.PacienteCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteDNI", paciente.PacienteDNI));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteNombreCompleto", paciente.PacienteNombreCompleto));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteFechaNacimiento", paciente.PacienteFechaNacimiento));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteDireccion", paciente.PacienteDireccion));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteTelefono", paciente.PacienteTelefono));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCorreoElectronico", paciente.PacienteCorreoElectronico));
                comandoSQL.Parameters.Add(new SqlParameter("historialClinicoCodigo", paciente.HistoriaClinica.HistorialClinicoCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch ( SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarPaciente(Paciente paciente)
        {
            string procedimientoSQL = "pro_Actualizar_Paciente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", paciente.PacienteCodigo));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteNombreCompleto", (object)(paciente.PacienteNombreCompleto?.ToUpper()) ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteDireccion", (object)paciente.PacienteDireccion ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteTelefono", (object)paciente.PacienteTelefono ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCorreoElectronico", (object)paciente.PacienteCorreoElectronico ?? DBNull.Value));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarPaciente(Paciente paciente)
        {
            string procedimientoSQL = "pro_Eliminar_Paciente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", paciente.PacienteCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_ELIMINACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RecuperarPaciente(Paciente pacienteCodigo)
        {
            string procedimientoSQL = "pro_Recuperar_Paciente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", pacienteCodigo.PacienteCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_ELIMINACION);
            }
        }

        //tengo esto en la capa de persistencia

        public Paciente MostrarPacientePorCodigo(string pacienteCodigo)
        {
            Paciente paciente = null;
            string procedimientoSQL = "pro_Mostrar_Paciente_por_codigo";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", pacienteCodigo));

                // Usar using para asegurar el cierre del SqlDataReader
                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    if (resultadoSQL.Read())
                    {
                        paciente = new Paciente
                        {
                            PacienteCodigo = resultadoSQL["pacienteCodigo"].ToString().Trim(),
                            PacienteDNI = resultadoSQL["pacienteDNI"].ToString().Trim(),
                            PacienteNombreCompleto = resultadoSQL["pacienteNombreCompleto"].ToString().Trim(),
                            PacienteFechaNacimiento = resultadoSQL["pacienteFechaNacimiento"] != DBNull.Value
                                ? Convert.ToDateTime(resultadoSQL["pacienteFechaNacimiento"])
                                : DateTime.MinValue,
                            PacienteDireccion = resultadoSQL["pacienteDireccion"] != DBNull.Value
                                ? resultadoSQL["pacienteDireccion"].ToString().Trim()
                                : null,
                            PacienteTelefono = resultadoSQL["pacienteTelefono"] != DBNull.Value
                                ? resultadoSQL["pacienteTelefono"].ToString().Trim()
                                : null,
                            PacienteCorreoElectronico = resultadoSQL["pacienteCorreoElectronico"] != DBNull.Value
                                ? resultadoSQL["pacienteCorreoElectronico"].ToString().Trim()
                                : null,
                            PacienteEstado = resultadoSQL["pacienteEstado"].ToString().Trim()
                        };
                    }
                    else
                    {
                        return null; // Retornar null si no se encuentra el paciente
                    }
                }
            }
            catch (SqlException exSQL)
            {
                throw new Exception($"Error al ejecutar el procedimiento almacenado: {exSQL.Message}", exSQL);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}", ex);
            }

            return paciente; // Devolver el paciente encontrado o null
        }


        public Paciente MostarPacienteDni(string pacienteDNI) 
        {
            Paciente paciente;
            string procedimientoSQL = "pro_Buscar_Paciente_dni";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteDNI", pacienteDNI));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                if (resultadoSQL.Read())
                {
                    paciente = new Paciente
                    {
                        PacienteCodigo = resultadoSQL["pacienteCodigo"].ToString().Trim(),
                        PacienteDNI = resultadoSQL["pacienteDNI"].ToString().Trim(),
                        PacienteNombreCompleto = resultadoSQL["pacienteNombreCompleto"].ToString().Trim(),
                        PacienteFechaNacimiento = resultadoSQL["pacienteFechaNacimiento"] != DBNull.Value ? Convert.ToDateTime(resultadoSQL["pacienteFechaNacimiento"]) : DateTime.MinValue,
                        PacienteDireccion = resultadoSQL["pacienteDireccion"] != DBNull.Value ? resultadoSQL["pacienteDireccion"].ToString().Trim() : null,
                        PacienteTelefono = resultadoSQL["pacienteTelefono"] != DBNull.Value ? resultadoSQL["pacienteTelefono"].ToString().Trim() : null,
                        PacienteCorreoElectronico = resultadoSQL["pacienteCorreoElectronico"] != DBNull.Value ? resultadoSQL["pacienteCorreoElectronico"].ToString().Trim() : null,
                        PacienteEstado = resultadoSQL["pacienteEstado"].ToString().Trim()
                    };
                }
                else 
                {
                    return null;
                }
            }
            catch (SqlException exSQL)
            {
                throw exSQL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return paciente;
        }


        public List<Paciente> BuscarPaciente(string dni = null, string nombreCompleto = null, string telefono = null)
        {
            List<Paciente> listaPacientes = new List<Paciente>();
            string procedimientoSQL = "pro_Buscar_Paciente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);

                comandoSQL.Parameters.Add(new SqlParameter("@pacienteDNI", (object)dni ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteNombreCompleto", (object)nombreCompleto ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteTelefono", (object)telefono ?? DBNull.Value));

                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    Paciente paciente = ObtenerPaciente(resultadoSQL);
                    listaPacientes.Add(paciente);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_CONSULTA);
            }
            return listaPacientes;
        }


        public List<Paciente> ListarPacientes()
        {
            List<Paciente> listaPacientes = new List<Paciente>();
            string procedimientoSQL = "pro_listar_pacientes";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                while (resultadoSQL.Read())
                {
                    Paciente paciente = new Paciente
                    {
                        PacienteCodigo = resultadoSQL.GetString(0),
                        PacienteDNI = resultadoSQL.GetString(1),
                        PacienteNombreCompleto = resultadoSQL.GetString(2),
                        PacienteFechaNacimiento = resultadoSQL.GetDateTime(3),
                        PacienteDireccion = resultadoSQL.IsDBNull(4) ? null : resultadoSQL.GetString(4),
                        PacienteTelefono = resultadoSQL.IsDBNull(5) ? null : resultadoSQL.GetString(5),
                        PacienteCorreoElectronico = resultadoSQL.IsDBNull(6) ? null : resultadoSQL.GetString(6),
                        PacienteEstado = resultadoSQL.GetString(7),
                        HistoriaClinica = new HistoriaClinica
                        {
                            HistorialClinicoCodigo = resultadoSQL.IsDBNull(8) ? null : resultadoSQL.GetString(8)
                        }
                    };

                    listaPacientes.Add(paciente);
                }
                resultadoSQL.Close();
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_CONSULTA);
            }
            return listaPacientes;
        }

        private Paciente ObtenerPaciente(SqlDataReader resultadoSQL)
        {
            Paciente paciente = new Paciente
            {
                PacienteCodigo = resultadoSQL.GetString(0),
                PacienteDNI = resultadoSQL.GetString(2),
                PacienteNombreCompleto = resultadoSQL.GetString(3),
                PacienteFechaNacimiento = resultadoSQL.GetDateTime(4),
                PacienteDireccion = resultadoSQL.IsDBNull(5) ? null : resultadoSQL.GetString(5),
                PacienteTelefono = resultadoSQL.IsDBNull(6) ? null : resultadoSQL.GetString(6),
                PacienteCorreoElectronico = resultadoSQL.IsDBNull(7) ? null : resultadoSQL.GetString(7),
                PacienteEstado = resultadoSQL.GetString(8)
            };
            return paciente;
        }
    }
}

