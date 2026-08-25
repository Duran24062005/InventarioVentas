Para este proyecto, y teniendo en cuenta que **no quieren complicarlo demasiado**, yo cambiaría la estructura inicial por una **arquitectura de monolito modular organizada por funcionalidades**.

La idea clave sería:

**Una sola API + una sola base de datos + un solo despliegue**, pero el código queda dividido en módulos independientes: Categorías, Productos, Clientes y Ventas.

## 1. Estructura general

```text
InventarioVentas/
│
├── InventarioVentas.sln
│
└── src/
    └── InventarioVentas.API/
        │
        ├── Modules/
        │   │
        │   ├── Categorias/
        │   │   ├── Controllers/
        │   │   │   └── CategoriesController.cs
        │   │   │
        │   │   ├── DTOs/
        │   │   │   ├── CreateCategoryDto.cs
        │   │   │   ├── UpdateCategoryDto.cs
        │   │   │   └── CategoriaResponseDto.cs
        │   │   │
        │   │   ├── Models/
        │   │   │   └── Categoria.cs
        │   │   │
        │   │   ├── Interfaces/
        │   │   │   └── ICategoriaService.cs
        │   │   │
        │   │   ├── Services/
        │   │   │   └── CategoriaService.cs
        │   │   │
        │   │   └── Validators/
        │   │       └── CreateCategoryValidator.cs
        │   │
        │   ├── Productos/
        │   │   ├── Controllers/
        │   │   │   └── ProductosController.cs
        │   │   │
        │   │   ├── DTOs/
        │   │   │   ├── CreateProductDto.cs
        │   │   │   ├── ActualizarProductoDto.cs
        │   │   │   └── ProductResponseDto.cs
        │   │   │
        │   │   ├── Models/
        │   │   │   └── Producto.cs
        │   │   │
        │   │   ├── Interfaces/
        │   │   │   └── IProductService.cs
        │   │   │
        │   │   ├── Services/
        │   │   │   └── ProductService.cs
        │   │   │
        │   │   └── Validators/
        │   │       └── CreateProductValidator.cs
        │   │
        │   ├── Clientes/
        │   │   ├── Controllers/
        │   │   │   └── ClientesController.cs
        │   │   │
        │   │   ├── DTOs/
        │   │   │   ├── CrearClienteDto.cs
        │   │   │   └── ClienteResponseDto.cs
        │   │   │
        │   │   ├── Models/
        │   │   │   └── Cliente.cs
        │   │   │
        │   │   ├── Interfaces/
        │   │   │   └── IClienteService.cs
        │   │   │
        │   │   ├── Services/
        │   │   │   └── ClienteService.cs
        │   │   │
        │   │   └── Validators/
        │   │       └── CrearClienteValidator.cs
        │   │
        │   └── Ventas/
        │       ├── Controllers/
        │       │   └── VentasController.cs
        │       │
        │       ├── DTOs/
        │       │   ├── CrearVentaDto.cs
        │       │   ├── CrearDetalleVentaDto.cs
        │       │   ├── VentaResponseDto.cs
        │       │   └── DetalleVentaResponseDto.cs
        │       │
        │       ├── Models/
        │       │   ├── Venta.cs
        │       │   └── DetalleVenta.cs
        │       │
        │       ├── Interfaces/
        │       │   └── IVentaService.cs
        │       │
        │       ├── Services/
        │       │   └── VentaService.cs
        │       │
        │       └── Validators/
        │           └── CrearVentaValidator.cs
        │
        ├── Data/
        │   ├── AppDbContext.cs
        │   │
        │   └── Configurations/
        │       ├── CategoriaConfiguration.cs
        │       ├── ProductoConfiguration.cs
        │       ├── ClienteConfiguration.cs
        │       ├── VentaConfiguration.cs
        │       └── DetalleVentaConfiguration.cs
        │
        ├── Common/
        │   ├── Exceptions/
        │   │   ├── NotFoundException.cs
        │   │   ├── BusinessException.cs
        │   │   └── ValidationException.cs
        │   │
        │   └── Responses/
        │       └── ApiResponse.cs
        │
        ├── Middleware/
        │   └── ExceptionMiddleware.cs
        │
        ├── Migrations/
        │
        ├── Extensions/
        │   └── DependencyInjection.cs
        │
        ├── Program.cs
        ├── appsettings.json
        └── appsettings.Development.json
```

Esta sería mi recomendación concreta para **su proyecto actual**.

---

# 2. ¿Por qué esto es un monolito modular?

Porque todo sigue estando dentro de una sola aplicación:

```text
InventarioVentas.API
```

No tenemos:

```text
Categoria.API
Producto.API
Cliente.API
Venta.API
```

Eso ya empezaría a parecer una arquitectura de servicios separados.

Aquí tenemos:

```text
                ┌────────────────────────┐
                │ InventarioVentas.API   │
                │                        │
                │  ┌──────────────────┐  │
                │  │ Categorías       │  │
                │  └──────────────────┘  │
                │                        │
                │  ┌──────────────────┐  │
                │  │ Productos        │  │
                │  └──────────────────┘  │
                │                        │
Cliente HTTP ──►│  ┌──────────────────┐  │
                │  │ Clientes         │  │
                │  └──────────────────┘  │
                │                        │
                │  ┌──────────────────┐  │
                │  │ Ventas           │  │
                │  └──────────────────┘  │
                │                        │
                └───────────┬────────────┘
                            │
                            ▼
                    ┌───────────────┐
                    │ PostgreSQL    │
                    └───────────────┘
```

Es decir:

**Monolito** → una aplicación.

**Modular** → cada funcionalidad importante tiene su propio espacio y responsabilidades.

---

# 3. La diferencia con la estructura original

Ustedes inicialmente tenían:

```text
Inventario.API
├── Controllers
├── DTOs
├── Services
├── Interfaces
├── Models
├── Data
└── Migrations
```

Esto funciona, pero cuando el proyecto crece empieza a quedar así:

```text
Controllers/
├── CategoriesController.cs
├── ProductosController.cs
├── ClientesController.cs
└── VentasController.cs

Services/
├── CategoriaService.cs
├── ProductService.cs
├── ClienteService.cs
└── VentaService.cs

DTOs/
├── CategoriaDto.cs
├── ProductoDto.cs
├── ClienteDto.cs
├── VentaDto.cs
├── DetalleVentaDto.cs
...
```

El problema es que para trabajar en **Productos** tienes que saltar entre cinco carpetas diferentes.

En el monolito modular:

```text
Modules/
└── Productos/
    ├── Controllers/
    ├── DTOs/
    ├── Interfaces/
    ├── Models/
    ├── Services/
    └── Validators/
```

Todo lo relacionado con producto está junto.

Para un equipo de tres personas esto les va a facilitar bastante el trabajo.

---

# 4. Módulo Categorías

Quedaría:

```text
Modules/
└── Categorias/
    ├── Controllers/
    │   └── CategoriesController.cs
    │
    ├── DTOs/
    │   ├── CreateCategoryDto.cs
    │   ├── UpdateCategoryDto.cs
    │   └── CategoriaResponseDto.cs
    │
    ├── Models/
    │   └── Categoria.cs
    │
    ├── Interfaces/
    │   └── ICategoriaService.cs
    │
    ├── Services/
    │   └── CategoriaService.cs
    │
    └── Validators/
        └── CreateCategoryValidator.cs
```

La entidad:

```text
Categoria
──────────────
Id
Nombre
Descripcion
FechaCreacion
Estado
```

Y la relación:

```text
Categoria
    │
    │ 1
    │
    │
    │ N
    ▼
Producto
```

Los endpoints:

```http
POST   /api/categorias
GET    /api/categorias
GET    /api/categorias/{id}
PUT    /api/categorias/{id}
DELETE /api/categorias/{id}
```

---

# 5. Módulo Productos

```text
Modules/
└── Productos/
    ├── Controllers/
    │   └── ProductosController.cs
    │
    ├── DTOs/
    │   ├── CreateProductDto.cs
    │   ├── ActualizarProductoDto.cs
    │   └── ProductResponseDto.cs
    │
    ├── Models/
    │   └── Producto.cs
    │
    ├── Interfaces/
    │   └── IProductService.cs
    │
    ├── Services/
    │   └── ProductService.cs
    │
    └── Validators/
        └── CreateProductValidator.cs
```

Entidad:

```text
Producto
────────────────
Id
Nombre
Codigo
Precio
Stock
Estado
CategoriaId
FechaCreacion
```

Las validaciones se podrían dividir en dos tipos.

FluentValidation valida:

```text
Nombre obligatorio
Código obligatorio
Precio > 0
Stock >= 0
CategoriaId obligatorio
```

Mientras que `ProductService` valida cosas que necesitan consultar la base de datos:

```text
¿Existe la categoría?

¿Ya existe ese código?
```

Eso es importante.

No metería esas consultas dentro del validator si están comenzando con .NET.

Manténganlo sencillo.

---

# 6. Módulo Clientes

```text
Modules/
└── Clientes/
    ├── Controllers/
    │   └── ClientesController.cs
    │
    ├── DTOs/
    │   ├── CrearClienteDto.cs
    │   └── ClienteResponseDto.cs
    │
    ├── Models/
    │   └── Cliente.cs
    │
    ├── Interfaces/
    │   └── IClienteService.cs
    │
    ├── Services/
    │   └── ClienteService.cs
    │
    └── Validators/
        └── CrearClienteValidator.cs
```

Entidad:

```text
Cliente
────────────────
Id
NombreCompleto
Documento
Email
Telefono
FechaRegistro
```

Aquí `ClienteService` verifica:

```text
Documento no repetido
```

Y FluentValidation:

```text
Nombre obligatorio
Email válido
Documento obligatorio
```

---

# 7. Módulo Ventas

Aquí estaría la lógica más importante de todo el proyecto.

Yo pondría **Venta y DetalleVenta dentro del mismo módulo**, no crearía dos módulos distintos.

```text
Modules/
└── Ventas/
    ├── Controllers/
    │   └── VentasController.cs
    │
    ├── DTOs/
    │   ├── CrearVentaDto.cs
    │   ├── CrearDetalleVentaDto.cs
    │   ├── VentaResponseDto.cs
    │   └── DetalleVentaResponseDto.cs
    │
    ├── Models/
    │   ├── Venta.cs
    │   └── DetalleVenta.cs
    │
    ├── Interfaces/
    │   └── IVentaService.cs
    │
    ├── Services/
    │   └── VentaService.cs
    │
    └── Validators/
        └── CrearVentaValidator.cs
```

¿Por qué juntos?

Porque `DetalleVenta` realmente no tiene sentido solo.

Normalmente no quieres hacer:

```http
POST /api/detalleventa
```

La operación real es:

```http
POST /api/ventas
```

Y dentro de la venta envías sus productos.

Por ejemplo:

```json
{
  "clienteId": 10,
  "productos": [
    {
      "productoId": 5,
      "cantidad": 2
    },
    {
      "productoId": 8,
      "cantidad": 1
    }
  ]
}
```

---

# 8. Flujo para registrar una venta

Este es probablemente el flujo de negocio más importante que deberían implementar.

```text
POST /api/ventas
        │
        ▼
VentasController
        │
        ▼
IVentaService
        │
        ▼
VentaService
        │
        ├── 1. Buscar cliente
        │
        ├── 2. Validar que exista
        │
        ├── 3. Buscar productos
        │
        ├── 4. Validar existencia
        │
        ├── 5. Validar stock
        │
        ├── 6. Obtener precio
        │
        ├── 7. Calcular subtotales
        │
        ├── 8. Calcular total
        │
        ├── 9. Descontar stock
        │
        ├── 10. Crear Venta
        │
        ├── 11. Crear DetalleVenta
        │
        └── 12. SaveChangesAsync()
                    │
                    ▼
               PostgreSQL
```

Aquí es donde se cumple el criterio:

> No existe lógica de negocio dentro de los Controllers.

El `VentasController` **no calcula el total**.

El `VentasController` **no descuenta inventario**.

El `VentasController` **no consulta manualmente productos**.

Todo eso corresponde a:

```text
VentaService
```

---

# 9. Responsabilidad de cada parte

La separación quedaría conceptualmente así:

```text
HTTP
 │
 ▼
Controller
 │
 ▼
DTO
 │
 ▼
Service
 │
 ▼
DbContext
 │
 ▼
PostgreSQL
```

### Controller

Se encarga de HTTP.

Por ejemplo:

```text
Recibe POST
       ↓
Llama al servicio
       ↓
Devuelve 201 Created
```

Debe ser pequeño.

### DTO

Representa lo que entra o sale de la API.

Por ejemplo:

```text
CreateProductDto
```

podría contener:

```text
Nombre
Codigo
Precio
Stock
CategoriaId
```

No necesita recibir:

```text
Id
FechaCreacion
```

porque los genera el sistema.

### Service

Aquí está la lógica de negocio.

Por ejemplo:

```text
ProductService
```

realiza:

```text
Validar código único
Validar categoría
Crear producto
Consultar productos
Actualizar producto
Eliminar/desactivar producto
```

### Interface

Define el contrato.

```text
IProductService
```

Por ejemplo conceptualmente:

```csharp
CrearAsync(...)
ObtenerTodosAsync()
ObtenerPorIdAsync(...)
ActualizarAsync(...)
EliminarAsync(...)
```

Y después:

```text
ProductService
```

implementa esa interfaz.

### Model

Representa la entidad de base de datos:

```text
Producto
Categoria
Cliente
Venta
DetalleVenta
```

### Validator

Se encarga principalmente de validar los DTO.

Por ejemplo:

```text
CreateProductValidator

Precio > 0
Stock >= 0
Nombre obligatorio
Código obligatorio
```

---

# 10. El DbContext

No crearía un DbContext por módulo para este ejercicio.

Eso agregaría complejidad innecesaria.

Usaría:

```text
Data/
└── AppDbContext.cs
```

Y dentro:

```csharp
DbSet<Categoria>
DbSet<Producto>
DbSet<Cliente>
DbSet<Venta>
DbSet<DetalleVenta>
```

Conceptualmente:

```text
AppDbContext
│
├── Categorias
├── Productos
├── Clientes
├── Ventas
└── DetallesVenta
        │
        ▼
    PostgreSQL
```

---

# 11. Relaciones de toda la base de datos

Su modelo queda bastante limpio:

```text
┌───────────────┐
│   Categoria   │
├───────────────┤
│ Id            │
│ Nombre        │
│ Descripcion   │
│ FechaCreacion │
│ Estado        │
└───────┬───────┘
        │
        │ 1:N
        ▼
┌───────────────┐
│   Producto    │
├───────────────┤
│ Id            │
│ Nombre        │
│ Codigo        │
│ Precio        │
│ Stock         │
│ Estado        │
│ CategoriaId   │
│ FechaCreacion │
└───────┬───────┘
        │
        │ 1:N
        ▼
┌───────────────────┐
│   DetalleVenta    │
├───────────────────┤
│ Id                │
│ VentaId           │
│ ProductoId        │
│ Cantidad          │
│ PrecioUnitario    │
│ Subtotal          │
└────────┬──────────┘
         │
         │ N:1
         ▼
┌────────────────┐
│     Venta      │
├────────────────┤
│ Id             │
│ FechaVenta     │
│ ClienteId      │
│ Total          │
└───────┬────────┘
        │
        │ N:1
        ▼
┌────────────────┐
│    Cliente     │
├────────────────┤
│ Id             │
│ NombreCompleto │
│ Documento      │
│ Email          │
│ Telefono       │
│ FechaRegistro  │
└────────────────┘
```

Eso representa:

```text
Categoria 1 ─────── N Producto

Producto  1 ─────── N DetalleVenta

Venta     1 ─────── N DetalleVenta

Cliente   1 ─────── N Venta
```

---

# 12. Configuración de Entity Framework

Además del `AppDbContext`, recomiendo poner las configuraciones aquí:

```text
Data/
└── Configurations/
    ├── CategoriaConfiguration.cs
    ├── ProductoConfiguration.cs
    ├── ClienteConfiguration.cs
    ├── VentaConfiguration.cs
    └── DetalleVentaConfiguration.cs
```

Por ejemplo `ProductoConfiguration` sería responsable de cosas como:

```text
Codigo → UNIQUE

Precio → decimal(18,2)

CategoriaId → Foreign Key

Categoria → 1:N Productos
```

Esto evita llenar el `AppDbContext` de configuraciones.

---

# 13. ¿Necesitan Repository?

Para **este proyecto**, yo diría:

**No.**

No agregaría:

```text
IProductoRepository
ProductoRepository
IVentaRepository
VentaRepository
IClienteRepository
ClienteRepository
...
```

Porque terminarían con:

```text
Controller
   ↓
Service
   ↓
Repository
   ↓
DbContext
   ↓
PostgreSQL
```

sin obtener un beneficio importante para un proyecto pequeño.

Con EF Core pueden trabajar perfectamente así:

```text
Controller
     ↓
Service
     ↓
AppDbContext
     ↓
PostgreSQL
```

Eso les permite aprender mejor:

- ASP.NET Core
- Inyección de dependencias
- EF Core
- LINQ
- Async/await
- DTOs
- servicios
- validaciones
- relaciones

sin meter patrones solamente por meterlos.

---

# 14. Inyección de dependencias

En:

```text
Extensions/
└── DependencyInjection.cs
```

pueden registrar:

```text
ICategoriaService → CategoriaService

IProductService → ProductService

IClienteService → ClienteService

IVentaService → VentaService
```

Y luego los Controllers reciben solamente sus servicios.

Por ejemplo:

```text
ProductosController
        │
        ▼
IProductService
        │
        ▼
ProductService
```

Esto cumple perfectamente el criterio:

> ✅ Se utiliza inyección de dependencias.

---

# 15. Manejo de errores

Para evitar tener `try/catch` en absolutamente todos los Controllers:

```text
Common/
└── Exceptions/
    ├── NotFoundException.cs
    ├── BusinessException.cs
    └── ValidationException.cs
```

más:

```text
Middleware/
└── ExceptionMiddleware.cs
```

Entonces, por ejemplo, si alguien intenta registrar:

```json
{
  "nombre": "Monitor",
  "precio": -500
}
```

pueden devolver:

```json
{
  "mensaje": "El precio debe ser mayor a cero"
}
```

con:

```http
400 Bad Request
```

Si buscan:

```http
GET /api/productos/9999
```

y no existe:

```json
{
  "mensaje": "Producto no encontrado"
}
```

con:

```http
404 Not Found
```

---

# 16. Dependencias entre módulos

Aquí hay algo importante para entender un monolito modular.

Los módulos **sí pueden relacionarse**, porque el negocio lo necesita.

Por ejemplo:

```text
Ventas
  │
  ├──── necesita Cliente
  │
  └──── necesita Producto
```

porque para registrar una venta necesitamos saber:

```text
¿Existe el cliente?

¿Existe el producto?

¿Tiene suficiente stock?
```

Mientras:

```text
Productos
   │
   └──── necesita Categoria
```

porque todo producto debe pertenecer a una categoría.

Visualmente:

```text
┌──────────────┐
│  Categorias  │
└──────┬───────┘
       │
       ▼
┌──────────────┐
│  Productos   │◄───────────┐
└──────────────┘            │
                            │
                       ┌────┴─────┐
                       │  Ventas  │
                       └────┬─────┘
                            │
                            ▼
                       ┌──────────┐
                       │ Clientes │
                       └──────────┘
```

No tienen que intentar aislarlos como si fueran microservicios.

---

# 17. Cómo repartiría el proyecto entre los tres

Con la distribución que ya definieron, esta arquitectura les cae bastante bien.

| Persona | Responsabilidad |
|---|---|
| Persona con experiencia .NET | Configuración del proyecto, EF Core, DbContext, relaciones, migraciones, DI |
| Persona JavaScript/Express | Controllers, endpoints, DTOs, FluentValidation, HTTP |
| Tú | Services, reglas de negocio, arquitectura modular, integración y refactorización |

Pero les recomiendo **no dividirlo como "yo nunca toco Controllers y tú nunca tocas Services"**.

La división sirve para tener responsables, pero todos deberían entender el flujo:

```text
Request
   ↓
Controller
   ↓
DTO
   ↓
Validator
   ↓
Service
   ↓
DbContext
   ↓
PostgreSQL
```

Porque ese flujo es justamente lo que probablemente les van a evaluar.

---

# 18. Estructura final que yo usaría

Si mañana tuviera que iniciar este proyecto con ustedes, usaría exactamente esta:

```text
InventarioVentas.API
│
├── Modules
│   │
│   ├── Categorias
│   │   ├── Controllers
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   ├── Models
│   │   ├── Services
│   │   └── Validators
│   │
│   ├── Productos
│   │   ├── Controllers
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   ├── Models
│   │   ├── Services
│   │   └── Validators
│   │
│   ├── Clientes
│   │   ├── Controllers
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   ├── Models
│   │   ├── Services
│   │   └── Validators
│   │
│   └── Ventas
│       ├── Controllers
│       ├── DTOs
│       ├── Interfaces
│       ├── Models
│       ├── Services
│       └── Validators
│
├── Common
│   ├── Exceptions
│   └── Responses
│
├── Data
│   ├── Configurations
│   └── AppDbContext.cs
│
├── Middleware
│   └── ExceptionMiddleware.cs
│
├── Extensions
│   └── DependencyInjection.cs
│
├── Migrations
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

Y **no agregaría Clean Architecture, CQRS, MediatR, Unit of Work, Repository Pattern ni varios proyectos `.csproj` todavía**. Para el objetivo que tienen, eso les puede hacer gastar más tiempo entendiendo infraestructura que aprendiendo ASP.NET Core.

El siguiente paso natural sería definir **qué debe contener cada archivo** (`Categoria.cs`, `CreateProductDto.cs`, `IProductService.cs`, `ProductService.cs`, `ProductosController.cs`, `AppDbContext.cs`, etc.) y con eso ya tendrían prácticamente el esqueleto completo para empezar a programar.

## Composición técnica implementada

La base de la API se compone en `Program.cs` mediante `AddControllers`, `AddEndpointsApiExplorer` y `AddSwaggerGen`. El pipeline aplica `UseHttpsRedirection` y `MapControllers`.

Swashbuckle es la estrategia única para documentar OpenAPI en esta etapa. Swagger UI y `/swagger/v1/swagger.json` se habilitan únicamente en Development; no se combinan con `AddOpenApi` ni con `Microsoft.AspNetCore.OpenApi`.

La API ya no expone el endpoint de ejemplo `weatherforecast`. Mientras no existan controllers funcionales, Swagger mostrará únicamente la especificación base de la aplicación.

## Empaquetado local con Docker

La API puede empaquetarse en una imagen multi-stage con `Dockerfile` y ejecutarse mediante el servicio `api` de `docker-compose.yml`. Esto conserva el modelo de una sola API y un solo despliegue lógico; Docker solo cambia la forma de ejecutar el proceso.

La configuración actual es de desarrollo y agrega PostgreSQL mediante Compose; el esquema completo y las migraciones todavía pertenecen a PRD-003 y PRD-004. Los detalles de puertos, ambiente y seguridad están en [`docs/Docker.md`](Docker.md).
