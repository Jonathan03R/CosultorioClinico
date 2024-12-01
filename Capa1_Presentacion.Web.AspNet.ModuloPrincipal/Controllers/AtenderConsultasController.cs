using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class AtenderConsultasController : Controller
    {
        // GET: AtenderConsultas
        public ActionResult Consultas()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListarCosultas()
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaMedicosFormatada;

            try
            {
               
            }
            catch (Exception ex)
            {
                listaMedicosFormatada = new List<object>();
                accionExitosa = false;
                mensajeRetorno = $"Error: {ex.Message}";
            }

            return null;
        }


    }
}