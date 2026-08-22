# Services de Productos

Aqui se implementan las reglas de productos e inventario: unicidad del codigo, precio valido, stock no negativo, categoria existente y estado del producto.

La venta debe procesarse en el service de `Ventas` como una unidad de trabajo. Si el cambio de stock depende de la transaccion completa, no lo escondas en un controller ni lo repartas entre varias capas sin documentarlo.
