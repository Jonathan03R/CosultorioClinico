
# Sistema de Gestión de Consultorio Clínico

Este proyecto es un sistema de gestión para un consultorio clínico desarrollado en **Visual Studio**, siguiendo una arquitectura de **n-capas**. 

## Arquitectura del Proyecto

El sistema está estructurado en las siguientes capas:

1. **Capa de Presentación (Capa1_Presentacion)**  
   Implementada como una **aplicación web ASP.NET Framework**. Contiene la interfaz gráfica del usuario (GUI), diseñada para interactuar directamente con el sistema. Aquí se incluyen vistas y controladores que se comunican con la capa de aplicación.

2. **Capa de Aplicación (Capa2_Aplicacion)**  
   Actúa como intermediario entre la capa de presentación y las demás capas. Maneja la lógica del negocio, coordina las operaciones y valida datos antes de enviarlos a las otras capas. Incluye servicios como `GestionarCitaServicio` y `GestionarPacienteServicio`.

3. **Capa de Dominio (Capa3_Dominio)**  
   Define las entidades, reglas de negocio y transferencia de datos. Contiene las clases principales que representan los datos y comportamientos del sistema, ubicadas dentro de las carpetas **Entidades** y **TransferenciaDatos**.

4. **Capa de Persistencia (Capa4_Persistencia)**  
   Implementada para trabajar con **SQL Server**. Se encarga de la conexión y las operaciones con la base de datos, como consultas, inserciones, actualizaciones y eliminaciones. Incluye:
   - **ModuloBase:** Contiene excepciones y scripts SQL para la creación de tablas.
   - **ModuloPrincipal:** Contiene las clases de acceso a datos como `PacienteSQL`, `CitasSQL`, entre otras.

## Requisitos Previos

- **Visual Studio** (versión 2022 o superior)
- **SQL Server** (para la base de datos del proyecto)
- **ASP.NET Framework** (para la capa de presentación)

## Configuración del Proyecto

1. Clona este repositorio en tu máquina local:
   ```bash
   git clone https://github.com/tuUsuario/consultorio-clinico.git
   ```

2. Configura la cadena de conexión en la clase `AccesoSQLServer` dentro del **ModuloBase** de la **Capa4_Persistencia**:
   ```csharp
   conexion.ConnectionString = "Data Source=TuServer; Initial Catalog=BdClinicaWeb;Integrated Security=true";
   ```

3. Restaura los paquetes NuGet necesarios desde Visual Studio.

4. Configura la base de datos:
   - Ejecuta el script `TablasCompletas.sql` en tu SQL Server para crear la base de datos y las tablas.

5. Ejecuta el proyecto desde la **Capa1_Presentacion** como proyecto de inicio.

## Funcionalidades Principales

- **Gestión de Pacientes:** Registrar, actualizar y listar pacientes.
- **Gestión de Citas:** Asignación y control de citas médicas.
- **Gestión de Médicos:** Registro y consulta de médicos.
- **Gestión de Historias Clínicas:** Almacenamiento y consulta de antecedentes médicos.

## Tecnologías Utilizadas

- **Lenguaje:** C#  
- **Base de Datos:** SQL Server  
- **Framework de Presentación:** ASP.NET Framework  
- **IDE:** Visual Studio  
- **Arquitectura:** n-capas

## Estructura del Proyecto

```plaintext
├── Capa1_Presentacion
│   └── Web
│       └── ModuloPrincipal
├── Capa2_Aplicacion
│   └── ModuloPrincipal
│       ├── Servicios
│       └── Referencias
├── Capa3_Dominio
│   └── ModuloPrincipal
│       ├── Entidades
│       └── TransferenciaDatos
├── Capa4_Persistencia
│   ├── SqlServer
│   │   ├── ModuloBase
│   │   └── ModuloPrincipal
```

## Contribución

Si deseas colaborar, sigue los pasos:
1. Haz un fork del repositorio.
2. Crea una rama para tu funcionalidad (`git checkout -b nueva-funcionalidad`).
3. Realiza un pull request.

## Licencia

Este proyecto se encuentra bajo la licencia **MIT**. Consulta el archivo `LICENSE` para más detalles.

---

¡Gracias por usar nuestro sistema de gestión para consultorios clínicos! Si tienes preguntas o sugerencias, no dudes en contactarnos.
