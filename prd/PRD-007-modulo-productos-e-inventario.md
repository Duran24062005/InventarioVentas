# PRD-007: Módulo de productos e inventario

| Campo | Valor |
| --- | --- |
| Estado | En progreso |
| Prioridad | Alta |
| Dependencias | PRD-006 |
| Módulo propietario | `Modules/Products` |
| Historias relacionadas | HU02 y HU03 |

## Problema y objetivo

El sistema necesita administrar los productos que pueden venderse, su categoría, precio, existencia y estado. Actualmente existe un esqueleto inicial de DTOs, contrato de service y validator, pero todavía no hay entidad, controller ni persistencia funcional.

## Alcance

- Entidad, DTOs, interfaces, service, validators y controller de Productos.
- Crear, listar, consultar, actualizar y desactivar productos.
- Validar código único, precio positivo, stock no negativo y categoría existente.
- Incluir información de categoría en las respuestas de consulta.
- Permitir que Ventas consuma capacidades de inventario sin acceder arbitrariamente a las carpetas internas.

Fuera de alcance: registro de ventas, movimientos históricos de inventario, múltiples bodegas, ajustes masivos y reportes avanzados.

## Contrato HTTP

| Método | Endpoint | Éxito | Errores principales |
| --- | --- | --- | --- |
| `POST` | `/api/productos` | `201 Created` | `400 Bad Request` |
| `GET` | `/api/productos` | `200 OK` | — |
| `GET` | `/api/productos/{id}` | `200 OK` | `404 Not Found` |
| `PUT` | `/api/productos/{id}` | `200 OK` | `400`, `404` |
| `DELETE` | `/api/productos/{id}` | `200 OK` | `404 Not Found` |

Request mínimo:

```json
{
  "nombre": "Café premium",
  "codigo": "CAF-001",
  "precio": 12500,
  "stock": 40,
  "categoriaId": 1
}
```

Las respuestas son DTOs y pueden incluir datos de la categoría; la entidad persistente no se expone directamente.

## Reglas funcionales

- El nombre, código, precio, stock y categoría son obligatorios.
- El código del producto es único.
- El precio debe ser mayor que cero.
- El stock inicial no puede ser negativo.
- La categoría referenciada debe existir y cumplir la política de estado definida por Categorías.
- Un producto inactivo no puede venderse.
- `FechaCreacion` y `Estado` se controlan desde backend.
- `DELETE` aplica eliminación lógica mediante `Estado = false`.
- El precio unitario de una venta se toma de la base de datos, no del request HTTP.

## Interfaces y componentes

- DTOs de crear, actualizar y respuesta.
- `IProductService` con capacidades de consulta y mantenimiento necesarias para Productos y Ventas.
- `ProductService` para unicidad, categoría, estado y persistencia.
- Validators para estructura, rangos y campos obligatorios.
- `ProductosController` limitado a coordinación HTTP.

## Impacto en datos e integraciones

Usa la relación con Categorías y la tabla configurada en PRD-003. Ventas dependerá de una consulta controlada de producto, existencia, estado, precio y stock.

## Estado actual de implementación

- Existen `CreateProductDto`, `ProductResponseDto`, `IProductService`, `ProductService` y `CreateProductValidator`.
- `ProductService` está vacío y no hay `ProductsController`; el modelo interno es `Product` y está ubicado en `Modules/Products/Models`.
- El DTO actual usa `int CategoryId`, mientras los modelos `Category` y `Product` usan `Guid`; esta incompatibilidad debe resolverse al cerrar PRD-002.
- El validator cubre nombre, código, precio, stock y categoría, pero todavía no está registrado ni ejecutado por la API.
- No existen consultas de unicidad, verificación de categoría, persistencia, actualización, desactivación ni control de stock.

## Criterios de aceptación

- Se crea un producto válido asociado a una categoría existente.
- Se rechaza precio menor o igual a cero.
- Se rechaza stock negativo.
- Se rechaza un código repetido con un error claro.
- Se rechaza una categoría inexistente.
- Las consultas devuelven productos con información de categoría.
- Un producto inactivo no puede ser utilizado por Ventas.
- `DELETE` conserva el registro y cambia su estado.
- El controller no modifica stock directamente.
- La entidad, controller, service, persistencia y pruebas se implementan antes de marcar el módulo como terminado.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Crear producto válido | `201` y producto persistido. |
| Crear con precio `0` o negativo | `400`. |
| Crear con stock negativo | `400`. |
| Crear con código existente | `400` por duplicidad. |
| Crear con categoría inexistente | Error controlado, `400` o `404` según PRD-005. |
| Consultar lista vacía | `200` con lista vacía. |
| Consultar producto existente | `200` con categoría incluida. |
| Desactivar producto | Registro conservado y no vendible. |

## Riesgos y decisiones pendientes

- Debe definirse si el precio y el stock podrán editarse después de una venta; no se debe sobrescribir el precio histórico de `DetalleVenta`.
- Debe definirse si el listado excluye productos inactivos por defecto.
- La concurrencia de descuentos de stock pertenece a PRD-009 y no debe resolverse con cambios aislados en este módulo.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `main` |
| Commit o PR | Pendiente |
| Archivos modificados | DTOs, interfaz, service vacío y validator inicial en `Modules/Products` |
| Pruebas ejecutadas | Revisión estática; no hay endpoints ni pruebas funcionales |
| Endpoints verificados | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), secciones 5.2, 6.2, 7.2, 9, 11 y 15.
- [`src/InventarioVentas.API/Modules/Products/README.md`](../src/InventarioVentas.API/Modules/Products/README.md).
- READMEs de `Controllers`, `DTOs`, `Interfaces`, `Services` y `Validators` de Productos.
