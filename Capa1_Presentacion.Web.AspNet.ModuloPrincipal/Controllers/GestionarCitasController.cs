using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal.Entidad;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class GestionarCitasController : Controller
    {
        private readonly GestionarCitaServicio gestionarCitaServicio;

        public GestionarCitasController()
        {
            gestionarCitaServicio = new GestionarCitaServicio();
        }

        // Página principal de gestión de citas
        public ActionResult GestionCita()
        {
            return View();
        }

        // Listar citas de un paciente específico
        [HttpGet]
        public JsonResult ObtenerCitasPorPaciente(string pacienteCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<Cita> listaCitas;

            try
            {
                listaCitas = gestionarCitaServicio.ObtenerCitasPorPaciente(pacienteCodigo);
                accionExitosa = true;
                mensajeRetorno = "";
            }
            catch (Exception ex)
            {
                listaCitas = null;
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { data = listaCitas, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        // Registrar una nueva cita
        [HttpPost]
        public JsonResult RegistrarCita(Cita cita)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                gestionarCitaServicio.RegistrarCita(cita);
                accionExitosa = true;
                mensajeRetorno = "Cita registrada exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        // Actualizar una cita existente
        [HttpPost]
        public JsonResult ActualizarCita(Cita cita)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                gestionarCitaServicio.ActualizarCita(cita);
                accionExitosa = true;
                mensajeRetorno = "Cita actualizada exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        // Obtener detalles de una cita específica por código
        [HttpGet]
        public JsonResult ObtenerCitaPorId(string citaCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;
            Cita cita;

            try
            {
                cita = gestionarCitaServicio.ObtenerCitaPorId(citaCodigo);
                accionExitosa = true;
                mensajeRetorno = "";
            }
            catch (Exception ex)
            {
                cita = null;
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { data = cita, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        // Cancelar una cita
        [HttpPost]
        public JsonResult CancelarCita(string citaCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                gestionarCitaServicio.CancelarCita(citaCodigo);
                accionExitosa = true;
                mensajeRetorno = "Cita cancelada exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }
    }
}


