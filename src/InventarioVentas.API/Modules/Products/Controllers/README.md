# Controllers de Productos

Aqui van los controllers que exponen los endpoints HTTP de productos.

Deben limitarse a recibir la solicitud, activar validacion, llamar al service y devolver la respuesta. No deben modificar stock directamente ni consultar el `DbContext` para resolver reglas de negocio.
