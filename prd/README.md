# PRDs de InventarioVentas

Esta carpeta contiene los Product Requirements Documents que guían la evolución de InventarioVentas. Cada PRD representa un slice implementable y conserva la relación entre necesidad, diseño, código, pruebas y evidencia.

## Estado del baseline

| Dato | Estado registrado |
| --- | --- |
| Fecha de referencia | 2026-08-27 |
| Rama | `develop` |
| SDK .NET | `10.0.111` |
| Compilación | Exitosa: 0 errores, 0 advertencias |
| Arranque actual | Requiere `ConnectionStrings:DefaultConnection`; el único contexto registrado es `AppDbContext` |
| Implementación funcional | CRUD de Categorías, Productos y Clientes; creación y consulta de Ventas |
| Persistencia | `AppDbContext` único con migraciones de inventario, clientes y ventas |
| Pruebas funcionales | No implementadas |

## Mapa de PRDs

| ID | Documento | Estado | Dependencias |
| --- | --- | --- | --- |
| PRD-001 | [Base técnica y composición de la API](PRD-001-base-tecnica-y-composicion-api.md) | En progreso | Ninguna |
| PRD-002 | [Modelo de dominio](PRD-002-modelo-de-dominio.md) | En progreso | PRD-001 |
| PRD-003 | [Persistencia con Entity Framework](PRD-003-persistencia-entity-framework.md) | En progreso | PRD-002 |
| PRD-004 | [Configuración de base de datos y migraciones](PRD-004-configuracion-base-datos-y-migraciones.md) | Propuesto | PRD-003 |
| PRD-005 | [Errores, validaciones y respuestas](PRD-005-errores-validaciones-y-respuestas.md) | En progreso | PRD-001, PRD-003 |
| PRD-006 | [Módulo de categorías](PRD-006-modulo-categorias.md) | En progreso | PRD-002, PRD-003, PRD-005 |
| PRD-007 | [Módulo de productos e inventario](PRD-007-modulo-productos-e-inventario.md) | En progreso | PRD-006 |
| PRD-008 | [Módulo de clientes](PRD-008-modulo-clientes.md) | Implementación parcial; verificación pendiente | PRD-002, PRD-005 |
| PRD-009 | [Módulo de ventas y transacción de stock](PRD-009-modulo-ventas-y-transaccion-stock.md) | Implementación parcial; verificación pendiente | PRD-007, PRD-008 |
| PRD-010 | [Pruebas y verificación de la API](PRD-010-pruebas-y-verificacion-api.md) | Propuesto | PRD-005 a PRD-009 |
| PRD-011 | [Cierre, documentación y definición de terminado](PRD-011-cierre-documentacion-y-definicion-de-terminado.md) | Propuesto | PRD-001 a PRD-010 |
| PRD-012 | [Dockerización y ejecución con contenedores](PRD-012-dockerizacion-y-ejecucion-contenedores.md) | En progreso | PRD-001 |

## Orden de implementación

La secuencia recomendada es `PRD-001` → `PRD-002` → `PRD-003` → `PRD-004` y `PRD-005`; después deben cerrarse Categorías y Productos antes de avanzar con Ventas. PRD-010 y PRD-011 cierran la verificación y la documentación. PRD-012 puede validarse de nuevo cuando la API arranque con sus dependencias registradas.

El estado ejecutable y el paso a paso consolidado se mantienen en [`../todo-task.md`](../todo-task.md). Los estados `En progreso` reflejan código parcial o una integración incompleta; no significan que el PRD cumpla todavía todos sus criterios de aceptación.

Cada PRD debe actualizar su estado y su sección de trazabilidad cuando se implemente. No se debe marcar como terminado solo porque el código compile: también deben cumplirse sus criterios de aceptación y registrarse las pruebas ejecutadas.

## Relación con historias de usuario

| Historia | PRD relacionado |
| --- | --- |
| HU01 - Crear categoría | PRD-006 |
| HU02 - Crear producto | PRD-007 |
| HU03 - Consultar productos | PRD-007 |
| HU04 - Registrar venta | PRD-009 |

## Decisiones abiertas

Estas preguntas ya están identificadas en `docs/System_Artifact.md` y deben resolverse antes de implementar el comportamiento que las necesita:

- Si `Estado` será `bool` o un enum.
- Si se permitirá editar precio y stock después de registrar ventas.
- Si se incorporará cancelación de ventas en una fase futura.
- Si los listados mostrarán solo registros activos o también inactivos.
- Si se agregará autenticación y autorización posteriormente.

Las decisiones tomadas deben quedar en el PRD afectado y, si cambian reglas funcionales o arquitectura, también en la documentación propietaria de `docs/`.

## Convención de trazabilidad

Cada PRD contiene una sección final con estos campos:

- Rama de trabajo.
- Commit o pull request.
- Archivos modificados.
- Pruebas y comandos ejecutados.
- Evidencia o enlace a la validación.
- Fecha y responsable.

Mientras el trabajo no se implemente, estos campos permanecen como `pendiente`; no se deben rellenar con datos supuestos.
