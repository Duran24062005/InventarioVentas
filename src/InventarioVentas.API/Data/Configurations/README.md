# Configurations

Aqui viven las configuraciones de Entity Framework Core, normalmente clases que implementan `IEntityTypeConfiguration<T>`.

Cada configuracion debe describir el esquema de una entidad: claves, columnas, precision, indices, relaciones y restricciones. No debe validar solicitudes HTTP ni decidir si una operacion de negocio esta permitida.

Mantener una configuracion por entidad ayuda a que `DbContext` no se convierta en un archivo enorme. Si una entidad pertenece a un modulo, conserva su configuracion identificable y evita mezclar configuraciones de entidades no relacionadas.
