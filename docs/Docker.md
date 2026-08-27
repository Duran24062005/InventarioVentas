# Ejecución con Docker

InventarioVentas puede ejecutarse como un contenedor de la API. La imagen usa una compilación multi-stage: el SDK de .NET 10 solo participa en la etapa de build y la imagen final contiene el runtime de ASP.NET Core y la aplicación publicada.

## Alcance actual

Compose contiene la API y un servicio PostgreSQL para desarrollo. La API usa un único `AppDbContext` y el repositorio contiene migraciones para el esquema actual. Todavía está pendiente ejecutar y verificar el flujo completo de migraciones y endpoints dentro de los contenedores.

## Requisitos

- Docker Engine.
- Docker Compose v2 mediante `docker compose`.

Verificar la instalación:

```bash
docker --version
docker compose version
```

## Construir la imagen

Desde la raíz del repositorio:

```bash
docker build -t inventarioventas-api:dev .
```

El `Dockerfile` copia primero el `.csproj` para aprovechar la caché de restauración de paquetes y después publica la aplicación en modo `Release`.

## Ejecutar con Docker Compose

### Configurar la contraseña de PostgreSQL

`docker-compose.yml` exige la variable `POSTGRES_PASSWORD`. Si ejecutas `docker compose up -d` sin definirla, Compose detiene el proceso antes de crear los contenedores y muestra un error similar a:

```text
required variable POSTGRES_PASSWORD is missing a value
```

La opción recomendada para la sesión actual de la terminal es:

```bash
export POSTGRES_PASSWORD='<tu-password-local>'
docker compose up -d
```

También puedes definirla solo para un comando:

```bash
POSTGRES_PASSWORD='<tu-password-local>' docker compose up -d
```

El proyecto incluye un archivo `.env` local en la raíz. Edita su valor antes de iniciar los servicios:

```dotenv
POSTGRES_PASSWORD=tu-password-local
```

Docker Compose carga automáticamente `.env` desde esa ubicación, por lo que ya no necesitas ejecutar `export POSTGRES_PASSWORD=...`. El archivo está excluido por `.gitignore`; no lo subas al repositorio porque contiene una credencial local.

Nota: `.env` lo carga Docker Compose, no `dotnet run` ni `dotnet watch`. Si ejecutas la API localmente con Hot Reload, configura la conexión mediante User Secrets o `ConnectionStrings__DefaultConnection`.

Si necesitas reconstruir la imagen después de un cambio de código, usa:

```bash
docker compose up -d --build
```

Con la composición actual, la API queda disponible en:

- Swagger UI: <http://localhost:5011/swagger>
- Especificación JSON: <http://localhost:5011/swagger/v1/swagger.json>

Detener los servicios:

```bash
docker compose down
```

Para eliminar también los datos locales de PostgreSQL, usa `docker compose down --volumes`.

## Ejecutar la imagen directamente

```bash
docker run --rm \
  --publish 5011:5011 \
  --env ASPNETCORE_ENVIRONMENT=Development \
  --env ConnectionStrings__DefaultConnection="Host=<postgres-host>;Port=5432;Database=inventarioventas;Username=postgres;Password=<tu-password>" \
  inventarioventas-api:dev
```

La imagen escucha en el puerto interno `5011`, definido mediante `ASPNETCORE_HTTP_PORTS`. El puerto publicado puede cambiarse en el lado izquierdo de `--publish`.

## Configuración y seguridad

- `docker-compose.yml` está orientado a desarrollo, establece `ASPNETCORE_ENVIRONMENT=Development` y espera `POSTGRES_PASSWORD` desde el entorno.
- La API recibe `ConnectionStrings__DefaultConnection` apuntando al host Compose `postgres`.
- No se deben agregar connection strings completas, contraseñas, tokens ni certificados al `Dockerfile` o `.dockerignore`; Compose solo debe construir la conexión mediante variables externas.
- El contenedor actual expone HTTP; HTTPS y certificados deben resolverse en un reverse proxy o en una configuración de despliegue posterior.
- Para producción se debe usar `ASPNETCORE_ENVIRONMENT=Production`, una configuración de secretos externa y una política explícita de exposición de Swagger.
- `.dockerignore` excluye artefactos de compilación, documentación y metadatos del repositorio del contexto de build.

## Diagnóstico rápido

```bash
docker compose ps
docker compose logs -f api
```

Si el puerto `5011` está ocupado, cambia el lado izquierdo de `5011:5011`. Si el puerto `5432` está ocupado, define `POSTGRES_PORT` con otro puerto; el puerto interno de PostgreSQL permanece en `5432`.

## Referencias

- [`Dockerfile`](../Dockerfile)
- [`docker-compose.yml`](../docker-compose.yml)
- [PRD-012: Dockerización y ejecución con contenedores](../prd/PRD-012-dockerizacion-y-ejecucion-contenedores.md)
- [Comandos de configuración del proyecto](project_configuration_commands.md)
