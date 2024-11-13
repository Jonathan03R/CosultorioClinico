using System;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionPacienteInvalido : Exception
    {
        public const string NO_EXISTE_REGISTRO = "No existe el paciente.";
        public const string NO_EXISTEN_REGISTROS = "No existen pacientes.";
        public const string ERROR_DE_CONSULTA = "No se pudo consultar el/los pacientes, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_CREACION = "No se pudo crear el paciente, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ACTUALIZACION = "No se pudo modificar el paciente, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ELIMINACION = "No se pudo eliminar el paciente, intente nuevamente o consulte con el administrador.";

        public ExcepcionPacienteInvalido(string mensaje)
            : base(mensaje)
        {
        }
    }
}
