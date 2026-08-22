# Services de Clientes

Aqui se implementan las operaciones y reglas de negocio de clientes, incluyendo unicidad del documento, validacion funcional y coordinacion con persistencia.

El service no conoce detalles de la respuesta HTTP. Si un cliente no existe, comunica el resultado mediante el mecanismo de errores definido por la aplicacion y deja que el middleware o controller lo traduzca.
