# PRD-012: Dockerización y ejecución con contenedores

| Campo | Valor |
| --- | --- |
| Estado | Terminado |
| Prioridad | Media |
| Dependencias | PRD-001 |
| Módulo propietario | Infraestructura y ejecución local |
| Historias relacionadas | Soporte de desarrollo y despliegue |

## Problema y objetivo

El proyecto puede ejecutarse con el SDK local de .NET, pero no tiene una forma reproducible de empaquetar y levantar la API. Este PRD agrega una imagen Docker multi-stage y una configuración Docker Compose para desarrollo, manteniendo fuera de alcance la base de datos hasta que exista la persistencia funcional.

## Alcance

- Crear un `Dockerfile` multi-stage para .NET 10.
- Crear `.dockerignore` para reducir el contexto y excluir artefactos o secretos.
- Crear `docker-compose.yml` para ejecutar la API en el puerto `8080`.
- Documentar build, ejecución, logs, detención y configuración por ambiente.
- Mantener Swagger disponible en el Compose de desarrollo.

Fuera de alcance: SQL Server en Compose, migraciones, HTTPS dentro del contenedor, despliegue productivo, reverse proxy, registro de imágenes y CI/CD.

## Decisiones técnicas

- Imagen de build: `mcr.microsoft.com/dotnet/sdk:10.0`.
- Imagen final: `mcr.microsoft.com/dotnet/aspnet:10.0`.
- Publicación `Release` con `UseAppHost=false`.
- Puerto interno: `8080`, mediante `ASPNETCORE_HTTP_PORTS`.
- Compose de desarrollo: `ASPNETCORE_ENVIRONMENT=Development`.
- No se agregan secretos ni una base de datos que todavía no está implementada.

## Artefactos y contratos

- `Dockerfile`: construye y publica `InventarioVentas.API`.
- `.dockerignore`: excluye `.git`, `bin`, `obj`, documentación y archivos locales del contexto.
- `docker-compose.yml`: define el servicio `api`, su build, ambiente y publicación `8080:8080`.
- `docs/Docker.md`: documenta el flujo de uso y sus límites.

## Criterios de aceptación

- `docker build -t inventarioventas-api:dev .` termina correctamente.
- `docker compose config` genera una configuración válida.
- `docker compose up --build` inicia la API.
- Swagger responde en `http://localhost:8080/swagger` dentro del Compose de desarrollo.
- `/swagger/v1/swagger.json` responde correctamente desde el contenedor.
- La imagen final no requiere el SDK para ejecutarse.
- El contexto no incluye `.git`, `bin`, `obj`, `docs`, `prd` ni secretos locales.
- La documentación explica que SQL Server queda pendiente de PRD-003/004.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Construir imagen desde la raíz | Build exitoso. |
| Validar Compose | Configuración válida. |
| Levantar el servicio | Contenedor `api` en ejecución. |
| Consultar Swagger | `200 OK`. |
| Detener Compose | Contenedor detenido y recursos del proyecto removidos. |
| Revisar configuración | Sin secretos ni conexión de base de datos ficticia. |

## Riesgos y decisiones pendientes

- Las imágenes `10.0` siguen la línea del framework del proyecto; una estrategia de actualización o fijación por digest deberá definirse para despliegues controlados.
- HTTPS, health checks y observabilidad de contenedor se deben definir cuando exista un entorno de despliegue real.
- Cuando se implemente persistencia, el Compose deberá incorporar la estrategia de SQL Server y sus secretos sin copiar credenciales al repositorio.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `feature/tecnic-base-and-api-composition` |
| Commit o PR | `b60d21d feat: :whale: add Docker image and Compose service` |
| Archivos modificados | `Dockerfile`, `.dockerignore`, `docker-compose.yml`, `docs/Docker.md` y documentación relacionada |
| Pruebas ejecutadas | `docker compose config` válido; `docker build -t inventarioventas-api:dev .` exitoso; `docker compose up --build --detach` exitoso |
| Evidencia adicional | Contenedor `inventarioventas-api-1` levantado en `0.0.0.0:8080->8080`; `/swagger/v1/swagger.json` respondió `200` con OpenAPI `3.0.4`; Compose detenido y limpiado con `docker compose down` |
| Responsable y fecha de implementación | Codex, 2026-08-24 |

## Referencias

- [`docs/Docker.md`](../docs/Docker.md)
- [`docs/Architecture.md`](../docs/Architecture.md)
- [`prd/PRD-001-base-tecnica-y-composicion-api.md`](PRD-001-base-tecnica-y-composicion-api.md)
- [`src/InventarioVentas.API/README.md`](../src/InventarioVentas.API/README.md)
