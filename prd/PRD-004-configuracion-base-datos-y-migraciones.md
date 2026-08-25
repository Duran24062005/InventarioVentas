# PRD-004: Configuración de base de datos y migraciones

| Campo | Valor |
| --- | --- |
| Estado | Propuesto |
| Prioridad | Alta |
| Dependencias | PRD-003 |
| Módulo propietario | `Data` y configuración de ambientes |
| Historias relacionadas | HU01–HU04 |

## Problema y objetivo

El proyecto tiene el proveedor de PostgreSQL y las herramientas de EF Core, pero no tiene una migración inicial ni un procedimiento reproducible para crear el esquema completo. Este PRD define cómo preparar y versionar la infraestructura local de persistencia sin guardar secretos.

## Alcance

- Definir la clave `ConnectionStrings:DefaultConnection` como contrato de configuración.
- Configurar valores locales mediante User Secrets, variables de entorno o un mecanismo equivalente.
- Crear la migración inicial después de aprobar el modelo y las configuraciones de PRD-003.
- Documentar comandos de creación, actualización y revisión de migraciones.
- Verificar que una base de datos vacía pueda construirse desde las migraciones.

Fuera de alcance: despliegue productivo, backups, alta disponibilidad, CI/CD, datos de prueba permanentes y administración de credenciales.

## Procedimiento técnico

La secuencia de trabajo será:

1. Confirmar que PRD-002 y PRD-003 están aceptados.
2. Configurar una conexión local a PostgreSQL sin escribir credenciales en archivos versionados.
3. Ejecutar `dotnet ef migrations add InitialCreate` desde el proyecto API.
4. Revisar la migración generada contra el modelo esperado.
5. Ejecutar `dotnet ef database update` sobre una base de datos de desarrollo.
6. Registrar el nombre de la migración, el ambiente y la evidencia en este PRD.

Los comandos definitivos deben quedar alineados con `docs/project_configuration_commands.md`.

## Contratos de configuración

- Clave: `ConnectionStrings:DefaultConnection`.
- Motor objetivo: PostgreSQL mediante `Npgsql.EntityFrameworkCore.PostgreSQL`.
- La API debe fallar de forma clara si necesita persistencia y la conexión no está configurada.
- `appsettings.json` puede conservar la estructura de configuración, pero no debe contener secretos reales.

## Actores y consumidores

- Desarrolladores que levantan la base de datos local.
- `AppDbContext` al resolver la conexión.
- EF Core CLI durante creación y aplicación de migraciones.
- Entornos de desarrollo y pruebas que suministren su propia cadena.

## Impacto en datos e integraciones

Se crea el esquema inicial de PostgreSQL con tablas, relaciones, índices y precisión definidos en PRD-003. La migración es un artefacto de infraestructura y no debe contener lógica de negocio.

## Criterios de aceptación

- Existe una configuración documentada para `DefaultConnection`.
- No hay contraseñas, tokens ni cadenas reales en archivos versionados.
- La migración inicial se genera sin errores a partir del modelo actual.
- Una base de datos vacía puede actualizarse con `dotnet ef database update`.
- Las tablas y restricciones generadas corresponden a PRD-002 y PRD-003.
- El procedimiento está documentado para que otra persona pueda repetirlo.
- La API puede arrancar sin crear migraciones implícitamente durante cada inicio.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Ejecutar `dotnet ef migrations list` | La migración inicial aparece disponible. |
| Aplicar migraciones a una base vacía | Proceso exitoso y esquema creado. |
| Ejecutar el comando sin conexión configurada | Error claro y accionable. |
| Revisar archivos versionados | No contienen secretos. |
| Consultar índices y claves en PostgreSQL | Coinciden con PRD-003. |
| Ejecutar la API con una conexión válida | Arranque exitoso. |

## Riesgos y decisiones pendientes

- La disponibilidad de PostgreSQL local o de una instancia de desarrollo debe confirmarse antes de ejecutar la migración.
- No se deben usar credenciales compartidas en la documentación.
- La estrategia de datos semilla queda fuera del alcance inicial y solo se agregará si existe una necesidad documentada.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | Pendiente |
| Commit o PR | Pendiente |
| Archivos modificados | Pendiente |
| Pruebas ejecutadas | Pendiente |
| Migración generada | Pendiente |
| Base de datos/ambiente verificado | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), secciones 10, 13, 14 y 17.
- [`docs/project_configuration_commands.md`](../docs/project_configuration_commands.md).
- [`src/InventarioVentas.API/Data/README.md`](../src/InventarioVentas.API/Data/README.md).
- [`src/InventarioVentas.API/appsettings.json`](../src/InventarioVentas.API/appsettings.json).
