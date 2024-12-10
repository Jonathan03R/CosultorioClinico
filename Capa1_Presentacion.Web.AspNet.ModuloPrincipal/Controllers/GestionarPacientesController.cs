using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
                    PacienteCodigo = p.PacienteCodigo ?? "null",
                    PacienteDNI = p.PacienteDNI ?? "null",
                    PacienteNombreCompleto = p.PacienteNombreCompleto ?? "null",
                    PacienteFechaNacimiento = p.PacienteFechaNacimiento.ToString("yyyy-MM-dd") ?? "null",
                    PacienteDireccion = p.PacienteDireccion ?? "null",
                    PacienteTelefono = p.PacienteTelefono ?? "null",
                    PacienteCorreoElectronico = p.PacienteCorreoElectronico ?? "null",
                    PacienteEstado = p.PacienteEstado == "A" ? "Activo" : "Inactivo",
                    PacienteHistorialClinicoCodigo = p.HistoriaClinica.HistorialClinicoCodigo ?? "null",
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

        [HttpPost]
        public JsonResult RegistrarPaciente(Paciente paciente, List<ContactoEmergencia> contactoEmergencia)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
              
                gestionarPacienteServicio.RegistrarPacienteConHistoria(paciente, contactoEmergencia);
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
                // Llamar al servicio para actualizar el paciente directamente
                gestionarPacienteServicio.ActualizarPaciente(paciente);

                accionExitosa = true;
                mensajeRetorno = "Paciente actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = $"Error al actualizar el paciente: {ex.Message}";
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno });
        }


        [HttpPost]
        public JsonResult ActualizarContactoEmergencia(ContactoEmergencia contactoEmergencia)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                gestionarPacienteServicio.ActualizarContactoEmergencia(contactoEmergencia);

                accionExitosa = true;
                mensajeRetorno = "Contacto de emergencia actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = $"Error al actualizar el contacto de emergencia: {ex.Message}";
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

        [HttpPost]
        public JsonResult RecuperarPaciente(string PacienteCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                var paciente = new Paciente
                {
                    PacienteCodigo = PacienteCodigo
                };

                gestionarPacienteServicio.cambiarEstadoActivosPaciente(paciente);

                accionExitosa = true;
                mensajeRetorno = "Paciente marcado como Activo exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno });
        }

        [HttpGet]
        public JsonResult ListarContactosPorPaciente(string pacienteCodigo)
        {
            try
            {
                var contactos = gestionarPacienteServicio.ListarContactoDeEmergencias(pacienteCodigo);
                return Json(new { success = true, data = contactos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}