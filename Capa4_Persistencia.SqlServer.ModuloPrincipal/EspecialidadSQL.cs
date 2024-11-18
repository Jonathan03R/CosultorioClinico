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
                    // Crear el objeto Medico y asignar sus propiedades
                    Medico medico = new Medico
                    {
                        MedicoCodigo = resultadoSQL.GetString(0),
                        MedicoNombre = resultadoSQL.GetString(1), 
                        MedicoApellido = resultadoSQL.GetString(2),
                        Especialidad = new Especialidad
                        {
                            EspecialidadNombre = resultadoSQL.GetString(3) 
                        }
                    };

                    listaMedicos.Add(medico);
                }
                resultadoSQL.Close();
            }
            catch (SqlException)
            {
                throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.ERROR_DE_CONSULTA);
            }
            return listaMedicos;
        }

        public List<Especialidad> Pro_Listar_Especialidad()
        {
            List<Especialidad> listaEspecialidades = new List<Especialidad>();
            string procedimientoSQL = "Pro_Listar_Especialidad";
            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
               
                while (resultadoSQL.Read())
                {

                    Especialidad especialidad = new Especialidad
                    {
                        EspecialidadCodigo = resultadoSQL.GetString(0),
                        EspecialidadNombre = resultadoSQL.GetString(1),
                        EspecialidadDescripcion = resultadoSQL.GetString(2)
                    };
                    listaEspecialidades.Add(especialidad);
                }
                resultadoSQL.Close ();
            }
            catch (SqlException ex)
            {
                //throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.ERROR_DE_CONSULTA);
                throw ex;
            }
            return listaEspecialidades;


        }
    }
}
