# PRD-001: Base técnica y composición de la API

| Campo | Valor |
| --- | --- |
| Estado | Propuesto |
| Prioridad | Alta |
| Dependencias | Ninguna |
| Módulo propietario | Aplicación/API |
| Historias relacionadas | Base para HU01–HU04 |

## Problema y objetivo

El proyecto compila como una API .NET 10, pero todavía conserva el endpoint de ejemplo `weatherforecast` y no tiene una composición preparada para los módulos de negocio. Este PRD define la base mínima para que la aplicación arranque, exponga la API real y registre sus dependencias de forma explícita.

## Alcance

- Mantener `InventarioVentas.API` como una sola API y un solo despliegue.
- Configurar el pipeline HTTP y la composición de servicios en `Program.cs` o extensiones claramente propietarias.
- Retirar el endpoint y los tipos del ejemplo `weatherforecast`.
- Conservar OpenAPI/Swagger para explorar y validar los endpoints.
- Mantener la configuración por ambiente y evitar secretos en archivos versionados.
- Verificar que la solución compile y arranque sin endpoints de ejemplo.

Fuera de alcance: entidades, base de datos, migraciones, autenticación, autorización y endpoints funcionales de módulos.

## Actores y consumidores

- Desarrolladores que ejecutan la API localmente.
- Clientes HTTP que consumirán los módulos de inventario y ventas.
- Herramientas de documentación y prueba como Swagger/OpenAPI.

## Cambios técnicos

- Definir una composición única y explícita para controllers, OpenAPI, validadores, `DbContext`, middleware y services.
- Mantener la separación entre composición de la aplicación y reglas de negocio.
- Preparar el registro por módulo sin crear un contenedor global de lógica funcional.
- Dejar la aplicación lista para recibir configuración de SQL Server en el PRD de persistencia.

## Interfaces y contratos afectados

- El endpoint `/weatherforecast` deja de formar parte de la API.
- La API debe conservar el contrato de documentación OpenAPI en desarrollo.
- La ruta base de los módulos seguirá la convención `/api/<recurso>` definida en `docs/System_Artifact.md`.

## Impacto en datos e integraciones

No modifica datos ni crea integraciones externas. Solo establece el punto de composición que utilizarán los PRDs posteriores.

## Criterios de aceptación

- La aplicación compila con .NET 10 sin errores ni advertencias.
- La aplicación arranca mediante `dotnet run --project src/InventarioVentas.API/InventarioVentas.API.csproj`.
- `/weatherforecast` ya no está expuesto.
- OpenAPI/Swagger sigue disponible en el ambiente de desarrollo.
- La configuración de servicios está registrada en un punto identificable y documentado.
- No se agregan secretos ni credenciales a `appsettings.json` o `appsettings.Development.json`.
- La solución conserva la arquitectura de monolito modular documentada.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Ejecutar `dotnet restore` | Restauración exitosa. |
| Ejecutar `dotnet build` | 0 errores y 0 advertencias. |
| Ejecutar la API | Arranque exitoso. |
| Solicitar `/weatherforecast` | Recurso no encontrado. |
| Abrir la especificación OpenAPI en desarrollo | Especificación disponible sin el endpoint de ejemplo. |

## Riesgos y decisiones pendientes

- La solución contiene referencias tanto a OpenAPI de ASP.NET como a Swashbuckle; debe mantenerse una estrategia coherente y documentada para no duplicar configuración.
- La ubicación exacta de los métodos de extensión de registro se decidirá según el crecimiento real de `Program.cs`, respetando `Extensions/README.md`.
- La autenticación y autorización permanecen fuera del alcance inicial.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | Pendiente |
| Commit o PR | Pendiente |
| Archivos modificados | Pendiente |
| Pruebas ejecutadas | Baseline: `dotnet build --no-restore` exitoso el 2026-08-24 |
| Evidencia adicional | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/Architecture.md`](../docs/Architecture.md)
- [`docs/System_Artifact.md`](../docs/System_Artifact.md)
- [`src/InventarioVentas.API/README.md`](../src/InventarioVentas.API/README.md)
- [`src/InventarioVentas.API/Extensions/README.md`](../src/InventarioVentas.API/Extensions/README.md)
