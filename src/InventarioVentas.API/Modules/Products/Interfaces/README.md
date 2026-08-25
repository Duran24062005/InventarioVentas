# Interfaces de Productos

Aqui se definen los contratos de los services de productos y las operaciones que otros modulos necesitan consumir de forma controlada.

Las interfaces no deben filtrar detalles de SQL, `DbContext` o HTTP. Expresan capacidades del inventario, no la implementacion usada para lograrlas.
