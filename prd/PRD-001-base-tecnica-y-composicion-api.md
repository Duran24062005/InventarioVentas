# PRD-001: Base técnica y composición de la API

| Campo | Valor |
| --- | --- |
| Estado | Terminado |
| Prioridad | Alta |
| Dependencias | Ninguna |
| Módulo propietario | Aplicación/API |
| Historias relacionadas | Base para HU01–HU04 |

## Problema y objetivo

La API necesitaba reemplazar el scaffold de .NET por una composición preparada para los módulos de negocio. Este PRD deja configurada la base mínima para que la aplicación arranque, exponga documentación Swagger en desarrollo y registre controllers de forma explícita.

## Alcance

- Mantener `InventarioVentas.API` como una sola API y un solo despliegue.
- Configurar el pipeline HTTP y la composición de servicios en `Program.cs`.
- Retirar el endpoint y los tipos del ejemplo `weatherforecast`.
- Conservar Swagger para explorar y validar los endpoints.
- Mantener la configuración por ambiente y evitar secretos en archivos versionados.
- Verificar que la solución compile y arranque sin endpoints de ejemplo.

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
- `app.MapControllers()` conecta los controllers futuros al pipeline.
- Se eliminó `Microsoft.AspNetCore.OpenApi` y no se usa `AddOpenApi`; Swashbuckle es la estrategia única de documentación.
- La composición permanece en `Program.cs` porque todavía no hay registros de módulos que justifiquen métodos de extensión adicionales.
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
- La aplicación arranca mediante `dotnet run --project src/InventarioVentas.API/InventarioVentas.API.csproj`.
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
| Ejecutar la API | Arranque exitoso. |
| Solicitar `/weatherforecast` | `404 Not Found`. |
| Abrir `/swagger` en desarrollo | Swagger UI disponible. |
| Solicitar `/swagger/v1/swagger.json` en desarrollo | `200 OK` con la especificación disponible. |
| Ejecutar en un ambiente distinto de Development | Swagger no se expone automáticamente. |

## Riesgos y decisiones pendientes

- La composición futura de validadores, `DbContext`, middleware y services deberá agregarse en los PRDs que sean propietarios de cada responsabilidad.
- Si `Program.cs` crece con registros de módulos, la extracción a `Extensions` debe documentarse sin mover reglas de negocio.
- La autenticación y autorización permanecen fuera del alcance inicial.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `feature/tecnic-base-and-api-composition` |
| Commit o PR | `8e89456 refactor: :recycle: compose API controllers and Swagger` |
| Archivos de implementación | `src/InventarioVentas.API/Program.cs`, `src/InventarioVentas.API/InventarioVentas.API.csproj` |
| Archivos de documentación | README raíz, `docs/`, README de la API, perfiles locales e `InventarioVentas.API.http` |
| Pruebas ejecutadas | `dotnet build --no-restore`: 0 errores y 0 advertencias; Swagger Development `200`; `weatherforecast` `404`; Swagger Production `404` |
| Evidencia adicional | `/swagger/v1/swagger.json` respondió OpenAPI `3.0.4` con `paths` vacío, consistente con que aún no existen controllers funcionales |
| Responsable y fecha de implementación | Codex, 2026-08-24 |

## Referencias

- [`docs/Architecture.md`](../docs/Architecture.md)
- [`docs/System_Artifact.md`](../docs/System_Artifact.md)
- [`src/InventarioVentas.API/README.md`](../src/InventarioVentas.API/README.md)
- [`src/InventarioVentas.API/Extensions/README.md`](../src/InventarioVentas.API/Extensions/README.md)
