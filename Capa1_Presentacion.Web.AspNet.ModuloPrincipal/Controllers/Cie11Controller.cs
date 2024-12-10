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

        

        // POST: Filtra los datos mientras el usuario escribe
        [HttpPost]
        public async Task<JsonResult> FiltrarDatos(string termino)
        {
            try
            {
                // Valida que no sea nulo o vacío
                if (string.IsNullOrWhiteSpace(termino))
                {
                    return Json(new { success = false, message = "Por favor, ingrese un término para filtrar." });
                }

                // Busca el término utilizando la capa de aplicación
                var resultados = await cie11Servicio.BuscarTermino(termino);

                // Devuelve solo los primeros 50 resultados para optimizar el rendimiento
                var resultadosLimitados = resultados.Take(50).ToList();

                return Json(new { success = true, data = resultadosLimitados });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al filtrar los datos: {ex.Message}" });
            }
        }
    }
}