# PRD-001: Base técnica y composición de la API

| Campo | Valor |
| --- | --- |
| Estado | En progreso |
| Prioridad | Alta |
| Dependencias | Ninguna |
| Módulo propietario | Aplicación/API |
| Historias relacionadas | Base para HU01–HU04 |

## Problema y objetivo

La API necesitaba reemplazar el scaffold de .NET por una composición preparada para los módulos de negocio. La composición base ya fue incorporada, pero la integración actual de Categorías agregó una dependencia de persistencia que todavía no está registrada; por eso la aplicación compila, pero no arranca.

## Alcance

- Mantener `InventarioVentas.API` como una sola API y un solo despliegue.
- Configurar el pipeline HTTP y la composición de servicios en `Program.cs`.
- Retirar el endpoint y los tipos del ejemplo `weatherforecast`.
- Conservar Swagger para explorar y validar los endpoints.
- Mantener la configuración por ambiente y evitar secretos en archivos versionados.
- Verificar que la solución compile y que el arranque quede validado después de registrar las dependencias de los módulos.

Fuera de alcance: entidades, base de datos, migraciones, autenticación, autorización y endpoints funcionales de módulos.

## Actores y consumidores

- Desarrolladores que ejecutan la API localmente.
- Clientes HTTP que consumirán los módulos de inventario y ventas.
- Herramientas de documentación y prueba como Swagger/OpenAPI.

## Cambios técnicos implementados

- `builder.Services.AddControllers()` registra el soporte para controllers.
- `builder.Services.AddEndpointsApiExplorer()` permite descubrir contratos de controllers.
- `builder.Services.AddSwaggerGen()` registra la generación de Swagger.
- `app.UseSwagger()` y `app.UseSwaggerUI()` se ejecutan únicamente en Development.
- `app.UseHttpsRedirection()` mantiene el tráfico HTTP redirigido a HTTPS cuando existe un perfil HTTPS.
- `app.MapControllers()` conecta los controllers al pipeline; actualmente aparece duplicado y debe quedar una sola llamada.
- `builder.Services.AddScoped<ICategoriasService, CategoriasService>()` registra el servicio actual de Categorías.
- Se eliminó `Microsoft.AspNetCore.OpenApi` y no se usa `AddOpenApi`; Swashbuckle es la estrategia única de documentación.
- La composición permanece en `Program.cs` mientras se termina de definir el registro por módulo.
- `CategoriaDbContext`, validadores y middleware todavía no están registrados en la composición.
- La separación entre composición y reglas de negocio queda preparada para los PRDs posteriores.

## Interfaces y contratos afectados

- El endpoint `/weatherforecast` deja de formar parte de la API.
- Swagger UI debe estar disponible en `/swagger` en desarrollo.
- La especificación JSON debe estar disponible en `/swagger/v1/swagger.json` en desarrollo.
- La ruta base de los módulos seguirá la convención `/api/<recurso>` definida en `docs/System_Artifact.md`.

## Impacto en datos e integraciones

No modifica datos ni crea integraciones externas. Solo establece el punto de composición que utilizarán los PRDs posteriores.

## Criterios de aceptación

- La aplicación compila con .NET 10 sin errores ni advertencias.
- La aplicación debe arrancar mediante `dotnet run --project src/InventarioVentas.API/InventarioVentas.API.csproj` una vez resuelto el registro de `CategoriaDbContext`.
- `/weatherforecast` ya no está expuesto.
- Swagger UI está disponible en `/swagger` durante Development.
- La especificación JSON está disponible en `/swagger/v1/swagger.json` durante Development.
- La configuración de servicios está registrada en un punto identificable y documentado.
- No se agregan secretos ni credenciales a `appsettings.json` o `appsettings.Development.json`.
- La solución conserva la arquitectura de monolito modular documentada.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Ejecutar `dotnet restore` | Restauración exitosa. |
| Ejecutar `dotnet build` | 0 errores y 0 advertencias. |
| Ejecutar la API | Actualmente falla al construir DI porque no encuentra `CategoriaDbContext`; debe corregirse antes de marcar el PRD como terminado. |
| Solicitar `/weatherforecast` | `404 Not Found`. |
| Abrir `/swagger` en desarrollo | Swagger UI disponible. |
| Solicitar `/swagger/v1/swagger.json` en desarrollo | `200 OK` con la especificación disponible. |
| Ejecutar en un ambiente distinto de Development | Swagger no se expone automáticamente. |

## Riesgos y decisiones pendientes

- La composición de `DbContext`, validadores y middleware deberá agregarse en los PRDs propietarios de cada responsabilidad.
- El registro de Categorías no puede quedar habilitado sin registrar su dependencia de persistencia.
- Si `Program.cs` crece con registros de módulos, la extracción a `Extensions` debe documentarse sin mover reglas de negocio.
- La autenticación y autorización permanecen fuera del alcance inicial.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `main` |
| Commit o PR | `6170d3e chore: :twisted_rightwards_arrows: merge develop changes` |
| Archivos de implementación | `src/InventarioVentas.API/Program.cs`, `src/InventarioVentas.API/InventarioVentas.API.csproj` |
| Archivos de documentación | README raíz, `docs/`, README de la API, perfiles locales e `InventarioVentas.API.http` |
| Pruebas ejecutadas | `dotnet build InventarioVentas.slnx --no-restore`: 0 errores y 0 advertencias; `dotnet run` no arranca por `CategoriaDbContext` no registrado |
| Evidencia adicional | La composición ya descubre el controller de Categorías, pero la validación del host falla antes de exponer Swagger |
| Responsable y fecha de implementación | Codex, 2026-08-25 |

## Referencias

- [`docs/Architecture.md`](../docs/Architecture.md)
- [`docs/System_Artifact.md`](../docs/System_Artifact.md)
- [`src/InventarioVentas.API/README.md`](../src/InventarioVentas.API/README.md)
- [`src/InventarioVentas.API/Extensions/README.md`](../src/InventarioVentas.API/Extensions/README.md)
