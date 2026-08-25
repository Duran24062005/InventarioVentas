# Properties

Esta carpeta contiene configuracion local del proyecto, principalmente `launchSettings.json`, que define perfiles para ejecutar y depurar la API desde herramientas de desarrollo.

Aqui no deben agregarse reglas de negocio, configuracion de produccion ni secretos. Los cambios en los perfiles locales no deben asumir que todos los colaboradores usan el mismo puerto o IDE; documenta cualquier requisito especial en `docs/project_configuration_commands.md`.

Los perfiles actuales usan `http://localhost:5011` y `https://localhost:7176`, y establecen `ASPNETCORE_ENVIRONMENT=Development`. En esos perfiles Swagger UI queda disponible bajo `/swagger`.
