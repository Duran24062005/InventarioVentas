# PRD-003: Persistencia con Entity Framework Core

| Campo | Valor |
| --- | --- |
| Estado | En progreso |
| Prioridad | Alta |
| Dependencias | PRD-002 |
| Módulo propietario | `Data` |
| Historias relacionadas | HU01–HU04 |

## Problema y objetivo

El modelo de dominio definido en PRD-002 todavía no tiene una frontera de persistencia completa. Existe un `CategoriaDbContext` inicial usado por el service de Categorías, registrado con PostgreSQL, pero no corresponde todavía al `AppDbContext` previsto y no tiene migraciones.

## Alcance

- Crear `AppDbContext` en `Data`.
- Exponer `DbSet` para las cinco entidades.
- Crear configuraciones separadas por entidad en `Data/Configurations`.
- Configurar claves, relaciones, restricciones, índices y precisión decimal.
- Registrar el contexto mediante inyección de dependencias.

Fuera de alcance: creación de migraciones y base de datos local, que pertenecen a PRD-004; services, controllers, DTOs, autenticación y reportes.

## Configuración esperada

El contexto debe incluir:

```csharp
DbSet<Categoria> Categorias
DbSet<Producto> Productos
DbSet<Cliente> Clientes
DbSet<Venta> Ventas
DbSet<DetalleVenta> DetalleVentas
```

Las configuraciones deben cubrir:

- Llaves primarias y generación de identificadores.
- Relación obligatoria de `Producto` con `Categoria`.
- Relación obligatoria de `Venta` con `Cliente`.
- Relación obligatoria de `DetalleVenta` con `Venta` y `Producto`.
- Índice único para `Producto.Codigo`.
- Índice único para `Cliente.Documento`.
- Precisión consistente, por ejemplo `decimal(18,2)`, para precio, subtotal y total.
- Longitudes y nulabilidad coherentes con el modelo.

## Actores y consumidores

- Services de los módulos, que consultarán y persistirán entidades.
- Comandos de EF Core y migraciones.
- PostgreSQL como motor de datos objetivo mediante `Npgsql.EntityFrameworkCore.PostgreSQL`.
- Pruebas de persistencia y de integración de la API.

## Interfaces y tipos afectados

- `AppDbContext` como punto de acceso de persistencia.
- Clases `IEntityTypeConfiguration<T>` o configuración equivalente.
- Registro de `DbContext` con la cadena `ConnectionStrings:DefaultConnection`.

El contexto no debe filtrarse hacia DTOs ni controllers, y las configuraciones no deben contener reglas de negocio de ventas o inventario.

## Estado actual de implementación

- Existe `Data/Configurations/CategoriaDb.cs` con un `CategoriaDbContext` y un único `DbSet<Categoria>`.
- `CategoriasService` depende de ese contexto para ejecutar un CRUD inicial.
- No existen todavía `AppDbContext`, las entidades completas del dominio ni las cinco configuraciones esperadas.
- `Program.cs` registra `ICategoriasService` y `CategoriaDbContext` mediante `UseNpgsql`.
- `appsettings.json` declara `ConnectionStrings:DefaultConnection` sin valor; el valor real debe llegar desde User Secrets, variables de entorno o Compose.

## Impacto en datos

Este PRD define el esquema lógico que utilizará la migración inicial. Las restricciones de base de datos deben reforzar, no reemplazar, las validaciones de entrada y de service.

## Criterios de aceptación

- `AppDbContext` contiene los cinco `DbSet` esperados.
- Las cinco entidades tienen configuración de tabla y clave primaria.
- Las relaciones y claves foráneas son obligatorias donde lo exige el dominio.
- Los índices de código de producto y documento de cliente son únicos.
- Los campos monetarios usan precisión explícita y no quedan con la precisión por defecto.
- El contexto se registra por inyección de dependencias sin crear conexiones manuales en los services.
- `dotnet build` continúa pasando sin errores ni advertencias.
- El código de persistencia permanece dentro de `Data` y `Data/Configurations`.
- El `CategoriaDbContext` provisional se reemplaza o integra en el `AppDbContext` definido por este PRD.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Resolver `AppDbContext` desde el contenedor | Se obtiene una instancia configurada. |
| Inspeccionar el modelo EF | Existen las cinco entidades y sus relaciones. |
| Revisar el índice de `Producto.Codigo` | Está marcado como único. |
| Revisar el índice de `Cliente.Documento` | Está marcado como único. |
| Revisar propiedades monetarias | Tienen precisión explícita. |
| Ejecutar compilación | 0 errores y 0 advertencias. |

## Riesgos y decisiones pendientes

- La cadena de conexión no debe contener secretos en el repositorio; usar variables de entorno o User Secrets en desarrollo.
- Debe definirse el comportamiento de borrado de relaciones para no eliminar ventas históricas accidentalmente.
- La estrategia de nombres de tablas y columnas debe mantenerse consistente antes de generar la migración.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `main` |
| Commit o PR | Pendiente |
| Archivos modificados | `src/InventarioVentas.API/Data/Configurations/CategoriaDb.cs` y `src/InventarioVentas.API/Modules/Categorias/Services/CategoriasService.cs` contienen la implementación parcial actual |
| Pruebas ejecutadas | `dotnet restore`, `dotnet build InventarioVentas.slnx --no-restore`: 0 errores y 0 advertencias; arranque sin conexión falla con el mensaje esperado |
| Evidencia adicional | La implementación parcial no cumple aún el contrato de cinco entidades, relaciones, índices y precisión decimal |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), sección 10.
- [`docs/Architecture.md`](../docs/Architecture.md).
- [`src/InventarioVentas.API/Data/README.md`](../src/InventarioVentas.API/Data/README.md).
- [`src/InventarioVentas.API/Data/Configurations/README.md`](../src/InventarioVentas.API/Data/Configurations/README.md).
