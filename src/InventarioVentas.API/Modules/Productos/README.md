# Modulo Productos

Este modulo administra los productos del inventario, su relacion con categorias, precios, stock y estado.

## Que debe ir aqui

- Endpoints de productos.
- DTOs de entrada y salida de productos.
- Entidad `Producto` y sus reglas propias.
- Contratos y services para operaciones de inventario.
- Validadores de solicitudes de productos.

El modulo es responsable de que un producto tenga datos validos, codigo unico, precio positivo, stock no negativo y una categoria existente. La venta utiliza este modulo para consultar productos, pero el flujo transaccional de vender pertenece a `Ventas`.

La estructura esta creada; la implementacion funcional debe seguir las reglas de `docs/System_Artifact.md`.
