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
        private DateTime consultaFechaHoraFinal;
        private string consultaMedicoCodigo;
        private string consultaPacienteCodigo;
        private string consultaMotivo;
        private string consultaEstado;

        private Paciente paciente;
        private Medico medicos;
        private Cita cita;

        public string ConsultaCodigo { get => consultaCodigo; set => consultaCodigo = value; }
        public DateTime ConsultaFechaHoraFinal { get => consultaFechaHoraFinal; set => consultaFechaHoraFinal = value; }
        public string ConsultaMedicoCodigo { get => consultaMedicoCodigo; set => consultaMedicoCodigo = value; }
        public string ConsultaPacienteCodigo { get => consultaPacienteCodigo; set => consultaPacienteCodigo = value; }
        public string ConsultaMotivo { get => consultaMotivo; set => consultaMotivo = value; }
        public string ConsultaEstado { get => consultaEstado; set => consultaEstado = value; }
        public Paciente Paciente { get => paciente; set => paciente = value; }
        public Medico Medicos { get => medicos; set => medicos = value; }
        public Cita Cita { get => cita; set => cita = value; }



        /*Regla 2:*/
        public bool EsDatosValidosConsulta()
        {
            bool esCodigoValido = !string.IsNullOrEmpty(consultaCodigo);
            bool esFechaValida = consultaFechaHoraFinal != DateTime.MinValue;
            bool esMedicoCodigoValido = !string.IsNullOrEmpty(consultaMedicoCodigo);
            bool esPacienteCodigoValido = !string.IsNullOrEmpty(consultaPacienteCodigo);
            bool esMotivoValido = !string.IsNullOrEmpty(consultaMotivo);
            bool esEstadoValido = !string.IsNullOrEmpty(consultaEstado);

            // Retorna true si todos los datos son válidos
            return esCodigoValido && esFechaValida && esMedicoCodigoValido && esPacienteCodigoValido && esMotivoValido && esEstadoValido;
        }

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
            if (consultaEstado == "Finalizado")
            {
                return false; 
            }

            // Si la consulta no está finalizada, se puede modificar la asistencia
            if (estaAsistiendo)
            {
                consultaEstado = "Asistencia"; 
            }
            else
            {
                consultaEstado = "Inasistencia"; 
            }

            return true;
        }

        /*Regla 6 en RecetaMedica: ValidarEmisionRecetaMedica*/

        /*Regla 7 en Historia Clinica: RegistrarCambioHistorialClinico:*/
    }
    
}
