# InventarioVentas

API REST para gestionar categorias, productos, clientes y ventas. El proyecto usa .NET 10, ASP.NET Core Web API, Entity Framework Core y SQL Server.

## Como pensar el proyecto

La aplicacion es un monolito modular: existe una sola API y un solo despliegue, pero cada funcionalidad vive dentro de su propio modulo. Los modulos actuales son `Categorias`, `Productos`, `Clientes` y `Ventas`.

La regla mas importante es que cada cambio debe quedar en el modulo al que pertenece. Las piezas compartidas solo deben contener comportamiento realmente transversal y no reglas de una funcionalidad concreta.

## Donde encontrar cada cosa

- [docs/](docs/README.md): decisiones de arquitectura, alcance funcional y comandos de configuracion.
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

Los comandos de instalacion y configuracion estan en [docs/project_configuration_commands.md](docs/project_configuration_commands.md).

## Reglas para colaborar

1. Antes de crear una carpeta nueva, revisa la documentacion de la carpeta padre.
2. Los controllers atienden HTTP; no deben contener reglas de negocio.
3. Los DTOs son contratos de la API; no se deben exponer las entidades directamente.
4. Los services implementan las reglas de negocio del modulo.
5. `Common`, `Data`, `Extensions` y `Middleware` no deben convertirse en un lugar para esconder logica de cualquier modulo.
6. Si una decision cambia la estructura o el contrato del sistema, actualiza la documentacion correspondiente.

## Estado actual

La estructura modular ya esta creada, pero la implementacion funcional se encuentra en una etapa inicial. Las carpetas de los modulos contienen README para explicar donde debe ir cada pieza antes de agregar codigo.

No se documentan `bin/` ni `obj/` porque son salidas generadas por .NET. Tampoco se agregan README a `.git`, `.agents` o `.codex`, porque no forman parte del codigo funcional del proyecto.
