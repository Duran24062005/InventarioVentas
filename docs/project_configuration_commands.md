Sí. Para el monolito modular que definimos, yo arrancaría así en **.NET 10**.

Primero verifica que tengas instalado el SDK:

```bash
dotnet --version
```

Idealmente debes tener disponible .NET 10. También puedes revisar todos los SDK:

```bash
dotnet --list-sdks
```

### 1. Crear carpeta, solución y proyecto

```bash
mkdir InventarioVentas
cd InventarioVentas

dotnet new sln -n InventarioVentas

mkdir src

dotnet new webapi -n InventarioVentas.API -o src/InventarioVentas.API --framework net10.0

dotnet sln add src/InventarioVentas.API/InventarioVentas.API.csproj
```

Después entra al proyecto:

```bash
cd src/InventarioVentas.API
```

### 2. Instalar Entity Framework Core para PostgreSQL

Como el proyecto será .NET 10, mantendría EF Core en la rama **10.x**. La versión usada por el proyecto es 10.0.11.

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 10.0.3

dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.11

dotnet add package Microsoft.EntityFrameworkCore.Tools --version 10.0.11
```

### 3. Instalar herramienta de migraciones

```bash
dotnet tool install --global dotnet-ef --version 10.0.11
```

Comprueba:

```bash
dotnet ef --version
```

Si ya la tenías instalada y quieres actualizarla:

```bash
dotnet tool update --global dotnet-ef --version 10.0.11
```

### 4. Instalar y configurar Swagger

```bash
dotnet add package Swashbuckle.AspNetCore
```

La API usa Swashbuckle como estrategia unica de documentacion OpenAPI. No es necesario agregar `Microsoft.AspNetCore.OpenApi` ni configurar simultaneamente `AddOpenApi`; mantener ambas estrategias duplicaria la composicion de documentacion.

### 5. FluentValidation

```bash
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions
```

Yo usaría estos paquetes y **no complicaría todavía el proyecto con AutoMapper**. Podemos hacer los mapeos DTO ↔ Entity manualmente para que entiendan qué está pasando.

### 6. Crear estructura del monolito modular

Desde:

```text
InventarioVentas/src/InventarioVentas.API
```

creamos primero los módulos:

```bash
mkdir Modules
mkdir Modules/Categories
mkdir Modules/Products
mkdir Modules/Customers
mkdir Modules/Sales
```

Categorías:

```bash
mkdir Modules/Categories/Controllers
mkdir Modules/Categories/DTOs
mkdir Modules/Categories/Interfaces
mkdir Modules/Categories/Models
mkdir Modules/Categories/Services
mkdir Modules/Categories/Validators
```

Productos:

```bash
mkdir Modules/Products/Controllers
mkdir Modules/Products/DTOs
mkdir Modules/Products/Interfaces
mkdir Modules/Products/Models
mkdir Modules/Products/Services
mkdir Modules/Products/Validators
```

Clientes:

```bash
mkdir Modules/Customers/Controllers
mkdir Modules/Customers/DTOs
mkdir Modules/Customers/Interfaces
mkdir Modules/Customers/Models
mkdir Modules/Customers/Services
mkdir Modules/Customers/Validators
```

Ventas:

```bash
mkdir Modules/Sales/Controllers
mkdir Modules/Sales/DTOs
mkdir Modules/Sales/Interfaces
mkdir Modules/Sales/Models
mkdir Modules/Sales/Services
mkdir Modules/Sales/Validators
```

Ahora las carpetas compartidas:

```bash
mkdir Data
mkdir Data/Configurations

mkdir Common
mkdir Common/Exceptions
mkdir Common/Responses

mkdir Middleware

mkdir Extensions
```

Al final deberías tener aproximadamente:

```text
InventarioVentas/
│
├── InventarioVentas.sln
│
└── src/
    └── InventarioVentas.API/
        │
        ├── Modules/
        │   ├── Categorias/
        │   │   ├── Controllers/
        │   │   ├── DTOs/
        │   │   ├── Interfaces/
        │   │   ├── Models/
        │   │   ├── Services/
        │   │   └── Validators/
        │   │
        │   ├── Productos/
        │   │   ├── Controllers/
        │   │   ├── DTOs/
        │   │   ├── Interfaces/
        │   │   ├── Models/
        │   │   ├── Services/
        │   │   └── Validators/
        │   │
        │   ├── Clientes/
        │   │   ├── Controllers/
        │   │   ├── DTOs/
        │   │   ├── Interfaces/
        │   │   ├── Models/
        │   │   ├── Services/
        │   │   └── Validators/
        │   │
        │   └── Ventas/
        │       ├── Controllers/
        │       ├── DTOs/
        │       ├── Interfaces/
        │       ├── Models/
        │       ├── Services/
        │       └── Validators/
        │
        ├── Common/
        ├── Data/
        ├── Extensions/
        ├── Middleware/
        ├── Program.cs
        └── appsettings.json
```

### 7. Probar que el proyecto compile

Desde la raíz del repositorio, la solución y el proyecto se validan así:

```bash
dotnet restore InventarioVentas.slnx
dotnet build InventarioVentas.slnx --no-restore
```

La raíz contiene la solución `InventarioVentas.slnx`, pero el archivo de proyecto está en `src/InventarioVentas.API/InventarioVentas.API.csproj`. Por eso `dotnet run` debe recibir explícitamente la ruta del proyecto:

```bash
dotnet run --project src/InventarioVentas.API/InventarioVentas.API.csproj
```

Para ejecutar la API fuera de Docker, configura la conexión mediante User Secrets o una variable de entorno. Por ejemplo:

```bash
dotnet user-secrets init --project src/InventarioVentas.API/InventarioVentas.API.csproj
dotnet user-secrets set --project src/InventarioVentas.API/InventarioVentas.API.csproj "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=inventarioventas;Username=postgres;Password=<tu-password>"
```

La misma configuración puede suministrarse con la variable `ConnectionStrings__DefaultConnection`.

Con una conexión PostgreSQL configurada, la API debería iniciar sin errores. Sin `ConnectionStrings:DefaultConnection`, `Program.cs` falla de forma explícita y accionable. Después de configurar la conexión, verifica Swagger en:

```text
http://localhost:5011/swagger
https://localhost:7176/swagger
http://localhost:5011/swagger/v1/swagger.json
```

La ruta `/weatherforecast` ya no existe; la API solo expone endpoints cuando se implementan y mapean controllers de los módulos.

### 8. Ejecutar con Docker

Desde la raíz del repositorio:

```bash
docker compose up --build
```

La API queda disponible en `http://localhost:8080/swagger`. Para detenerla:

```bash
docker compose down
```

El flujo Docker está documentado con más detalle en [`docs/Docker.md`](Docker.md). Compose incluye la API y PostgreSQL; define `POSTGRES_PASSWORD` antes de levantarlo.

### 9. Inicializar Git

Desde la raíz `InventarioVentas`:

```bash
cd ../..
```

Comprueba dónde estás:

```bash
pwd
```

Luego:

```bash
git init
```

Puedes crear el `.gitignore` oficial para .NET:

```bash
dotnet new gitignore
```

Primer commit:

```bash
git add .
git commit -m "chore: configuracion inicial del proyecto"
```

---

## Después de estos comandos, NO creen todavía la migración

Primero nos falta crear:

```text
Categoria.cs
Producto.cs
Cliente.cs
Venta.cs
DetalleVenta.cs
```

y después:

```text
AppDbContext.cs
```

y configurar en `appsettings.json`:

```text
ConnectionStrings
```

Solo entonces ejecutaríamos:

```bash
dotnet ef migrations add InitialCreate
```

y:

```bash
dotnet ef database update
```

El orden que recomiendo seguir ahora es:

**1. Crear entidades → 2. Crear relaciones → 3. Crear AppDbContext → 4. Configurar PostgreSQL → 5. Registrar DbContext en Program.cs → 6. Crear migración → 7. Crear BD → 8. Empezar Categorías.**

Si seguimos en ese orden, puedo darte ahora **el código exacto de las 5 entidades (`Categoria`, `Producto`, `Cliente`, `Venta` y `DetalleVenta`) explicando línea por línea para que los tres entiendan qué están haciendo**.
