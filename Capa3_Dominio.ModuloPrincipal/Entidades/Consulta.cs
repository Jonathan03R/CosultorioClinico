using Capa3_Dominio.ModuloPrincipal.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    public class Consulta
    {
        private string consultaCodigo;
        private DateTime? consultaFechaHoraFinal;
        private string consultaMotivo;
        private string consultaEstado;

        private Cita cita;

        public string ConsultaCodigo { get => consultaCodigo; set => consultaCodigo = value; }
        public DateTime? ConsultaFechaHoraFinal { get => consultaFechaHoraFinal; set => consultaFechaHoraFinal = value; } // Nullable
        public string ConsultaMotivo { get => consultaMotivo; set => consultaMotivo = value; }
        public string ConsultaEstado { get => consultaEstado; set => consultaEstado = value; }
        public Cita Cita { get => cita; set => cita = value; }

        /*Regla 4:*/
        public bool ValidarEnvioHistorialClinico()
        {
            // Verifica si la consulta está finalizada, se envia los datos a la HistoriaClinica
            if (ConsultaEstado == "finalizada")
            {
                return true;
            }
            else
            {
                // Si la consulta no está finalizada, no se pueden enviar los datos a la HistoriaClinica
                return false;
            }
        }
       
        /*Regla 5:*/
        public bool ModificarAsistencia(bool estaAsistiendo)
        {
            // Verificar si la consulta está finalizada
            if (ConsultaEstado == "Finalizado")
            {
                return false; 
            }

            // Si la consulta no está finalizada, se puede modificar la asistencia
            if (estaAsistiendo)
            {
                ConsultaEstado = "Asistencia"; 
            }
            else
            {
                ConsultaEstado = "Inasistencia"; 
            }

            return true;
        }

        /*Regla 6 en RecetaMedica: ValidarEmisionRecetaMedica*/

        /*Regla 7 en Historia Clinica: RegistrarCambioHistorialClinico:*/
    }
    
}
