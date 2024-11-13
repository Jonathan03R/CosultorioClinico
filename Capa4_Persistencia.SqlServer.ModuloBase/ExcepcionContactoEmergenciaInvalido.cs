using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionContactoEmergenciaInvalido : Exception
    {
        public const string ERROR_DE_CONSULTA = "No se pudo consultar los contactos de emergencia, intente nuevamente o consulte con el administrador.";

        public ExcepcionContactoEmergenciaInvalido(string mensaje)
            : base(mensaje)
        {
        }
    }
}
