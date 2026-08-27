# InventarioVentas.API

Este es el unico proyecto de API de la solucion. Su responsabilidad es recibir solicitudes HTTP, coordinar los modulos del sistema y exponer respuestas REST. Por eso representa el monolito completo, no un modulo individual.

## Piezas principales

- `Program.cs`: punto de entrada y configuracion del pipeline de ASP.NET Core.
- `Modules/`: funcionalidades de negocio aisladas por dominio.
- `Common/`: tipos transversales reutilizables.
- `Data/`: acceso y configuracion de persistencia con Entity Framework Core.
- `Extensions/`: metodos de registro y configuracion de servicios.
- `Middleware/`: comportamiento transversal del pipeline HTTP.
- `Properties/`: perfiles locales de ejecucion.
- `appsettings*.json`: configuracion por ambiente. No guardar secretos aqui.

## Composicion actual

La API registra `AddControllers`, `AddEndpointsApiExplorer` y `AddSwaggerGen`. El pipeline aplica `UseHttpsRedirection` y mapea controllers con `MapControllers`.

## Convención de nomenclatura

Los namespaces, carpetas de módulos, archivos de código, clases, interfaces, métodos, variables y propiedades se nombran en inglés. Las rutas HTTP y las claves JSON existentes conservan sus nombres en español para evitar romper consumidores actuales; los DTOs usan `JsonPropertyName` para separar el nombre interno en inglés del contrato externo.

Swagger UI y su especificacion JSON solo se habilitan en Development:

- `http://localhost:5011/swagger`
- `https://localhost:7176/swagger`
- `http://localhost:5011/swagger/v1/swagger.json`

La aplicacion usa Swashbuckle como estrategia unica de documentacion OpenAPI. No se combina con `AddOpenApi` ni con `Microsoft.AspNetCore.OpenApi` para evitar dos configuraciones de documentacion en paralelo.

## Flujo esperado

Una solicitud entra por un controller de un modulo, se transforma mediante DTOs, pasa por validadores y services, y finalmente usa `Data` cuando necesita persistencia. El controller devuelve la respuesta HTTP; la regla de negocio pertenece al service.

Los modulos pueden colaborar cuando una regla lo exige, pero no deben acceder arbitrariamente a las carpetas internas de otro modulo. Si una dependencia entre modulos se vuelve compleja, documenta la decision antes de introducir una abstraccion nueva.

Actualmente existen endpoints funcionales para Categorías, Productos, Clientes y Ventas. Todos los services usan el `AppDbContext` único, registrado mediante `AddApplicationServices`, y las migraciones se generan con ese contexto.

La ejecución requiere `ConnectionStrings:DefaultConnection`, que puede suministrarse mediante User Secrets, una variable de entorno o Docker Compose. Existen pruebas automatizadas iniciales; la verificación HTTP y contra PostgreSQL sigue pendiente.

## Antes de modificar

Revisa [docs/Architecture.md](../../docs/Architecture.md) y el README de la carpeta que vas a tocar. Mantener una sola API no significa mezclar todas las responsabilidades en `Program.cs` o en carpetas globales.
