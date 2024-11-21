using Capa3_Dominio.ModuloPrincipal.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal
{
    internal class Consulta
    {
        private string consultaCodigo;
        private DateTime consultaFechaHora;
        private string consultaMedicoCodigo;
        private string consultaPacienteCodigo;
        private string consultaMotivo;
        private string consultaEstado;

        public string ConsultaCodigo { get => consultaCodigo; set => consultaCodigo = value; }
        public DateTime ConsultaFechaHora { get => consultaFechaHora; set => consultaFechaHora = value; }
        public string ConsultaMedicoCodigo { get => consultaMedicoCodigo; set => consultaMedicoCodigo = value; }
        public string ConsultaPacienteCodigo { get => consultaPacienteCodigo; set => consultaPacienteCodigo = value; }
        public string ConsultaMotivo { get => consultaMotivo; set => consultaMotivo = value; }
        public string ConsultaEstado { get => consultaEstado; set => consultaEstado = value; }

        /*Regla 2:*/
        public bool EsDatosValidosConsulta()
        {
            bool esCodigoValido = !string.IsNullOrEmpty(consultaCodigo);
            bool esFechaValida = consultaFechaHora != DateTime.MinValue;
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

        /*Regla 6:*/
        public bool ValidarEmisionRecetaMedica(RecetaMedica receta)
        {
            // Validar si la receta corresponde a la consulta realizada
            if (this.ConsultaCodigo == receta.RecetaConsultaCodigo)
            {
                return true; 
            }
            return false; 
        }

        /*Regla 7:*/
        public bool RegistrarCambioHistorialClinico(HistoriaClinica historiaClinica)
        {
            // Validación simple para verificar si el objeto historiaClinica es válido
            if (historiaClinica != null && historiaClinica.FechaActualizacion != default(DateTime))
            {
                return true;
            }

            // Si la fecha no es correcta, no se realiza el cambio
            return false;
        }


        // Regla 09: Filtra consultas programadas para un médico en un día específico
        public static List<Consulta> ObtenerConsultasDelDia(List<Consulta> consultas, string medicoCodigo, DateTime fecha)
        {
            return consultas.Where(c =>
                c.ConsultaMedicoCodigo == medicoCodigo &&
                c.ConsultaFechaHora.Date == fecha.Date &&
                c.ConsultaEstado != "Cancelada"
            ).ToList();
        }
    }
    
}
