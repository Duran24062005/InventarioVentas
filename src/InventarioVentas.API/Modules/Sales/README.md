# Modulo Ventas

Este modulo registra ventas y sus detalles. Es el punto donde se coordina la existencia del cliente, la disponibilidad de productos, el calculo de subtotales y totales, y el descuento de stock.

## Que debe ir aqui

- Endpoints para registrar y consultar ventas.
- DTOs de venta y detalle de venta.
- Entidades `Venta` y `DetalleVenta`.
- Contratos y services del proceso de venta.
- Validadores de solicitudes de venta.

Una venta debe guardarse como una unidad de trabajo: si un detalle falla, no se guarda la venta ni se descuenta stock parcialmente. El precio unitario y los totales se calculan en backend; no se deben aceptar como valores confiables desde HTTP.

Ventas puede consultar Clientes y Productos mediante colaboraciones explicitas, pero no debe copiar sus entidades ni apropiarse de sus reglas de mantenimiento.
