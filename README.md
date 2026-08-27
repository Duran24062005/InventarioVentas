<div align="center">
    <img src="https://camo.githubusercontent.com/dd5617114d80a0ba78cf3399d700ae1597ae87c832a28062848c4fafb54546da/68747470733a2f2f6769746875622e6769746875626173736574732e636f6d2f6173736574732f70726f66696c652d66697273742d69737375652d6461726b2d6238646262303236383762322e737667" alt="github image">
    <h1>InventarioVentas</h1>
</div>

# Introducción

API REST para gestionar categorias, productos, clientes y ventas. El proyecto usa .NET 10, ASP.NET Core Web API, Entity Framework Core y PostgreSQL.

## Como pensar el proyecto

La aplicacion es un monolito modular: existe una sola API y un solo despliegue, pero cada funcionalidad vive dentro de su propio modulo. Los modulos actuales son `Categorias`, `Productos`, `Clientes` y `Ventas`.

La regla mas importante es que cada cambio debe quedar en el modulo al que pertenece. Las piezas compartidas solo deben contener comportamiento realmente transversal y no reglas de una funcionalidad concreta.

## Donde encontrar cada cosa

- [docs/](docs/README.md): decisiones de arquitectura, alcance funcional y comandos de configuracion.
- [docs/CI.md](docs/CI.md): workflow e instrucciones de integración continua.
- [docs/Testing.md](docs/Testing.md): alcance, comandos y limitaciones de las pruebas.
- [docs/Docker.md](docs/Docker.md): instrucciones para construir y ejecutar la API con Docker.
- [prd/](prd/README.md): PRDs ordenados por dependencia y registro de trazabilidad del proyecto.
- [todo-task.md](todo-task.md): estado actual, bloqueos y pasos pendientes de implementación.
- [src/](src/README.md): codigo fuente.
- [src/InventarioVentas.API/](src/InventarioVentas.API/README.md): API principal y composicion de la aplicacion.
- [docs/Architecture.md](docs/Architecture.md): explicacion completa de la arquitectura modular.
- [docs/System_Artifact.md](docs/System_Artifact.md): reglas funcionales y modelo de dominio.

## Inicio rapido

Desde la raiz del repositorio:

```bash
dotnet restore
dotnet build
dotnet run --project src/InventarioVentas.API/InventarioVentas.API.csproj
```

En desarrollo, Swagger UI queda disponible en `http://localhost:5011/swagger` y `https://localhost:7176/swagger` cuando se ejecuta con el perfil correspondiente. La especificacion JSON se encuentra en `/swagger/v1/swagger.json`.

Los comandos de instalacion y configuracion estan en [docs/project_configuration_commands.md](docs/project_configuration_commands.md).

Tambien puedes ejecutar la API con Docker Compose:

```bash
docker compose up --build
```

En ese caso Swagger queda disponible en <http://localhost:5011/swagger>. Compose levanta la API junto con PostgreSQL. Define `POSTGRES_PASSWORD` antes de iniciar los servicios para no versionar credenciales.

## Reglas para colaborar

1. Antes de crear una carpeta nueva, revisa la documentacion de la carpeta padre.
2. Los controllers atienden HTTP; no deben contener reglas de negocio.
3. Los DTOs son contratos de la API; no se deben exponer las entidades directamente.
4. Los services implementan las reglas de negocio del modulo.
5. `Common`, `Data`, `Extensions` y `Middleware` no deben convertirse en un lugar para esconder logica de cualquier modulo.
6. Si una decision cambia la estructura o el contrato del sistema, actualiza la documentacion correspondiente.

## Estado actual

La API cuenta con CRUD para Categorías, Productos y Clientes, además de consulta y creación de Ventas con descuento transaccional de stock. La persistencia usa un único `AppDbContext`, PostgreSQL y migraciones versionadas. El arranque requiere `ConnectionStrings:DefaultConnection`; si falta, la aplicación falla de forma explícita.

La suite automatizada inicial ya cubre validators, ventas y el modelo EF. La verificación funcional HTTP y contra PostgreSQL siguen pendientes; el estado operativo y los siguientes pasos se mantienen en [`todo-task.md`](todo-task.md).

No se documentan `bin/` ni `obj/` porque son salidas generadas por .NET. Tampoco se agregan README a `.git`, `.agents` o `.codex`, porque no forman parte del codigo funcional del proyecto.
