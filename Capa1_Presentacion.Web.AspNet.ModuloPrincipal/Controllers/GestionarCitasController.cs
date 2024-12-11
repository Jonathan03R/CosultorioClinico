using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa2_Aplicacion.ModuloPrincipal.Servicios;
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

        public ActionResult GestionCitaEliminar()
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
                    CitaCodigo = c.Cita.CitaCodigo,
                    Fecha = c.Cita.CitaFechaHora.ToString("dd-MM-yyyy HH:mm:ss"),
                    TipoConsulta = c.TipoConsulta.TipoConsultaDescripcion,
                    MedicoNombre = $"Dr. {c.Medico.MedicoNombre} {c.Medico.MedicoApellido}",
                    Especialidad = c.Medico.Especialidad.EspecialidadNombre,
                    Estado = GetEstadoDescripcion(c.Cita.CitaEstado), 
                    CodigoPaciente = c.Paciente.PacienteCodigo,
                    NombrePaciente = c.Paciente.PacienteNombreCompleto,
                    EspecialidadCod =c.Medico.Especialidad.EspecialidadCodigo,
                    MedicoCodigo = c.Medico.MedicoCodigo
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


        // Listar los horarios con citas para una especialidad y fecha específica
        [HttpGet]
        public JsonResult ListarHorariosConCitas(string especialidadCodigo, string fecha)
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaHorariosFormatada;

            try
            {
                DateTime fechaConsulta = DateTime.Parse(fecha);
                List<HorarioConCita> horariosConCitas = gestionarCitaServicio.MostrarHorariosConCitas(especialidadCodigo, fechaConsulta);
                listaHorariosFormatada = horariosConCitas.Select(h => new
                {
                    MedicoCodigo = h.MedicoCodigo,
                    HoraInicio = h.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = h.HoraFin.ToString(@"hh\:mm"),
                    ConsultaCodigo = h.ConsultaCodigo,
                    CitaCodigo = h.CitaCodigo,
                    PacienteCodigo = h.PacienteCodigo,
                    CitaEstado = GetEstadoDescripcion(h.CitaEstado) ,
                    NombreMedico = h.MedicoNombre,
                    NombrePaciente = h.PacienteNombre
                }).ToList<object>();

                accionExitosa = true;
                mensajeRetorno = "Consulta exitosa";
            }
            catch (Exception ex)
            {
                listaHorariosFormatada = new List<object>();
                accionExitosa = false;
                mensajeRetorno = $"Error: {ex.Message}";
            }

            // Retornar el resultado como JSON
            return Json(new { data = listaHorariosFormatada, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }


        // Listar citas de un paciente específico
        [HttpGet]
        public JsonResult ObtenerCitasPorPaciente(string pacienteCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<Consulta> listaCitas;

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
                if (citaDTO.CitaFechaHora == default(DateTime) || citaDTO.CitaFechaHora < new DateTime(1753, 1, 1))
                {
                    throw new ArgumentException("La fecha y hora de la cita es inválida.");
                }

                var consulta = new Consulta
                {
                    Cita = new Cita()
                    {
                        CitaFechaHora = citaDTO.CitaFechaHora,
                        CitaEstado = "P"// por seacaso le puse p pero no es necasrio por default es P 
                    },
                    Paciente = new Paciente()
                    {
                        PacienteCodigo = citaDTO.PacienteCodigo,
                    },
                    Medico = new Medico()
                    {
                        MedicoCodigo = citaDTO.MedicoCodigo,
                    },
                    TipoConsulta = new TipoConsulta()
                    {
                        TipoConsultaCodigo = citaDTO.TipoConsultaCodigo
                    }
                };

                gestionarCitaServicio.RegistrarCita(consulta);
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
            Consulta consulta;

            try
            {
                consulta = gestionarCitaServicio.ObtenerCitaPorId(citaCodigo);
                accionExitosa = true;
                mensajeRetorno = "";
            }
            catch (Exception ex)
            {
                consulta = null;
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { data = consulta, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerPacientePorDni(string dni)
        {
            bool accionExitosa;
            string mensajeRetorno;
            object pacienteFormateado;

            try
            {
                // Obtener el paciente utilizando el servicio
                Paciente paciente = gestionarCitaServicio.obtenerPacientePorDni(new Paciente { PacienteDNI = dni });

                // Formatear los datos como un objeto anónimo antes de enviarlo
                if (paciente != null)
                {
                    pacienteFormateado = new
                    {
                        PacienteCodigo = paciente.PacienteCodigo,
                        PacienteDNI = paciente.PacienteDNI,
                        PacienteNombreCompleto = paciente.PacienteNombreCompleto,
                        PacienteFechaNacimiento = paciente.PacienteFechaNacimiento.ToString("dd-MM-yyyy"), // Formato de fecha
                        PacienteDireccion = paciente.PacienteDireccion ?? "N/A", // Si no tiene dirección, coloca "N/A"
                        PacienteTelefono = paciente.PacienteTelefono ?? "N/A", // Lo mismo para teléfono
                        PacienteCorreoElectronico = paciente.PacienteCorreoElectronico ?? "N/A", // Lo mismo para correo
                        PacienteEstado = GetEstadoDescripcionDni(paciente.PacienteEstado) // Llamada al método para obtener el estado
                    };
                }
                else
                {
                    pacienteFormateado = null;
                }

                accionExitosa = true;
                mensajeRetorno = paciente != null ? "Consulta exitosa." : "Paciente no encontrado.";
            }
            catch (Exception ex)
            {
                pacienteFormateado = null;
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { data = pacienteFormateado, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        private string GetEstadoDescripcionDni(string estado)
        {
            if (estado == "A")
                return "Activo";
            else if (estado == "I")
                return "Inactivo";
            else
                return "Desconocido"; 
        }

        // Cancelar una cita
        [HttpPost]
        public JsonResult CancelarCita(string citaCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                gestionarCitaServicio.cambiarEstadoActivoConsultaCancelada(citaCodigo);
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


