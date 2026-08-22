# Exceptions

Aqui se definen excepciones reutilizables para representar errores conocidos de la aplicacion, por ejemplo un recurso inexistente o una regla de negocio incumplida.

Las excepciones describen el problema; no deben construir respuestas HTTP ni escribir directamente en la respuesta. Esa traduccion pertenece al middleware de manejo de errores.

Antes de agregar una excepcion, verifica que no sea una regla exclusiva de un modulo. Si solo aplica a Ventas o Productos, considera mantenerla cerca de ese modulo salvo que el middleware necesite un tipo comun.
