# PRD-012: Dockerización y ejecución con contenedores

| Campo | Valor |
| --- | --- |
| Estado | En progreso |
| Prioridad | Media |
| Dependencias | PRD-001 |
| Módulo propietario | Infraestructura y ejecución local |
| Historias relacionadas | Soporte de desarrollo y despliegue |

## Problema y objetivo

El proyecto tiene una imagen Docker multi-stage y una configuración Docker Compose para desarrollo. Compose ahora incluye PostgreSQL y entrega la conexión a la API; la validación funcional del contenedor sigue pendiente de ejecutar el flujo completo.

## Alcance

- Crear un `Dockerfile` multi-stage para .NET 10.
- Crear `.dockerignore` para reducir el contexto y excluir artefactos o secretos.
- Crear `docker-compose.yml` para ejecutar la API en el puerto `5011`.
- Documentar build, ejecución, logs, detención y configuración por ambiente.
- Mantener Swagger disponible en el Compose de desarrollo cuando la API pueda arrancar.

Fuera de alcance: migraciones, HTTPS dentro del contenedor, despliegue productivo, reverse proxy y registro de imágenes. El CI básico de compilación y pruebas se documenta en [`docs/CI.md`](../docs/CI.md).

## Decisiones técnicas

- Imagen de build: `mcr.microsoft.com/dotnet/sdk:10.0`.
- Imagen final: `mcr.microsoft.com/dotnet/aspnet:10.0`.
- Publicación `Release` con `UseAppHost=false`.
- Puerto interno: `5011`, mediante `ASPNETCORE_HTTP_PORTS`.
- Compose de desarrollo: `ASPNETCORE_ENVIRONMENT=Development`.
- No se agregan secretos al repositorio; la contraseña de PostgreSQL llega por `POSTGRES_PASSWORD`.
- El contenedor usa el `AppDbContext` único y recibe la cadena de conexión mediante `ConnectionStrings__DefaultConnection`.

## Artefactos y contratos

- `Dockerfile`: construye y publica `InventarioVentas.API`.
- `.dockerignore`: excluye `.git`, `bin`, `obj`, documentación y archivos locales del contexto.
- `docker-compose.yml`: define `api`, PostgreSQL, su health check, la conexión interna y la publicación de puertos.
- `docs/Docker.md`: documenta el flujo de uso y sus límites.

## Criterios de aceptación

- `docker build -t inventarioventas-api:dev .` termina correctamente.
- `docker compose config` genera una configuración válida.
- `docker compose up --build` inicia la API.
- Swagger responde en `http://localhost:5011/swagger` dentro del Compose de desarrollo.
- `/swagger/v1/swagger.json` responde correctamente desde el contenedor.
- La imagen final no requiere el SDK para ejecutarse.
- El contexto no incluye `.git`, `bin`, `obj`, `docs`, `prd` ni secretos locales.
- Compose incluye PostgreSQL para desarrollo y recibe la contraseña desde `POSTGRES_PASSWORD`.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Construir imagen desde la raíz | Build exitoso. |
| Validar Compose | Configuración válida. |
| Levantar el servicio | Pendiente: debe ejecutarse con `POSTGRES_PASSWORD` y validar la API contra PostgreSQL. |
| Consultar Swagger | Pendiente hasta resolver el arranque de la API. |
| Detener Compose | Contenedor detenido y recursos del proyecto removidos. |
| Revisar configuración | Sin secretos ni conexión de base de datos ficticia. |

## Riesgos y decisiones pendientes

- Las imágenes `10.0` siguen la línea del framework del proyecto; una estrategia de actualización o fijación por digest deberá definirse para despliegues controlados.
- HTTPS, health checks y observabilidad de contenedor se deben definir cuando exista un entorno de despliegue real.
- La aplicación de migraciones y la verificación funcional dentro de Compose siguen pendientes; las credenciales de PostgreSQL no deben copiarse al repositorio.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `main` |
| Commit o PR | `6170d3e chore: :twisted_rightwards_arrows: merge develop changes` |
| Archivos modificados | `Dockerfile`, `.dockerignore`, `docker-compose.yml`, `docs/Docker.md` y documentación relacionada |
| Pruebas ejecutadas | `docker compose config` y build documentados; la ejecución actual debe repetirse después de resolver el arranque de la API |
| Evidencia adicional | `docker compose config` valida API, PostgreSQL, health check y conexión interna; falta ejecutar `up --build` |
| Responsable y fecha de implementación | Codex, 2026-08-25 |

## Referencias

- [`docs/Docker.md`](../docs/Docker.md)
- [`docs/Architecture.md`](../docs/Architecture.md)
- [`prd/PRD-001-base-tecnica-y-composicion-api.md`](PRD-001-base-tecnica-y-composicion-api.md)
- [`src/InventarioVentas.API/README.md`](../src/InventarioVentas.API/README.md)
