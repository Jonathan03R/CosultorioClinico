xp_create_subdir 'C:\Base';
go
xp_create_subdir 'C:\Empresa';
go
xp_create_subdir 'C:\Reportes';
go

-- Crear la base de datos con nombres m�s relevantes para una cl�nica
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
-- Creaci�n de Tablas
-- ============================================================================
-- tala nesecarias para gestionasPacientes === responsable Yanmir

-- Tabla Especialidad (almacena las especialidades de los m�dicos)--- necesario para la gestionar citas****************
create table Administracion.Especialidad
(
    especialidadCodigo nchar(10) not null,
    especialidadNombre nvarchar(100) not null,
    especialidadDescripcion nvarchar(255),
    constraint EspecialidadPK primary key (especialidadCodigo)
) on gestionPacientes;
go

-- Tabla Medico (almacena la informaci�n de los m�dicos) --- necesario para la gestionar citas****************
create table Administracion.Medico
(
    medicoCodigo nchar(10) not null,
    medicoApellido nvarchar(100) not null,
    medicoNombre nvarchar(100) not null,
    medicoCorreo nvarchar(100) not null,
    medicoDNI nchar(8) not null unique,
    medicoTelefono nvarchar(15),
    medicoEstado nchar(1) constraint medicoEstadoDF default 'A', -- A = Activo, I = Inactivo
    especialidadCodigo nchar(10) not null,  -- Relaci�n con Especialidad
    constraint MedicoPK primary key (medicoCodigo),
    constraint MedicoEstadoCK check (medicoEstado = 'A' or medicoEstado = 'I'),
    constraint MedicoEspecialidadFK foreign key (especialidadCodigo) references Administracion.Especialidad(especialidadCodigo)  -- Clave for�nea en la creaci�n
) on gestionPacientes;
go

-- Crear tabla horario
create table Gestion.horario
    (
		horarioCodigo nchar(10),
		horarioDia nvarchar(15),
		horarioHoraInicio time,
		horarioHoraFin time,
		medicoCodigo nchar(10),  

		constraint medicosFK foreign key (medicoCodigo) references Administracion.Medico(medicoCodigo),
		constraint horarioCodigoPK primary key (horarioCodigo)
    )
go


-- Tabla HistoriaClinica (almacena el historial m�dico de los pacientes)
create table Salud.HistoriaClinica
(
    historialClinicoCodigo nchar(10) not null,
    antecedentesMedicos nvarchar(255), 
    alergias nvarchar(255),
    fechaCreacion date not null,
    constraint HistoriaClinicaPK primary key (historialClinicoCodigo),
) on gestionConsultas;
go

-- Tabla Pacientes (almacena la informaci�n general de los pacientes)  --- necesario para la gestionar citas****************	
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

-- Tabla ContactosEmergencia (almacena informaci�n de contactos de emergencia de los pacientes)
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
    constraint CitaEstadoCK check (citaEstado in ('P', 'N', 'A', 'C')) -- P: pendiente, N: No Asistida, A: atendida, C: cancelada
    )
go


--tablas de gesti�n de consultas 

create table Gestion.Consulta (
    consultaCodigo nchar(10),
    consultacitaCodigo nchar(10),
    consultaFechaHoraFinal datetime null,
    consultaMotivo nvarchar(255) null,
    --consultaEstado nchar(1) default 'P', 
    constraint ConsultaPK primary key (consultaCodigo),
    constraint consultacitaCodigoFK foreign key (consultacitaCodigo) references  Gestion.cita(citaCodigo),
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
('PAC0000001', '12345678', 'Juan P�rez', '1985-04-15', 'Av. Principal 123', '987654321', 'juan.perez@example.com','HIS0000001' ),
('PAC0000002', '87654321', 'Mar�a G�mez', '1990-06-20', 'Calle Secundaria 45', '987123456', 'maria.gomez@example.com', 'HIS0000002'),
('PAC0000003', '23456789', 'Carlos L�pez', '1982-11-10', 'Calle 3 de Abril 67', '987456123', 'carlos.lopez@example.com', 'HIS0000003'),
('PAC0000004', '34567891', 'Ana Torres', '1995-02-25', 'Pasaje Lima 8', '987789321', 'ana.torres@example.com', 'HIS0000004'),
('PAC0000005', '45678912', 'Luis Rojas', '1987-08-15', 'Jr. Ayacucho 342', '987963258', 'luis.rojas@example.com', 'HIS0000005');
go


insert into Salud.ContactosEmergencia (contactoEmergenciaCodigo, contactoEmergenciaNombre, contactoEmergenciaRelacion, contactoEmergenciaTelefono, pacienteCodigo)
values
('CEM0000001', 'Pedro P�rez', 'Padre', '987654111' , 'PAC0000001'),
('CEM0000002', 'Luc�a G�mez', 'Hermana', '987123111' , 'PAC0000001');
go



insert into Administracion.Especialidad (especialidadCodigo, especialidadNombre, especialidadDescripcion)
values
('ESP0000001', 'Cardiolog�a', 'Atenci�n especializada en problemas card�acos'),
('ESP0000002', 'Pediatr�a', 'Cuidado m�dico para ni�os y adolescentes');

go


insert into Administracion.Medico (medicoCodigo, medicoApellido, medicoNombre, medicoCorreo, medicoDNI, medicoTelefono, especialidadCodigo)
values
('MED0000001', 'Johny', 'ruiz', 'Johny@example.com', '12345679', '987111222', 'ESP0000001'),
('MED0000002', 'Maritza', 'De la cruz', 'maritza@example.com', '98765432', '987333444', 'ESP0000002'),
('MED0000003', 'Yanmir', 'Guerrero', 'yanmir@example.com', '98765433', '987333466', 'ESP0000002'),
('MED0000004', 'Daniel', 'Asmat', 'Daniel@example.com', '98765434', '987333488', 'ESP0000002'),
('MED0000005', 'Jonathan', 'Roque', 'Jona@example.com', '98765435', '987333411', 'ESP0000001');
go


insert into Gestion.horario (horarioCodigo, horarioDia, horarioHoraInicio , horarioHoraFin, medicoCodigo )
	values 
	('HRA0000001', 'MONDAY', '09:00', '17:00', 'MED0000001'),  --lunes
	('HRA0000002', 'TUESDAY', '09:00', '17:00', 'MED0000001'), --martes
	('HRA0000003', 'WEDNESDAY', '09:00', '17:00', 'MED0000001'), -- miercoles
	('HRA0000004', 'MONDAY', '09:00', '17:00', 'MED0000002'), -- lunes
	('HRA0000005', 'WEDNESDAY', '09:00', '17:00', 'MED0000002'), -- miercoles
	('HRA0000006', 'FRIDAY', '09:00', '12:00', 'MED0000002'), -- viernes
	('HRA0000007', 'FRIDAY', '12:00', '17:00', 'MED0000003'), -- viernes
	('HRA0000008', 'TUESDAY', '01:00', '17:00', 'MED0000003'), -- martes
	('HRA0000009', 'THURSDAY', '12:00', '17:00', 'MED0000003'), -- jueves
	('HRA0000010', 'FRIDAY', '12:00', '17:00', 'MED0000004'), -- viernes
	('HRA0000011', 'SATURDAY', '12:00', '17:00', 'MED0000004'), -- sabado
	('HRA0000012', 'THURSDAY', '12:00', '17:00', 'MED0000005'),-- Jueves 
	('HRA0000013', 'FRIDAY', '12:00', '17:00', 'MED0000005'), -- viernes
	('HRA0000014', 'SATURDAY', '12:00', '17:00', 'MED0000005'); -- sdabado
go

insert into Gestion.tipoConsulta (tipoConsultaCodigo, tipoConsultaDescripcion)
values
('TDC0000001', 'Consulta General'),
('TDC0000002', 'Consulta Especializada');
go
insert into Gestion.cita (citaCodigo, citaFechaHora, citaPacienteCodigo, citaTipoConsultaCodigo, citaMedicoCodigo)
values
('CIT0000001', '2024-12-16 10:00:00', 'PAC0000001', 'TDC0000001', 'MED0000001'),
('CIT0000006', '2024-12-20 09:00:00', 'PAC0000002', 'TDC0000002', 'MED0000002'),
('CIT0000002', '2024-12-19 15:00:00', 'PAC0000002', 'TDC0000002', 'MED0000002'),
('CIT0000003', '2024-12-20 09:00:00', 'PAC0000003', 'TDC0000001', 'MED0000003'),
('CIT0000004', '2024-12-21 12:00:00', 'PAC0000004', 'TDC0000002', 'MED0000004'),
('CIT0000005', '2024-12-21 14:00:00', 'PAC0000005', 'TDC0000002', 'MED0000004');
go


insert into Gestion.Consulta (consultaCodigo,consultacitaCodigo, consultaFechaHoraFinal, consultaMotivo)
values
('CON0000001','CIT0000001', '2024-12-1 10:00:00', 'Dolor en el pecho'),
('CON0000002','CIT0000002', '2024-12-1 11:30:00', 'Revisi�n pedi�trica'),
('CON0000003','CIT0000003', '2024-12-1 09:00:00', 'Control de presi�n arterial'),
('CON0000004','CIT0000004', '2024-12-1 14:00:00', 'Consulta de crecimiento');


insert into Salud.Diagnostico (diagnosticoCodigo, diagnosticoconsultaCodigo, diagnosticoDescripcion, diagnosticoFecha)
values
('DIA0000001', 'CON0000001', 'Angina de pecho', '2024-11-25'),
('DIA0000002', 'CON0000002', 'Peso dentro del rango normal', '2024-11-25'),
('DIA0000003', 'CON0000003', 'Hipertensi�n controlada', '2024-11-26'),
('DIA0000004', 'CON0000004', 'Crecimiento adecuado',�'2024-11-26');

insert into Salud.RecetaMedica (recetaCodigo, recetaConsultaCodigo, recetaDescripcion, recetaFecha, recetaTratamiento, recetaRecomendaciones)
values
('REC0000001', 'CON0000001', 'Nitroglicerina sublingual', '2024-11-25', '1 tableta al d�a', 'Evitar esfuerzos f�sicos'),
('REC0000002', 'CON0000002', 'Multivitam�nicos pedi�tricos', '2024-11-25', '1 por d�a', 'Mantener dieta equilibrada'),
('REC0000003', 'CON0000003', 'Losart�n 50mg', '2024-11-26', '1 tableta al d�a', 'Medir presi�n arterial diariamente'),
('REC0000004', 'CON0000004', 'Suplemento de calcio', '2024-11-26', '1 tableta al d�a', 'Seguir dieta rica�en�calcio');

insert into Gestion.Consulta (consultaCodigo,consultacitaCodigo, consultaFechaHoraFinal, consultaMotivo)
values
('CON0000005','CIT0000001', '2024-11-1 09:00:00', 'Control de presi�n arterial'),
('CON0000006','CIT0000002', '2024-11-1 14:00:00', 'Consulta de crecimiento');

