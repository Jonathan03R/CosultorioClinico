using System;
using System.Collections.Generic;
using System.Linq;
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

        // Listar Especialidades
        [HttpGet]
        public JsonResult ListarEspecialidades()
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaEspecialidadesFormatada;

            try
            {
                var listaEspecialidades = gestionarCitaServicio.ListarEspecialidades();

                listaEspecialidadesFormatada = listaEspecialidades.Select(e => new
                {
                    EspecialidadCodigo = e.EspecialidadCodigo ?? "null",
                    EspecialidadNombre = e.EspecialidadNombre ?? "null",
                    EspecialidadDescripcion = string.IsNullOrEmpty(e.EspecialidadDescripcion) ? "Sin descripción" : e.EspecialidadDescripcion
                }).ToList<object>();

                accionExitosa = true;
                mensajeRetorno = "Consulta exitosa.";
            }
            catch (Exception ex)
            {
                listaEspecialidadesFormatada = new List<object>();
                accionExitosa = false;
                mensajeRetorno = $"Error: {ex.Message}";
            }

            return Json(new { data = listaEspecialidadesFormatada, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        // Listar Tipos de Consulta
        [HttpGet]
        public JsonResult ListarTiposDeConsulta()
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaTiposConsultaFormatada;

            try
            {
                var listaTiposConsulta = gestionarCitaServicio.TipoConsulta();

                listaTiposConsultaFormatada = listaTiposConsulta.Select(tc => new
                {
                    TipoConsultaCodigo = tc.TipoConsultaCodigo ?? "null",
                    TipoConsultaDescripcion = string.IsNullOrEmpty(tc.TipoConsultaDescripcion) ? "Sin nombre" : tc.TipoConsultaDescripcion
                }).ToList<object>();

                accionExitosa = true;
                mensajeRetorno = "Consulta exitosa.";
            }
            catch (Exception ex)
            {
                listaTiposConsultaFormatada = new List<object>();
                accionExitosa = false;
                mensajeRetorno = $"Error: {ex.Message}";
            }

            return Json(new { data = listaTiposConsultaFormatada, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }


    }
}


