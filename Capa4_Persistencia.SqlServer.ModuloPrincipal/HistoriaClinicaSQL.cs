using Capa3_Dominio.ModuloPrincipal;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class HistoriaClinicaSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public HistoriaClinicaSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }


        // Método para agregar una historia clínica a la base de datos
        public void AgregarHistoriaClinica(HistoriaClinica historiaClinica)
        {
            string procedimientoSQL = "pro_Crear_HistoriaClinica";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@historialClinicoCodigo", historiaClinica.HistorialClinicoCodigo));
                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public HistoriaClinica MostrarHistoriaClinica(string pacienteCodigo)
        {
            HistoriaClinica historiaClinica = null;
            string procedimientoSQL = "pro_Mostrar_HistoriaClinica";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pacienteCodigo", pacienteCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                if (resultadoSQL.Read())
                {
                    historiaClinica = new HistoriaClinica
                    {
                        HistorialClinicoCodigo = resultadoSQL.GetString(0),
                    };
                }
                else
                {
                    throw new ExcepcionHistoriaClinicaInvalida(ExcepcionHistoriaClinicaInvalida.NO_EXISTE_REGISTRO);
                }

                resultadoSQL.Close();
            }
            catch (SqlException)
            {
                throw new ExcepcionHistoriaClinicaInvalida(ExcepcionHistoriaClinicaInvalida.ERROR_DE_CONSULTA);
            }

            return historiaClinica;
        }


        public List<Consulta> MostrasDetallesHistoriaClinica(string HistoriaCodigo) 
        {

            List<Consulta> listaConsultasHistoria = new List<Consulta>();   

            string procedimientoSql = "pro_listar_HistoriaClinica";

            try
            { 
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSql);
                comandoSQL.Parameters.Add(new SqlParameter("@historialClinicoCodigo", HistoriaCodigo));
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    Consulta consulta = new Consulta()
                    {
                        ConsultaCodigo = resultadoSQL.IsDBNull(0) ? null : resultadoSQL.GetString(0),
                        Cita = new Cita()
                        {
                            CitaCodigo = resultadoSQL.IsDBNull(1) ? null : resultadoSQL.GetString(1),
                            CitaEstado = resultadoSQL.GetString(7),
                        },
                        ConsultaFechaHoraFinal = resultadoSQL.IsDBNull(2) ? (DateTime?)null : resultadoSQL.GetDateTime(2),
                        ConsultaMotivo = resultadoSQL.IsDBNull(3) ? null : resultadoSQL.GetString(3),
                        TipoConsulta = new TipoConsulta()
                        {
                            TipoConsultaCodigo = resultadoSQL.IsDBNull(4) ? null : resultadoSQL.GetString(4)
                        },
                        Medico = new Medico()
                        {
                            MedicoCodigo = resultadoSQL.IsDBNull(5) ? null : resultadoSQL.GetString(5),
                        },
                        Paciente = new Paciente()
                        {
                            PacienteCodigo = resultadoSQL.IsDBNull(6) ? null : resultadoSQL.GetString(6)
                        }
                    };

                    listaConsultasHistoria.Add(consulta);
                }

                resultadoSQL.Close();
            }
            catch (SqlException ex) 
            {
                throw ex;
            }
            return listaConsultasHistoria; 
        }

        

    }
}
