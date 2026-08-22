# Services de Ventas

Aqui se implementa el flujo principal de venta: verificar cliente y productos, comprobar stock, capturar precios, calcular subtotales y total, guardar la venta y descontar existencias.

La operacion debe ser atomica. Coordina la transaccion en la capa de service y deja que `Data` ejecute la persistencia. Si una regla cambia, actualiza `docs/System_Artifact.md`.
