

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
Descripci�n: Muestra el historial cl�nico completo de un paciente, incluyendo su relaci�n 
con el m�dico que lo atendi�.
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente cuyo historial cl�nico se desea obtener.
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
Descripci�n: Muestra los contactos de emergencia asociados a un paciente espec�fico.
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente cuyos contactos de emergencia se desean obtener.
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
Descripci�n de procedimiento almacenado:
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
Descripci�n: agrega contasto de emergencia para un paciente en especifico
Par�metros:
    - @pacienteCodigo: C�digo �nico del paciente cuyos contactos de emergencia se desean obtener.
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
    @citaPacienteCodigo nchar(6),
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

--------------------------------------
/*************************************************************************************************************************
Procedimiento: pro_Cancelar_Cita
Descripci�n: procedimiento almacenado para cancelar una cita cambiando el estado de la cita.
Par�metros: 
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
Descripci�n de procedimiento almacenado:
---------------------------------------------------------------------------------------------
Agrega una nueva historia cl�nica para un paciente, especificando su c�digo de paciente, antecedentes m�dicos, 
alergias, y las fechas de creaci�n y actualizaci�n.

---------------------------------------------------------------------------------------------
Fecha        Usuario         Descripci�n de cambio
---------------------------------------------------------------------------------------------
<12/11/2024> <Jonathan Roque>      Creaci�n inicial
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



--*******************************************************************************************************************************************************
--PROCEDIMIENTOS ALMACENADOS PARA ATENDER CONSULTAS 
/******************************************************************************************
Procedimiento: pro_Guardar_Consulta
Descripci�n: Este procedimiento se enfoca �nicamente en insertar los datos proporcionados en la tabla Gestion.Consulta.
-@consultaCodigo: C�digo �nico para la consulta.
-@consultaFechaHora: Fecha y hora de la consulta.
-@consultaMedicoCodigo: C�digo del m�dico asociado a la consulta.
-@consultaPacienteCodigo: C�digo del paciente asociado.
-@consultaMotivo: Motivo de la consulta.
-@consultaEstado: Estado de la consulta (P = Pendiente, por defecto).
******************************************************************************************/
CREATE or alter PROCEDURE pro_Guardar_Consulta
    @consultaCodigo nchar(10),
    @consultaFechaHora datetime,
    @consultaMedicoCodigo nchar(10),
    @consultaPacienteCodigo nchar(10),
    @consultaMotivo nvarchar(255),
    @consultaEstado nchar(1) = 'P' 
AS
BEGIN
    BEGIN TRY
        INSERT INTO Gestion.Consulta (
            consultaCodigo,
            consultaFechaHora,
            consultaMedicoCodigo,
            consultaPacienteCodigo,
            consultaMotivo,
            consultaEstado
        )
        VALUES (
            @consultaCodigo,
            @consultaFechaHora,
            @consultaMedicoCodigo,
            @consultaPacienteCodigo,
            @consultaMotivo,
            @consultaEstado
        );

        PRINT 'Consulta guardada exitosamente.';
    END TRY
    BEGIN CATCH
        PRINT 'Error al guardar la consulta: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

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
CREATE or alter PROCEDURE pro_Listar_Consulta
AS
BEGIN
    BEGIN TRY
        SELECT 
            consultaCodigo AS Codigo,
            consultaFechaHora AS FechaHora,
            consultaMedicoCodigo AS Medico,
            consultaPacienteCodigo AS Paciente,
            consultaMotivo AS Motivo,
            consultaEstado AS Estado
        FROM 
            Gestion.Consulta
        ORDER BY 
            CASE 
                WHEN consultaEstado = 'P' THEN 1 
                WHEN consultaEstado = 'C' THEN 2
                ELSE 3 
            END,
            consultaFechaHora DESC; 

        PRINT 'Consultas listadas exitosamente.';
    END TRY
    BEGIN CATCH
        PRINT 'Error al listar consultas: ' + ERROR_MESSAGE();
    END CATCH
END;
GO
/******************************************************************************************
Procedimiento: pro_GuardarCambios_Consulta
Descripci�n: Este procedimiento almacenado actualizar� los datos de una consulta y, adicionalmente, 
registrar� el cambio en la tabla RegistroCambios para hacer un seguimiento de las modificaciones.
-@consultaCodigo: C�digo de la consulta a modificar.
-@nuevoMotivo: Nuevo motivo o descripci�n de la consulta.
-@nuevoEstado: Nuevo estado de la consulta (P, C, X).
-@cambiomedicoCodigo: El c�digo del m�dico que realiza el cambio.
-@cambioDescripcion: Descripci�n del cambio realizado.
******************************************************************************************/
CREATE or alter PROCEDURE pro_GuardarCambios_Consulta
(
    @consultaCodigo nchar(10),              
    @nuevoMotivo nvarchar(255),       
    @nuevoEstado nchar(1),                
    @cambiomedicoCodigo nchar(10),          
    @cambioDescripcion nvarchar(255)         
)
AS
BEGIN
    BEGIN TRANSACTION;

    UPDATE Gestion.Consulta
    SET 
        consultaMotivo = @nuevoMotivo,
        consultaEstado = @nuevoEstado
    WHERE consultaCodigo = @consultaCodigo;

    INSERT INTO Salud.RegistroCambios
    (
        cambioCodigo,
        cambioHistorialClinicoCodigo,
        cambioDescripcion,
        cambioFecha,
        cambiomedicoCodigo
    )
    VALUES
    (
        NEWID(),                                         
        (SELECT historialClinicoCodigo FROM Salud.HistoriaClinica WHERE pacienteCodigo = (SELECT consultaPacienteCodigo FROM Gestion.Consulta WHERE consultaCodigo = @consultaCodigo)),
        @cambioDescripcion,
        GETDATE(),                                      
        @cambiomedicoCodigo                               
    );

    COMMIT TRANSACTION;
END
GO

/******************************************************************************************
Procedimiento: pro_Mostrar_Consulta_Pacientes
Descripci�n: Procedimiento almacenado para mostrar los pacientes con su motivo de consulta.
-pacienteCodigo: El c�digo �nico del paciente.
-pacienteNombreCompleto: El nombre completo del paciente.
-pacienteDNI: El DNI del paciente.
-pacienteTelefono: El tel�fono del paciente.
-consultaCodigo: El c�digo de la consulta.
-consultaMotivo: El motivo de la consulta.
-La condici�n c.consultaEstado = 'P' filtra las consultas que est�n en estado Pendiente. Puedes quitar o modificar este filtro seg�n tus necesidades.
******************************************************************************************/
CREATE or alter PROCEDURE pro_Mostrar_Consulta_Pacientes
AS
BEGIN
    SELECT 
        p.pacienteCodigo,
        p.pacienteNombreCompleto,
        p.pacienteDNI,
        p.pacienteTelefono,
        c.consultaCodigo,
        c.consultaMotivo
    FROM 
        Salud.Pacientes p
    INNER JOIN 
        Gestion.Consulta c ON p.pacienteCodigo = c.consultaPacienteCodigo
    WHERE 
        c.consultaEstado = 'P' 
    ORDER BY 
        p.pacienteNombreCompleto; 
END
GO
/******************************************************************************************
Procedimiento: pro_Cambiar_Estado_Consulta
Descripci�n:  Procedimiento almacenado para cambiar el estado de una consulta.
-@consultaCodigo: El c�digo de la consulta que deseas modificar.
-@nuevoEstado: El nuevo estado que deseas asignar a la consulta. Los posibles valores son 'P', 'C', 'X'.
-@cambiomedicoCodigo: El c�digo del m�dico que est� realizando el cambio.
-@cambioDescripcion: Una descripci�n que indica qu� cambio se ha realizado.
******************************************************************************************/
CREATE or alter PROCEDURE pro_Cambiar_Estado_Consulta
(
    @consultaCodigo nchar(10),           
    @nuevoEstado nchar(1),                
    @cambiomedicoCodigo nchar(10),        
    @cambioDescripcion nvarchar(255)     
)
AS
BEGIN
    UPDATE Gestion.Consulta
    SET 
        consultaEstado = @nuevoEstado
    WHERE consultaCodigo = @consultaCodigo;

    INSERT INTO Salud.RegistroCambios
    (
        cambioCodigo,
        cambioHistorialClinicoCodigo,
        cambioDescripcion,
        cambioFecha,
        cambiomedicoCodigo
    )
    VALUES
    (
        NEWID(),                                  
        (SELECT historialClinicoCodigo FROM Salud.HistoriaClinica WHERE pacienteCodigo = (SELECT consultaPacienteCodigo FROM Gestion.Consulta WHERE consultaCodigo = @consultaCodigo)),
        @cambioDescripcion,
        GETDATE(),                                      
        @cambiomedicoCodigo                            
    );
END
GO



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
    set @vCodigoGenerado = @prefijo + right('000000' + cast(@vNuevoCodigo as varchar(6)), 6);
    select @vCodigoGenerado as CodigoUnico;

set nocount off;
go
