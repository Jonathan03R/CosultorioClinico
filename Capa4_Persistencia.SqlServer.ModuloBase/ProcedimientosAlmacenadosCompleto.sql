

use BdClinicaWeb
go

--PROCEDIMIENTOS ALMACENADOS PARA GESTIONAR PACIENTES == RESPONSABLE ROQUE GONZALES JONATHAN

/******************************************************************************************
Procedimiento: pro_Crear_Paciente
Descripci�n: Inserta un nuevo paciente en la tabla Salud.Pacientes. Se ingresan datos como
el c�digo del paciente, su historial cl�nico, DNI, nombre completo, fecha de nacimiento y otros datos.
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente.
    - @pacienteHistorialClinicoCodigo: C�digo del historial cl�nico asociado al paciente.
    - @pacienteDNI: Documento Nacional de Identidad del paciente.
    - @pacienteNombreCompleto: Nombre completo del paciente.
    - @pacienteFechaNacimiento: Fecha de nacimiento del paciente.
    - @pacienteDireccion: Direcci�n del paciente (opcional).
    - @pacienteTelefono: Tel�fono del paciente (opcional).
    - @pacienteCorreoElectronico: Correo electr�nico del paciente (opcional).
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
	@historialClinicoCodigo nchar(10) null
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
		historialClinicoCodigo
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
		@historialClinicoCodigo
	);
go


/******************************************************************************************
Procedimiento: pro_Eliminar_Paciente
Descripci�n: Se actualizara los datos necesarios del paciente
Par�metros:
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
Descripci�n: Cambia el estado de un paciente a 'I' (Inactivo) en lugar de eliminar el registro
de la base de datos.
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente que se desea marcar como inactivo.
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
Descripci�n: Cambia el estado de un paciente a 'A' (Activo) en lugar de eliminar el registro
de la base de datos.
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente que se desea marcar como activo.
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
Descripci�n: Retorna la informaci�n de un paciente espec�fico utilizando su c�digo �nico.
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente cuyo detalle se desea obtener.
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
Descripci�n: Muestra todos los pacientes que existen en la base de datos
******************************************************************************************/
create or alter procedure pro_listar_pacientes
	as
begin
    set nocount on;
    select 
        P.pacienteCodigo,
        P.pacienteDNI,
        P.pacienteNombreCompleto,
        P.pacienteFechaNacimiento,
        P.pacienteDireccion,
        P.pacienteTelefono,
        P.pacienteCorreoElectronico,
        P.pacienteEstado,
		p.historialClinicoCodigo
    from 
        Salud.Pacientes as P
    set nocount off;
end
go

/******************************************************************************************
Procedimiento: pro_Mostrar_HistoriaClinica
Descripci�n: Muestra el historial cl�nico completo de un paciente, incluyendo su relaci�n 
con el m�dico que lo atendi�.
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente cuyo historial cl�nico se desea obtener.
******************************************************************************************/
create or alter procedure pro_Mostrar_HistoriaClinica
    @pacienteCodigo nchar(10)
as
begin
    set nocount on;

    select
        hc.historialClinicoCodigo,
        p.pacienteCodigo,
        hc.antecedentesMedicos,
        hc.alergias,
        hc.fechaCreacion
    from Salud.Pacientes p
    inner join Salud.HistoriaClinica hc
        on p.historialClinicoCodigo = hc.historialClinicoCodigo
    where p.pacienteCodigo = @pacienteCodigo;
end;
go



/******************************************************************************************
Procedimiento: pro_Mostrar_ContactosEmergencia
Descripci�n: Muestra los contactos de emergencia asociados a un paciente espec�fico.
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente cuyos contactos de emergencia se desean obtener.
******************************************************************************************/
create or alter procedure pro_Mostrar_ContactosEmergencia
    @pacienteCodigo nchar(10)
as
begin
    set nocount on;

    select 
        ce.contactoEmergenciaCodigo,
        ce.contactoEmergenciaNombre,
        ce.contactoEmergenciaRelacion,
        ce.contactoEmergenciaTelefono
    from Salud.ContactosEmergencia ce
    where ce.pacienteCodigo = @pacienteCodigo;

    set nocount off;
end;
go

/******************************************************************************************
Descripci�n de procedimiento almacenado:
---------------------------------------------------------------------------------------------
Procedimiento almacenado para agregar un contacto de emergencia en la tabla `ContactosEmergencia`.

**********************************************************************************************/
create or alter procedure pro_Agregar_ContactoEmergencia 
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



create or alter procedure pro_Actualizar_ContactoEmergencia
    @contactoEmergenciaCodigo nchar(10),
    @contactoEmergenciaNombre nvarchar(100),
    @contactoEmergenciaRelacion nvarchar(50),
    @contactoEmergenciaTelefono nvarchar(15)
as
begin
    set nocount on;

    update Salud.ContactosEmergencia
    set
        contactoEmergenciaNombre = @contactoEmergenciaNombre,
        contactoEmergenciaRelacion = @contactoEmergenciaRelacion,
        contactoEmergenciaTelefono = @contactoEmergenciaTelefono
    where
        contactoEmergenciaCodigo = @contactoEmergenciaCodigo;
end;
go


/******************************************************************************************
Procedimiento: pro_Mostrar_MedicosConEspecialidad
Descripci�n: Muestra la lista de m�dicos junto con la especialidad a la que pertenecen.
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
Procedimiento: pro_Mostrar_Medico_por_codigo
Descripcion: Lista un medico dependiendo del medico
*********************************************************************************************/

create or alter procedure pro_Mostrar_Medico_por_codigo
    @medicoCodigo nchar(10)
as
begin
    set nocount on;

    select 
        medicoCodigo,
        medicoApellido,
        medicoNombre,
        medicoCorreo,
        medicoDNI,
        medicoTelefono,
        medicoEstado,
        especialidadCodigo
    from Administracion.Medico
    where medicoCodigo = @medicoCodigo;
end;
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
Descripci�n: Este procedimiento inserta directamente los valores en la tabla Gestion.cita
Par�metros: 
  -@citaCodigo: C�digo �nico para la cita.
  -@citaEstado: Estado de la cita, con un valor predeterminado de 'P' (Pendiente).
  -@citaFechaHora: Fecha y hora programadas para la cita.
  -@citaNotificacionCodigo: C�digo de notificaci�n (puede ser NULL).
  -@citaPacienteCodigo: C�digo del paciente.
  -@citaTipoConsultaCodigo: C�digo del tipo de consulta.
  -@citaMedicoCodigo: C�digo del m�dico.
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
Descripci�n: Procedimiento para buscar un paciente en la tabla Salud.paciente utilizando el DNI, el nombre completo o el tel�fono.
Par�metros: 
 -@pacienteDNI: El DNI del paciente que se desea buscar (puede ser NULL).
 -@pacienteNombreCompleto: El nombre completo del paciente que se desea buscar (puede ser NULL).
 -@pacienteTelefono: El tel�fono del paciente que se desea buscar (puede ser NULL).
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

/*************************************************************************************************************************
Procedimiento: pro_CambiarEstadoPaciente
Descripci�n: Cambiar el estado pacienteEstado con I de inactivo al momento de eliminar al paciente.
Par�metros: 
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
Descripci�n: Procedimiento para visualizar las citas que tiene un cliente.
Par�metros: 
 -@pacienteCodigo: C�digo del paciente para el que se desean visualizar las citas.

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
Descripci�n: Procedimiento para visualizar todas las citas del Dia
Par�metros: 
 -@pacienteCodigo: C�digo del paciente para el que se desean visualizar las citas.

***************************************************************************************************************************/

create or alter procedure pro_Mostrar_Citas
as
begin
    set nocount on;

    select 
        c.citaCodigo,
        c.citaEstado,
        c.citaFechaHora,
        c.citaTipoConsultaCodigo,
		m.medicoCodigo,
        m.medicoNombre,
        m.medicoApellido,
		e.especialidadCodigo,
		e.especialidadNombre,
		p.pacienteCodigo,
		p.pacienteNombreCompleto
    from 
        Gestion.cita as c
    join 
        Administracion.medico as m on c.citaMedicoCodigo = m.medicoCodigo
	join
        Administracion.especialidad as e on m.especialidadCodigo = e.especialidadCodigo  -- Relaci�n con Especialidad
    join
        Salud.Pacientes as p on c.citaPacienteCodigo = p.pacienteCodigo;

    set nocount off;
end
go

/******************************************************************************************
Descripci�n de procedimiento almacenado:
---------------------------------------------------------------------------------------------
Agrega una nueva historia cl�nica para un paciente, especificando su c�digo de paciente, antecedentes m�dicos, 
alergias, y las fechas de creaci�n y actualizaci�n.

---------------------------------------------------------------------------------------------
Fecha        Usuario         Descripci�n de cambio
---------------------------------------------------------------------------------------------
<12/11/2024> <Jonathan Roque>      Creaci�n inicial
**************************************************************************************/
create or alter procedure pro_Agregar_HistoriaClinica
    @historialClinicoCodigo nchar(10),
    @antecedentesMedicos nvarchar(255) = null,
    @alergias nvarchar(255) = null,
    @fechaCreacion date
	as
	begin
		set nocount on;

		insert into Salud.HistoriaClinica
		(
			historialClinicoCodigo,
			antecedentesMedicos,
			alergias,
			fechaCreacion
		)
		values
		(
			@historialClinicoCodigo,
			@antecedentesMedicos,
			@alergias,
			@fechaCreacion
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



--*******************************************************************************************************************************************************
--PROCEDIMIENTOS ALMACENADOS PARA ATENDER CONSULTAS 
/******************************************************************************************
Procedimiento: pro_Crear_Consulta
Descripci�n: Este procedimiento se enfoca �nicamente en insertar los datos proporcionados en la tabla Gestion.Consulta.
-@consultaCodigo: C�digo �nico para la consulta.
-@consultaCitaCodigo: Codigo de la cita
-@consultaFechaHoraFinal: Fecha y hora de la consulta.
-@consultaMedicoCodigo: C�digo del m�dico asociado a la consulta.
-@consultaPacienteCodigo: C�digo del paciente asociado.
-@consultaMotivo: Motivo de la consulta.
-@consultaEstado: Estado de la consulta (P = Pendiente, por defecto).
******************************************************************************************/
    create or alter procedure pro_Crear_Consulta
    @consultaCodigo nchar(10),
    @consultaCitaCodigo nchar(10),
    @consultaFechaHoraFinal datetime = null,
    @consultaMedicoCodigo nchar(10),
    @consultaPacienteCodigo nchar(10),
    @consultaMotivo nvarchar(255) = null
    as
    begin
        set nocount on;

    -- Inserci�n directa en la tabla Gestion.Consulta
    insert into Gestion.Consulta (
        consultaCodigo,
        consultacitaCodigo,
        consultaFechaHoraFinal,
        consultaMotivo
    )
    values (
        @consultaCodigo,
        @consultaCitaCodigo,
        @consultaFechaHoraFinal,
        @consultaMotivo
    );
    set nocount off;
end;
go




/******************************************************************************************
Procedimiento: pro_Listar_Consulta
Descripci�n: Procedimiento almacenado que devuelve una lista de consultas ordenadas seg�n la importancia.
-Codigo: C�digo de la consulta.
-FechaHora: Fecha y hora de la consulta.
-Medico: C�digo del m�dico.
-Paciente: C�digo del paciente.
-Motivo: Motivo de la consulta.
-Estado: Estado de la consulta (P, C, X).
******************************************************************************************/

create or alter procedure pro_Listar_Consulta
as
begin
    set nocount on;

    select 
        c.consultaCodigo,
        ct.citaFechaHora as HoraCitaInicio,
		ct.citaPacienteCodigo as PacienteCodigo,
		ct.citaMedicoCodigo as MedicoCodigo,
		ct.citaEstado as EstadoCita,
        c.consultaFechaHoraFinal,
        c.consultaMotivo
    from 
        Gestion.Consulta c
    left join 
        Gestion.Cita ct on c.consultacitaCodigo = ct.citaCodigo; 

    set nocount off;
end;
go



/******************************************************************************************
Procedimiento: pro_Cambiar_Estado_Cita
Descripci�n:  Procedimiento almacenado para cambiar el estado de una consulta.
-@consultaCodigo: El c�digo de la consulta que deseas modificar.
-@nuevoEstado: El nuevo estado que deseas asignar a la consulta. Los posibles valores son 'P', 'C', 'X'.
-@cambiomedicoCodigo: El c�digo del m�dico que est� realizando el cambio.
-@cambioDescripcion: Una descripci�n que indica qu� cambio se ha realizado.
******************************************************************************************/


create or alter procedure pro_Actualizar_Estado_CitaPendiente
    @citaCodigo nchar(10)
as
begin
    set nocount on;

    update Gestion.cita
    set citaEstado = 'P'
    where citaCodigo = @citaCodigo;

    set nocount off;
end;
go

create or alter procedure pro_Actualizar_Estado_CitaCancelado
    @citaCodigo nchar(10)
as
begin
    set nocount on;

    update Gestion.cita
    set citaEstado = 'C'
    where citaCodigo = @citaCodigo;

    set nocount off;
end;
go

create or alter procedure pro_Actualizar_Estado_CitaNoAsistio
    @citaCodigo nchar(10)
as
begin
    set nocount on;

    update Gestion.cita
    set citaEstado = 'N'
    where citaCodigo = @citaCodigo;

    set nocount off;
end;
go

create or alter procedure pro_Actualizar_Estado_CitaAtendido
    @citaCodigo nchar(10)
as
begin
    set nocount on;

    update Gestion.cita
    set citaEstado = 'A'
    where citaCodigo = @citaCodigo;

    set nocount off;
end
go

/******************************************************************************************
Descripci�n de procedimiento almacenado: Genera un c�digo �nico basado en un prefijo y la secuencia en una columna espec�fica.
---------------------------------------------------------------------------------------------
 Fecha     Usuario      Descripci�n de cambio
---------------------------------------------------------------------------------------------
 12/11/2024  Usuario       Creaci�n del procedimiento con ajuste a nchar(10)
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