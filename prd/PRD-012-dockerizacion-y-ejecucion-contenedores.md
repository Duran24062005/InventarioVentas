# PRD-012: Dockerización y ejecución con contenedores

| Campo | Valor |
| --- | --- |
| Estado | En progreso |
| Prioridad | Media |
| Dependencias | PRD-001 |
| Módulo propietario | Infraestructura y ejecución local |
| Historias relacionadas | Soporte de desarrollo y despliegue |

## Problema y objetivo

El proyecto tiene una imagen Docker multi-stage y una configuración Docker Compose para desarrollo. La infraestructura de empaquetado está creada, pero la API actual no llega a iniciar porque la dependencia de `CategoriaDbContext` no está registrada; por eso la validación funcional del contenedor sigue pendiente.

## Alcance

- Crear un `Dockerfile` multi-stage para .NET 10.
- Crear `.dockerignore` para reducir el contexto y excluir artefactos o secretos.
- Crear `docker-compose.yml` para ejecutar la API en el puerto `8080`.
- Documentar build, ejecución, logs, detención y configuración por ambiente.
- Mantener Swagger disponible en el Compose de desarrollo cuando la API pueda arrancar.

Fuera de alcance: SQL Server en Compose, migraciones, HTTPS dentro del contenedor, despliegue productivo, reverse proxy, registro de imágenes y CI/CD.

## Decisiones técnicas

- Imagen de build: `mcr.microsoft.com/dotnet/sdk:10.0`.
- Imagen final: `mcr.microsoft.com/dotnet/aspnet:10.0`.
- Publicación `Release` con `UseAppHost=false`.
- Puerto interno: `8080`, mediante `ASPNETCORE_HTTP_PORTS`.
- Compose de desarrollo: `ASPNETCORE_ENVIRONMENT=Development`.
- No se agregan secretos ni una base de datos que todavía no está implementada.
- El contenedor actual no resuelve ni corrige la configuración de DI de la aplicación; ese bloqueo pertenece a PRD-003/004/006.

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
| Levantar el servicio | Pendiente: actualmente la aplicación falla al construir DI por `CategoriaDbContext` no registrado. |
| Consultar Swagger | Pendiente hasta resolver el arranque de la API. |
| Detener Compose | Contenedor detenido y recursos del proyecto removidos. |
| Revisar configuración | Sin secretos ni conexión de base de datos ficticia. |

## Riesgos y decisiones pendientes

- Las imágenes `10.0` siguen la línea del framework del proyecto; una estrategia de actualización o fijación por digest deberá definirse para despliegues controlados.
- HTTPS, health checks y observabilidad de contenedor se deben definir cuando exista un entorno de despliegue real.
- Cuando se implemente persistencia, el Compose deberá incorporar la estrategia de SQL Server y sus secretos sin copiar credenciales al repositorio.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `main` |
| Commit o PR | `6170d3e chore: :twisted_rightwards_arrows: merge develop changes` |
| Archivos modificados | `Dockerfile`, `.dockerignore`, `docker-compose.yml`, `docs/Docker.md` y documentación relacionada |
| Pruebas ejecutadas | `docker compose config` y build documentados; la ejecución actual debe repetirse después de resolver el arranque de la API |
| Evidencia adicional | El contenedor usa `Development` y publica `8080`, pero la aplicación falla durante la validación de DI por `CategoriaDbContext` no registrado |
| Responsable y fecha de implementación | Codex, 2026-08-25 |

## Referencias

- [`docs/Docker.md`](../docs/Docker.md)
- [`docs/Architecture.md`](../docs/Architecture.md)
- [`prd/PRD-001-base-tecnica-y-composicion-api.md`](PRD-001-base-tecnica-y-composicion-api.md)
- [`src/InventarioVentas.API/README.md`](../src/InventarioVentas.API/README.md)
