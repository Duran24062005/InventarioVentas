# PRD-013: Autenticación JWT

| Campo | Valor |
| --- | --- |
| Estado | En implementación |
| Prioridad | Alta |
| Dependencias | PRD-001, PRD-003 y PRD-005 |
| Módulo propietario | `Modules/auth` y composición de la API |

## Problema y objetivo

La API dispone de usuarios basados en ASP.NET Core Identity, pero no autentica credenciales ni protege los endpoints. Se implementa un login que emite access tokens JWT y un middleware que los valida.

## Alcance

- Registro de usuarios con email, nombre y contraseña.
- Login con `UserManager` y generación de JWT firmado.
- Protección global de endpoints; solo registro y login son anónimos.
- Endpoint `/api/auth/me` para comprobar los claims validados.
- Configuración Bearer en Swagger y variables de entorno para la clave.
- Migración de tablas de Identity en el mismo `AppDbContext`.

Fuera de alcance: refresh tokens, roles, confirmación de email, recuperación de contraseña y revocación de access tokens.

## Contrato HTTP

- `POST /api/auth/register`: recibe `nombre`, `email` y `password`; devuelve `201` con datos básicos del usuario.
- `POST /api/auth/login`: recibe `email` y `password`; devuelve `200` con `token`, `expiresAt` y datos básicos del usuario.
- `GET /api/auth/me`: requiere `Authorization: Bearer <token>` y devuelve los claims de identidad.

Credenciales inválidas devuelven `401` sin revelar si falló el email o la contraseña.

## Configuración

Se usan `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience` y `Jwt:ExpiresMinutes`. La clave exige al menos 32 bytes y no se versiona. La duración inicial del access token es de 60 minutos.

## Criterios de aceptación

- La API compila y arranca con una configuración JWT válida.
- Registro y login funcionan contra las tablas de Identity.
- Un token válido permite acceder a endpoints protegidos.
- Un token ausente, inválido o expirado devuelve `401`.
- `/api/auth/me` devuelve el usuario asociado al token.
- La migración `AddIdentityUsers` está disponible para PostgreSQL.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `develop` |
| Commit o PR | Pendiente |
| Pruebas ejecutadas | Compilación; pruebas HTTP pendientes |
| Responsable y fecha | Pendiente |
