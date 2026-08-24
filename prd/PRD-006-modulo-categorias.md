# PRD-006: Módulo de categorías

| Campo | Valor |
| --- | --- |
| Estado | Propuesto |
| Prioridad | Alta |
| Dependencias | PRD-002, PRD-003 y PRD-005 |
| Módulo propietario | `Modules/Categorias` |
| Historias relacionadas | HU01 |

## Problema y objetivo

Los productos necesitan una clasificación persistida y validada, pero el módulo de Categorías todavía es solo una estructura de carpetas. Este PRD implementa el ciclo de vida mínimo de una categoría y deja una capacidad clara para que Productos valide sus referencias.

## Alcance

- Entidad, DTOs, interfaces, service, validator y controller de Categorías.
- Crear, listar, consultar, actualizar y desactivar categorías.
- Validar nombre obligatorio y datos de entrada.
- Evitar que el controller consulte directamente el `DbContext`.
- Mantener la trazabilidad de categorías relacionadas con productos.

Fuera de alcance: mantenimiento de productos, autenticación, paginación avanzada y eliminación física de datos históricos.

## Contrato HTTP

| Método | Endpoint | Éxito | Errores principales |
| --- | --- | --- | --- |
| `POST` | `/api/categorias` | `201 Created` | `400 Bad Request` |
| `GET` | `/api/categorias` | `200 OK` | — |
| `GET` | `/api/categorias/{id}` | `200 OK` | `404 Not Found` |
| `PUT` | `/api/categorias/{id}` | `200 OK` | `400`, `404` |
| `DELETE` | `/api/categorias/{id}` | `200 OK` | `404 Not Found` |

Request mínimo de creación/actualización:

```json
{
  "nombre": "Bebidas",
  "descripcion": "Productos líquidos"
}
```

La respuesta debe usar un DTO y no exponer directamente la entidad EF Core.

## Reglas funcionales

- `Nombre` es obligatorio y no puede estar vacío.
- `FechaCreacion` se asigna en backend.
- `Estado` inicia activo.
- `DELETE` debe aplicar eliminación lógica mediante `Estado = false`, conforme a la recomendación vigente.
- Las consultas deben respetar la decisión documentada sobre mostrar solo activas o también inactivas.
- Una categoría inexistente se comunica mediante el contrato de errores de PRD-005.

## Interfaces y componentes

- DTOs de crear, actualizar y respuesta.
- `ICategoriaService` orientado a capacidades del dominio.
- `CategoriaService` para validación funcional, persistencia y mapeo.
- Validator FluentValidation para requests.
- `CategoriasController` limitado a HTTP y delegación.

## Impacto en datos e integraciones

Usa la tabla y relaciones definidas en PRD-003. Productos dependerá de la existencia de una categoría válida, pero no copiará sus reglas ni su entidad.

## Criterios de aceptación

- Se puede crear una categoría válida y se devuelve `201 Created`.
- No se puede crear ni actualizar una categoría sin nombre válido.
- Se pueden listar y consultar categorías mediante DTOs.
- Consultar o modificar un identificador inexistente devuelve `404 Not Found`.
- `DELETE` desactiva la categoría sin borrar su registro físico.
- El controller no contiene reglas de negocio ni acceso directo al contexto.
- Los cambios se guardan mediante el service y la base de datos configurada.
- El módulo conserva su código dentro de `Modules/Categorias`.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Crear categoría válida | `201` y registro persistido. |
| Crear categoría sin nombre | `400` con validación clara. |
| Listar sin registros | `200` con lista vacía. |
| Consultar categoría existente | `200` con DTO de respuesta. |
| Consultar categoría inexistente | `404`. |
| Actualizar categoría existente | `200` y cambios persistidos. |
| Desactivar categoría | `200`, `Estado` inactivo y registro conservado. |

## Riesgos y decisiones pendientes

- Debe confirmarse si el listado excluye categorías inactivas por defecto.
- Debe definirse si se permite desactivar una categoría que todavía tenga productos activos; la implementación no debe ocultar esta decisión.
- La unicidad del nombre no está definida en el artefacto funcional y no debe inventarse sin decisión documentada.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | Pendiente |
| Commit o PR | Pendiente |
| Archivos modificados | Pendiente |
| Pruebas ejecutadas | Pendiente |
| Endpoints verificados | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), secciones 5.1, 7.1, 9, 11 y 15.
- [`src/InventarioVentas.API/Modules/Categorias/README.md`](../src/InventarioVentas.API/Modules/Categorias/README.md).
- READMEs de `Controllers`, `DTOs`, `Interfaces`, `Services` y `Validators` de Categorías.
