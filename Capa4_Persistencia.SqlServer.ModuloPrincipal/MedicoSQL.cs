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
    public class MedicoSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public MedicoSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }
        public List<Medico> MostrarMedicosConEspecialidad()
        {
            List<Medico> listaMedicos = new List<Medico>();
            string procedimientoSQL = "pro_Mostrar_MedicosConEspecialidad";

            try
            {
                using (SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL))
                {
                    using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                    {
                        while (resultadoSQL.Read())
                        {
                            // Crear el objeto Medico y asignar sus propiedades
                            Medico medico = new Medico
                            {
                                MedicoCodigo = resultadoSQL["medicoCodigo"]?.ToString(),       // Columna: medicoCodigo
                                MedicoNombre = resultadoSQL["medicoNombre"]?.ToString(),       // Columna: medicoNombre
                                MedicoApellido = resultadoSQL["medicoApellido"]?.ToString(),   // Columna: medicoApellido
                                Especialidad = new Especialidad
                                {
                                    EspecialidadNombre = resultadoSQL["especialidadNombre"]?.ToString() // Columna: especialidadNombre
                                }
                            };

                            // Agregar el objeto Medico a la lista
                            listaMedicos.Add(medico);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception($"Error al acceder a las columnas del resultado: {ex.Message}");
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en la consulta SQL: {ex.Message}");
            }

            return listaMedicos;
        }


        public Medico ObtenerMedicoPorCodigo(string codigoMedico)
        {
            Medico medico = null;
            string procedimientoSQL = "pro_Mostrar_Medico_por_codigo";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@medicoCodigo", codigoMedico));

                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    if (resultadoSQL.Read())
                    {
                        medico = new Medico
                        {
                            MedicoCodigo = resultadoSQL["medicoCodigo"].ToString().Trim(),
                            MedicoApellido = resultadoSQL["medicoApellido"].ToString().Trim(),
                            MedicoNombre = resultadoSQL["medicoNombre"].ToString().Trim(),
                            MedicoCorreo = resultadoSQL["medicoCorreo"].ToString().Trim(),
                            MedicoDNI = resultadoSQL["medicoDNI"].ToString().Trim(),
                            MedicoTelefono = resultadoSQL["medicoTelefono"] != DBNull.Value
                                ? resultadoSQL["medicoTelefono"].ToString().Trim()
                                : null,
                            MedicoEstado = resultadoSQL["medicoEstado"].ToString().Trim(),
                            Especialidad = new Especialidad
                            {
                                EspecialidadCodigo = resultadoSQL["especialidadCodigo"].ToString().Trim()
                            }
                        };
                    }
                    else
                    {
                        throw new Exception($"No se encontró un médico con el código: {codigoMedico}");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Error al obtener el médico por código: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}", ex);
            }

            return medico;
        }


        private Medico ObtenerMedicoConEspecialidad(SqlDataReader resultadoSQL)
        {
            Medico medico = new Medico
            {
                MedicoCodigo = resultadoSQL.GetString(0),
                MedicoNombre = resultadoSQL.GetString(1),
                MedicoApellido = resultadoSQL.GetString(2),
                Especialidad = new Especialidad
                {
                    EspecialidadCodigo = resultadoSQL.GetString(3),
                    EspecialidadNombre = resultadoSQL.GetString(4),
                    EspecialidadDescripcion = resultadoSQL.GetString(5)
                }
            };
            return medico;
        }
    }
}
