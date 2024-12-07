using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa2_Aplicacion.ModuloPrincipal.Servicios;
using Capa3_Dominio.ModuloPrincipal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class AtenderConsultasController : Controller
    {
        private readonly AtenderConsultaServicio atenderConsultaServicio;

        public AtenderConsultasController()
        {
            atenderConsultaServicio = new AtenderConsultaServicio();
        }
        // GET: AtenderConsultas
        public ActionResult Consultas()
        {
            return View();
        }


        // Listar las consultas del día
        [HttpGet]
        public JsonResult ListarConsultas()
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaConsultasFormatada;

            try
            {
                List<Consulta> consultasDelDia = atenderConsultaServicio.MostrarConsultasDelDia();
                listaConsultasFormatada = consultasDelDia.Select(c => new
                {
                    ConsultaCodigo = c.ConsultaCodigo,
                    CitaCiodigo = c.Cita.CitaCodigo,
                    ConsultaFechaCita = c.Cita.CitaFechaHora.ToString("dd-MMMM-yyyy"),
                    ConsultaHoraFecha = c.Cita.CitaFechaHora.ToString("hh:mm tt"),
                    ConsultaFechaHoraFinal = c.ConsultaFechaHoraFinal.HasValue
                    ? c.ConsultaFechaHoraFinal.Value.ToString("dd-MM-yyyy HH:mm:ss")
                    : null,
                    MedicoNombre = $"{c.Cita.CitaMedico.MedicoNombre} {c.Cita.CitaMedico.MedicoApellido}",
                    PacienteNombre = c.Cita.CitaPaciente.PacienteNombreCompleto,
                    ConsultaMotivo = c.ConsultaMotivo,
                    ConsultaEstado = GetEstadoCita(c.Cita.CitaEstado),
                }).ToList<object>();

                accionExitosa = true;
                mensajeRetorno = "Consulta exitosa";
            }
            catch (Exception ex)
            {
                listaConsultasFormatada = new List<object>();
                accionExitosa = false;
                mensajeRetorno = $"Error: {ex.Message}";
            }

            // Retornar el resultado como JSON
            return Json(new { data = listaConsultasFormatada, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        private string GetEstadoCita(string estado)
        {
            if (estado == "P")
                return "Pendiente";
            else if (estado == "N")
                return "No asistio";
            else if (estado == "A")
                return "Atendido";
            else if (estado == "C")
                return "Cancelado";
            else
                return "Desconocido";
        }
    }
}