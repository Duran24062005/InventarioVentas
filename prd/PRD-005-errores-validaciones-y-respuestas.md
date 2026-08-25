# PRD-005: Errores, validaciones y respuestas

| Campo | Valor |
| --- | --- |
| Estado | En progreso |
| Prioridad | Alta |
| Dependencias | PRD-001 y PRD-003 |
| Módulo propietario | `Common`, `Middleware` y composición de la API |
| Historias relacionadas | HU01–HU04 |

## Problema y objetivo

Los módulos todavía no comparten un contrato conectado al pipeline para errores, validaciones ni respuestas HTTP. Ya existen tipos comunes y dos implementaciones de middleware, pero la composición no registra ninguna de ellas y debe consolidarse una sola estrategia antes de considerar terminado este PRD.

## Alcance

- Definir un formato común para respuestas exitosas cuando sea necesario.
- Definir un formato común para errores simples y errores de validación múltiple.
- Crear excepciones reutilizables para recurso inexistente y reglas de negocio incumplidas.
- Crear middleware de manejo uniforme de excepciones.
- Registrar validadores FluentValidation y conectarlos al pipeline HTTP.
- Separar validaciones de forma de las validaciones que requieren estado de la base de datos.

Fuera de alcance: logging avanzado, telemetría, autenticación, autorización y reglas específicas de un único módulo.

## Contrato HTTP

La respuesta de error debe conservar un mensaje claro y, cuando aplique, una colección de errores de validación. Como mínimo se deben representar:

```json
{
  "mensaje": "La solicitud contiene datos invalidos",
  "errores": [
    "El nombre es obligatorio",
    "El precio debe ser mayor a cero"
  ]
}
```

La API debe usar, como mínimo, `200 OK`, `201 Created`, `400 Bad Request` y `404 Not Found` según el resultado de la operación. Los detalles técnicos de una excepción no deben enviarse al consumidor en producción.

## Distribución de responsabilidades

- Validators: campos requeridos, longitudes, formatos, rangos y estructura del request.
- Services: existencia, unicidad, estado de registros, disponibilidad de stock y reglas que requieren consultas.
- Exceptions: describen fallos conocidos sin construir respuestas HTTP.
- Middleware: traduce excepciones a respuestas HTTP consistentes.
- Controllers: reciben DTOs, llaman al service y seleccionan el resultado HTTP.

## Interfaces y tipos afectados

- Modelos comunes en `Common/Responses`.
- Excepciones en `Common/Exceptions`.
- `ExceptionMiddleware` en `Middleware`.
- Registro de FluentValidation y middleware en `Program.cs` o `Extensions`.
- Contratos de services, que no deben devolver `IActionResult`.

## Impacto en datos e integraciones

No cambia el esquema de datos. Afecta el contrato HTTP de todos los módulos y establece una convención que consumidores de la API podrán reutilizar.

## Estado actual de implementación

- Existen `BusinessException`, `NotFoundException` y `ValidationException` en `Common/Exceptions`.
- Existe `ApiResponse<T>` en `Common/Responses`, pero ningún controller actual lo utiliza.
- Hay dos middlewares (`ExceptionMiddleware` y `ExceptionHandlingMiddleware`) con formatos, idioma y comportamiento distintos; ninguno está registrado en `Program.cs`.
- `CrearCategoriaValidator` y `CrearProductoValidator` existen, pero no están registrados ni ejecutados automáticamente.
- La API todavía no tiene un contrato de errores verificado mediante requests reales.

## Criterios de aceptación

- Una solicitud inválida devuelve `400 Bad Request` con mensaje claro.
- Una excepción de recurso inexistente devuelve `404 Not Found`.
- Una regla de negocio incumplida devuelve un error controlado y no una excepción no tratada.
- Los errores no exponen stack traces ni credenciales.
- Los validadores pueden registrarse por módulo sin acoplarse a `DbContext`.
- El middleware tiene un orden documentado en `Program.cs`.
- Los controllers no duplican la traducción de las mismas excepciones.
- El formato de error es consistente en Categorías, Productos, Clientes y Ventas.
- Se conserva una única implementación de middleware y una única convención de respuesta antes de cerrar el PRD.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Request sin campo obligatorio | `400` con el mensaje de validación correspondiente. |
| Request con varios errores | `400` con colección de errores. |
| Service lanza recurso inexistente | `404` con mensaje controlado. |
| Service lanza regla de negocio | `400` con mensaje controlado. |
| Excepción inesperada | Respuesta genérica segura y registro técnico. |
| Request válido | El middleware no altera la respuesta exitosa. |

## Riesgos y decisiones pendientes

- Debe mantenerse una única convención de nombres y formato para no fragmentar el contrato.
- El logging técnico debe evitar datos sensibles de requests y cadenas de conexión.
- Si se adopta Problem Details en el futuro, debe documentarse como cambio de contrato y no mezclarse silenciosamente con el formato actual.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | `main` |
| Commit o PR | Pendiente |
| Archivos modificados | `src/InventarioVentas.API/Common`, `src/InventarioVentas.API/Middleware` y validators iniciales de Categorías y Productos |
| Pruebas ejecutadas | Revisión estática; no hay pruebas funcionales y la API no arranca por DI |
| Contrato HTTP verificado | Pendiente; middleware y validadores aún no están conectados |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), secciones 6, 8 y 9.
- [`src/InventarioVentas.API/Common/README.md`](../src/InventarioVentas.API/Common/README.md).
- [`src/InventarioVentas.API/Common/Exceptions/README.md`](../src/InventarioVentas.API/Common/Exceptions/README.md).
- [`src/InventarioVentas.API/Common/Responses/README.md`](../src/InventarioVentas.API/Common/Responses/README.md).
- [`src/InventarioVentas.API/Middleware/README.md`](../src/InventarioVentas.API/Middleware/README.md).
