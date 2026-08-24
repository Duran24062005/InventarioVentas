# Exceptions

Aqui se definen excepciones reutilizables para representar errores conocidos de la aplicacion, por ejemplo un recurso inexistente o una regla de negocio incumplida.

Las excepciones describen el problema; no deben construir respuestas HTTP ni escribir directamente en la respuesta. Esa traduccion pertenece al middleware de manejo de errores.

Antes de agregar una excepcion, verifica que no sea una regla exclusiva de un modulo. Si solo aplica a Ventas o Productos, considera mantenerla cerca de ese modulo salvo que el middleware necesite un tipo comun.



## Para asegurar el manejo centralizado de errores y evitar saturar los Controllers con bloques try/catch redundantes, se incorporó la infraestructura transversal básica:

> ~ NotFoundException.cs: Representa la ausencia de un recurso solicitado (mapea automáticamente a código 404 Not Found).

> ~ BusinessException.cs: Representa el incumplimiento de una regla de negocio del sistema (mapea automáticamente a código 400 Bad Request).

> ~ ValidationException.cs: Maneja fallos en los datos de entrada o DTOs recibidos (mapea automáticamente a código 400 Bad Request).


