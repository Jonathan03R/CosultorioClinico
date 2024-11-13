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
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    Medico medico = ObtenerMedicoConEspecialidad(resultadoSQL);
                    listaMedicos.Add(medico);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.ERROR_DE_CONSULTA);
            }
            return listaMedicos;
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
