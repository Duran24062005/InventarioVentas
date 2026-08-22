# System Artifact: Sistema de Gestion de Inventario y Ventas

## 1. Identificacion del proyecto

**Nombre:** Sistema de Gestion de Inventario y Ventas  
**Tipo:** API REST backend  
**Stack obligatorio:** .NET 10, ASP.NET Core Web API, Entity Framework Core, SQL Server, Swagger, Git  
**Objetivo principal:** construir una API REST para administrar categorias, productos, clientes y ventas, aplicando un monolito modular, validaciones, persistencia con EF Core y buenas practicas backend.

Este artefacto funciona como guia tecnica y funcional para implementar, revisar y validar el proyecto.

## 2. Problema y objetivo

El sistema debe permitir controlar un inventario basico y registrar ventas de productos. Al registrar una venta, la aplicacion debe validar la existencia del cliente y los productos, comprobar disponibilidad de stock, calcular totales y descontar existencias automaticamente.

El objetivo no es construir una solucion empresarial compleja, sino un proyecto pequeno pero suficientemente completo para practicar:

- C#.
- ASP.NET Core Web API.
- Entity Framework Core.
- SQL Server.
- API REST.
- Arquitectura por capas.
- Validaciones.
- Manejo correcto de respuestas HTTP.
- Separacion de responsabilidades.

## 3. Alcance funcional

El alcance inicial incluye cinco modulos:

- Categorias.
- Productos.
- Clientes.
- Ventas.
- Detalle de venta.

Quedan fuera del alcance inicial:

- Autenticacion y autorizacion.
- Roles de usuario.
- Facturacion electronica.
- Pagos.
- Devoluciones.
- Cancelacion de ventas.
- Reportes avanzados.
- Interfaz grafica.
- Multiples bodegas o sucursales.

## 4. Arquitectura propuesta

El proyecto debe usar una arquitectura de monolito modular dentro de una API principal llamada `InventarioVentas.API`. Cada modulo organiza internamente sus controllers, DTOs, interfaces, modelos, services y validators.

Estructura recomendada:

```text
InventarioVentas.API
├── Modules
│   ├── Categorias
│   ├── Productos
│   ├── Clientes
│   └── Ventas
├── Common
├── Data
├── Extensions
└── Middleware
```

Responsabilidades:

| Area | Responsabilidad |
| --- | --- |
| `Controllers` | Exponer endpoints REST, recibir DTOs, devolver respuestas HTTP. No deben contener logica de negocio. |
| `DTOs` | Definir contratos de entrada y salida de la API. |
| `Services` | Contener reglas de negocio, validaciones funcionales y operaciones principales. |
| `Interfaces` | Definir contratos para los servicios y facilitar inyeccion de dependencias. |
| `Models` | Representar entidades persistentes del dominio. |
| `Data` | Contener `DbContext` y configuracion de EF Core. |
| `Migrations` | Registrar migraciones de base de datos generadas por EF Core. |

Regla principal de arquitectura:

> Los controllers no deben tener logica de negocio. Toda regla relevante debe vivir en servicios.

## 5. Modelo de dominio

### 5.1 Categoria

Permite organizar productos.

Campos:

| Campo | Tipo sugerido | Reglas |
| --- | --- | --- |
| `Id` | `int` | Llave primaria. |
| `Nombre` | `string` | Obligatorio. No debe estar vacio. |
| `Descripcion` | `string?` | Opcional. |
| `FechaCreacion` | `DateTime` | Se asigna al crear. |
| `Estado` | `bool` | Indica si la categoria esta activa. |

Relacion:

- Una categoria tiene muchos productos.
- `Categoria 1 ---- N Productos`.

### 5.2 Producto

Permite administrar inventario.

Campos:

| Campo | Tipo sugerido | Reglas |
| --- | --- | --- |
| `Id` | `int` | Llave primaria. |
| `Nombre` | `string` | Obligatorio. |
| `Codigo` | `string` | Obligatorio y unico. |
| `Precio` | `decimal` | Debe ser mayor a 0. |
| `Stock` | `int` | No puede ser negativo. |
| `Estado` | `bool` | Indica si el producto esta activo. |
| `CategoriaId` | `int` | Llave foranea obligatoria. |
| `FechaCreacion` | `DateTime` | Se asigna al crear. |

Reglas:

- El codigo del producto debe ser unico.
- El precio debe ser mayor a 0.
- El stock no puede ser negativo.
- Todo producto debe pertenecer a una categoria existente.
- No se debe permitir vender productos inactivos.

Relacion:

- Un producto pertenece a una categoria.
- Un producto puede aparecer en muchos detalles de venta.

### 5.3 Cliente

Permite registrar compradores.

Campos:

| Campo | Tipo sugerido | Reglas |
| --- | --- | --- |
| `Id` | `int` | Llave primaria. |
| `NombreCompleto` | `string` | Obligatorio. |
| `Documento` | `string` | Obligatorio y unico. |
| `Email` | `string` | Debe tener formato valido. |
| `Telefono` | `string?` | Opcional. |
| `FechaRegistro` | `DateTime` | Se asigna al crear. |

Reglas:

- El documento debe ser unico.
- El email debe ser valido.
- El nombre completo es obligatorio.

Relacion:

- Un cliente puede tener muchas ventas.
- `Cliente 1 ---- N Ventas`.

### 5.4 Venta

Representa una compra realizada por un cliente.

Campos:

| Campo | Tipo sugerido | Reglas |
| --- | --- | --- |
| `Id` | `int` | Llave primaria. |
| `FechaVenta` | `DateTime` | Se asigna al registrar la venta. |
| `ClienteId` | `int` | Llave foranea obligatoria. |
| `Total` | `decimal` | Debe calcularse desde los detalles. |

Reglas:

- Debe existir el cliente.
- La venta debe tener al menos un detalle.
- El total no se recibe como dato confiable desde el cliente HTTP; debe calcularse en backend.

Relacion:

- Una venta pertenece a un cliente.
- Una venta tiene muchos detalles.
- `Venta 1 ---- N DetalleVenta`.

### 5.5 DetalleVenta

Representa los productos vendidos dentro de una venta.

Campos:

| Campo | Tipo sugerido | Reglas |
| --- | --- | --- |
| `Id` | `int` | Llave primaria. |
| `VentaId` | `int` | Llave foranea obligatoria. |
| `ProductoId` | `int` | Llave foranea obligatoria. |
| `Cantidad` | `int` | Debe ser mayor a 0. |
| `PrecioUnitario` | `decimal` | Debe guardar el precio del producto al momento de la venta. |
| `Subtotal` | `decimal` | `Cantidad * PrecioUnitario`. |

Reglas:

- La cantidad debe ser mayor a 0.
- El producto debe existir y estar activo.
- Debe existir stock suficiente.
- El subtotal debe ser calculado por backend.

Relaciones:

- `Venta 1 ---- N DetalleVenta`.
- `Producto 1 ---- N DetalleVenta`.

## 6. Reglas de negocio

### 6.1 Reglas generales

- No se deben aceptar datos obligatorios vacios.
- Los identificadores recibidos por parametro deben existir en base de datos.
- Las respuestas de error deben tener mensajes claros.
- La API debe responder con codigos HTTP coherentes.
- Las validaciones deben ocurrir antes de guardar cambios.
- La logica de negocio debe vivir en servicios.

### 6.2 Reglas de inventario

- El stock inicial de un producto no puede ser negativo.
- El precio de un producto debe ser mayor a cero.
- El codigo de producto debe ser unico.
- Al registrar una venta valida, el stock debe descontarse automaticamente.
- Si no hay stock suficiente, la venta no debe guardarse.

### 6.3 Reglas de ventas

- La venta debe procesarse como una unidad de trabajo.
- Si falla cualquier detalle, no se debe guardar la venta ni descontar stock parcialmente.
- El `PrecioUnitario` debe tomarse desde el producto en base de datos al momento de vender.
- El `Subtotal` de cada detalle debe calcularse en backend.
- El `Total` de la venta debe ser la suma de los subtotales.

Recomendacion tecnica:

- Usar una transaccion de base de datos al registrar ventas para evitar inconsistencias entre venta, detalle y stock.

### 6.4 Regla recomendada para eliminacion

Como `Categoria` y `Producto` tienen campo `Estado`, se recomienda aplicar eliminacion logica:

- `DELETE /api/categorias/{id}` cambia `Estado` a `false`.
- `DELETE /api/productos/{id}` cambia `Estado` a `false`.

Esto conserva historial y evita romper relaciones con ventas existentes.

## 7. Contratos REST minimos

### 7.1 Categorias

| Metodo | Endpoint | Descripcion | Respuestas esperadas |
| --- | --- | --- | --- |
| `POST` | `/api/categorias` | Crear categoria. | `201 Created`, `400 Bad Request` |
| `GET` | `/api/categorias` | Listar categorias. | `200 OK` |
| `GET` | `/api/categorias/{id}` | Consultar categoria por id. | `200 OK`, `404 Not Found` |
| `PUT` | `/api/categorias/{id}` | Actualizar categoria. | `200 OK`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/categorias/{id}` | Desactivar o eliminar categoria. | `200 OK`, `404 Not Found` |

DTO sugerido para crear categoria:

```json
{
  "nombre": "Bebidas",
  "descripcion": "Productos liquidos"
}
```

### 7.2 Productos

| Metodo | Endpoint | Descripcion | Respuestas esperadas |
| --- | --- | --- | --- |
| `POST` | `/api/productos` | Crear producto. | `201 Created`, `400 Bad Request` |
| `GET` | `/api/productos` | Listar productos. | `200 OK` |
| `GET` | `/api/productos/{id}` | Consultar producto por id. | `200 OK`, `404 Not Found` |
| `PUT` | `/api/productos/{id}` | Actualizar producto. | `200 OK`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/productos/{id}` | Desactivar o eliminar producto. | `200 OK`, `404 Not Found` |

DTO sugerido para crear producto:

```json
{
  "nombre": "Cafe premium",
  "codigo": "CAF-001",
  "precio": 12500,
  "stock": 40,
  "categoriaId": 1
}
```

La respuesta de productos debe incluir informacion de categoria.

### 7.3 Clientes

| Metodo | Endpoint | Descripcion | Respuestas esperadas |
| --- | --- | --- | --- |
| `POST` | `/api/clientes` | Crear cliente. | `201 Created`, `400 Bad Request` |
| `GET` | `/api/clientes` | Listar clientes. | `200 OK` |

DTO sugerido para crear cliente:

```json
{
  "nombreCompleto": "Maria Perez",
  "documento": "123456789",
  "email": "maria.perez@example.com",
  "telefono": "3001234567"
}
```

### 7.4 Ventas

| Metodo | Endpoint | Descripcion | Respuestas esperadas |
| --- | --- | --- | --- |
| `POST` | `/api/ventas` | Registrar venta. | `201 Created`, `400 Bad Request`, `404 Not Found` |
| `GET` | `/api/ventas` | Listar ventas. | `200 OK` |
| `GET` | `/api/ventas/{id}` | Consultar venta por id. | `200 OK`, `404 Not Found` |

DTO sugerido para registrar venta:

```json
{
  "clienteId": 1,
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2
    }
  ]
}
```

Respuesta sugerida:

```json
{
  "id": 10,
  "fechaVenta": "2026-08-22T00:00:00",
  "clienteId": 1,
  "total": 25000,
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2,
      "precioUnitario": 12500,
      "subtotal": 25000
    }
  ]
}
```

## 8. Formato de respuestas

### 8.1 Respuesta exitosa de creacion

```json
{
  "id": 1,
  "mensaje": "Recurso creado correctamente"
}
```

### 8.2 Respuesta de error

```json
{
  "mensaje": "El precio debe ser mayor a cero"
}
```

### 8.3 Respuesta de validacion con multiples errores

```json
{
  "mensaje": "La solicitud contiene datos invalidos",
  "errores": [
    "El nombre es obligatorio",
    "El precio debe ser mayor a cero"
  ]
}
```

## 9. Validaciones esperadas

| Caso | Resultado esperado |
| --- | --- |
| Crear categoria sin nombre. | `400 Bad Request`. |
| Crear producto con precio menor o igual a 0. | `400 Bad Request`. |
| Crear producto con stock negativo. | `400 Bad Request`. |
| Crear producto con codigo repetido. | `400 Bad Request`. |
| Crear producto con categoria inexistente. | `400 Bad Request` o `404 Not Found`. |
| Crear cliente sin nombre. | `400 Bad Request`. |
| Crear cliente con documento repetido. | `400 Bad Request`. |
| Crear cliente con email invalido. | `400 Bad Request`. |
| Registrar venta con cliente inexistente. | `404 Not Found`. |
| Registrar venta sin detalles. | `400 Bad Request`. |
| Registrar venta con producto inexistente. | `404 Not Found`. |
| Registrar venta sin stock suficiente. | `400 Bad Request`. |
| Registrar venta valida. | `201 Created` y descuento de stock. |

## 10. Configuracion de Entity Framework Core

El `DbContext` debe incluir:

```csharp
public DbSet<Categoria> Categorias { get; set; }
public DbSet<Producto> Productos { get; set; }
public DbSet<Cliente> Clientes { get; set; }
public DbSet<Venta> Ventas { get; set; }
public DbSet<DetalleVenta> DetalleVentas { get; set; }
```

Configuraciones recomendadas:

- Indice unico para `Producto.Codigo`.
- Indice unico para `Cliente.Documento`.
- Precision decimal para precios, subtotales y totales, por ejemplo `decimal(18,2)`.
- Relacion obligatoria entre `Producto` y `Categoria`.
- Relacion obligatoria entre `Venta` y `Cliente`.
- Relacion obligatoria entre `DetalleVenta` y `Venta`.
- Relacion obligatoria entre `DetalleVenta` y `Producto`.

## 11. Historias de usuario

### HU01 - Crear categoria

Como administrador quiero crear categorias para clasificar productos.

Criterios:

- Debe permitir registrar nombre y descripcion.
- No debe permitir nombres vacios.
- Debe devolver `201 Created` al crear.
- Debe guardar informacion en base de datos.

### HU02 - Crear producto

Como administrador quiero registrar productos.

Criterios:

- Debe validar campos obligatorios.
- Debe validar precio positivo.
- Debe validar categoria existente.
- Debe devolver error si el codigo ya existe.

### HU03 - Consultar productos

Como usuario quiero consultar productos disponibles.

Criterios:

- Debe retornar lista de productos.
- Debe incluir informacion de categoria.
- Debe responder `200 OK`.
- Debe manejar lista vacia correctamente.

### HU04 - Registrar venta

Como vendedor quiero registrar una venta.

Criterios:

- Debe validar que exista el cliente.
- Debe validar disponibilidad del producto.
- Debe descontar stock automaticamente.
- Debe calcular total.
- Debe guardar detalle de venta.

## 12. Distribucion del trabajo

### Persona 1 - Experiencia .NET

Responsabilidades:

- Configuracion inicial del proyecto.
- Inyeccion de dependencias.
- Entity Framework Core.
- `DbContext`.
- Migraciones.
- Buenas practicas C#.

### Persona 2 - Backend JavaScript/Express

Responsabilidades:

- Controllers.
- Endpoints REST.
- DTOs.
- Validaciones.
- Manejo de respuestas HTTP.

### Persona 3 - Java/Python/TypeScript

Responsabilidades:

- Arquitectura.
- Servicios.
- Logica de negocio.
- Refactorizacion.
- Integracion general.

## 13. Plan de implementacion sugerido

1. Crear solucion y proyecto `InventarioVentas.API`.
2. Instalar paquetes necesarios de EF Core, SQL Server, Swagger y FluentValidation.
3. Crear modelos de dominio.
4. Configurar `ApplicationDbContext`.
5. Configurar relaciones, indices unicos y precision decimal.
6. Crear DTOs de entrada y salida.
7. Crear interfaces de servicios.
8. Implementar servicios con reglas de negocio.
9. Implementar validators.
10. Implementar controllers.
11. Configurar inyeccion de dependencias.
12. Crear migracion inicial.
13. Aplicar migracion a SQL Server.
14. Probar endpoints desde Swagger.
15. Corregir validaciones, respuestas HTTP y casos borde.
16. Documentar decisiones relevantes en el repositorio.

## 14. Criterios de aceptacion final

### Codigo

- La aplicacion compila sin errores.
- La solucion esta organizada por responsabilidades.
- No existe logica de negocio dentro de los controllers.
- Se utiliza inyeccion de dependencias.
- Los servicios encapsulan las reglas funcionales.

### Base de datos

- La base de datos esta normalizada minimo hasta tercera forma normal.
- Existen relaciones correctamente configuradas.
- Existen indices unicos para codigo de producto y documento de cliente.
- Las migraciones crean la base de datos desde cero.

### API REST

- Todos los endpoints minimos funcionan desde Swagger.
- Se utilizan correctamente los metodos `GET`, `POST`, `PUT` y `DELETE`.
- Se manejan codigos HTTP correctos:
  - `200 OK`.
  - `201 Created`.
  - `400 Bad Request`.
  - `404 Not Found`.

### Validaciones

- No permite datos invalidos.
- Devuelve mensajes claros de error.
- Valida reglas de negocio antes de guardar cambios.
- No permite ventas con stock insuficiente.

### Entity Framework Core

- Usa `DbContext`.
- Usa migraciones.
- Configura relaciones entre entidades.
- Evita inconsistencias al registrar ventas.

## 15. Casos de prueba recomendados

| Escenario | Resultado esperado |
| --- | --- |
| Crear una categoria valida. | Categoria guardada y respuesta `201 Created`. |
| Crear una categoria sin nombre. | Error `400 Bad Request`. |
| Crear un producto valido. | Producto guardado y respuesta `201 Created`. |
| Crear un producto con codigo repetido. | Error claro de duplicidad. |
| Consultar productos sin registros. | Lista vacia con `200 OK`. |
| Consultar productos con categoria. | Cada producto incluye datos de categoria. |
| Crear cliente valido. | Cliente guardado y respuesta `201 Created`. |
| Crear cliente con email invalido. | Error `400 Bad Request`. |
| Registrar venta valida. | Venta y detalles guardados; stock descontado. |
| Registrar venta con stock insuficiente. | Error y ningun cambio parcial en base de datos. |
| Registrar venta con cliente inexistente. | Error `404 Not Found`. |

## 16. Riesgos y decisiones pendientes

Riesgos:

- Descontar stock sin transaccion puede dejar datos inconsistentes.
- Permitir eliminacion fisica de productos con ventas historicas puede romper integridad o trazabilidad.
- Recibir totales desde el cliente HTTP puede permitir manipulacion de precios.
- Duplicar logica entre controllers y services dificulta mantenimiento.

Decisiones recomendadas:

- Usar eliminacion logica para categorias y productos mediante `Estado`.
- Calcular precios, subtotales y totales siempre en backend.
- Usar transacciones al registrar ventas.
- Mantener DTOs separados de entidades.
- Usar FluentValidation para validaciones de entrada.

Preguntas abiertas:

- El campo `Estado` sera `bool` o un enum?
- Se permitira editar precio y stock de productos despues de tener ventas?
- Las ventas podran anularse en una version futura?
- Los listados deben mostrar solo registros activos o tambien inactivos?
- Se agregara autenticacion en una fase posterior?

## 17. Definicion de terminado

El proyecto se considera terminado cuando:

- La API compila y ejecuta correctamente.
- La base de datos se crea desde migraciones.
- Swagger permite probar todos los endpoints minimos.
- Los modulos de categorias, productos, clientes y ventas funcionan.
- Las ventas descuentan stock correctamente.
- Las validaciones impiden datos invalidos.
- Los errores devuelven mensajes claros.
- El codigo respeta la arquitectura de monolito modular y sus responsabilidades internas.
- Las decisiones relevantes quedan documentadas.
