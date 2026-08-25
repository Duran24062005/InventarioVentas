# Controllers de Categorias

Aqui van los controllers que exponen los endpoints HTTP de categorias.

Un controller debe recibir parametros y DTOs, invocar el service correspondiente y devolver codigos HTTP coherentes. No debe consultar directamente el `DbContext`, calcular reglas de negocio ni decidir como se guarda una categoria.
