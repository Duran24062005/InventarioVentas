# Common

Contiene piezas pequenas y verdaderamente transversales que pueden ser usadas por varios modulos de la API. Su objetivo es evitar duplicacion sin convertir esta carpeta en un deposito de logica de negocio.

## Que puede ir aqui

- Excepciones tecnicas o de aplicacion reutilizables.
- Formatos de respuesta compartidos por varios endpoints.
- Tipos comunes que no pertenecen a Categorias, Productos, Clientes o Ventas.

## Que no debe ir aqui

No coloques entidades, DTOs, services o validadores de un modulo concreto. Tampoco agregues una clase a `Common` solo porque es comoda de importar; primero confirma que realmente tiene mas de un consumidor y que no pertenece a un dominio.
