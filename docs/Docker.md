# Ejecución con Docker

InventarioVentas puede ejecutarse como un contenedor de la API. La imagen usa una compilación multi-stage: el SDK de .NET 10 solo participa en la etapa de build y la imagen final contiene el runtime de ASP.NET Core y la aplicación publicada.

## Alcance actual

La configuración actual contiene únicamente la API. Existe un `CategoriaDbContext` provisional, pero todavía no hay `AppDbContext`, cadena de conexión, migraciones ni un contenedor de SQL Server. La base de datos se incorporará cuando se implementen PRD-003 y PRD-004.

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

```bash
docker compose up --build
```

Cuando la composición de dependencias esté completa, la API quedará disponible en:

- Swagger UI: <http://localhost:8080/swagger>
- Especificación JSON: <http://localhost:8080/swagger/v1/swagger.json>

Detener y eliminar el contenedor:

```bash
docker compose down
```

## Ejecutar la imagen directamente

```bash
docker run --rm \
  --publish 8080:8080 \
  --env ASPNETCORE_ENVIRONMENT=Development \
  inventarioventas-api:dev
```

La imagen escucha en el puerto interno `8080`, definido mediante `ASPNETCORE_HTTP_PORTS`. El puerto publicado puede cambiarse en el lado izquierdo de `--publish`.

## Configuración y seguridad

- `docker-compose.yml` está orientado a desarrollo y establece `ASPNETCORE_ENVIRONMENT=Development` para habilitar Swagger.
- No se deben agregar connection strings, contraseñas, tokens ni certificados al `Dockerfile`, `.dockerignore` o `docker-compose.yml`.
- El contenedor actual expone HTTP; HTTPS y certificados deben resolverse en un reverse proxy o en una configuración de despliegue posterior.
- Para producción se debe usar `ASPNETCORE_ENVIRONMENT=Production`, una configuración de secretos externa y una política explícita de exposición de Swagger.
- `.dockerignore` excluye artefactos de compilación, documentación y metadatos del repositorio del contexto de build.

## Diagnóstico rápido

```bash
docker compose ps
docker compose logs -f api
```

Si el puerto `8080` está ocupado, cambia el lado izquierdo de `8080:8080` en `docker-compose.yml`; el puerto interno debe permanecer en `8080` salvo que también se actualice `ASPNETCORE_HTTP_PORTS`.

## Referencias

- [`Dockerfile`](../Dockerfile)
- [`docker-compose.yml`](../docker-compose.yml)
- [PRD-012: Dockerización y ejecución con contenedores](../prd/PRD-012-dockerizacion-y-ejecucion-contenedores.md)
- [Comandos de configuración del proyecto](project_configuration_commands.md)
