# Modules

Esta carpeta organiza el dominio por funcionalidades. Cada subcarpeta representa un modulo del monolito modular y debe poder entenderse sin recorrer todas las carpetas globales de la API.

Los modulos actuales son `Categorias`, `Productos`, `Clientes` y `Ventas`. Dentro de cada uno se repiten las mismas responsabilidades: `Controllers`, `DTOs`, `Interfaces`, `Models`, `Services` y `Validators`.

## Regla de propiedad

Si una clase conoce principalmente una funcionalidad, debe vivir en esa funcionalidad. Las colaboraciones entre modulos deben ser pequenas, explicitas y justificadas por una regla del dominio. No dupliques una entidad o una regla para evitar pensar como comunicar los modulos.

Cuando se agregue un nuevo modulo, actualiza `docs/Architecture.md` y crea su README siguiendo la misma guia.
