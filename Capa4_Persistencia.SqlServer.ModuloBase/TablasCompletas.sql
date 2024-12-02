xp_create_subdir 'C:\Base';
go
xp_create_subdir 'C:\Empresa';
go
xp_create_subdir 'C:\Reportes';
go

-- Crear la base de datos con nombres más relevantes para una clínica
create database BdClinicaWeb
    on primary
    (name = 'BdClinicaDatosGenerales', filename ='C:\Base\BdClinicaDatosGenerales.mdf',
        size = 50MB, maxsize = 30GB, filegrowth = 100MB),
    (name = 'BdClinicaHistorialMedico', filename ='C:\Base\BdClinicaHistorialMedico.ndf',
        size = 50MB, maxsize = 30GB, filegrowth = 100MB),
    filegroup gestionPacientes
    (name = 'ClinicaGestionPacientes01', filename ='C:\Empresa\ClinicaGestionPacientes01.ndf',
        size = 30MB, maxsize = 80GB, filegrowth = 100MB),
    (name = 'ClinicaGestionPacientes02', filename ='C:\Reportes\ClinicaGestionPacientes02.ndf',
        size = 80MB, filegrowth = 100MB),
    filegroup gestionCitas
    (name = 'ClinicaGestionCitas01', filename ='C:\Empresa\ClinicaGestionCitas01.ndf',
        size = 30MB, maxsize = 80GB, filegrowth = 100MB),
    (name = 'ClinicaGestionCitas02', filename ='C:\Reportes\ClinicaGestionCitas02.ndf',
        size = 80MB, filegrowth = 100MB),
	filegroup gestionConsultas
    (name = 'ClinicaGestionConsultas01', filename ='C:\Empresa\ClinicaGestionConsultas01.ndf',
        size = 30MB, maxsize = 80GB, filegrowth = 100MB),
    (name = 'ClinicaGestionConsultas02', filename ='C:\Reportes\ClinicaGestionConsultas02.ndf',
        size = 80MB, filegrowth = 100MB)
    log on
    (name = 'BdClinicaLog', filename ='C:\Reportes\BdClinicaLog.ldf',
        size = 80MB, filegrowth = 100MB)
go

-- Usar la nueva base de datos
use BdClinicaWeb
go

-- Crear esquemas generales
create schema Administracion
go
create schema Salud
go
create schema Gestion
go

-- ============================================================================
-- Creación de Tablas
-- ============================================================================
-- tala nesecarias para gestionasPacientes === responsable Yanmir

-- Tabla Especialidad (almacena las especialidades de los médicos)--- necesario para la gestionar citas****************
create table Administracion.Especialidad
(
    especialidadCodigo nchar(10) not null,
    especialidadNombre nvarchar(100) not null,
    especialidadDescripcion nvarchar(255),
    constraint EspecialidadPK primary key (especialidadCodigo)
) on gestionPacientes;
go

-- Tabla Medico (almacena la información de los médicos) --- necesario para la gestionar citas****************
create table Administracion.Medico
(
    medicoCodigo nchar(10) not null,
    medicoApellido nvarchar(100) not null,
    medicoNombre nvarchar(100) not null,
    medicoCorreo nvarchar(100) not null,
    medicoDNI nchar(8) not null unique,
    medicoTelefono nvarchar(15),
    medicoEstado nchar(1) constraint medicoEstadoDF default 'A', -- A = Activo, I = Inactivo
    especialidadCodigo nchar(10) not null,  -- Relación con Especialidad
    constraint MedicoPK primary key (medicoCodigo),
    constraint MedicoEstadoCK check (medicoEstado = 'A' or medicoEstado = 'I'),
    constraint MedicoEspecialidadFK foreign key (especialidadCodigo) references Administracion.Especialidad(especialidadCodigo)  -- Clave foránea en la creación
) on gestionPacientes;
go


-- Tabla HistoriaClinica (almacena el historial médico de los pacientes)
create table Salud.HistoriaClinica
(
    historialClinicoCodigo nchar(10) not null,
	-- no tenia sentido tener codigo de paciente aqui
    --pacienteCodigo nchar(10) not null,
    --medicoCodigo nchar(10) not null,
    antecedentesMedicos nvarchar(255), 
    alergias nvarchar(255),
    fechaCreacion date not null,
    constraint HistoriaClinicaPK primary key (historialClinicoCodigo),
    --constraint HistoriaClinicaPacienteFK foreign key (pacienteCodigo) references Salud.Pacientes(pacienteCodigo),
    --constraint HistoriaClinicaMedicoFK foreign key (medicoCodigo) references Administracion.Medico(medicoCodigo)
) on gestionConsultas;
go

-- Tabla Pacientes (almacena la información general de los pacientes)  --- necesario para la gestionar citas****************	
create table Salud.Pacientes
(
    pacienteCodigo nchar(10) not null,
    pacienteDNI nchar(8) not null unique,
    pacienteNombreCompleto nvarchar(100) not null,
    pacienteFechaNacimiento date not null,
    pacienteDireccion nvarchar(255),
    pacienteTelefono nvarchar(15),
    pacienteCorreoElectronico nvarchar(100),
    pacienteEstado nchar(1) constraint pacienteEstadoDF default 'A', -- A = Activo, I = Inactivo

	historialClinicoCodigo nchar(10) null unique,
    constraint PacientesPK primary key (pacienteCodigo),
    constraint PacientesEstadoCK check (pacienteEstado = 'A' or pacienteEstado = 'I'),
	constraint historialClinicoCodigoFK foreign key (historialClinicoCodigo) references Salud.HistoriaClinica(historialClinicoCodigo), 
) on gestionPacientes;
go

-- Tabla ContactosEmergencia (almacena información de contactos de emergencia de los pacientes)
create table Salud.ContactosEmergencia
(
    contactoEmergenciaCodigo nchar(10) not null,
    contactoEmergenciaNombre nvarchar(100) not null,
    contactoEmergenciaRelacion nvarchar(50) not null,
    contactoEmergenciaTelefono nvarchar(15) not null,
	pacienteCodigo nchar(10) not null,
    constraint ContactosEmergenciaPK primary key (contactoEmergenciaCodigo),
	constraint pacienteCodigoPk foreign key (pacienteCodigo) references Salud.Pacientes(pacienteCodigo)
) on gestionPacientes;
go




--- tablas necesarias para  gestionarCitas == responsable Jhony

-- Crear tabla tipoConsulta
create table Gestion.tipoConsulta
    (
    tipoConsultaCodigo nchar(10),
    tipoConsultaDescripcion nvarchar(255),
    constraint TipoConsultaPK primary key (tipoConsultaCodigo)
    )
go

-- Crear tabla notificacion
create table Gestion.notificacion
    (
    notificacionCodigo nchar(8),
    notificacionMensaje nvarchar(255),
    notificacionDestinatario nvarchar(50),
    notificacionfechaDeEnvio datetime not null,
    constraint notificacionCodigoPK primary key (notificacionCodigo)
    )
go


-- Crear tabla horario
create table Gestion.horario
    (
    horarioCodigo nchar(6),
    horarioDia nvarchar(15),
    horarioDisponibilidad bit,
    horarioHoraInicio time,
    horarioHoraFin time,
    constraint horarioCodigoPK primary key (horarioCodigo)
    )
go

-- Crear tabla cita
create table Gestion.cita
    (
    citaCodigo nchar(10),
    citaEstado nchar(1) default 'P',
    citaFechaHora datetime not null,
    citaNotificacionCodigo nchar(8),
    citaPacienteCodigo nchar(10),
    citaTipoConsultaCodigo nchar(10),
	citaMedicoCodigo nchar(10),

    constraint CitaPK primary key (citaCodigo),
    constraint CitaPacienteFK foreign key (citaPacienteCodigo) references Salud.pacientes(pacienteCodigo),
    constraint CitaNotificacionFK foreign key (citaNotificacionCodigo) references Gestion.notificacion(notificacionCodigo),
	constraint CitaMedicoFK foreign key (citaMedicoCodigo) references Administracion.medico(medicoCodigo),
    constraint CitaTipoConsultaFK foreign key (citaTipoConsultaCodigo) references Gestion.tipoConsulta(tipoConsultaCodigo),
    constraint CitaEstadoCK check (citaEstado in ('P', 'C', 'X')) -- P: pendiente, C: confirmada, X: cancelada
    )
go


--tablas de gestión de consultas 
create table Gestion.Consulta (
    consultaCodigo nchar(10),
    consultacitaCodigo nchar(10),
    consultaFechaHoraFinal datetime null,
    --problemas de redundancia para que el codigo del medico si ya existe en Citas

        --consultaMedicoCodigo nchar(10),
        --consultaPacienteCodigo nchar(10),
    consultaMotivo nvarchar(255) null,
    consultaEstado nchar(1) default 'P', 
    constraint ConsultaPK primary key (consultaCodigo),
    --constraint ConsultaMedicoFK foreign key (consultaMedicoCodigo) references Administracion.Medico(medicoCodigo),
    --constraint ConsultaPacienteFK foreign key (consultaPacienteCodigo) references Salud.Pacientes(pacienteCodigo),
    constraint consultacitaCodigoFK foreign key (consultacitaCodigo) references  Gestion.cita(citaCodigo),
    constraint ConsultaEstadoCK check (consultaEstado in ('P', 'N', 'A', 'C'))
) on gestionConsultas
go

create table Salud.Diagnostico (
    diagnosticoCodigo nchar(10),
    diagnosticoconsultaCodigo nchar(10),
    diagnosticoDescripcion nvarchar(255) not null,
    diagnosticoFecha date not null,
    constraint DiagnosticoPK primary key (diagnosticoCodigo),
    constraint DiagnosticoConsultaFK foreign key (diagnosticoconsultaCodigo) references Gestion.Consulta(consultaCodigo)
) on gestionConsultas
go 

create table Salud.RecetaMedica (
    recetaCodigo nchar(10),
    recetaConsultaCodigo nchar(10),
    recetaDescripcion nvarchar(255) not null,
    recetaFecha date not null,
	recetaTratamiento nvarchar(100) not null,
	recetaRecomendaciones nvarchar(100) not null,
    constraint RecetaMedicaPK primary key (recetaCodigo),
    constraint RecetaConsultaFK foreign key (recetaConsultaCodigo) references Gestion.Consulta(consultaCodigo)
) on gestionConsultas
go

--insert para hacer pruebas

insert into Salud.HistoriaClinica([historialClinicoCodigo], [fechaCreacion])
values 
('HIS0000001','2024-11-20' ),
('HIS0000002','2024-11-20' ),
('HIS0000003','2024-11-20' ),
('HIS0000004','2024-11-20' ),
('HIS0000005','2024-11-20' )

go

insert into Salud.Pacientes (pacienteCodigo, pacienteDNI, pacienteNombreCompleto, pacienteFechaNacimiento, pacienteDireccion, pacienteTelefono, pacienteCorreoElectronico,  historialClinicoCodigo)
values
('PAC0000001', '12345678', 'Juan Pérez', '1985-04-15', 'Av. Principal 123', '987654321', 'juan.perez@example.com','HIS0000001' ),
('PAC0000002', '87654321', 'María Gómez', '1990-06-20', 'Calle Secundaria 45', '987123456', 'maria.gomez@example.com', 'HIS0000002'),
('PAC0000003', '23456789', 'Carlos López', '1982-11-10', 'Calle 3 de Abril 67', '987456123', 'carlos.lopez@example.com', 'HIS0000003'),
('PAC0000004', '34567891', 'Ana Torres', '1995-02-25', 'Pasaje Lima 8', '987789321', 'ana.torres@example.com', 'HIS0000004'),
('PAC0000005', '45678912', 'Luis Rojas', '1987-08-15', 'Jr. Ayacucho 342', '987963258', 'luis.rojas@example.com', 'HIS0000005');
go


insert into Salud.ContactosEmergencia (contactoEmergenciaCodigo, contactoEmergenciaNombre, contactoEmergenciaRelacion, contactoEmergenciaTelefono, pacienteCodigo)
values
('CEM0000001', 'Pedro Pérez', 'Padre', '987654111' , 'PAC0000001'),
('CEM0000002', 'Lucía Gómez', 'Hermana', '987123111' , 'PAC0000001');
go



insert into Administracion.Especialidad (especialidadCodigo, especialidadNombre, especialidadDescripcion)
values
('ESP0000001', 'Cardiología', 'Atención especializada en problemas cardíacos'),
('ESP0000002', 'Pediatría', 'Cuidado médico para niños y adolescentes');

go


insert into Administracion.Medico (medicoCodigo, medicoApellido, medicoNombre, medicoCorreo, medicoDNI, medicoTelefono, especialidadCodigo)
values
('MED0000001', 'García', 'Roberto', 'roberto.garcia@example.com', '12345679', '987111222', 'ESP0000001'),
('MED0000002', 'Martínez', 'Sofía', 'sofia.martinez@example.com', '98765432', '987333444', 'ESP0000002');
go

insert into Gestion.tipoConsulta (tipoConsultaCodigo, tipoConsultaDescripcion)
values
('TDC0000001', 'Consulta General'),
('TDC0000002', 'Consulta Especializada');
go
insert into Gestion.cita (citaCodigo, citaFechaHora, citaPacienteCodigo, citaTipoConsultaCodigo, citaMedicoCodigo)
values
('CIT0000001', '2024-11-20 10:30:00', 'PAC0000001', 'TDC0000001', 'MED0000001'),
('CIT0000002', '2024-11-21 15:00:00', 'PAC0000002', 'TDC0000002', 'MED0000002'),
('CIT0000003', '2024-11-22 09:00:00', 'PAC0000003', 'TDC0000001', 'MED0000001'),
('CIT0000004', '2024-11-23 11:00:00', 'PAC0000004', 'TDC0000002', 'MED0000002');
go


insert into Gestion.Consulta (consultaCodigo,consultacitaCodigo, consultaFechaHoraFinal, consultaMotivo, consultaEstado)
values
('CON0000001','CIT0000001', '2024-12-1 10:00:00', 'Dolor en el pecho', 'P'),
('CON0000002','CIT0000002', '2024-12-1 11:30:00', 'Revisión pediátrica', 'P'),
('CON0000003','CIT0000003', '2024-12-1 09:00:00', 'Control de presión arterial', 'P'),
('CON0000004','CIT0000004', '2024-12-1 14:00:00', 'Consulta de crecimiento', 'P');


insert into Salud.Diagnostico (diagnosticoCodigo, diagnosticoconsultaCodigo, diagnosticoDescripcion, diagnosticoFecha)
values
('DIA0000001', 'CON0000001', 'Angina de pecho', '2024-11-25'),
('DIA0000002', 'CON0000002', 'Peso dentro del rango normal', '2024-11-25'),
('DIA0000003', 'CON0000003', 'Hipertensión controlada', '2024-11-26'),
('DIA0000004', 'CON0000004', 'Crecimiento adecuado', '2024-11-26');

insert into Salud.RecetaMedica (recetaCodigo, recetaConsultaCodigo, recetaDescripcion, recetaFecha, recetaTratamiento, recetaRecomendaciones)
values
('REC0000001', 'CON0000001', 'Nitroglicerina sublingual', '2024-11-25', '1 tableta al día', 'Evitar esfuerzos físicos'),
('REC0000002', 'CON0000002', 'Multivitamínicos pediátricos', '2024-11-25', '1 por día', 'Mantener dieta equilibrada'),
('REC0000003', 'CON0000003', 'Losartán 50mg', '2024-11-26', '1 tableta al día', 'Medir presión arterial diariamente'),
('REC0000004', 'CON0000004', 'Suplemento de calcio', '2024-11-26', '1 tableta al día', 'Seguir dieta rica en calcio');

insert into Gestion.Consulta (consultaCodigo,consultacitaCodigo, consultaFechaHoraFinal, consultaMotivo, consultaEstado)
values
('CON0000005','CIT0000001', '2024-11-1 09:00:00', 'Control de presión arterial', 'P'),
('CON0000006','CIT0000002', '2024-11-1 14:00:00', 'Consulta de crecimiento', 'P');

