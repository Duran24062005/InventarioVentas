# Services de Categorias

Aqui se implementan las operaciones y reglas de negocio de categorias. El service valida condiciones funcionales, coordina persistencia y transforma datos entre entidades y DTOs cuando corresponda.

No debe depender del contexto HTTP ni devolver `IActionResult`. Los controllers son la capa que traduce el resultado a HTTP.
