# Data

Esta carpeta es la frontera de persistencia de la API. Aqui se configura Entity Framework Core, el `DbContext`, las relaciones y el mapeo entre entidades y tablas.

## Responsabilidades

- Registrar el contexto de base de datos.
- Configurar claves, relaciones, restricciones, precision y nombres de tablas.
- Coordinar el guardado y consulta de entidades desde los services.

La carpeta no debe contener endpoints ni reglas de negocio de un modulo. El service decide que operacion necesita; `Data` define como se persiste.

Las migraciones generadas por EF Core deben seguir la convencion acordada para el proyecto y nunca deben editarse como si fueran codigo de negocio. Antes de crear una carpeta de migraciones, revisa `docs/Architecture.md`.
