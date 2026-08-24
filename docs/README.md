# Documentacion

Esta carpeta conserva las decisiones y referencias que explican como construir y mantener InventarioVentas. La documentacion existe para que una persona nueva pueda entender el proyecto antes de modificar codigo.

La hoja de ruta implementable y su trazabilidad se mantienen en [`../prd/README.md`](../prd/README.md). Los PRDs deben leerse junto con los documentos de arquitectura y reglas funcionales de esta carpeta.

## Archivos

- `Architecture.md`: explica el monolito modular, sus modulos y las reglas de separacion de responsabilidades.
- `System_Artifact.md`: define el problema, el alcance, el modelo de dominio y las reglas funcionales.
- `project_configuration_commands.md`: contiene los comandos para crear, configurar, restaurar, compilar y ejecutar el proyecto con .NET 10.
- [`../prd/README.md`](../prd/README.md): indice de PRDs, dependencias, historias relacionadas y decisiones abiertas.

La composicion tecnica actual usa `AddControllers`, `AddEndpointsApiExplorer`, `AddSwaggerGen`, `UseHttpsRedirection` y `MapControllers`. Swagger UI se expone solo en Development; no se deben agregar secretos a los archivos `appsettings`.

## Como usar esta carpeta

Lee primero `Architecture.md` para ubicar una funcionalidad. Consulta `System_Artifact.md` antes de implementar una regla de negocio y `project_configuration_commands.md` cuando necesites preparar el entorno.

Si una implementacion cambia una regla funcional, un contrato HTTP, una dependencia o la estructura de carpetas, actualiza aqui el documento que corresponda. No uses esta carpeta para guardar notas temporales o decisiones que solo aplican a un archivo concreto.
