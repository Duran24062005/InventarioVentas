# Modulo Categorias

Este modulo administra las categorias que permiten organizar los productos. Es el propietario de las reglas y contratos relacionados con crear, consultar, actualizar y cambiar el estado de una categoria.

## Que debe ir aqui

- Endpoints de categorias.
- DTOs de entrada y salida de categorias.
- Entidad `Categoria` y sus reglas propias.
- Contratos y services de categoria.
- Validadores de las solicitudes de categoria.

## Dependencias

Productos necesita conocer categorias validas, pero esa colaboracion no justifica mover la logica de categorias a Productos. La existencia, estado y datos de una categoria se mantienen en este modulo.

El modulo esta preparado estructuralmente, pero sus archivos funcionales aun deben implementarse conforme a `docs/System_Artifact.md`.
