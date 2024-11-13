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
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_CREACION);
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
            Cita cita = new Cita(
                resultadoSQL.GetString(0),  
                resultadoSQL.GetString(1), 
                resultadoSQL.GetDateTime(2)
            )
            {
                CitaTipoConsulta = new TipoConsulta
                {
                    TipoConsultaCodigo = resultadoSQL.GetString(3)
                },
                CitaNotificacion = resultadoSQL.IsDBNull(4) ? null : new Notificacion
                {
                    NotificacionCodigo = resultadoSQL.GetString(4)
                },
                CitaMedico = new Medico
                {
                    MedicoNombre = resultadoSQL.GetString(5),
                    MedicoApellido = resultadoSQL.GetString(6)
                }
            };
            return cita;
        }
    }
}