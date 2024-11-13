using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionCitaInvalida : Exception
    {
        public const string NO_EXISTE_REGISTRO = "No existe la cita.";
        public const string NO_EXISTEN_REGISTROS = "No existen citas.";
        public const string ERROR_DE_CONSULTA = "No se pudo consultar la(s) cita(s), intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_CREACION = "No se pudo crear la cita, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ACTUALIZACION = "No se pudo modificar la cita, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ELIMINACION = "No se pudo eliminar la cita, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_CANCELACION = "No se pudo cancelar la cita, intente nuevamente o consulte con el administrador.";

        public ExcepcionCitaInvalida(string mensaje)
            : base(mensaje)
        {
        }
    }
}