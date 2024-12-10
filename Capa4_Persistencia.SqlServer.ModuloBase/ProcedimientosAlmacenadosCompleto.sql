

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
Descripción: Muestra el historial clínico completo de un paciente, incluyendo su relación 
con el médico que lo atendió.
Parámetros:
    - @pacienteCodigo: Código único del paciente cuyo historial clínico se desea obtener.
******************************************************************************************/
CREATE OR ALTER PROCEDURE pro_Mostrar_HistoriaClinica
    @pacienteCodigo NCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.pacienteCodigo,               
        hc.historialClinicoCodigo,      
        hc.HistoriaClinicafechaCreacion,
        c.consultaCodigo,
        c.consultacitaCodigo
    FROM Salud.Pacientes p
    INNER JOIN Salud.HistoriaClinica hc
        ON p.historialClinicoCodigo = hc.historialClinicoCodigo
    LEFT JOIN Gestion.Consulta c
        ON p.pacienteCodigo = c.pacienteCodigo
    WHERE p.pacienteCodigo = @pacienteCodigo;
END;
GO

exec pro_Mostrar_HistoriaClinica @pacienteCodigo = PAC0000001
GO

/******************************************************************************************
Procedimiento: pro_Mostrar_ContactosEmergencia
Descripción: Muestra los contactos de emergencia asociados a un paciente específico.
Parámetros:
    - @pacienteCodigo: Código único del paciente cuyos contactos de emergencia se desean obtener.
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
Descripción de procedimiento almacenado:
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
    @citaFechaHora datetime
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Gestion.cita 
    (
        citaCodigo,
        citaEstado,
        citaFechaHora
    )
    VALUES 
    (
        @citaCodigo,
        @citaEstado,
        @citaFechaHora
    );
END
GO


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

CREATE OR ALTER PROCEDURE pro_VisualizarCitasPaciente
    @pacienteCodigo nchar(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.citaCodigo,
        c.citaEstado,
        c.citaFechaHora,
        tc.tipoConsultaDescripcion,
        m.medicoNombre,
        m.medicoApellido,
        con.consultaFechaHoraFinal
    FROM 
        Gestion.cita c
    INNER JOIN 
        Gestion.Consulta con ON c.citaCodigo = con.consultacitaCodigo
    INNER JOIN 
        Gestion.tipoConsulta tc ON con.tipoConsultaCodigo = tc.tipoConsultaCodigo
    INNER JOIN 
        Administracion.Medico m ON con.medicoCodigo = m.medicoCodigo
    INNER JOIN 
        Salud.Pacientes p ON con.pacienteCodigo = p.pacienteCodigo
    INNER JOIN 
        Salud.HistoriaClinica hc ON hc.historialClinicoCodigo = p.historialClinicoCodigo
    WHERE 
        p.pacienteCodigo = @pacienteCodigo;
END;
GO

--exec pro_VisualizarCitasPaciente @pacienteCodigo = PAC0000001

/*************************************************************************************************************************
Procedimiento: pro_Mostrar_Citas
Descripción: Procedimiento para visualizar todas las citas del Dia
Parámetros: 
 -@pacienteCodigo: Código del paciente para el que se desean visualizar las citas.

***************************************************************************************************************************/

CREATE OR ALTER PROCEDURE pro_Mostrar_Citas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.citaCodigo,
        c.citaEstado,
        c.citaFechaHora,
        tc.tipoConsultaDescripcion AS citaTipoConsultaDescripcion,
        m.medicoCodigo,
        m.medicoNombre,
        m.medicoApellido,
        e.especialidadCodigo,
        e.especialidadNombre,
        p.pacienteCodigo,
        p.pacienteNombreCompleto
    FROM 
        Gestion.cita AS c
    LEFT JOIN 
        Gestion.Consulta AS con ON c.citaCodigo = con.consultacitaCodigo
    LEFT JOIN 
        Administracion.Medico AS m ON con.medicoCodigo = m.medicoCodigo
    LEFT JOIN 
        Administracion.Especialidad AS e ON m.especialidadCodigo = e.especialidadCodigo
    LEFT JOIN 
        Salud.Pacientes AS p ON con.pacienteCodigo = p.pacienteCodigo
    LEFT JOIN 
        Gestion.tipoConsulta AS tc ON con.tipoConsultaCodigo = tc.tipoConsultaCodigo;

    SET NOCOUNT OFF;
END;
GO


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
create or alter procedure pro_Crear_HistoriaClinica
    @historialClinicoCodigo nchar(10)
	as
	begin
		set nocount on;

		insert into Salud.HistoriaClinica
		(
			historialClinicoCodigo
		)
		values
		(
			@historialClinicoCodigo
		);

		set nocount off;
	end
go

/******************************************************************************************
Descripción de procedimiento almacenado:
---------------------------------------------------------------------------------------------
listar la hostorial clinica de un paciente.

**************************************************************************************/
CREATE OR ALTER PROCEDURE pro_listar_HistoriaClinica
    @historialClinicoCodigo NCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.consultaCodigo,
        c.consultacitaCodigo,
        c.consultaFechaHoraFinal,
        c.tipoConsultaCodigo,
        c.medicoCodigo,
        c.pacienteCodigo,
		ci.citaEstado
    FROM 
        Gestion.Consulta AS c
    INNER JOIN 
        Salud.Pacientes AS p ON c.pacienteCodigo = p.pacienteCodigo
	inner join
		Gestion.cita as ci on c.consultacitaCodigo = ci.citaCodigo
    WHERE 
        p.historialClinicoCodigo = @historialClinicoCodigo;

    SET NOCOUNT OFF;
END;
GO

--exec pro_listar_HistoriaClinica @historialClinicoCodigo = HIS0000002
--go
/******************************************************************************************
Descripción de procedimiento almacenado:
---------------------------------------------------------------------------------------------
Lista los diagnosticos por consulta
**************************************************************************************/

CREATE OR ALTER PROCEDURE pro_listar_DiagnosticosPorConsulta
    @consultaCodigo NCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        diagnosticoCodigo,
        diagnosticoDescripcion
    FROM 
        Salud.Diagnostico
    WHERE 
        diagnosticoconsultaCodigo = @consultaCodigo;

    SET NOCOUNT OFF;
END;
GO

CREATE OR ALTER PROCEDURE pro_listar_RecetasPorConsulta
    @consultaCodigo NCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        recetaCodigo,
        recetaDescripcion,
        recetaTratamiento,
        recetaRecomendaciones
    FROM 
        Salud.RecetaMedica
    WHERE 
        recetaConsultaCodigo = @consultaCodigo;

    SET NOCOUNT OFF;
END;
GO


--exec pro_listar_RecetasPorConsulta @consultaCodigo = CON0000002
--go
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
Descripción: Este procedimiento se enfoca únicamente en insertar los datos proporcionados en la tabla Gestion.Consulta.
-@consultaCodigo: Código único para la consulta.
-@consultaCitaCodigo: Codigo de la cita
-@consultaFechaHoraFinal: Fecha y hora de la consulta.
-@consultaMedicoCodigo: Código del médico asociado a la consulta.
-@consultaPacienteCodigo: Código del paciente asociado.
-@consultaMotivo: Motivo de la consulta.
-@consultaEstado: Estado de la consulta (P = Pendiente, por defecto).
******************************************************************************************/
create or alter procedure pro_Crear_Consulta
    @consultaCodigo nchar(10),
    @consultaCitaCodigo nchar(10),
    @consultaFechaHoraFinal datetime = null,

	@medicoCodigo nchar(10),
    @tipoConsultaCodigo nchar(10),
	@pacienteCodigo nchar(10)
    as
    begin
        set nocount on;

    -- Inserción directa en la tabla Gestion.Consulta
    insert into Gestion.Consulta (
		[consultaCodigo], [consultacitaCodigo], [consultaFechaHoraFinal], [medicoCodigo], [tipoConsultaCodigo], [pacienteCodigo]
    )
    values (
        @consultaCodigo,
        @consultaCitaCodigo,
        @consultaFechaHoraFinal,
		@medicoCodigo,
		@tipoConsultaCodigo,
		@pacienteCodigo
    );
    set nocount off;
end;
go


/******************************************************************************************
Procedimiento: pro_Listar_Consulta
Descripción: Procedimiento almacenado que devuelve una lista de consultas ordenadas según la importancia.
-Codigo: Código de la consulta.
-FechaHora: Fecha y hora de la consulta.
-Medico: Código del médico.
-Paciente: Código del paciente.
-Motivo: Motivo de la consulta.
-Estado: Estado de la consulta (P, C, X).
******************************************************************************************/

CREATE OR ALTER PROCEDURE pro_Listar_Consulta
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        C.consultaCodigo,
		C.consultacitaCodigo,
		ci.citaFechaHora,
		ci.citaEstado,
		C.consultaFechaHoraFinal,
		c.pacienteCodigo,
		p.pacienteNombreCompleto,
		c.medicoCodigo,
		me.medicoNombre,
		me.medicoApellido,
		c.tipoConsultaCodigo,
		p.historialClinicoCodigo
		
    FROM 
        Gestion.Consulta AS c
    LEFT join 
		Gestion.cita as ci on ci.citaCodigo = c.consultacitaCodigo
	left join 
		Administracion.Medico as me on c.medicoCodigo = me.medicoCodigo
	left join
		Salud.Pacientes as p on c.pacienteCodigo = p.pacienteCodigo
    SET NOCOUNT OFF;
END;
GO



/******************************************************************************************
Procedimiento: pro_Cambiar_Estado_Cita
Descripción:  Procedimiento almacenado para cambiar el estado de una consulta.
-@consultaCodigo: El código de la consulta que deseas modificar.
-@nuevoEstado: El nuevo estado que deseas asignar a la consulta. Los posibles valores son 'P', 'C', 'X'.
-@cambiomedicoCodigo: El código del médico que está realizando el cambio.
-@cambioDescripcion: Una descripción que indica qué cambio se ha realizado.
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



create or alter procedure pro_Actualizar_Estado_CitaAtendiendose
    @citaCodigo nchar(10)
as
begin
    set nocount on;

    update Gestion.cita
    set citaEstado = 'T'
    where citaCodigo = @citaCodigo;

    set nocount off;
end
go


/******************************************************************************************
Descripción de procedimiento almacenado: Verificar si la cita existe
---------------------------------------------------------------------------------------------
**************************************************************************************/



--CREATE or alter PROCEDURE pro_Verificar_Cita_Existente
--		@fechaHora DATETIME,
--		@medicoCodigo VARCHAR(50)
--	AS
--	BEGIN
--		IF EXISTS (SELECT 1 FROM Gestion.cita WHERE CitaFechaHora = @fechaHora AND CitaMedicoCodigo = @medicoCodigo)
--		BEGIN
--			SELECT 1;  -- La cita ya existe
--		END
--		ELSE
--		BEGIN
--			SELECT 0;  -- No hay cita existente
--		END
--	END
--go
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


/******************************************************************************************
Descripción de procedimiento almacenado: 
Este procedimiento me lista la agenda de un medico en una fecha elegida 
---------------------------------------------------------------------------------------------
 Fecha     Usuario      Descripción de cambio
---------------------------------------------------------------------------------------------
 12/11/2024  Usuario       Creación del procedimiento con ajuste a nchar(10)
**************************************************************************************/
--EXEC pro_Listar_AgendaMedico @medicoCodigo = 'MED0000004', @fecha = '2024-12-21';


CREATE OR ALTER PROCEDURE pro_Listar_AgendaMedico
    @medicoCodigo nchar(10),
    @fecha date
AS
BEGIN
    -- Variables para almacenar las horas de inicio y fin del horario
    DECLARE @horaInicio time;
    DECLARE @horaFin time;
    
    -- Seleccionar el horario del médico para el día especificado
    SELECT 
        @horaInicio = h.horarioHoraInicio,
        @horaFin = h.horarioHoraFin
    FROM 
        Gestion.horario h
    WHERE 
        h.medicoCodigo = @medicoCodigo
        AND UPPER(h.horarioDia) = UPPER(DATENAME(weekday, @fecha));

    -- Validar si se encontró el horario
    IF @horaInicio IS NULL OR @horaFin IS NULL
    BEGIN
        PRINT 'No se encontró horario para el médico en el día especificado.';
        RETURN;
    END

    -- Crear una tabla temporal para los intervalos de tiempo
    CREATE TABLE #IntervalosTiempo (
        HoraInicio time,
        HoraFin time
    );
    
    -- Insertar intervalos de tiempo basados en el horario del médico
    DECLARE @intervaloMinutos int = 60; -- Intervalo de 1 hora (60 minutos)
    DECLARE @currentInicio time = @horaInicio;
    DECLARE @currentFin time;

    WHILE @currentInicio < @horaFin
    BEGIN
        SET @currentFin = DATEADD(minute, @intervaloMinutos, @currentInicio);
        
        IF @currentFin <= @horaFin
        BEGIN
            INSERT INTO #IntervalosTiempo (HoraInicio, HoraFin)
            VALUES (@currentInicio, @currentFin);
        END
        
        SET @currentInicio = @currentFin;
    END;
    
    -- Seleccionar la agenda del médico
    SELECT 
        i.HoraInicio,
        i.HoraFin,
        COALESCE(c.citaEstado, 'Libre') AS estado,
        c.citaCodigo,
        c.citaPacienteCodigo,
        c.citaMedicoCodigo,
        ISNULL(m.medicoNombre + ' ' + m.medicoApellido, 'N/A') AS nombreMedico,
        ISNULL(p.pacienteNombreCompleto, 'N/A') AS pacienteNombreCompleto
    FROM 
        #IntervalosTiempo i
    LEFT JOIN 
        Gestion.cita c ON c.citaMedicoCodigo = @medicoCodigo 
                       AND CONVERT(date, c.citaFechaHora) = @fecha
                       AND CONVERT(time, c.citaFechaHora) >= i.HoraInicio
                       AND CONVERT(time, c.citaFechaHora) < DATEADD(minute, @intervaloMinutos, i.HoraInicio)
    LEFT JOIN 
        Administracion.Medico m ON c.citaMedicoCodigo = m.medicoCodigo
    LEFT JOIN 
        Salud.Pacientes p ON c.citaPacienteCodigo = p.pacienteCodigo
    ORDER BY 
        i.HoraInicio;

    -- Eliminar la tabla temporal
    DROP TABLE #IntervalosTiempo;
END
GO

--EXEC pro_Listar_AgendaMedico @medicoCodigo = 'MED0000004', @fecha = '2024-12-21';


/******************************************************************************************
Descripción de procedimiento almacenado: 
Procedimiento para crear detaññes de ima comsulta

**************************************************************************************/
CREATE or alter PROCEDURE pro_registrar_DetallesConsulta
    @DetallesConsultaCodigo NCHAR(10),
    @DetallesConsultaHistoriaEnfermedad NVARCHAR(500) = NULL,
    @DetallesConsultaRevisiones NVARCHAR(500) = NULL,
    @DetallesConsultaEvaluacionPsico NVARCHAR(500) = NULL,
    @DetallesConsultaMotivoConsulta NVARCHAR(500) = NULL,
    @ConsultaCodigo NCHAR(10)
AS
BEGIN
        -- Insertar en la tabla DetallesConsulta
        INSERT INTO Gestion.DetallesConsulta (
            detallesConsultaCodigo,
            detallesConsultaHistoriaEnfermedad,
            detallesConsultaRevisiones,
            detallesConsultaEvaluacionPsico,
            detallesConsultaMotivoConsulta,
            consultaCodigo
        )
        VALUES (
            @DetallesConsultaCodigo,
            @DetallesConsultaHistoriaEnfermedad,
            @DetallesConsultaRevisiones,
            @DetallesConsultaEvaluacionPsico,
            @DetallesConsultaMotivoConsulta,
            @ConsultaCodigo
        );
END;
GO

--procedimiento aqui oe
CREATE OR ALTER PROCEDURE pro_registrar_RecetaMedica
        @RecetaCodigo NCHAR(10),
        @RecetaConsultaCodigo NCHAR(10),
        @RecetaDescripcion NVARCHAR(500),
        @RecetaTratamiento NVARCHAR(500),
        @RecetaRecomendaciones NVARCHAR(500)
AS
BEGIN
        -- Insertar en la tabla RecetaMedica
        INSERT INTO Salud.RecetaMedica (
            recetaCodigo,
            recetaConsultaCodigo,
            recetaDescripcion,
            recetaTratamiento,
            recetaRecomendaciones
        )
        VALUES (
            @RecetaCodigo,
            @RecetaConsultaCodigo,
            @RecetaDescripcion,
            @RecetaTratamiento,
            @RecetaRecomendaciones
    );
END;
GO


CREATE OR ALTER PROCEDURE pro_registrar_Diagnostico
    @DiagnosticoCodigo NCHAR(10),
    @DiagnosticoConsultaCodigo NCHAR(10),
    @DiagnosticoDescripcion NVARCHAR(255),
    @DiagnosticoCodigoCie11  nvarchar(50) NULL
AS
BEGIN
    INSERT INTO Salud.Diagnostico (
        diagnosticoCodigo,
        diagnosticoconsultaCodigo,
        diagnosticoDescripcion,
        diagnosticosCodigoCie11
    )
    VALUES (
        @DiagnosticoCodigo,
        @DiagnosticoConsultaCodigo,
        @DiagnosticoDescripcion,
        @DiagnosticoCodigoCie11

    );
END;
GO
