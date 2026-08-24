# PRD-009: Módulo de ventas y transacción de stock

| Campo | Valor |
| --- | --- |
| Estado | Propuesto |
| Prioridad | Crítica |
| Dependencias | PRD-007 y PRD-008 |
| Módulo propietario | `Modules/Ventas` |
| Historias relacionadas | HU04 |

## Problema y objetivo

Registrar una venta implica coordinar cliente, productos, cantidades, precios, detalles y stock. Si una parte falla después de guardar otra, el inventario queda inconsistente. Este PRD implementa el flujo de venta como una única operación atómica y conserva el precio histórico de cada detalle.

## Alcance

- Entidades, DTOs, interfaces, validators, service y controller de Ventas.
- Crear y consultar ventas y sus detalles.
- Verificar cliente existente.
- Verificar producto existente, activo y con stock suficiente.
- Leer precios desde la base de datos.
- Calcular subtotales y total en backend.
- Descontar stock dentro de la misma transacción que guarda venta y detalles.

Fuera de alcance: pagos, facturación electrónica, cancelación, devoluciones, descuentos, impuestos, múltiples bodegas y autenticación.

## Contrato HTTP

| Método | Endpoint | Éxito | Errores principales |
| --- | --- | --- | --- |
| `POST` | `/api/ventas` | `201 Created` | `400`, `404` |
| `GET` | `/api/ventas` | `200 OK` | — |
| `GET` | `/api/ventas/{id}` | `200 OK` | `404 Not Found` |

Request de creación:

```json
{
  "clienteId": 1,
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2
    }
  ]
}
```

El request no acepta como fuente confiable `PrecioUnitario`, `Subtotal` ni `Total`.

Respuesta esperada:

```json
{
  "id": 10,
  "fechaVenta": "2026-08-22T00:00:00",
  "clienteId": 1,
  "total": 25000,
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2,
      "precioUnitario": 12500,
      "subtotal": 25000
    }
  ]
}
```

## Flujo transaccional

1. Validar estructura del request y que exista al menos un detalle.
2. Verificar que el cliente exista.
3. Cargar todos los productos necesarios y confirmar que estén activos.
4. Confirmar stock suficiente para cada detalle.
5. Capturar el precio actual de cada producto.
6. Calcular subtotales y total en backend.
7. Iniciar una transacción de base de datos.
8. Crear la venta y sus detalles.
9. Descontar stock.
10. Guardar cambios y confirmar la transacción.

Si cualquier paso posterior al inicio de la operación falla, se revierte la venta, sus detalles y todos los descuentos de stock. La implementación debe evitar que una misma operación descuente parcialmente el inventario.

## Reglas funcionales

- La venta debe tener al menos un detalle.
- Cada cantidad debe ser mayor que cero.
- Cliente y productos deben existir.
- Los productos deben estar activos.
- Debe existir stock suficiente.
- `PrecioUnitario` se toma del producto al momento de vender.
- `Subtotal = Cantidad * PrecioUnitario`.
- `Total` es la suma de subtotales.
- La venta se guarda como unidad de trabajo.

## Interfaces y componentes

- DTOs de creación, detalle y respuesta.
- `IVentaService` para el proceso de venta y consultas.
- `VentaService` como coordinador de validación, cálculo, transacción y persistencia.
- Validators para estructura, identificadores y cantidades.
- `VentasController` limitado a HTTP.

## Impacto en datos e integraciones

Relaciona Clientes, Productos, Ventas y DetalleVenta. El módulo consume capacidades de Clientes y Productos, pero es propietario de la transacción que coordina la venta. Los precios guardados en los detalles son un snapshot histórico.

## Criterios de aceptación

- Una venta válida devuelve `201 Created`.
- El total y los subtotales son calculados exclusivamente por backend.
- El stock se descuenta exactamente una vez por cada detalle.
- Una venta con cliente inexistente devuelve `404` y no modifica datos.
- Una venta con producto inexistente o inactivo devuelve error controlado y no modifica datos.
- Una venta con stock insuficiente no guarda venta, detalles ni descuentos parciales.
- Una venta con varios detalles es atómica.
- La consulta de una venta devuelve sus detalles y valores históricos.
- El controller no contiene cálculos, consultas directas ni control transaccional.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Venta válida con un producto | `201`, total correcto y stock descontado. |
| Venta válida con varios productos | Todos los detalles guardados y total sumado. |
| Request sin detalles | `400` y ningún cambio. |
| Cliente inexistente | `404` y ningún cambio. |
| Producto inexistente | Error controlado y ningún cambio. |
| Producto inactivo | Error controlado y ningún cambio. |
| Stock insuficiente en un detalle | Rollback completo, sin cambios parciales. |
| Precio alterado en el request | Se ignora; se usa el precio de base de datos. |
| Consultar venta existente | `200` con detalles, subtotales y total. |

## Riesgos y decisiones pendientes

- Debe definirse el comportamiento cuando el mismo producto aparece más de una vez en la misma venta; se recomienda consolidar o rechazar duplicados antes de descontar stock.
- Debe definirse la estrategia de concurrencia para dos ventas simultáneas sobre el mismo stock.
- La cancelación de ventas y devolución de stock quedan explícitamente fuera de esta versión.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | Pendiente |
| Commit o PR | Pendiente |
| Archivos modificados | Pendiente |
| Pruebas ejecutadas | Pendiente |
| Endpoints verificados | Pendiente |
| Rollback verificado | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), secciones 5.4, 5.5, 6.3, 7.4, 9, 11, 15 y 16.
- [`src/InventarioVentas.API/Modules/Ventas/README.md`](../src/InventarioVentas.API/Modules/Ventas/README.md).
- [`src/InventarioVentas.API/Modules/Productos/README.md`](../src/InventarioVentas.API/Modules/Productos/README.md).
- [`src/InventarioVentas.API/Modules/Clientes/README.md`](../src/InventarioVentas.API/Modules/Clientes/README.md).
