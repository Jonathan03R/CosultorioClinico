using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionMedicoInvalido : Exception
    {
        public const string NO_EXISTE_REGISTRO = "No existe el médico.";
        public const string NO_EXISTEN_REGISTROS = "No existen médicos.";
        public const string ERROR_DE_CONSULTA = "No se pudo consultar el/los médicos, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_CREACION = "No se pudo crear el médico, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ACTUALIZACION = "No se pudo modificar el médico, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ELIMINACION = "No se pudo eliminar el médico, intente nuevamente o consulte con el administrador.";

        public ExcepcionMedicoInvalido(string mensaje)
            : base(mensaje)
        {
        }
    }
}
