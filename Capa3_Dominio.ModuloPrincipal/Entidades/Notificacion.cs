using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Notificacion
    {
        private string notificacionCodigo, notificacionMensaje, notificacionDestinatario;
        private DateTime notificacionFechaDeEnvio;



        public string NotificacionCodigo { get => notificacionCodigo; set => notificacionCodigo = value; }
        public string NotificacionMensaje { get => notificacionMensaje; set => notificacionMensaje = value; }
        public string NotificacionDestinatario { get => notificacionDestinatario; set => notificacionDestinatario = value; }
        public DateTime NotificacionFechaDeEnvio { get => notificacionFechaDeEnvio; set => notificacionFechaDeEnvio = value; }


        public void Enviar()
        {

            if (string.IsNullOrWhiteSpace(notificacionDestinatario) || !notificacionDestinatario.Contains("@"))
            {
                Console.WriteLine("Dirección de correo electrónico no válida.");
                return;
            }


            Console.WriteLine($"Enviando correo a {notificacionDestinatario}: {notificacionMensaje}");

        }
    }
}




