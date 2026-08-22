# Validators de Ventas

Aqui van los validadores de FluentValidation para requests de ventas y detalles.

Valida que haya detalles, que los identificadores tengan formato valido y que las cantidades sean mayores que cero. La existencia de registros, el estado del producto y el stock disponible se verifican en el service dentro de la operacion transaccional.
