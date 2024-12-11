using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa3_Dominio.ModuloPrincipal.TransferenciaDatos;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class HorariosSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public HorariosSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }
        public List<HorarioConCita> ListarHorariosConCitas(string especialidadCodigo, DateTime fecha)
        {
            List<HorarioConCita> horariosConCitas = new List<HorarioConCita>();
            string procedimientoSQL = "pro_Listar_HorariosEspecialidadConCitas";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.AddWithValue("@especialidadCodigo", especialidadCodigo);
                comandoSQL.Parameters.AddWithValue("@fecha", fecha);

                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    while (resultadoSQL.Read())
                    {
                        HorarioConCita horarioConCita = new HorarioConCita()
                        {
                            MedicoCodigo = resultadoSQL.GetString(0),
                            MedicoNombre = resultadoSQL.IsDBNull(1) ? null : resultadoSQL.GetString(1),
                            HoraInicio = resultadoSQL.GetTimeSpan(2),
                            HoraFin = resultadoSQL.GetTimeSpan(3),
                            ConsultaCodigo = resultadoSQL.IsDBNull(4) ? null : resultadoSQL.GetString(4),
                            CitaCodigo = resultadoSQL.IsDBNull(5) ? null : resultadoSQL.GetString(5),
                            PacienteCodigo = resultadoSQL.IsDBNull(6) ? null : resultadoSQL.GetString(6),
                            CitaEstado = resultadoSQL.IsDBNull(7) ? null : resultadoSQL.GetString(7),
                            PacienteNombre= resultadoSQL.IsDBNull(8) ? null : resultadoSQL.GetString(8),
                        };

                        horariosConCitas.Add(horarioConCita);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar horarios con citas: {ex.Message}", ex);
            }

            return horariosConCitas;
        }

        //este es como un DTO
        
    }
}




