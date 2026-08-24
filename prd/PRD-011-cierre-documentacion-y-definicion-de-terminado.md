# PRD-011: Cierre, documentación y definición de terminado

| Campo | Valor |
| --- | --- |
| Estado | Propuesto |
| Prioridad | Alta |
| Dependencias | PRD-001 a PRD-010 |
| Módulo propietario | Documentación y calidad del repositorio |
| Historias relacionadas | HU01–HU04 |

## Problema y objetivo

Una implementación puede compilar y aun así quedar sin evidencia, con decisiones dispersas o con documentación desactualizada. Este PRD define el cierre verificable de cada slice y del proyecto inicial completo.

## Alcance

- Mantener actualizado `prd/README.md` con estados y dependencias.
- Completar la trazabilidad de cada PRD con ramas, commits, archivos y pruebas reales.
- Actualizar `docs/Architecture.md`, `docs/System_Artifact.md` o READMEs cuando cambien contratos, reglas o estructura.
- Mantener comandos de configuración y ejecución reproducibles.
- Aplicar una definición de terminado común al proyecto.

Fuera de alcance: nuevas funcionalidades no incluidas en PRD-001 a PRD-010, despliegue productivo, autenticación y operación empresarial.

## Definición de terminado por PRD

Un PRD puede pasar a `Terminado` solo cuando:

- Su implementación está en una rama o commit identificable.
- Sus criterios de aceptación se verificaron.
- Sus pruebas pasan y están registradas.
- La documentación propietaria quedó actualizada.
- No deja decisiones pendientes ocultas que afecten el comportamiento entregado.
- El índice y el estado del PRD reflejan la realidad.

## Definición de terminado del proyecto inicial

### Código

- La solución compila sin errores ni advertencias.
- No existe el endpoint `weatherforecast`.
- La lógica de negocio está en services y la API está organizada como monolito modular.
- Los controllers no consultan directamente `DbContext` ni calculan reglas.
- Los DTOs protegen las entidades persistentes.

### Persistencia

- La base de datos se crea desde migraciones.
- Las relaciones, índices únicos y precisión decimal son correctos.
- No existen secretos versionados.
- El flujo de venta conserva consistencia transaccional.

### API

- Categorías, Productos, Clientes y Ventas exponen los endpoints acordados.
- Los códigos HTTP son coherentes.
- Los errores tienen un formato uniforme.
- Swagger/OpenAPI y el archivo HTTP permiten verificar los contratos principales.

### Reglas funcionales

- No se aceptan datos obligatorios inválidos.
- Los códigos de producto y documentos de cliente son únicos.
- No se venden productos inexistentes, inactivos o sin stock suficiente.
- Los precios, subtotales y totales se calculan en backend.
- El stock no queda parcialmente descontado si falla una venta.

### Documentación

- `README.md` raíz enlaza la documentación relevante.
- `docs/README.md` enlaza el índice de PRDs.
- Las decisiones arquitectónicas y funcionales están en `docs/`.
- Los READMEs de módulos y carpetas siguen describiendo la implementación real.

## Checklist de cierre

| Verificación | Estado |
| --- | --- |
| `dotnet restore` | Pendiente |
| `dotnet build` | Pendiente |
| `dotnet test` | Pendiente |
| Migraciones aplicadas | Pendiente |
| Endpoints principales verificados | Pendiente |
| Rollback de venta verificado | Pendiente |
| Documentación sincronizada | Pendiente |
| Decisiones abiertas revisadas | Pendiente |
| PRDs actualizados con evidencia | Pendiente |

## Riesgos y decisiones pendientes

- Un PRD no debe marcarse como terminado por conveniencia de planificación.
- Las decisiones abiertas del índice deben cerrarse, mantenerse explícitamente abiertas o convertirse en nuevos PRDs.
- La documentación debe actualizarse en el repositorio que posee la regla, no en una nota temporal.

## Trazabilidad

| Evidencia | Valor |
| --- | --- |
| Rama | Pendiente |
| Commit o PR | Pendiente |
| Archivos modificados | Pendiente |
| Pruebas ejecutadas | Pendiente |
| Checklist de cierre | Pendiente |
| Responsable y fecha de implementación | Pendiente |

## Referencias

- [`prd/README.md`](README.md).
- [`README.md`](../README.md).
- [`docs/README.md`](../docs/README.md).
- [`docs/Architecture.md`](../docs/Architecture.md).
- [`docs/System_Artifact.md`](../docs/System_Artifact.md).
