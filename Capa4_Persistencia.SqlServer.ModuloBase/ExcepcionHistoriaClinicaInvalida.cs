using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionHistoriaClinicaInvalida : Exception
    {
        public const string NO_EXISTE_REGISTRO = "No existe el historial clínico.";
        public const string ERROR_DE_CONSULTA = "No se pudo consultar el historial clínico, intente nuevamente o consulte con el administrador.";

        public ExcepcionHistoriaClinicaInvalida(string mensaje)
            : base(mensaje)
        {
        }
    }
}
