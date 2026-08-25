# TODO de InventarioVentas

Documento de seguimiento técnico y funcional. Refleja el estado revisado el **2026-08-25** en la rama `main`.

## Estado actual

- [x] La solución compila con .NET 10: `dotnet build InventarioVentas.slnx --no-restore` termina con 0 errores y 0 advertencias.
- [x] La API tiene composición inicial, Swagger y Docker multi-stage.
- [x] Categorías tiene un CRUD parcial con controller, DTOs, service, validator y `CategoriaDbContext` provisional.
- [x] Productos tiene DTOs, contrato de service, service vacío y validator inicial.
- [ ] La API arranca correctamente.
- [ ] Existe un `AppDbContext` completo y una conexión a PostgreSQL.
- [ ] Hay migraciones, pruebas funcionales o pruebas de integración.

### Bloqueo inmediato

`CategoriasService` recibe `CategoriaDbContext` y ambos servicios ya están registrados en DI. El arranque requiere `ConnectionStrings:DefaultConnection`; si falta, `Program.cs` falla con un mensaje claro indicando que PostgreSQL necesita esa configuración. El contexto sigue siendo provisional y debe integrarse en el `AppDbContext` completo.

## Paso a paso pendiente

### 1. Cerrar el modelo de dominio — PRD-002

- [ ] Crear las entidades `Categoria`, `Producto`, `Cliente`, `Venta` y `DetalleVenta`.
- [ ] Resolver la incompatibilidad actual entre `Categoria.Id` (`Guid`) y `Producto.CategoriaId` (`int`).
- [ ] Confirmar tipos, campos obligatorios, navegación y estrategia para `Estado`.
- [ ] Definir qué campos controla el backend (`FechaCreacion`, `FechaRegistro`, `Estado`, totales y precios históricos).
- [ ] Actualizar los README de modelos con las decisiones finales.
- [ ] Verificar que el modelo represente las cuatro relaciones del dominio.

**Salida:** entidades completas, relaciones claras y decisiones de dominio documentadas.

### 2. Unificar la persistencia — PRD-003

- [ ] Reemplazar o integrar `CategoriaDbContext` dentro de un único `AppDbContext` en `Data`.
- [ ] Agregar los cinco `DbSet` esperados.
- [ ] Crear una configuración EF Core por entidad.
- [ ] Configurar claves, relaciones, nulabilidad, longitudes e índices únicos.
- [ ] Configurar precisión explícita para `Precio`, `PrecioUnitario`, `Subtotal` y `Total`.
- [ ] Registrar el contexto en DI desde `Program.cs` o una extensión propietaria.
- [ ] Confirmar que `CategoriasService` use el contexto definitivo.

**Salida:** `dotnet build` pasa y `AppDbContext` puede resolverse desde DI.

### 3. Configurar PostgreSQL y migraciones — PRD-004

- [ ] Agregar el contrato `ConnectionStrings:DefaultConnection` sin secretos versionados.
- [ ] Definir el mecanismo local: User Secrets o variables de entorno.
- [x] Incorporar PostgreSQL al entorno local mediante Docker Compose.
- [ ] Crear y revisar la migración inicial.
- [ ] Ejecutar `dotnet ef database update` contra una base de datos de desarrollo aislada.
- [ ] Documentar comandos de migración, actualización y reversión.

**Salida:** una base vacía puede construirse reproduciblemente desde el repositorio y la configuración local.

### 4. Consolidar errores, validaciones y respuestas — PRD-005

- [ ] Elegir una sola implementación entre `ExceptionMiddleware` y `ExceptionHandlingMiddleware`.
- [ ] Alinear el formato de errores y decidir si se usará `ApiResponse<T>` o respuestas de error independientes.
- [ ] Registrar el middleware en el orden correcto del pipeline.
- [ ] Registrar FluentValidation y los validators de cada módulo.
- [ ] Separar validaciones de forma de reglas que requieren consultas a la base de datos.
- [ ] Agregar logging seguro para excepciones inesperadas.
- [ ] Verificar `400`, `404` y `500` con requests reales.

**Salida:** un contrato de error único, documentado y comprobado.

### 5. Terminar Categorías — PRD-006

- [ ] Ajustar los DTOs para que `FechaCreacion` y `Estado` controlados por backend no sean obligatorios al crear.
- [ ] Alinear el validator con el contrato definitivo, incluyendo la semántica de `false` para `Estado`.
- [ ] Registrar el contexto definitivo y comprobar el CRUD contra PostgreSQL.
- [ ] Cambiar `DELETE` a eliminación lógica si se confirma la política de historial.
- [ ] Definir si los listados muestran categorías activas solamente o todas.
- [ ] Alinear los códigos HTTP de `PUT` y `DELETE` con el contrato elegido.
- [ ] Agregar pruebas para creación, consulta, actualización, inexistencia y desactivación.

**Salida:** Categorías cumple sus criterios de aceptación y sus endpoints están verificados.

### 6. Implementar Productos e inventario — PRD-007

- [ ] Crear la entidad `Producto` y su configuración EF Core.
- [ ] Crear `ProductosController`.
- [ ] Completar `ProductoService` y registrar `IProductoService`.
- [ ] Implementar unicidad de código y existencia/estado de categoría.
- [ ] Implementar creación, consulta, actualización y desactivación.
- [ ] Definir qué ocurre con precio y stock después de una venta.
- [ ] Verificar que las respuestas incluyan la categoría sin exponer entidades.
- [ ] Agregar pruebas de precio, stock, código duplicado, categoría inexistente y producto inactivo.

**Salida:** Productos administra inventario y expone el contrato que necesitará Ventas.

### 7. Implementar Clientes — PRD-008

- [ ] Crear entidad, DTOs, validator, service, interfaz y controller.
- [ ] Implementar unicidad de documento y validación de email.
- [ ] Definir consulta, alta y estado del cliente.
- [ ] Agregar pruebas de datos obligatorios, documento duplicado y cliente inexistente.

**Salida:** Clientes puede crear y consultar compradores válidos.

### 8. Implementar Ventas y descuento transaccional de stock — PRD-009

- [ ] Crear `Venta`, `DetalleVenta`, DTOs, validator, service, interfaz y controller.
- [ ] Validar cliente, productos activos, cantidades y stock suficiente.
- [ ] Tomar `PrecioUnitario` desde la base de datos.
- [ ] Calcular subtotales y total exclusivamente en backend.
- [ ] Ejecutar venta, detalles y descuento de stock dentro de una transacción.
- [ ] Garantizar que un error no deje descuentos parciales.
- [ ] Agregar pruebas de venta válida, stock insuficiente, producto inactivo y rollback.

**Salida:** registrar una venta modifica el stock de forma atómica y conserva el precio histórico.

### 9. Crear la estrategia de pruebas — PRD-010

- [ ] Crear proyectos de pruebas y documentar el framework elegido.
- [ ] Cubrir validators, services, `AppDbContext`, relaciones, índices y migraciones.
- [ ] Cubrir controllers, códigos HTTP, DTOs y middleware.
- [ ] Ejecutar pruebas contra PostgreSQL de prueba o un entorno aislado documentado.
- [ ] Registrar comandos y resultados en la trazabilidad de cada PRD.

**Salida:** la API tiene evidencia reproducible más allá de una compilación exitosa.

### 10. Cerrar documentación y definición de terminado — PRD-011

- [ ] Actualizar el estado de cada PRD solo cuando sus criterios estén cumplidos.
- [ ] Completar las tablas de trazabilidad con rama, commit, archivos, pruebas y fecha.
- [ ] Revisar README raíz, `docs/`, README de módulos y contratos HTTP.
- [ ] Registrar decisiones abiertas que no se hayan resuelto.
- [ ] Confirmar que no queden secretos, duplicidad de middleware ni dependencias sin registrar.

**Salida:** otra persona puede entender, ejecutar y verificar el sistema sin depender de esta conversación.

### 11. Revalidar Docker — PRD-012

- [ ] Confirmar que la API arranque dentro del contenedor después de completar DI y persistencia.
- [x] Decidir que Compose incluirá PostgreSQL y gestionar su contraseña mediante variable de entorno.
- [ ] Ejecutar `docker compose config`.
- [ ] Ejecutar `docker compose up --build`.
- [ ] Verificar Swagger y endpoints funcionales dentro del contenedor.
- [ ] Documentar health checks, HTTPS y configuración de producción cuando exista un entorno real.

**Salida:** el flujo Docker reproduce el entorno de desarrollo completo y tiene evidencia de ejecución.

## Comandos de verificación rápida

```bash
dotnet restore
dotnet build InventarioVentas.slnx --no-restore
dotnet run --project src/InventarioVentas.API/InventarioVentas.API.csproj
docker compose config
docker compose up --build
```

No marcar un paso como terminado solo porque el proyecto compile. Cada paso debe tener código, criterios cumplidos y evidencia de prueba.
