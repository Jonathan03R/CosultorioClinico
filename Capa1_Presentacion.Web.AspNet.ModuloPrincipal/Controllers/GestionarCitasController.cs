using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa3_Dominio.ModuloPrincipal.TransferenciaDatos;

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


        [HttpGet]
        public JsonResult ListarMedicosConEspecialidad()
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaMedicosFormatada;

            try
            {
                // Llama al método del servicio
                var listaMedicos = gestionarCitaServicio.ObtenerMedicosConEspecialidad();

                // Formatea los datos para el frontend
                listaMedicosFormatada = listaMedicos.Select(m => new
                {
                    MedicoCodigo = m.MedicoCodigo,
                    MedicoNombre = $"{m.MedicoNombre} {m.MedicoApellido}",
                    EspecialidadNombre = m.Especialidad.EspecialidadNombre,
                }).ToList<object>();

                accionExitosa = true;
                mensajeRetorno = "Consulta exitosa.";
            }
            catch (Exception ex)
            {
                listaMedicosFormatada = new List<object>();
                accionExitosa = false;
                mensajeRetorno = $"Error: {ex.Message}";
            }

            return Json(new { data = listaMedicosFormatada, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        // Obtener todas las citas
        [HttpGet]
        public JsonResult ObtenerTodaslasCitas()
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaCitaFormateada;

            try
            {
                var listaCitas = gestionarCitaServicio.ObtenerTodasCitas();
                listaCitaFormateada = listaCitas.Select(c => new{
                    CitaCodigo = c.CitaCodigo,
                    Fecha = c.CitaFechaHora.ToString("dd-MM-yyyy HH:mm:ss"),
                    TipoConsulta = c.CitaTipoConsulta.TipoConsultaCodigo,
                    MedicoNombre = $"Dr. {c.CitaMedico.MedicoNombre} {c.CitaMedico.MedicoApellido}",
                    Especialidad = c.CitaMedico.Especialidad.EspecialidadNombre,
                    Estado = GetEstadoDescripcion(c.CitaEstado), 
                    CodigoPaciente = c.CitaPaciente.PacienteCodigo,
                    NombrePaciente = c.CitaPaciente.PacienteNombreCompleto,
                    EspecialidadCod = c.CitaMedico.Especialidad.EspecialidadCodigo,
                    MedicoCodigo = c.CitaMedico.MedicoCodigo
                }).ToList<object>();
                accionExitosa = true;
                mensajeRetorno = "Consulta exitosa.";
            }
            catch (Exception ex)
            {
                listaCitaFormateada  = null;
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { data = listaCitaFormateada, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }


        private string GetEstadoDescripcion(string estado)
        {
            if (estado == "P")
                return "Pendiente";
            else if (estado == "N")
                return "No Asistida";
            else if (estado == "A")
                return "Atendida";
            else if (estado == "C")
                return "Cancelada";
            else
                return "N/A";
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
        public JsonResult RegistrarCita( CitaDTO citaDTO)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                var cita = new Cita
                {
                    //CitaCodigo = citaDTO.CitaCodigo,
                    CitaFechaHora = citaDTO.CitaFechaHora,
                    CitaEstado = "P" 
                };

                // Crear las instancias de los objetos relacionados
                cita.CitaPaciente = new Paciente { PacienteCodigo = citaDTO.PacienteCodigo };
                cita.CitaMedico = new Medico { MedicoCodigo = citaDTO.MedicoCodigo };
                cita.CitaTipoConsulta = new TipoConsulta { TipoConsultaCodigo = citaDTO.TipoConsultaCodigo };


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

        [HttpGet]
        public JsonResult ObtenerPacientePorDni(string dni)
        {
            bool accionExitosa;
            string mensajeRetorno;
            Paciente paciente = new Paciente { PacienteDNI = dni };

            try
            {
                paciente = gestionarCitaServicio.obtenerPacientePorDni(paciente);
                accionExitosa = true;
                mensajeRetorno = "";
            }
            catch (Exception ex)
            {
                paciente = null;
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { data = paciente, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
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


