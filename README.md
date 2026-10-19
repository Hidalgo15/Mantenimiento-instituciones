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

## Nuevos módulos añadidos
Se han incorporado módulos para gestión de contratos y su relación con requisitos. Archivo principales y ubicación:
- Controladores: MantenimientoPresentation/Controllers
  - ClasificacionContratoController.cs
  - RequisitosContratosController.cs
  - RelacionContratoRequisitoController.cs
- Vistas: MantenimientoPresentation/Views/{ClasificacionContrato,RequisitosContratos,RelacionContratoRequisito} (Index y parciales de grid)
- DTOs: Mantenimiento.Core.Application/DTOs/{ClasificacionContrato,RequisitoContrato,RelacionContratoRequisito}
- Entidades: Mantenimiento.Core/Entities (nuevas entidades bajo carpetas ClasificacionContrato y RelacionContratoRequisito)
- Repositorios: Mantenimiento.Infraestructure/Repositories
  - ClasificacionContratoRepository.cs
  - RequisitoContratoRepository.cs
  - RelacionContratoRequisitoRepository.cs
- Servicios de aplicación: Mantenimiento.Core.Application/Services
  - ClasificacionContratoService.cs
  - RequisitoContratoService.cs
  - RelacionContratoRequisitoService.cs

Estas piezas permiten:
- Administrar clasificaciones de contrato (catálogos y CRUD básico).
- Gestionar requisitos específicos de contratos (crear, actualizar, eliminar, listar).
- Relacionar contratos con requisitos y obtener catálogos para selección en vistas.

## Registro en la DI (Program.cs)
Los nuevos servicios y repositorios se registran en `MantenimientoPresentation/Program.cs` con AddScoped, por ejemplo:

- IClasificacionContratoRepository -> ClasificacionContratoRepository
- IClasificacionContratoService -> ClasificacionContratoService
- IRequisitoContratoRepository -> RequisitoContratoRepository
- IRequisitoContratoService -> RequisitoContratoService
- IRelacionContratoRequisitoRepository -> RelacionContratoRequisitoRepository
- IRelacionContratoRequisitoService -> RelacionContratoRequisitoService

## Notas específicas sobre los nuevos módulos
- Los repositorios usan los mismos patrones que el resto del proyecto: ejecución de procedimientos almacenados con parámetros (FromSqlRaw / ExecuteSqlRawAsync) y mapeos en ApplicationDbContext cuando corresponda.
- Se proveen DTOs y mapeos (Mappers) en Core.Application/Mappings para convertir entre entidades y DTOs usados por los controladores y vistas.
- Ver las vistas parciales (_ClasificacionContratoGridPartial.cshtml, _RequisitosContratosGridPartial.cshtml, _RelacionContratoRequisitoGridPartial.cshtml) para ejemplos de interacción UI -> endpoints JSON.


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

- Revisar permisos y validaciones en los nuevos controladores para evitar que operaciones de modificación queden sin restricciones.

## Contribución
- Trabajar en una rama por feature/bugfix.
- Mantener la convención de capas y registrar nuevos servicios/repositorios en `Program.cs`.

## Licencia y autores
Repositorio privado / sin license especificada. Contactar al mantenedor para detalles.

## Procedimientos almacenados (listado por repositorio)
A continuación se listan los procedimientos almacenados referenciados en los repositorios (según el código) agrupados por archivo/repo. Se incluyen plantillas SQL básicas (sólo esqueleto) para crear cada SP en la base de datos; completar la lógica según la necesidad.

Nota: los esquemas aparecen tal como en el código (dbo, ct, etc.). Revise permisos y tipos de datos en la base real.

### Repositorio: AnalistaSupervisorRepository.cs
- dbo.sp_Obtener_Mantenimiento_Analistas_Supervisores
- dbo.sp_Obtener_Supervisores_Disponibles
- dbo.sp_Asignar_Supervisor_Analista
- dbo.sp_Reasignar_Supervisor_Analista
- dbo.sp_Eliminar_Asignacion_Analista

### Repositorio: CapituloRepository.cs
- dbo.sp_capitulo_Obtener
- dbo.sp_capitulo_Insertar
- dbo.sp_capitulo_Actualizar
- dbo.sp_capitulo_Eliminar

### Repositorio: SubCapituloRepository.cs
- dbo.sp_subcapitulo_Obtener
- dbo.sp_subcapitulo_Crear
- dbo.sp_subcapitulo_Actualizar
- dbo.sp_subcapitulo_Eliminar

### Repositorio: DafRepository.cs
- dbo.sp_daf_Obtener
- dbo.sp_daf_Crear
- dbo.sp_daf_Actualizar
- dbo.sp_daf_Eliminar (referenciado como sp_daf_Eliminar)

### Repositorio: CategoriaTramiteRepository.cs
- No usa stored procedures; consulta la vista 'v_categoria_tramite' mediante SQL directo en el repositorio.

### Repositorio: FondoEspecialesRepository.cs
- dbo.sp_FondoEspeciales_Insertar
- dbo.sp_FondoEspeciales_Actualizar

### Repositorio: InstitucionRepository.cs
- No usa stored procedures; consulta la vista 'v_institucion' mapeada en ApplicationDbContext.

### Repositorio: TipoTramiteContratoRepository.cs
- No usa stored procedures; consulta tabla/vista TipoTramiteContratos vía DbSet.

### Repositorio: UnidadEjecutoraRepository.cs
- dbo.sp_unidad_ejecutora_Obtener
- dbo.sp_unidad_ejecutora_Crear
- dbo.sp_unidad_ejecutora_Actualizar
- dbo.sp_unidad_ejecutora_Eliminar

### Repositorio: TipoTramitePagoRepository.cs
- dbo.sp_TipoTramitePago_Obtener

### Repositorio: CuentaMapeoRepository.cs
- dbo.sp_Cuentas_Obtener
- dbo.sp_Cuentas_Insertar
- dbo.sp_Cuentas_Actualizar
- dbo.sp_Cuentas_Eliminar

### Repositorio: CuentaCtRepository.cs (schema ct)
- ct.sp_CuentasCt_Obtener
- ct.sp_CuentasCt_Insertar
- ct.sp_CuentasCt_Actualizar
- ct.sp_CuentasCt_Eliminar

### Repositorio: CuentaInstitucionRepository.cs
- dbo.sp_cta_institucion_ObtenerPorEstructura
- dbo.sp_cta_institucion_Insertar
- dbo.sp_cta_institucion_Actualizar
- dbo.sp_cta_institucion_Eliminar

### Repositorio: CuentaRequisitoRepository.cs (schema ct)
- ct.sp_Obtener_Relacion_Cuenta_Requisitos
- ct.sp_Asignar_Cuenta_Requisito
- ct.sp_Actualizar_Relacion_Cuenta_Requisitos
- ct.sp_Eliminar_Relacion_Cuenta_Requisitos
- ct.sp_Obtener_Catalogo_ctCuentas
- ct.sp_Obtener_Catalogo_ctRequisitos

### Repositorio: RequisitoRepository.cs (schema ct)
- [ct].[sp_Obtener_Requisitos]
- [ct].[sp_Crear_Requisito]
- [ct].[sp_Actualizar_Requisito]
- [ct].[sp_Eliminar_Requisito]

### Repositorio: RequisitoContratoRepository.cs
- dbo.sp_requisitos_contratos_Obtener
- dbo.sp_requisitos_contratos_Crear
- dbo.sp_requisitos_contratos_Actualizar
- dbo.sp_requisitos_contratos_Eliminar

### Repositorio: RelacionContratoRequisitoRepository.cs
- dbo.sp_Obtener_Relacion_Contrato_Requisito
- dbo.sp_Asignar_Contrato_Requisito
- dbo.sp_Actualizar_Relacion_Contrato_Requisito
- dbo.sp_Eliminar_Relacion_Contrato_Requisito
- dbo.sp_Obtener_Catalogo_Clasificacion_Contratos
- dbo.sp_Obtener_Catalogo_Requisitos_Contratos

### Repositorio: ClasificacionContratoRepository.cs
- dbo.sp_clasificacion_contratos_Obtener
- dbo.sp_clasificacion_contratos_Crear
- dbo.sp_clasificacion_contratos_Actualizar
- dbo.sp_clasificacion_contratos_Eliminar
- dbo.sp_Obtener_Catalogo_Proceso_Seleccion
