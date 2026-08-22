# Codigo fuente

Aqui viven los proyectos ejecutables y las librerias de la solucion. Actualmente la solucion contiene un unico proyecto: `InventarioVentas.API`.

## Que debe ir aqui

- Un nuevo proyecto `.csproj` solo si existe una razon arquitectonica clara y se documenta en `docs/Architecture.md`.
- Codigo que sea parte de una aplicacion ejecutable o de una libreria reutilizable del sistema.

La logica de negocio no debe colocarse directamente en `src/`; debe vivir dentro del proyecto y modulo que la posee. Antes de crear un proyecto adicional, verifica si el monolito modular actual ya resuelve la necesidad.

Las carpetas `bin/` y `obj/` pueden aparecer dentro de los proyectos despues de compilar. Son artefactos generados, no codigo fuente y no deben editarse ni versionarse.
