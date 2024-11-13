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
    public class EspecialidadSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public EspecialidadSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }
        public List<Especialidad> MostrarEspecialidadesConMedicos()
        {
            List<Especialidad> listaEspecialidades = new List<Especialidad>();
            string procedimientoSQL = "pro_Mostrar_MedicosConEspecialidad";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    Especialidad especialidad = ObtenerEspecialidad(resultadoSQL);
                    listaEspecialidades.Add(especialidad);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionEspecialidadInvalido(ExcepcionEspecialidadInvalido.ERROR_DE_CONSULTA);
            }
            return listaEspecialidades;
        }
        private Especialidad ObtenerEspecialidad(SqlDataReader resultadoSQL)
        {
            Especialidad especialidad = new Especialidad
            {
                EspecialidadNombre = resultadoSQL.GetString(3) 
            };
            return especialidad;
        }
    }
}
