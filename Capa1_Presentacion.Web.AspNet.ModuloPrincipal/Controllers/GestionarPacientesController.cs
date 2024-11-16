using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class GestionarPacientesController : Controller
    {
        private readonly GestionarPacienteServicio gestionarPacienteServicio;
        public GestionarPacientesController()
        {
            // Instanciación directa del servicio 
            gestionarPacienteServicio = new GestionarPacienteServicio();
        }

        public ActionResult ListaPacientes()
        {
            return View();
        }

        // Listar pacientes activos
        [HttpGet]
        public JsonResult ListarPacientes()
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaPacientesFormatada;

            try
            {
                var listaPacientes = gestionarPacienteServicio.listarPacientes();

                listaPacientesFormatada = listaPacientes.Select(p => new {
                    PacienteCodigo = p.Paciente?.PacienteCodigo ?? "null",
                    PacienteNombreCompleto = p.Paciente?.PacienteNombreCompleto ?? "null",
                    PacienteEstado = p.Paciente?.PacienteEstado == "A" ? "Activo" : "Inactivo",
                    PacienteTelefono = p.Paciente?.PacienteTelefono ?? "null",
                    PacienteDNI = p.Paciente?.PacienteDNI ?? "null",
                    PacienteHistorialClinicoCodigo = p.HistorialClinicoCodigo ?? "null",
                    PacienteSeguro = "Sin seguro",
                    PacienteFechaActivacion = DateTime.Now.ToString("yyyy-MM-dd"),
                    PacienteNotas = "No hay notas adicionales"
                }).ToList<object>();

                accionExitosa = true;
                mensajeRetorno = "Consulta exitosa";
            }
            catch (Exception ex)
            {
                listaPacientesFormatada = new List<object>();
                accionExitosa = false;
                mensajeRetorno = $"Error: {ex.Message}";
            }

            return Json(new { data = listaPacientesFormatada, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        // Registrar un nuevo paciente
        [HttpPost]
        public JsonResult RegistrarPaciente(Paciente paciente)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                gestionarPacienteServicio.RegistrarPacienteConHistoria(paciente);
                accionExitosa = true;
                mensajeRetorno = "Paciente registrado exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno });
        }


        // Actualizar un paciente existente
        [HttpPost]
        public JsonResult ActualizarPaciente(Paciente paciente)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                gestionarPacienteServicio.ActualizarPaciente(paciente);
                accionExitosa = true;
                mensajeRetorno = "Paciente actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno });
        }


        // Eliminar a un paciente (Solo se cambia a incativo 🤡)
        [HttpPost]
        public JsonResult EliminarPaciente(string PacienteCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                var paciente = new Paciente
                {
                    PacienteCodigo = PacienteCodigo
                };

                gestionarPacienteServicio.cambiarEstadoInactivoPaciente(paciente);

                accionExitosa = true;
                mensajeRetorno = "Paciente marcado como inactivo exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno });
        }
    }
}