# Controllers de Ventas

Aqui van los controllers que exponen los endpoints HTTP de ventas.

El controller recibe el request, activa validacion, llama al service de ventas y traduce el resultado a HTTP. No debe calcular totales, descontar stock ni guardar entidades directamente.
