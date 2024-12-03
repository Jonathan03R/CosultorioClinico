using Capa2_Aplicacion.ModuloPrincipal.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class Cie11Controller : Controller
    {
        // GET: Cie11
        private readonly Cie11Servicio cie11Servicio;

        public Cie11Controller()
        {
            cie11Servicio = new Cie11Servicio(); // Instancia el servicio de la capa de aplicación
        }

        // Vista para buscar términos en CIE-11
        public ActionResult Buscar()
        {
            return View();
        }

        // POST: Realiza la búsqueda en la API y retorna resultados
        [HttpPost]
        public async Task<JsonResult> BuscarTermino(string termino)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termino))
                {
                    return Json(new { success = false, message = "El término de búsqueda no puede estar vacío." });
                }

                // Llama a la capa de aplicación para buscar el término
                var resultados = await cie11Servicio.BuscarTermino(termino);

                return Json(new { success = true, data = resultados });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error en la capa de presentación: {ex.Message}" });
            }
        }
    }
}