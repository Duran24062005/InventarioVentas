# PRD-002: Modelo de dominio

| Campo | Valor |
| --- | --- |
| Estado | Propuesto |
| Prioridad | Alta |
| Dependencias | PRD-001 |
| Módulo propietario | Dominio compartido por Categorías, Productos, Clientes y Ventas |
| Historias relacionadas | HU01–HU04 |

## Problema y objetivo

La API no tiene todavía entidades que representen inventario, compradores ni ventas. Este PRD define el modelo persistente mínimo y sus relaciones para que los servicios y la base de datos compartan las mismas reglas funcionales.

## Alcance

Crear las entidades `Categoria`, `Producto`, `Cliente`, `Venta` y `DetalleVenta`, con sus propiedades, relaciones e invariantes básicas.

Fuera de alcance: DTOs HTTP, configuraciones de EF Core, migraciones, servicios, autenticación, pagos, devoluciones y cancelación de ventas.

## Modelo y reglas

### Categoria

Debe contener `Id`, `Nombre`, `Descripcion` opcional, `FechaCreacion` y `Estado`. El nombre es obligatorio. Una categoría puede tener muchos productos.

### Producto

Debe contener `Id`, `Nombre`, `Codigo`, `Precio`, `Stock`, `Estado`, `CategoriaId` y `FechaCreacion`. El código es obligatorio y único; el precio debe ser mayor que cero; el stock no puede ser negativo; la categoría es obligatoria.

### Cliente

Debe contener `Id`, `NombreCompleto`, `Documento`, `Email`, `Telefono` opcional y `FechaRegistro`. El nombre y documento son obligatorios; el documento es único; el email debe tener formato válido. Un cliente puede tener muchas ventas.

### Venta

Debe contener `Id`, `FechaVenta`, `ClienteId` y `Total`. Una venta pertenece a un cliente y debe tener uno o más detalles. El total es un valor calculado por backend.

### DetalleVenta

Debe contener `Id`, `VentaId`, `ProductoId`, `Cantidad`, `PrecioUnitario` y `Subtotal`. La cantidad debe ser mayor que cero; el precio unitario se captura desde el producto al vender; el subtotal se calcula como `Cantidad * PrecioUnitario`.

## Relaciones

```text
Categoria 1 ──── N Producto
Cliente   1 ──── N Venta
Venta     1 ──── N DetalleVenta
Producto  1 ──── N DetalleVenta
```

Las relaciones deben conservar la trazabilidad histórica de las ventas. La decisión concreta entre eliminación lógica y física se documenta en PRD-006 y PRD-007; la recomendación vigente es usar `Estado` para categorías y productos.

## Actores y consumidores

- Servicios de cada módulo.
- `AppDbContext` y configuraciones de persistencia.
- Controllers mediante DTOs, sin exponer directamente las entidades.
- El proceso transaccional de Ventas.

## Interfaces y tipos afectados

- Entidades de dominio en `Modules/*/Models`.
- Propiedades de navegación y claves foráneas necesarias para EF Core.
- Tipos monetarios con `decimal`; cantidades y stock con `int`, de acuerdo con el artefacto funcional.

## Impacto en datos

Este modelo será la fuente para las tablas, claves, índices, relaciones y precisión decimal del PRD-003. No debe implementarse una migración hasta que el modelo y sus configuraciones estén revisados.

## Criterios de aceptación

- Existen las cinco entidades dentro de los módulos propietarios.
- Las propiedades obligatorias y opcionales coinciden con `docs/System_Artifact.md`.
- Las relaciones entre categorías, productos, clientes, ventas y detalles están expresadas en el modelo.
- Los valores calculados de venta no dependen de datos confiables enviados desde HTTP.
- No hay DTOs, controllers ni lógica de persistencia dentro de los modelos.
- Las reglas de unicidad y precisión quedan listas para ser reforzadas por EF Core en PRD-003.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Instanciar una categoría con nombre | El modelo acepta el estado válido. |
| Instanciar un producto con stock negativo | La regla queda identificada para validación y persistencia; no se considera estado válido. |
| Crear un detalle con cantidad cero | La regla de dominio lo marca como inválido. |
| Crear una venta sin detalles | No cumple el modelo funcional de una venta válida. |
| Relacionar una venta con un cliente y detalles | Las referencias representan las relaciones esperadas. |

## Riesgos y decisiones pendientes

- Debe definirse si `Estado` será `bool` o enum antes de congelar el esquema.
- Debe definirse si el precio y el stock pueden editarse después de existir ventas.
- Las validaciones que requieren consultas a la base de datos no deben duplicarse dentro de las entidades; se resolverán en services.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | Pendiente |
| Commit o PR | Pendiente |
| Archivos modificados | Pendiente |
| Pruebas ejecutadas | Pendiente |
| Evidencia adicional | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), secciones 5, 6, 10 y 11.
- [`docs/Architecture.md`](../docs/Architecture.md).
- [`src/InventarioVentas.API/Modules/README.md`](../src/InventarioVentas.API/Modules/README.md).
- READMEs de `Models` de cada módulo.
