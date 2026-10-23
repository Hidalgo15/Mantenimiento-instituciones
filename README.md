# Sistema de Mantenimiento Institucional

Aplicación web para consultar y mantener información institucional y catálogos relacionados con la gestión de cuentas y contratos. Esta guía explica qué permite hacer el sistema, qué se necesita para ejecutarlo y cómo está organizado el código.

## Para las personas usuarias

### ¿Qué permite hacer?

Desde el panel principal se accede a estos módulos:

- **Estructura institucional:** capítulos, subcapítulos, DAF y unidades ejecutoras.
- **Cuentas y requisitos:** cuentas CT, requisitos, asignación de requisitos a cuentas, cuentas de instituciones y mapeo de cuentas.
- **Gestión complementaria:** asignación de analistas y supervisores, fondos especiales y responsables de actividades.
- **Contratos:** clasificación de contratos, requisitos de contratos y relación entre contratos y requisitos.

Las opciones disponibles dependen de la información de la base de datos a la que se conecte la aplicación. Para utilizar los módulos se requiere iniciar sesión y que el entorno tenga acceso a SQL Server.

### Inicio de sesión

El inicio de sesión actual es **temporal y simulado**: el código acepta el usuario `admin` y la contraseña `123456`, definidos en `MantenimientoPresentation/Controllers/AuthController.cs`; no consulta un directorio institucional ni una tabla de usuarios. Úsalos solo para desarrollo local: **no son una cuenta segura y no deben usarse en producción**. Véase [Seguridad y limitaciones](#seguridad-y-limitaciones).

## Requisitos previos

- Windows y .NET 8 SDK.
- Visual Studio 2022 o posterior con las herramientas de desarrollo de ASP.NET y web, o PowerShell/terminal con `dotnet`.
- Acceso a una instancia de **SQL Server** y a una base compatible con las tablas, vistas y procedimientos almacenados que utiliza la aplicación.
- Una cadena de conexión autorizada por la persona responsable de la base de datos.

## Ejecutar localmente

Los siguientes comandos se ejecutan desde la carpeta `ManteniminetoInstituciones`, donde está `ManteniminetoInstituciones.sln`.

1. Abre la solución `ManteniminetoInstituciones.sln` en Visual Studio, o abre PowerShell en esa carpeta.
2. Configura la cadena de conexión **en la sesión de PowerShell**. Sustituye los valores de ejemplo por los datos proporcionados para tu entorno:

   ```powershell
	 $env:ConnectionStrings__ConexionSIGOB = "Server=<servidor>;Database=<base_de_datos>;User Id=<usuario>;Password=<contraseña>;"
   ```

	  `ConexionSIGOB` es el nombre de configuración que lee la aplicación. La variable de entorno sobrescribe el valor de `appsettings.json` para los procesos iniciados desde esa sesión. Para ejecutar desde Visual Studio, define esta variable en el entorno de Windows antes de abrir el IDE o inicia Visual Studio desde esta sesión de PowerShell. No guardes contraseñas en el repositorio ni las compartas.

3. Restaura y compila la solución:

   ```powershell
   dotnet restore .\ManteniminetoInstituciones.sln
   dotnet build .\ManteniminetoInstituciones.sln
   ```

4. Inicia la aplicación con el perfil HTTPS:

   ```powershell
   dotnet run --project .\MantenimientoPresentation\MantenimientoPresentation.csproj --launch-profile https
   ```

5. Abre `https://localhost:7149` si el navegador no se abre automáticamente.

El perfil HTTPS y la URL proceden de `MantenimientoPresentation/Properties/launchSettings.json`. La aplicación puede mostrar la pantalla de acceso sin consultar SQL Server, pero las funciones que leen o modifican datos necesitan una conexión válida y los objetos de base de datos correspondientes.

## Base de datos

La aplicación usa SQL Server a través de Entity Framework Core y repositorios que ejecutan procedimientos almacenados. La cadena se obtiene de `ConnectionStrings:ConexionSIGOB`.

El repositorio contiene el archivo `Database/Store Procedures/Stores Procedures modulos.sql`, con procedimientos de algunos módulos. **No es un instalador completo de la base de datos**: no crea por sí solo la base, todas sus tablas, vistas ni datos de referencia. El proyecto tampoco ejecuta migraciones ni inicializa la base al arrancar. Antes de usar un entorno nuevo, la persona responsable de SQL Server debe verificar y preparar el esquema, las vistas, los procedimientos y los permisos requeridos. No ejecutes scripts sobre una base compartida o de producción sin autorización y revisión.

## Para desarrolladores

### Tecnologías y arquitectura

La solución apunta a **.NET 8** y la interfaz usa **ASP.NET Core MVC con vistas Razor** (controladores y `Views`; no Razor Pages). Está separada en cuatro proyectos:

| Proyecto | Responsabilidad |
| --- | --- |
| `MantenimientoPresentation` | Aplicación web: controladores, vistas, configuración, autenticación y punto de entrada (`Program.cs`). |
| `Mantenimiento.Core.Application` | Servicios de aplicación, interfaces de servicio, DTO y mapeos. |
| `Mantenimiento.Core` (`Mantenimiento.Core.Domain.csproj`) | Entidades de dominio e interfaces de repositorio. |
| `Mantenimiento.Infraestructure` (`Mantenimiento.Infraestructure.Persistence.csproj`) | `ApplicationDbContext`, mapeos de Entity Framework Core y repositorios de acceso a datos. |

El flujo habitual es **controlador → servicio → interfaz de repositorio → repositorio → SQL Server**. Las dependencias se registran en `MantenimientoPresentation/Program.cs`. Parte de los repositorios consulta tablas o vistas mediante Entity Framework Core; otros invocan procedimientos almacenados, especialmente con `FromSqlRaw` y `ExecuteSqlRawAsync`.

### Puntos de entrada y configuración

- Solución: `ManteniminetoInstituciones.sln`.
- Proyecto web: `MantenimientoPresentation/MantenimientoPresentation.csproj`.
- Arranque, registro de dependencias y rutas: `MantenimientoPresentation/Program.cs`.
- Configuración: `MantenimientoPresentation/appsettings.json` y `appsettings.Development.json`.
- Perfil local: `MantenimientoPresentation/Properties/launchSettings.json`.
- Entidades y contratos: `Mantenimiento.Core/Entities` y `Mantenimiento.Core/RepositoryInterfaces`.
- Servicios, DTO y mapeos: `Mantenimiento.Core.Application`.
- Persistencia: `Mantenimiento.Infraestructure/DbContext` y `Mantenimiento.Infraestructure/Repositories`.
- Interfaz: `MantenimientoPresentation/Controllers` y `MantenimientoPresentation/Views`.
- Procedimientos SQL incluidos: `Database/Store Procedures/Stores Procedures modulos.sql`.

Para incorporar o modificar un módulo, revisa las capas relacionadas y registra sus servicios y repositorios en `Program.cs`. Mantén sincronizados los nombres y tipos de columnas entre entidades, mapeos de `ApplicationDbContext`, resultados de procedimientos y DTO que utiliza la vista.

### Compilación y pruebas

Desde la carpeta de la solución:

```powershell
dotnet build .\ManteniminetoInstituciones.sln
```

La solución actual contiene los cuatro proyectos de aplicación indicados, pero no incluye un proyecto de pruebas automatizadas.

## Seguridad y limitaciones

- `appsettings.json` contiene actualmente una cadena de conexión con credenciales. No publiques ni reutilices esos valores. Si corresponden a credenciales activas, solicita su rotación y mantenlas fuera del control de versiones; usa una variable de entorno como la descrita arriba.
- El inicio de sesión está simulado con credenciales fijas en el código. Sustituye este mecanismo por autenticación institucional antes de cualquier despliegue real. No despliegues la aplicación con las credenciales de desarrollo.
- La autenticación mediante cookies expira a los 20 minutos según la configuración actual; esto no reemplaza un diseño completo de identidad, autorización por roles ni gestión de usuarios.
- La aplicación depende de procedimientos, vistas, tablas y permisos propios de la base institucional. Un error de conexión o la ausencia de esos objetos puede impedir que los módulos de datos funcionen.
- No se encontraron migraciones de Entity Framework Core en los proyectos actuales.
