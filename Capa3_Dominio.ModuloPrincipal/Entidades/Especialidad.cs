using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public class Especialidad
    {
        private string especialidadCodigo;
        private string especialidadNombre;
        private string especialidadDescripcion;

        public string EspecialidadCodigo { get => especialidadCodigo; set => especialidadCodigo = value; }
        public string EspecialidadNombre { get => especialidadNombre; set => especialidadNombre = value; }
        public string EspecialidadDescripcion { get => especialidadDescripcion; set => especialidadDescripcion = value; }

    
    }
}
