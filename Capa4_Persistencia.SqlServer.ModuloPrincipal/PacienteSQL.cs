﻿using Capa3_Dominio.ModuloPrincipal;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteEstado", paciente.PacienteEstado));
                comandoSQL.ExecuteNonQuery();
            }
            catch ( SqlException ex)
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
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteNombreCompleto", (object)paciente.PacienteNombreCompleto ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteDireccion", (object)paciente.PacienteDireccion ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteTelefono", (object)paciente.PacienteTelefono ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCorreoElectronico", (object)paciente.PacienteCorreoElectronico ?? DBNull.Value));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void EliminarPaciente(Paciente pacienteCodigo)
        {
            string procedimientoSQL = "pro_Eliminar_Paciente";
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

        public Paciente MostrarPacientePorCodigo (Paciente pacienteCodigo)
        {
            Paciente paciente = null;
            string procedimientoSQL = "pro_Mostrar_Paciente_por_codigo";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", pacienteCodigo.PacienteCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                if (resultadoSQL.Read())
                {
                    paciente = ObtenerPaciente(resultadoSQL);
                }
                else
                {
                    throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.NO_EXISTE_REGISTRO);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_CONSULTA);
            }
            return paciente;
        }

        public Paciente MostarPacienteDni(Paciente paciente)
        {
            Paciente pacienteResultado = null;
            string procedimientoSQL = "pro_Buscar_Paciente_dni";

            try
            {
                using (SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL))
                {
                    string pacienteDniTrimmed = paciente.PacienteDNI.Trim();
                    comandoSQL.Parameters.Add(new SqlParameter("@pacienteDNI", SqlDbType.NChar, 8) { Value = pacienteDniTrimmed });

                    using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                    {
                        if (resultadoSQL.Read()) // Verifica si hay una fila disponible
                        {
                            pacienteResultado = new Paciente
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

                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return pacienteResultado;
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

        public List<Paciente> BuscarPaciente(string dni = null, string nombreCompleto = null, string telefono = null)
        {
            List<Paciente> listaPacientes = new List<Paciente>();
            string procedimientoSQL = "pro_Buscar_Paciente";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);

                // Parámetros del procedimiento almacenado
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
        public List<HistoriaClinica> ListarPacientes()
        {
            List<HistoriaClinica> listaHistoriasClinicas = new List<HistoriaClinica>();
            string procedimientoSQL = "pro_listar_pacientes";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                while (resultadoSQL.Read())
                {
                    // Crear el objeto Paciente y asignar sus propiedades
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

                    // Crear el objeto HistoriaClinica y asignar el código si está disponible
                    HistoriaClinica historiaClinica = new HistoriaClinica
                    {
                        HistorialClinicoCodigo = resultadoSQL.IsDBNull(1) ? null : resultadoSQL.GetString(1), // Código de historia clínica
                        Paciente = paciente // Asignar el paciente a la historia clínica
                    };

                    listaHistoriasClinicas.Add(historiaClinica);
                }
                resultadoSQL.Close();
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_CONSULTA);
            }
            return listaHistoriasClinicas;
        }


    }
}

