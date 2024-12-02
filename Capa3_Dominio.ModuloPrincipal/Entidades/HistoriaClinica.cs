using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{

    public class HistoriaClinica
    {

        private string historialClinicoCodigo;
        private string antecedentesMedicos;
        private string alergias;
        private DateTime fechaCreacion;

        public string HistorialClinicoCodigo { get => historialClinicoCodigo; set => historialClinicoCodigo = value; }
        public string AntecedentesMedicos { get => antecedentesMedicos; set => antecedentesMedicos = value; }
        public string Alergias { get => alergias; set => alergias = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    }
}
