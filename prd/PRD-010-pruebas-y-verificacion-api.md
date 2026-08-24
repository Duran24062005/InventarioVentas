# PRD-010: Pruebas y verificación de la API

| Campo | Valor |
| --- | --- |
| Estado | Propuesto |
| Prioridad | Alta |
| Dependencias | PRD-005, PRD-006, PRD-007, PRD-008 y PRD-009 |
| Módulo propietario | Pruebas y calidad transversal |
| Historias relacionadas | HU01–HU04 |

## Problema y objetivo

El proyecto solo tiene una verificación de compilación y no cuenta con pruebas funcionales de los módulos ni del flujo transaccional de ventas. Este PRD define una estrategia de pruebas que permita detectar regresiones en validaciones, contratos HTTP, persistencia y stock.

## Alcance

- Pruebas unitarias de validators y reglas de cálculo.
- Pruebas de services para casos válidos y errores de negocio.
- Pruebas de persistencia para relaciones, índices y precisión.
- Pruebas de integración/API para endpoints y códigos HTTP.
- Verificación del flujo de venta con rollback.
- Pruebas manuales o automatizadas desde Swagger y `InventarioVentas.API.http`.

Fuera de alcance: pruebas de carga, seguridad ofensiva, disponibilidad multi-región y pruebas de interfaz gráfica.

## Pirámide de pruebas

### Unitarias

Cubrir validaciones de campos, cálculo de subtotales y total, mapeos y decisiones que no requieran base de datos.

### Integración

Cubrir `AppDbContext`, relaciones, índices únicos, migraciones y operaciones de service contra una base SQL Server de pruebas o un ambiente aislado equivalente documentado.

### API

Cubrir requests y responses reales, códigos HTTP, DTOs, middleware de errores y comportamiento de los endpoints.

## Matriz mínima de escenarios

| Área | Escenarios obligatorios |
| --- | --- |
| Categorías | Crear válida, nombre vacío, consultar inexistente, actualizar y desactivar. |
| Productos | Crear válida, precio inválido, stock negativo, código repetido, categoría inexistente, producto inactivo. |
| Clientes | Crear válido, nombre vacío, email inválido, documento repetido y lista vacía. |
| Ventas | Venta válida, varios detalles, cliente inexistente, producto inexistente, producto inactivo, stock insuficiente y request sin detalles. |
| Persistencia | Relaciones, índices únicos, precisión decimal y migración desde base vacía. |
| Transversal | Formato de errores, `400`, `404`, `201`, `200` y ausencia de detalles internos. |

## Verificación crítica de ventas

La prueba de stock insuficiente debe comprobar simultáneamente que:

- No existe la venta.
- No existen sus detalles.
- Ningún producto fue descontado parcialmente.
- El error devuelto es consistente con PRD-005.

También debe verificarse que el precio enviado artificialmente por el consumidor no modifica el precio tomado de la base de datos.

## Interfaces y artefactos afectados

- Proyecto o proyectos de pruebas que se agreguen a la solución.
- Casos de prueba de services, validators, `DbContext` y controllers.
- Colección de requests de `InventarioVentas.API.http` y documentación OpenAPI.
- Scripts o configuración de ambiente de pruebas, sin secretos versionados.

## Criterios de aceptación

- Las pruebas se ejecutan con un comando documentado y reproducible.
- Existen pruebas para todos los escenarios mínimos de la matriz.
- La prueba de rollback demuestra ausencia de cambios parciales.
- Las pruebas de contrato verifican códigos HTTP y estructura de respuestas.
- La compilación y todas las pruebas pasan antes de marcar funcionalidad como terminada.
- Los fallos de infraestructura se distinguen de fallos funcionales.
- La evidencia de ejecución se registra en el PRD correspondiente y en PRD-011.

## Comandos y evidencia esperada

Como mínimo se deben registrar los comandos equivalentes a:

```bash
dotnet restore
dotnet build
dotnet test
dotnet ef migrations list
```

Para pruebas manuales, registrar endpoint, request, response, código HTTP y resultado. Si se usa una base de datos de pruebas, registrar el ambiente sin incluir credenciales.

## Riesgos y decisiones pendientes

- Debe definirse el proveedor o estrategia concreta de base de datos aislada para pruebas de integración; no usar SQLite como sustituto silencioso si se pretende validar comportamiento específico de SQL Server.
- Las pruebas concurrentes de stock pueden requerir una estrategia de aislamiento adicional.
- Los datos de pruebas deben ser reproducibles y no depender de una base personal persistente.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | Pendiente |
| Commit o PR | Pendiente |
| Archivos modificados | Pendiente |
| Comandos ejecutados | Baseline: `dotnet build --no-restore` exitoso el 2026-08-24 |
| Resultado de pruebas | Pendiente |
| Evidencia de rollback | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`docs/System_Artifact.md`](../docs/System_Artifact.md), secciones 9, 14, 15, 16 y 17.
- [`src/InventarioVentas.API/InventarioVentas.API.http`](../src/InventarioVentas.API/InventarioVentas.API.http).
- [`src/InventarioVentas.API/README.md`](../src/InventarioVentas.API/README.md).
