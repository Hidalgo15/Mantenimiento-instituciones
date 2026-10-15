# Mantenimiento Instituciones — Documentación técnica

Resumen técnico breve del proyecto.

## Descripción
Aplicación web ASP.NET Core (Razor/Controllers+Views) para el mantenimiento de catálogo de instituciones, cuentas y elementos relacionados. Está organizada en capas: Presentation (interfaz), Core.Domain (entidades e interfaces), Core.Application (servicios y DTOs) e Infraestructure (DbContext y repositorios).

## Tecnologías principales
- .NET 8
- ASP.NET Core (MVC Controllers + Views)
- Entity Framework Core (sin migraciones incluidas en el repositorio)
- SQL Server (procedimientos almacenados utilizados desde los repositorios)

## Estructura del repositorio
- MantenimientoPresentation: proyecto web (Controllers, Views, Program.cs, configuración). Punto de entrada: MantenimientoPresentation/Program.cs
- Mantenimiento.Core: entidades de dominio y las interfaces de repositorio.
- Mantenimiento.Core.Application: DTOs, servicios de aplicación y mapeos entre entidades y DTOs.
- Mantenimiento.Infraestructure: ApplicationDbContext (mapeos a tablas y vistas) y repositorios que implementan las interfaces del Core.

## Arquitectura y patrones
- Separación por capas (Presentation / Application / Domain / Infrastructure).
- Repositorios implementados en Infrastructure consumen procedimientos almacenados mediante FromSqlRaw y ExecuteSqlRawAsync.
- Servicios de aplicación (Core.Application) exponen la lógica usada por los controladores.
- Inyección de dependencias configurada en Program.cs: todos los repositorios y servicios se registran con AddScoped.

## Base de datos
- La aplicación usa SQL Server. La clave de conexión esperada en appsettings.json se llama: `ConexionSIGOB`.
- Muchos repositorios llaman a procedimientos almacenados (ejemplo: ct.sp_CuentasCt_Obtener, ct.sp_CuentasCt_Insertar, etc.). Revisar los repositorios en Mantenimiento.Infraestructure/Repositories para ver los nombres.
- ApplicationDbContext mapea tablas y vistas usando Fluent API en OnModelCreating; algunas entidades usan .HasNoKey() para vistas o resultados de SP.

## Endpoints y controladores clave
Los controladores están en MantenimientoPresentation/Controllers. Algunos controladores importantes:
- HomeController
- InstitucionController (consultas y mantenimiento de instituciones)
- CuentaInstitucionController
- CuentaCtController (gestiona cuentas CT, usa procedimientos en schema `ct`)
- CapituloController, SubCapituloController, DafController, UnidadEjecutoraController
- CuentaRequisitoController, CuentaMapeoController, RequisitoController

## Cómo ejecutar el proyecto (desarrollo)
1. Abrir la solución `ManteniminetoInstituciones.sln` en Visual Studio 2022/2026 o VS Code.
2. Asegurarse de tener .NET 8 SDK instalado.
3. Configurar la cadena de conexión `ConexionSIGOB` en `MantenimientoPresentation/appsettings.json` o en secrets:

   "ConnectionStrings": {
	 "ConexionSIGOB": "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;"
   }

4. Ejecutar el proyecto `MantenimientoPresentation` (como proyecto de inicio) desde Visual Studio o con `dotnet run --project ManteniminetoInstituciones/MantenimientoPresentation`.

## Consideraciones técnicas y notas de implementación
- Repositorios usan ADO/EF Core SQL parameters (SqlParameter) para evitar inyección SQL; se usan FromSqlRaw y ExecuteSqlRawAsync.
- Muchos DTOs y mapeos están en Core.Application/Mappings. Mantener sincronía entre los nombres de columna en ApplicationDbContext y las propiedades de las entidades.
- Vistas y procedimientos: algunas entidades están mapeadas a vistas con HasNoKey(); su uso es de solo lectura.
- No se incluyen migraciones EF en el repo; si se requieren, crear migraciones en el proyecto Infrastructure apuntando a ApplicationDbContext.

## Buenas prácticas y mejoras sugeridas
- Añadir manejo centralizado de errores y logging (por ejemplo, Serilog). Actualmente Program.cs configura manejo básico de excepciones para producción.
- Añadir pruebas unitarias e integración para servicios y repositorios.
- Documentar los procedimientos almacenados (parámetros esperados y esquemas) fuera del código para facilitar mantenimiento.
- Considerar uso de Dapper para procedimientos almacenados si se busca rendimiento en consultas muy complejas.

## Contribución
- Trabajar en una rama por feature/bugfix.
- Mantener la convención de capas y registrar nuevos servicios/repositorios en `Program.cs`.

## Licencia y autores
Repositorio privado / sin license especificada. Contactar al mantenedor para detalles.

---
Archivo generado automáticamente: README técnico resumido del proyecto.
