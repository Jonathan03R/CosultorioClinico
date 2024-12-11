using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa2_Aplicacion.ModuloPrincipal.Servicios;
using Capa3_Dominio.ModuloPrincipal;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa3_Dominio.ModuloPrincipal.Entidades;
using Capa3_Dominio.ModuloPrincipal.TransferenciaDatos;
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

        public ActionResult Atendiendo()
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
                    CitaCodigo = c.Cita.CitaCodigo,
                    ConsultaFechaCita = c.Cita.CitaFechaHora.ToString("dd-MMMM-yyyy"),
                    ConsultaHoraFecha = c.Cita.CitaFechaHora.ToString("hh:mm tt"),
                    ConsultaFechaHoraFinal = c.ConsultaFechaHoraFinal.HasValue
                    ? c.ConsultaFechaHoraFinal.Value.ToString("dd-MM-yyyy HH:mm:ss")
                    : null,
                    MedicoNombre = $"{c.Medico.MedicoNombre} {c.Medico.MedicoApellido}",
                    PacienteNombre = c.Paciente.PacienteNombreCompleto,
                    ConsultaEstado = GetEstadoCita(c.Cita.CitaEstado),
                    FechaCitaFilter = c.Cita.CitaFechaHora.ToString("dd-MM-yyyy HH:mm:ss"),
                    HistoriaClinica = c.HistoriaClinica.HistorialClinicoCodigo, 
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


        // Actualizar una cita existente
        [HttpPost]
        public JsonResult ActualizarEstadoConsultaProceso(string consultaCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                atenderConsultaServicio.cambiarEstadoAtencionEnProceso(consultaCodigo);
                accionExitosa = true;
                mensajeRetorno = "Atendiendo Citas";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActualizarEstadoFinalizadoConsulta(string citaCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                atenderConsultaServicio.cambiarEstadoActivoConsultaAtendido(citaCodigo);
                accionExitosa = true;
                mensajeRetorno = "Consulta Finalizada";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }


        //detalles de la historia medica 
        [HttpPost]
        public JsonResult ObtenerDetallesConsulta(string HistorialClinicoCodigo) 
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaConsultasFormateada = null;
            try
            {
                var listaConsultas = atenderConsultaServicio.historiaClinicaDetalles(HistorialClinicoCodigo);
                listaConsultasFormateada = listaConsultas.Select(a => new { 
                    consultaCodigo = a.ConsultaCodigo,
                    consultaFechaFinal = a.ConsultaFechaHoraFinal,
                    Cita = new 
                    {
                        citaCodigo = a.Cita.CitaCodigo,
                        citaFecha = a.Cita.CitaFechaHora,
                        citaEstado = a.Cita.CitaEstado,
                    },
                    Medico = new 
                    {
                        medicoCodigo = a.Medico.MedicoCodigo
                    },
                    Paciente = new 
                    {
                        pacienteCodigo = a.Paciente.PacienteCodigo
                    },
                    TipoConsulta = new
                    {
                        tipoConsultaCodigo = a.TipoConsulta.TipoConsultaCodigo
                    },
                    diagnosticos = a.Diagnosticos1.Select(d => new { 
                        diagnosticoCodigo = d.DiagnosticoCodigo,
                        diagnosticoDescripcion = d.DiagnosticoDescripcion
                    }).ToList(),
                    recetas = a.RecetasMedicas1
                        .Where(r => a.Cita.CitaEstado != "P" && a.Cita.CitaEstado != "T")  // Filtrar por CitaEstado
                        .Select(r => new
                        {
                            recetaCodigo = r.RecetaCodigo,
                            recetaDescripcion = r.RecetaDescripcion,
                            recetaTratamiento = r.RecetaTratamiento,
                            recetaRecomendaciones = r.RecetaRecomendaciones
                        }).ToList()
                }).ToList<object>();

                accionExitosa = true;
                mensajeRetorno = "historial Clinica devuelta correctamente";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno, data = listaConsultasFormateada }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ObtenerDatosPaciente(string pacienteCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;
            object datosPaciente = null;

            try
            {
                // Llama al método del servicio para obtener los datos del paciente
                var paciente = atenderConsultaServicio.DatosPaciente(pacienteCodigo);
                int edad = CalcularEdad(paciente.PacienteFechaNacimiento);
                // Mapea los datos del paciente al formato requerido
                datosPaciente = new
                {
                    pacienteCodigo = paciente.PacienteCodigo,
                    nombreCompleto = paciente.PacienteNombreCompleto,
                    fechaNacimiento = paciente.PacienteFechaNacimiento.ToString("dd/MM/yyyy"),
                    //sexo = paciente.Sexo,
                    direccion = paciente.PacienteDireccion,
                    telefono = paciente.PacienteTelefono,
                    email = paciente.PacienteCorreoElectronico,
                    dni = paciente.PacienteDNI,
                    edad = edad,
                };

                accionExitosa = true;
                mensajeRetorno = "Datos del paciente obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }

            // Retorna la respuesta en formato JSON
            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno, data = datosPaciente }, JsonRequestBehavior.AllowGet);
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            var today = DateTime.Today;
            int edad = today.Year - fechaNacimiento.Year;

            // Verifica si ya pasó el cumpleaños este año
            if (fechaNacimiento.Date > today.AddYears(-edad))
            {
                edad--;
            }

            return edad;
        }

        [HttpPost]
        public JsonResult ListarCitasPrevias(string pacienteCodigo)
        {
            bool accionExitosa;
            string mensajeRetorno;
            List<object> listaCitasPreviasFormatada;

            try
            {
                List<Consulta> citasPrevias = atenderConsultaServicio.ConsultasPrevias(pacienteCodigo);

                // Formatear las citas previas para el retorno
                listaCitasPreviasFormatada = citasPrevias.Select(c => new
                {
                    CitaCodigo = c.Cita.CitaCodigo,
                    CitaEstado = GetEstadoCita(c.Cita.CitaEstado),
                    CitaFecha = c.Cita.CitaFechaHora.ToString("dd 'de' MMMM, yyyy"), 
                    CitaHora = c.Cita.CitaFechaHora.ToString("hh:mm tt"),
                    MedicoNombre = $"{c.Medico.MedicoNombre} {c.Medico.MedicoApellido}",
                    TipoConsulta = c.TipoConsulta.TipoConsultaDescripcion,
                }).ToList<object>();

                accionExitosa = true;
                mensajeRetorno = "Citas previas obtenidas correctamente.";
            }
            catch (Exception ex)
            {
                listaCitasPreviasFormatada = new List<object>();
                accionExitosa = false;
                mensajeRetorno = $"Error: {ex.Message}";
            }

            // Retornar el resultado como JSON
            return Json(new { data = listaCitasPreviasFormatada, consultaExitosa = accionExitosa, mensaje = mensajeRetorno }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RegistrarDetallesConsulta(DetallesConsultaDTO detallesConsulta)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                var detaConsulta = new DetallesConsulta()
                {
                    DetallesConsultaEvaluacionPsico1 = detallesConsulta.DetallesConsultaEvaluacionPsico,
                    DetallesConsultaHistoriaEnfermedad1 = detallesConsulta.DetallesConsultaHistoriaEnfermedad,
                    DetallesConsultaMotivoConsulta1 = detallesConsulta.DetallesConsultaMotivoConsulta,
                    DetallesConsultaRevisiones1 = detallesConsulta.DetallesConsultaRevisiones,
                    Consulta = new Consulta() {
                        ConsultaCodigo = detallesConsulta.CodigoConsulta,
                    }
                };

                atenderConsultaServicio.RegistrarDetallesConsulta(detaConsulta);


                accionExitosa = true;
                mensajeRetorno = "Detalles de consulta registrados exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }
            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno });
        }


        [HttpPost]
        public JsonResult RegistrarRecetaMedica(RecetaMedicaDTO registrarRecetaMedica)
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                var registrarRec = new RecetaMedica()
                {
                    RecetaDescripcion = registrarRecetaMedica.recetaDescripcion,
                    RecetaTratamiento = registrarRecetaMedica.recetaTratamiento,
                    RecetaRecomendaciones = registrarRecetaMedica.recetaRecomendaciones,
                    Consulta = new Consulta()
                    {
                        ConsultaCodigo = registrarRecetaMedica.codigoConsulta,
                    }
                };

                atenderConsultaServicio.RegistrarRecetasMedicas(registrarRec);


                accionExitosa = true;
                mensajeRetorno = "Receta medica registrada exitosamente.";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }
            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno });
        }



        [HttpPost]
        public JsonResult RegistrarDiagnostico(DiagnosticoDTO  diagnosticoDTO )
        {
            bool accionExitosa;
            string mensajeRetorno;

            try
            {
                var dataDiagnostico = new Diagnostico()
                {
                    DiagnosticoCie11 = diagnosticoDTO.DiagnosticoCie11,
                    DiagnosticoDescripcion = diagnosticoDTO.DiagnosticoDescripcion,
                    Consulta = new Consulta() {
                        ConsultaCodigo = diagnosticoDTO.DiagnosticoconsultaCodigo
                    }
                };

                atenderConsultaServicio.RegistrarDiagnostico(dataDiagnostico);


                accionExitosa = true;
                mensajeRetorno = "Ya se registro !!";
            }
            catch (Exception ex)
            {
                accionExitosa = false;
                mensajeRetorno = ex.Message;
            }
            return Json(new { transaccionExitosa = accionExitosa, mensaje = mensajeRetorno });
        }



        // Métodos para las vistas parciales
        public ActionResult Portadas() { 
            return PartialView("_Portadas"); 
        } 
        public ActionResult Citas() { 
            return PartialView("_Citas"); 
        } 
        public ActionResult Recetas() {
            return PartialView("_Recetas"); 
        } 
        public ActionResult Inicio() { 
            return PartialView("_Inicio"); 
        } 
        public ActionResult Diagnosticos() { 
            return PartialView("_Diagnosticos"); 
        } 
        public ActionResult Tratamiento() { 
            return PartialView("_Tratamiento"); 
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
            else if (estado == "T")
                return "Atendiendo";
            else
                return "Desconocido";
        }
    }
}