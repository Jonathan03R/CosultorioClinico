using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicios
{
    public class AtenderConsultaServicio
    {

        private readonly AccesoSQLServer accesoSQLServer;
        private readonly CodigoSQL codigoSQL;
        private readonly CitaSQL citaSQL;


        public AtenderConsultaServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            codigoSQL = new CodigoSQL(accesoSQLServer);
            citaSQL = new CitaSQL(accesoSQLServer);
        }

        //Listar Consultas


        //Listar los pacientes de la fecha de hoy

        //Permitir Cambiar el estado de las consultas

        //Registrar Diagnostico
        //Listar Diagnostico

        //Listar Recetas  medicas anteriores del paciente

        //Registrar Receta Medica
    }
}
