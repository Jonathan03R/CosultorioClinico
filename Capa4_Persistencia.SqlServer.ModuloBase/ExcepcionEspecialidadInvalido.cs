using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionEspecialidadInvalido : Exception
    {
        public const string ERROR_DE_CONSULTA = "No se pudo consultar las especialidades, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_CREACION = "No se pudo crear la especialidad, intente nuevamente o consulte con el administrador.";

        public ExcepcionEspecialidadInvalido(string mensaje)
            : base(mensaje)
        {
        }
    }
}
