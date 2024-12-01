

use BdClinicaWeb
go

--PROCEDIMIENTOS ALMACENADOS PARA GESTIONAR PACIENTES == RESPONSABLE ROQUE GONZALES JONATHAN

/******************************************************************************************
Procedimiento: pro_Crear_Paciente
Descripción: Inserta un nuevo paciente en la tabla Salud.Pacientes. Se ingresan datos como
el código del paciente, su historial clínico, DNI, nombre completo, fecha de nacimiento y otros datos.
Parámetros:
    - @pacienteCodigo: Código único del paciente.
    - @pacienteHistorialClinicoCodigo: Código del historial clínico asociado al paciente.
    - @pacienteDNI: Documento Nacional de Identidad del paciente.
    - @pacienteNombreCompleto: Nombre completo del paciente.
    - @pacienteFechaNacimiento: Fecha de nacimiento del paciente.
    - @pacienteDireccion: Dirección del paciente (opcional).
    - @pacienteTelefono: Teléfono del paciente (opcional).
    - @pacienteCorreoElectronico: Correo electrónico del paciente (opcional).
    - @pacienteEstado: Estado del paciente (A = Activo, I = Inactivo). Por defecto es 'A'.
******************************************************************************************/
create or alter procedure pro_Crear_Paciente
    @pacienteCodigo nchar(10),
    @pacienteDNI nchar(8),
    @pacienteNombreCompleto nvarchar(100),
    @pacienteFechaNacimiento date,
    @pacienteDireccion nvarchar(255) = null,
    @pacienteTelefono nvarchar(15) = null,
    @pacienteCorreoElectronico nvarchar(100) = null,
    @pacienteEstado nchar(1) = 'A'
	as
	set nocount on;

	insert into Salud.Pacientes 
	(
		pacienteCodigo,
		pacienteDNI,
		pacienteNombreCompleto,
		pacienteFechaNacimiento,
		pacienteDireccion,
		pacienteTelefono,
		pacienteCorreoElectronico,
		pacienteEstado
	)
	values 
	(
		@pacienteCodigo,
		@pacienteDNI,
		@pacienteNombreCompleto,
		@pacienteFechaNacimiento,
		@pacienteDireccion,
		@pacienteTelefono,
		@pacienteCorreoElectronico,
		@pacienteEstado
	);
go


/******************************************************************************************
Procedimiento: pro_Eliminar_Paciente
Descripción: Se actualizara los datos necesarios del paciente
Parámetros:
    - son muchos pero alli revisalen ps :)
******************************************************************************************/

create or alter procedure pro_Actualizar_Paciente
    @pacienteCodigo nchar(10),
    @pacienteNombreCompleto nvarchar(100) = null,
    @pacienteDireccion nvarchar(255) = null,
    @pacienteTelefono nvarchar(15) = null,
    @pacienteCorreoElectronico nvarchar(100) = null
	as
	begin
		set nocount on;

		update Salud.Pacientes
		set 
			pacienteNombreCompleto = coalesce(@pacienteNombreCompleto, pacienteNombreCompleto),
			pacienteDireccion = coalesce(@pacienteDireccion, pacienteDireccion),
			pacienteTelefono = coalesce(@pacienteTelefono, pacienteTelefono),
			pacienteCorreoElectronico = coalesce(@pacienteCorreoElectronico, pacienteCorreoElectronico)
		where pacienteCodigo = @pacienteCodigo;
		set nocount off;
	end;
go

/******************************************************************************************
Procedimiento: pro_Eliminar_Paciente
Descripción: Cambia el estado de un paciente a 'I' (Inactivo) en lugar de eliminar el registro
de la base de datos.
Parámetros:
    - @pacienteCodigo: Código único del paciente que se desea marcar como inactivo.
******************************************************************************************/
create or alter procedure pro_Eliminar_Paciente
		@pacienteCodigo nchar(10)
	as
	set nocount on;

	update Salud.Pacientes
	set pacienteEstado = 'I'
	where pacienteCodigo = @pacienteCodigo;
go

/******************************************************************************************
Procedimiento: pro_Eliminar_Paciente
Descripción: Cambia el estado de un paciente a 'A' (Activo) en lugar de eliminar el registro
de la base de datos.
Parámetros:
    - @pacienteCodigo: Código único del paciente que se desea marcar como activo.
******************************************************************************************/

create or alter procedure pro_Recuperar_Paciente
		@pacienteCodigo nchar(10)
	as
	set nocount on;

	update Salud.Pacientes
	set pacienteEstado = 'A'
	where pacienteCodigo = @pacienteCodigo;
go

/******************************************************************************************
Procedimiento: pro_Mostrar_Paciente_por_codigo
Descripción: Retorna la información de un paciente específico utilizando su código único.
Parámetros:
    - @pacienteCodigo: Código único del paciente cuyo detalle se desea obtener.
******************************************************************************************/
create or alter procedure pro_Mostrar_Paciente_por_codigo
		@pacienteCodigo nchar(10)
	as
	set nocount on;

	select pacienteCodigo,
		   pacienteDNI,
		   pacienteNombreCompleto,
		   pacienteFechaNacimiento,
		   pacienteDireccion,
		   pacienteTelefono,
		   pacienteCorreoElectronico,
		   pacienteEstado
	from Salud.Pacientes
	where pacienteCodigo = @pacienteCodigo;
go

/******************************************************************************************
Procedimiento: pro_listar_pacientes
Descripción: Muestra todos los pacientes que existen en la base de datos
******************************************************************************************/
create or alter procedure pro_listar_pacientes
	as
begin
    set nocount on;

    select 
        P.pacienteCodigo,
        HC.historialClinicoCodigo as pacienteHistorialClinicoCodigo,
        P.pacienteDNI,
        P.pacienteNombreCompleto,
        P.pacienteFechaNacimiento,
        P.pacienteDireccion,
        P.pacienteTelefono,
        P.pacienteCorreoElectronico,
        P.pacienteEstado
    from 
        Salud.Pacientes as P
    left join 
        Salud.HistoriaClinica as HC on P.pacienteCodigo = HC.pacienteCodigo;

    set nocount off;
end
go

/******************************************************************************************
Procedimiento: pro_Mostrar_HistoriaClinica
Descripción: Muestra el historial clínico completo de un paciente, incluyendo su relación 
con el médico que lo atendió.
Parámetros:
    - @pacienteCodigo: Código único del paciente cuyo historial clínico se desea obtener.
******************************************************************************************/
create or alter procedure pro_Mostrar_HistoriaClinica
		@pacienteCodigo nchar(10)
	as
	set nocount on;

	select hc.historialClinicoCodigo,
		   hc.pacienteCodigo,
		   hc.antecedentesMedicos,
		   hc.alergias,
		   hc.fechaCreacion,
		   hc.fechaActualizacion
	from Salud.HistoriaClinica hc
	where hc.pacienteCodigo = @pacienteCodigo;
go

/******************************************************************************************
Procedimiento: pro_Mostrar_ContactosEmergencia
Descripción: Muestra los contactos de emergencia asociados a un paciente específico.
Parámetros:
    - @pacienteCodigo: Código único del paciente cuyos contactos de emergencia se desean obtener.
******************************************************************************************/
create or alter procedure pro_Mostrar_ContactosEmergencia
		@pacienteCodigo nchar(10)
	as
	set nocount on;

	select ce.contactoEmergenciaCodigo,
		   ce.contactoEmergenciaNombre,
		   ce.contactoEmergenciaRelacion,
		   ce.contactoEmergenciaTelefono
	from Salud.ContactosEmergencia ce
	where ce.pacienteCodigo = @pacienteCodigo;
go

/******************************************************************************************
Descripción de procedimiento almacenado:
---------------------------------------------------------------------------------------------
Procedimiento almacenado para agregar un contacto de emergencia en la tabla `ContactosEmergencia`.

**********************************************************************************************/
create or alter procedure pro_ContactosEmergencia_Agregar 
    @contactoEmergenciaCodigo nchar(10),
    @contactoEmergenciaNombre nvarchar(100),
    @contactoEmergenciaRelacion nvarchar(50),
    @contactoEmergenciaTelefono nvarchar(15),
    @pacienteCodigo nchar(10)
as
begin
    set nocount on;

    insert into Salud.ContactosEmergencia (
        contactoEmergenciaCodigo,
        contactoEmergenciaNombre,
        contactoEmergenciaRelacion,
        contactoEmergenciaTelefono,
        pacienteCodigo
    )
    values (
        @contactoEmergenciaCodigo,
        @contactoEmergenciaNombre,
        @contactoEmergenciaRelacion,
        @contactoEmergenciaTelefono,
        @pacienteCodigo
    );

    set nocount off;
end;
go



/******************************************************************************************
Procedimiento: pro_Agregar_ContactosEmergencia
Descripción: agrega contasto de emergencia para un paciente en especifico
Parámetros:
    - @pacienteCodigo: Código único del paciente cuyos contactos de emergencia se desean obtener.
******************************************************************************************/

create or alter procedure pro_AgregarContactosEmergencia
	@ContactoEmergenciaCodigo nchar(10),
	@ContactoEmergenciaNombre nvarchar(100),
	@ContactoEmergenciaRelacion nvarchar(50),
	@ContactoEmergenciaTelefono nvarchar(15),
	@CodigoPacientes nvarchar(10)
	as
	set nocount on;
	insert into Salud.ContactosEmergencia
	(
		contactoEmergenciaCodigo, 
		contactoEmergenciaNombre, 
		contactoEmergenciaRelacion,
		contactoEmergenciaTelefono,
		pacienteCodigo
	)values
	(
		@ContactoEmergenciaCodigo ,
		@ContactoEmergenciaNombre ,
		@ContactoEmergenciaRelacion ,
		@ContactoEmergenciaTelefono ,
		@CodigoPacientes 
	);
	
go

/******************************************************************************************
Procedimiento: pro_Mostrar_MedicosConEspecialidad
Descripción: Muestra la lista de médicos junto con la especialidad a la que pertenecen.
******************************************************************************************/
create or alter procedure pro_Mostrar_MedicosConEspecialidad
	as
	set nocount on;

	select m.medicoCodigo,
		   m.medicoNombre,
		   m.medicoApellido,
		   e.especialidadNombre
	from Administracion.Medico m
	inner join Administracion.Especialidad e on m.especialidadCodigo = e.especialidadCodigo;
go

/*********************************************************************************************
Procedimiento: pro_Listar_TipoConsulta
Descripcion: Lista los tipo de consulta que tiene la clinica
*********************************************************************************************/
create or alter procedure Pro_Listar_TipoConsulta
    as
    begin
        set nocount on
        select 
            tipoConsultaCodigo,  
            tipoConsultaDescripcion 
        from 
            Gestion.tipoConsulta
        order by 
            tipoConsultaDescripcion
end
go

/*********************************************************************************************
Procedimiento: pro_Listar_Especialidad
Descripcion: Lista los tipo de consulta que tiene la clinica
*********************************************************************************************/
create or alter procedure Pro_Listar_Especialidad
     as
    begin
        set nocount on
        select 
            especialidadCodigo,  
            especialidadNombre,  
            especialidadDescripcion
        from 
            Administracion.Especialidad
        order by 
            especialidadNombre
end
go



--PROCEDIMIENTO ALMACENADOS PARA GESTIONAR CITAS == Responsabloe Daniel Asmat

/*************************************************************************************************************************
Procedimiento: pro_Insertar_Cita
Descripción: Este procedimiento inserta directamente los valores en la tabla Gestion.cita
Parámetros: 
  -@citaCodigo: Código único para la cita.
  -@citaEstado: Estado de la cita, con un valor predeterminado de 'P' (Pendiente).
  -@citaFechaHora: Fecha y hora programadas para la cita.
  -@citaNotificacionCodigo: Código de notificación (puede ser NULL).
  -@citaPacienteCodigo: Código del paciente.
  -@citaTipoConsultaCodigo: Código del tipo de consulta.
  -@citaMedicoCodigo: Código del médico.
***************************************************************************************************************************/
--crear pacientes sp de gestionar pacientes 
CREATE or alter PROCEDURE pro_Insertar_Cita
    @citaCodigo nchar(10),
    @citaEstado nchar(1) = 'P',
    @citaFechaHora datetime,
    @citaNotificacionCodigo nchar(10) = NULL,
    @citaPacienteCodigo nchar(10),
    @citaTipoConsultaCodigo nchar(10),
    @citaMedicoCodigo nchar(10)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Gestion.cita 
    (
        citaCodigo,
        citaEstado,
        citaFechaHora,
        citaNotificacionCodigo,
        citaPacienteCodigo,
        citaTipoConsultaCodigo,
        citaMedicoCodigo
    )
    VALUES 
    (
        @citaCodigo,
        @citaEstado,
        @citaFechaHora,
        @citaNotificacionCodigo,
        @citaPacienteCodigo,
        @citaTipoConsultaCodigo,
        @citaMedicoCodigo
    );
END
GO
/*********************************
select * from Salud.paciente
select * from Gestion.notificacion
select * from Administracion.medico
select * from Gestion.tipoConsulta
EXEC pro_Insertar_Cita 
    @citaCodigo = 'CITA004', 
    @citaFechaHora = '2024-10-22 10:00', 
    @citaNotificacionCodigo = 'N001', 
    @citaPacienteCodigo = 'P001', 
    @citaTipoConsultaCodigo = 'TC01', 
    @citaMedicoCodigo = 'M001';
*****************************************/

/*************************************************************************************************************************
Procedimiento: pro_Buscar_Paciente
Descripción: Procedimiento para buscar un paciente en la tabla Salud.paciente utilizando el DNI, el nombre completo o el teléfono.
Parámetros: 
 -@pacienteDNI: El DNI del paciente que se desea buscar (puede ser NULL).
 -@pacienteNombreCompleto: El nombre completo del paciente que se desea buscar (puede ser NULL).
 -@pacienteTelefono: El teléfono del paciente que se desea buscar (puede ser NULL).
***************************************************************************************************************************/
CREATE or alter PROCEDURE pro_Buscar_Paciente
    @pacienteDNI nchar(8) = NULL,
    @pacienteNombreCompleto nvarchar(200) = NULL,
    @pacienteTelefono nvarchar(25) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        pacienteCodigo,
        pacienteNombreCompleto,
        pacienteCorreoElectronico,
        pacienteDireccion,
        pacienteTelefono,
        pacienteFechaNacimiento,
        pacienteEstado
    FROM 
        Salud.pacientes
    WHERE 
        (@pacienteDNI IS NOT NULL AND pacienteDNI = @pacienteDNI) OR
        (@pacienteNombreCompleto IS NOT NULL AND pacienteNombreCompleto LIKE '%' + @pacienteNombreCompleto + '%') OR
        (@pacienteTelefono IS NOT NULL AND pacienteTelefono = @pacienteTelefono);
END
GO


CREATE OR ALTER PROCEDURE pro_Buscar_Paciente_dni
    @pacienteDNI nchar(8) 
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        pacienteCodigo,
		pacienteDNI,
        pacienteNombreCompleto,
        pacienteCorreoElectronico,
        pacienteDireccion,
        pacienteTelefono,
        pacienteFechaNacimiento,
        pacienteEstado
    FROM 
        Salud.pacientes
    WHERE 
        pacienteDNI = @pacienteDNI;
END
GO

--------------------------------------
/*************************************************************************************************************************
Procedimiento: pro_Cancelar_Cita
Descripción: procedimiento almacenado para cancelar una cita cambiando el estado de la cita.
Parámetros: 
 -@citaCodigo: El codigo de la cita a cambiar estado.

***************************************************************************************************************************/
CREATE or alter PROCEDURE pro_Cancelar_Cita
    @citaCodigo nchar(10)
AS
BEGIN
    UPDATE Gestion.cita
    SET citaEstado = 'X'
    WHERE citaCodigo = @citaCodigo;
END
GO
--select * from Gestion.cita
--EXEC pro_Cancelar_Cita 'CITA001';

/*************************************************************************************************************************
Procedimiento: pro_CambiarEstadoPaciente
Descripción: Cambiar el estado pacienteEstado con I de inactivo al momento de eliminar al paciente.
Parámetros: 
 -@pacienteCodigo: El codigo del paciente.

***************************************************************************************************************************/
CREATE or alter PROCEDURE pro_CambiarEstadoPaciente
    @pacienteCodigo nchar(10)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Salud.paciente
    SET pacienteEstado = 'I'
    WHERE pacienteCodigo = @pacienteCodigo;
END
GO


/*************************************************************************************************************************
Procedimiento: pro_VisualizarCitasPaciente
Descripción: Procedimiento para visualizar las citas que tiene un cliente.
Parámetros: 
 -@pacienteCodigo: Código del paciente para el que se desean visualizar las citas.

***************************************************************************************************************************/

CREATE or alter PROCEDURE pro_VisualizarCitasPaciente
    @pacienteCodigo nchar(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.citaCodigo,
        c.citaEstado,
        c.citaFechaHora,
        c.citaTipoConsultaCodigo,
        c.citaNotificacionCodigo,
        m.medicoNombre,
        m.medicoApellido
    FROM 
        Gestion.cita c
    JOIN 
        Administracion.medico m ON c.citaMedicoCodigo = m.medicoCodigo
    WHERE 
        c.citaPacienteCodigo = @pacienteCodigo;
END
GO

/*************************************************************************************************************************
Procedimiento: pro_Mostrar_Citas
Descripción: Procedimiento para visualizar todas las citas del Dia
Parámetros: 
 -@pacienteCodigo: Código del paciente para el que se desean visualizar las citas.

***************************************************************************************************************************/

create or alter procedure pro_Mostrar_Citas
as
begin
    set nocount on;

    select 
        c.citaCodigo as CodigoCita,
        c.citaEstado as EstadoCita,
        c.citaFechaHora as FechaHoraCita,
        c.citaTipoConsultaCodigo as TipoConsultaCodigo,
        c.citaNotificacionCodigo as NotificacionCodigo,
        m.medicoNombre as NombreMedico,
        m.medicoApellido as ApellidoMedico,
		p.pacienteCodigo,
		p.pacienteNombreCompleto As NombrePaciente
    from 
        Gestion.cita as c
    join 
        Administracion.medico as m on c.citaMedicoCodigo = m.medicoCodigo
	join
		Salud.Pacientes as p on c.citaPacienteCodigo = p.pacienteCodigo;

    set nocount off;
end
go


/******************************************************************************************
Descripción de procedimiento almacenado:
---------------------------------------------------------------------------------------------
Agrega una nueva historia clínica para un paciente, especificando su código de paciente, antecedentes médicos, 
alergias, y las fechas de creación y actualización.

---------------------------------------------------------------------------------------------
Fecha        Usuario         Descripción de cambio
---------------------------------------------------------------------------------------------
<12/11/2024> <Jonathan Roque>      Creación inicial
**************************************************************************************/
create or alter procedure pro_AgregarHistoriaClinica
    @historialClinicoCodigo nchar(10),
    @pacienteCodigo nchar(10),
    @antecedentesMedicos nvarchar(255) = null,
    @alergias nvarchar(255) = null,
    @fechaCreacion date,
    @fechaActualizacion date
	as
	begin
		set nocount on;

		insert into Salud.HistoriaClinica
		(
			historialClinicoCodigo,
			pacienteCodigo,
			antecedentesMedicos,
			alergias,
			fechaCreacion,
			fechaActualizacion
		)
		values
		(
			@historialClinicoCodigo,
			@pacienteCodigo,
			@antecedentesMedicos,
			@alergias,
			@fechaCreacion,
			@fechaActualizacion
		);

		set nocount off;
	end
go


CREATE or alter PROCEDURE pro_Listar_Medicos
AS
BEGIN
    SELECT 
        medicoCodigo,
        medicoApellido,
        medicoNombre,
        medicoCorreo,
        medicoDNI,
        medicoTelefono,
        medicoEstado,
        especialidadCodigo
    FROM 
        Administracion.Medico
    ORDER BY 
        medicoApellido, medicoNombre;
END;
GO


/******************************************************************************************
Descripción de procedimiento almacenado: Genera un código único basado en un prefijo y la secuencia en una columna específica.
---------------------------------------------------------------------------------------------
 Fecha     Usuario      Descripción de cambio
---------------------------------------------------------------------------------------------
 12/11/2024  Usuario       Creación del procedimiento con ajuste a nchar(10)
**************************************************************************************/

create or alter procedure spGenerarCodigoUnico
    @prefijo nvarchar(3),         
    @tabla nvarchar(128),         
    @columnaCodigo nvarchar(128)
as
    declare @vNuevoCodigo int;
    declare @vCodigoGenerado nchar(10);
    declare @vSQL nvarchar(max);

    set nocount on;
    set @vSQL = '
        select @vNuevoCodigo = isnull(max(cast(substring(' + @columnaCodigo + ', 4, len(' + @columnaCodigo + ')) as int)), 0) + 1
        from ' + @tabla + '
        where ' + @columnaCodigo + ' like @prefijo + ''%''
    ';
    exec sp_executesql @vSQL, N'@vNuevoCodigo int output, @prefijo nvarchar(3)', @vNuevoCodigo output, @prefijo;
    set @vCodigoGenerado = @prefijo + right('0000000' + cast(@vNuevoCodigo as varchar(7)), 7);
    select @vCodigoGenerado as CodigoUnico;

set nocount off;
go