# PRD-008: Módulo de clientes

| Campo | Valor |
| --- | --- |
| Estado | Propuesto |
| Prioridad | Alta |
| Dependencias | PRD-002 y PRD-005 |
| Módulo propietario | `Modules/Customers` |
| Historias relacionadas | HU04 |

## Problema y objetivo

Una venta debe asociarse a un comprador identificable, pero el proyecto todavía no ofrece un módulo de clientes. Este PRD implementa el registro y consulta mínima de clientes, incluyendo las reglas de identificación y formato requeridas por el dominio.

## Alcance

- Entidad, DTOs, interfaces, service, validators y controller de Clientes.
- Registrar y consultar clientes.
- Validar nombre obligatorio, documento único y email válido.
- Exponer capacidades para que Ventas verifique la existencia del cliente.

Fuera de alcance: autenticación de clientes, cuentas de usuario, direcciones, historial avanzado, pagos y endpoints de actualización o eliminación no definidos por el contrato inicial.

## Contrato HTTP

| Método | Endpoint | Éxito | Errores principales |
| --- | --- | --- | --- |
| `POST` | `/api/clientes` | `201 Created` | `400 Bad Request` |
| `GET` | `/api/clientes` | `200 OK` | — |

Request mínimo:

```json
{
  "nombreCompleto": "María Pérez",
  "documento": "123456789",
  "email": "maria.perez@example.com",
  "telefono": "3001234567"
}
```

Las respuestas deben utilizar DTOs y no exponer directamente la entidad de persistencia.

## Reglas funcionales

- `NombreCompleto` es obligatorio.
- `Documento` es obligatorio y único.
- `Email` debe tener formato válido.
- `Telefono` es opcional.
- `FechaRegistro` se asigna en backend.
- Una venta solo puede referenciar un cliente existente.
- Las validaciones de forma pertenecen al validator; la unicidad y existencia pertenecen al service.

## Interfaces y componentes

- DTOs de creación y respuesta.
- `IClienteService` orientado a registrar, consultar y verificar clientes.
- `ClienteService` para unicidad, persistencia y mapeo.
- Validators FluentValidation para campos requeridos y email.
- `ClientesController` limitado a coordinación HTTP.

## Impacto en datos e integraciones

Usa la entidad y el índice único de documento definidos en PRD-002 y PRD-003. Ventas consumirá una capacidad de verificación, sin duplicar la entidad ni las reglas de Clientes.

## Criterios de aceptación

- Se registra un cliente válido y se devuelve `201 Created`.
- Se rechaza un cliente sin nombre.
- Se rechaza un email inválido.
- Se rechaza un documento repetido con un error controlado.
- El listado devuelve DTOs y maneja correctamente una lista vacía.
- La fecha de registro no puede ser manipulada desde el request.
- Ventas puede verificar la existencia del cliente mediante una colaboración explícita.
- El controller no contiene lógica de unicidad ni acceso directo al `DbContext`.

## Casos de prueba y verificación

| Caso | Resultado esperado |
| --- | --- |
| Registrar cliente válido | `201` y registro persistido. |
| Registrar sin nombre | `400`. |
| Registrar con email inválido | `400`. |
| Registrar documento existente | `400` por duplicidad. |
| Consultar lista sin clientes | `200` con lista vacía. |
| Consultar lista con clientes | `200` con DTOs completos. |
| Verificar cliente existente desde Ventas | La validación pasa. |
| Verificar cliente inexistente desde Ventas | Error `404` controlado. |

## Riesgos y decisiones pendientes

- El formato y longitud del documento pueden depender del país o del negocio; no debe agregarse una regla más restrictiva sin decisión.
- No se debe eliminar físicamente un cliente con ventas históricas.
- La necesidad de actualizar o desactivar clientes queda fuera del contrato inicial y debe entrar mediante un PRD posterior.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | Pendiente |
| Commit o PR | Pendiente |
| Archivos modificados | Pendiente |
| Pruebas ejecutadas | Pendiente |
| Endpoints verificados | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), secciones 5.3, 6.1, 7.3, 9, 11 y 15.
- [`src/InventarioVentas.API/Modules/Customers/README.md`](../src/InventarioVentas.API/Modules/Customers/README.md).
- READMEs de `Controllers`, `DTOs`, `Interfaces`, `Services` y `Validators` de Clientes.
