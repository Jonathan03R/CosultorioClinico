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
    HistoriaClinicafechaCreacion date default getdate(),
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

    constraint CitaPK primary key (citaCodigo),
    constraint CitaNotificacionFK foreign key (citaNotificacionCodigo) references Gestion.notificacion(notificacionCodigo),
    constraint CitaEstadoCK check (citaEstado in ('P', 'N', 'A', 'C', 'T')),-- P: pendiente, N: No Asistida, A: atendida, C: cancelada , T:'Atendiendo'
    )
go


--tablas de gesti�n de consultas 

create table Gestion.Consulta (
    consultaCodigo nchar(10),
    consultacitaCodigo nchar(10),
    consultaFechaHoraFinal datetime null,
	medicoCodigo nchar(10),
	tipoConsultaCodigo nchar(10),
	pacienteCodigo NCHAR(10)
    constraint ConsultaPK primary key (consultaCodigo),
    constraint consultacitaCodigoFK foreign key (consultacitaCodigo) references  Gestion.cita(citaCodigo),
	constraint medicoCodigo foreign key(medicoCodigo) references Administracion.medico(medicoCodigo),
	constraint tipoConsultaCodigo foreign key(tipoConsultaCodigo) references Gestion.tipoConsulta(tipoConsultaCodigo),
	constraint pacienteCodigoFK foreign key (pacienteCodigo) references Salud.pacientes(pacienteCodigo),
) on gestionConsultas
go


create table Gestion.DetallesConsulta(
	detallesConsultaCodigo nchar(10),
	detallesConsultaHistoriaEnfermedad nvarchar(500) null,
	detallesConsultaRevisiones nvarchar(500) null,
	detallesConsultaEvaluacionPsico nvarchar(500) null,
	detallesConsultaMotivoConsulta nvarchar(500) null,
	consultaCodigo nchar(10),
	constraint DetallesConsultaCodigoPK primary	key	(DetallesConsultaCodigo),
	constraint consultaCodigoFK foreign key (consultaCodigo) references Gestion.Consulta(consultaCodigo)
)on gestionConsultas
go

create table Salud.Diagnostico (
    diagnosticoCodigo nchar(10),
    diagnosticoconsultaCodigo nchar(10),
    diagnosticoDescripcion nvarchar(255) not null,
    diagnosticosCodigoCie11 nvarchar(50)
    constraint DiagnosticoPK primary key (diagnosticoCodigo),
    constraint DiagnosticoConsultaFK foreign key (diagnosticoconsultaCodigo) references Gestion.Consulta(consultaCodigo)
) on gestionConsultas
go 
--aquioi esta la receta checa
create table Salud.RecetaMedica (
    recetaCodigo nchar(10),
    recetaConsultaCodigo nchar(10),
    recetaDescripcion nvarchar(255) not null,
	recetaTratamiento nvarchar(100) not null,
	recetaRecomendaciones nvarchar(100) not null,
    constraint RecetaMedicaPK primary key (recetaCodigo),
    constraint RecetaConsultaFK foreign key (recetaConsultaCodigo) references Gestion.Consulta(consultaCodigo)
) on gestionConsultas
go

--insert para hacer pruebas

insert into Salud.HistoriaClinica([historialClinicoCodigo])
values 
('HIS0000001'),
('HIS0000002' ),
('HIS0000003' ),
('HIS0000004' ),
('HIS0000005' )

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
	('HRA0000008', 'TUESDAY', '13:00', '17:00', 'MED0000003'), -- martes
	('HRA0000009', 'THURSDAY', '13:00', '17:00', 'MED0000003'), -- jueves
	('HRA0000010', 'FRIDAY', '13:00', '17:00', 'MED0000004'), -- viernes
	('HRA0000011', 'SATURDAY', '13:00', '17:00', 'MED0000004'), -- sabado
	('HRA0000012', 'THURSDAY', '13:00', '17:00', 'MED0000005'),-- Jueves 
	('HRA0000013', 'FRIDAY', '13:00', '17:00', 'MED0000005'), -- viernes
	('HRA0000014', 'SATURDAY', '13:00', '17:00', 'MED0000005'); -- sdabado
go

insert into Gestion.tipoConsulta (tipoConsultaCodigo, tipoConsultaDescripcion)
values
('TDC0000001', 'Consulta General'),
('TDC0000002', 'Consulta Especializada');
go
insert into Gestion.cita (citaCodigo, citaFechaHora)
values
('CIT0000001', '2024-11-18 10:00:00'),
('CIT0000002', '2024-12-19 15:00:00'),
('CIT0000003', '2024-12-20 09:00:00'),
('CIT0000004', '2024-12-21 12:00:00'),
('CIT0000005', '2024-12-21 14:00:00'),
('CIT0000006', '2024-12-20 09:00:00');
go


insert into Gestion.Consulta (consultaCodigo,consultacitaCodigo, consultaFechaHoraFinal,[pacienteCodigo], [medicoCodigo], [tipoConsultaCodigo])
values
('CON0000001','CIT0000001', '2024-12-1 10:00:00', 'PAC0000001', 'MED0000001', 'TDC0000001'),
('CON0000002','CIT0000002', '2024-12-1 11:30:00', 'PAC0000002', 'MED0000002', 'TDC0000002'),
('CON0000003','CIT0000003', '2024-12-1 09:00:00', 'PAC0000003', 'MED0000003', 'TDC0000002'),
('CON0000004','CIT0000004', '2024-12-1 14:00:00', 'PAC0000004', 'MED0000004', 'TDC0000001'),
('CON0000005','CIT0000005', '2024-11-1 09:00:00', 'PAC0000005' , 'MED0000004', 'TDC0000001' ),
('CON0000006','CIT0000006', '2024-11-1 14:00:00', 'PAC0000002', 'MED0000002', 'TDC0000002');



insert into Salud.Diagnostico (diagnosticoCodigo, diagnosticoconsultaCodigo, diagnosticoDescripcion)
values
('DIA0000001', 'CON0000001', 'Angina de pecho'),
('DIA0000002', 'CON0000002', 'Peso dentro del rango normal'),
('DIA0000003', 'CON0000003', 'Hipertensi�n controlada'),
('DIA0000004', 'CON0000004', 'Crecimiento adecuado');

insert into Salud.RecetaMedica (recetaCodigo, recetaConsultaCodigo, recetaDescripcion, recetaTratamiento, recetaRecomendaciones)
values
('REC0000001', 'CON0000001', 'Nitroglicerina sublingual', '1 tableta al d�a', 'Evitar esfuerzos f�sicos'),
('REC0000002', 'CON0000002', 'Multivitam�nicos pedi�tricos', '1 por d�a', 'Mantener dieta equilibrada'),
('REC0000003', 'CON0000003', 'Losart�n 50mg', '1 tableta al d�a', 'Medir presi�n arterial diariamente'),
('REC0000004', 'CON0000004', 'Suplemento de calcio', '1 tableta al d�a', 'Seguir dieta rica�en�calcio');




-- Especialidades
insert into Administracion.Especialidad (especialidadCodigo, especialidadNombre, especialidadDescripcion)
values
('ESP0000003', 'Dermatolog�a', 'Tratamiento de enfermedades de la piel'),
('ESP0000004', 'Neurolog�a', 'Atenci�n especializada en el sistema nervioso');

go

-- M�dicos
insert into Administracion.Medico (medicoCodigo, medicoApellido, medicoNombre, medicoCorreo, medicoDNI, medicoTelefono, especialidadCodigo)
values
('MED0000006', 'Gonzalez', 'Mar�a', 'maria@example.com', '12345678', '987654321', 'ESP0000003'), -- Dr. Mar�a Gonzalez, Dermatolog�a
('MED0000007', 'Perez', 'Carlos', 'carlos@example.com', '87654321', '987123456', 'ESP0000004'), -- Dr. Carlos Perez, Neurolog�a
('MED0000008', 'Lopez', 'Ana', 'ana@example.com', '56781234', '987321654', 'ESP0000004'), -- Dr. Ana Lopez, Dermatolog�a
('MED0000009', 'Ramirez', 'Jorge', 'jorge@example.com', '43215678', '987456789', 'ESP0000003'); -- Dr. Jorge Ramirez, Neurolog�a

go

-- Horarios
insert into Gestion.horario (horarioCodigo, horarioDia, horarioHoraInicio, horarioHoraFin, medicoCodigo)
values
('HRA0000015', 'FRIDAY', '09:00', '12:00', 'MED0000006'),  -- Dr. Mar�a Gonzalez, Dermatolog�a -- viernes
('HRA0000016', 'FRIDAY', '13:00', '17:00', 'MED0000007'),  -- Dr. Carlos Perez, Neurolog�a
('HRA0000017', 'FRIDAY', '09:00', '12:00', 'MED0000008'),  -- Dr. Ana Lopez, Dermatolog�a
('HRA0000018', 'FRIDAY', '13:00', '17:00', 'MED0000009');  -- Dr. Jorge Ramirez, Neurolog�a

go


-- Insertar datos en Gestion.cita
insert into Gestion.cita (citaCodigo, citaFechaHora)
values
('CIT0000007', '2024-12-20 13:30:00'); -- Cita a las 13:30

-- Insertar datos en Gestion.Consulta
insert into Gestion.Consulta (consultaCodigo, consultacitaCodigo, consultaFechaHoraFinal, medicoCodigo, tipoConsultaCodigo, pacienteCodigo)
values
('CON0000007', 'CIT0000007', '2024-12-20 14:00:00', 'MED0000007', 'TDC0000001', 'PAC0000001'); -- Consulta relacionada con la cita
go

select * from Gestion.Consulta
go

-- Comentarios:
-- Dr. Mar�a Gonzalez, Dermatolog�a, trabaja los viernes de 9:00 a 12:00.
-- Dr. Carlos Perez, Neurolog�a, trabaja los viernes de 13:00 a 17:00.
-- Dr. Ana Lopez, Dermatolog�a, trabaja los viernes de 9:00 a 12:00.
-- Dr. Jorge Ramirez, Neurolog�a, trabaja los viernes de 13:00 a 17:00.
