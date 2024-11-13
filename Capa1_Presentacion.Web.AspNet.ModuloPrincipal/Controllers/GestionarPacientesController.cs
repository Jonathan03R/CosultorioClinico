using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal;
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

        // Constructor con inyección de dependencias
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
        public JsonResult ListarPacientesActivos()
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaPacientesFormatada;

            try
            {
                // Obtenemos los pacientes
                var listaPacientes = gestionarPacienteServicio.listarPacientesActivos();

                // Formateamos los datos para la tabla
                listaPacientesFormatada = listaPacientes.Select(p => new {
                    PacienteCodigo = p.Paciente?.PacienteCodigo ?? "null",
                    PacienteNombreCompleto = p.Paciente?.PacienteNombreCompleto ?? "null",
                    PacienteEstado = p.Paciente?.PacienteEstado == "A" ? "Activo" : "Inactivo",
                    PacienteTelefono = p.Paciente?.PacienteTelefono ?? "null",
                    PacienteDNI = p.Paciente?.PacienteDNI ?? "null",
                    PacienteHistorialClinicoCodigo = p.HistorialClinicoCodigo ?? "null", // Nombre coincidente con JS
                    PacienteSeguro = "Sin seguro", // Valor predeterminado
                    PacienteFechaActivacion = DateTime.Now.ToString("yyyy-MM-dd"), // Valor de ejemplo
                    PacienteNotas = "No hay notas adicionales" // Valor de ejemplo
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

   
    }
}