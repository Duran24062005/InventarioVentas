En tu proyecto, las migraciones deben generarse usando `AppDbContext`, porque es el contexto principal que contiene:

- `Categories`
- `Products`
- La configuración de productos y sus relaciones

`CategoryDbContext` es provisional y `CustomerDbContext` todavía no forma parte del contexto principal.

EF Core compara el modelo actual con el snapshot anterior al crear migraciones, y registra las migraciones aplicadas en una tabla de historial de PostgreSQL. [Documentación oficial de migraciones](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

## 1. Levanta PostgreSQL

Desde la raíz del proyecto:

```bash
docker compose up -d postgres
```

Verifica que esté funcionando:

```bash
docker compose ps
```

## 2. Configura la conexión para `dotnet ef`

Como ejecutarás `dotnet ef` desde tu máquina, debes usar `localhost`:

```bash
export ConnectionStrings__DefaultConnection='Host=localhost;Port=5432;Database=inventarioventas;Username=postgres;Password=TU_PASSWORD'
```

Importante: el archivo `.env` lo interpreta Docker Compose, pero `dotnet ef` no lo carga automáticamente.

## 3. Lista los contextos disponibles

```bash
dotnet ef dbcontext list \
  --project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --startup-project src/InventarioVentas.API/InventarioVentas.API.csproj
```

Esto te permite confirmar que `AppDbContext` existe.

## 4. Crea la primera migración

```bash
dotnet ef migrations add InitialInventorySchema \
  --context AppDbContext \
  --project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --startup-project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --output-dir Data/Migrations
```

Significado:

- `migrations add`: genera archivos de migración.
- `InitialInventorySchema`: nombre descriptivo de la migración.
- `--context AppDbContext`: evita confusión entre los distintos contextos.
- `--project`: proyecto donde se crearán los archivos.
- `--startup-project`: proyecto que EF ejecuta para cargar configuración y servicios.
- `--output-dir Data/Migrations`: carpeta de destino.

Este comando todavía no modifica PostgreSQL. Solo genera archivos C#.

## 5. Revisa la migración

Deberían aparecer archivos similares a:

```text
src/InventarioVentas.API/Data/Migrations/
├── 202..._InitialInventorySchema.cs
├── 202..._InitialInventorySchema.Designer.cs
└── AppDbContextModelSnapshot.cs
```

Revisa especialmente que incluya las tablas:

```text
Categories
Products
```

## 6. Aplica la migración a PostgreSQL

```bash
dotnet ef database update \
  --context AppDbContext \
  --project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --startup-project src/InventarioVentas.API/InventarioVentas.API.csproj
```

Este comando crea o actualiza el esquema y registra la migración aplicada en `__EFMigrationsHistory`. [Referencia oficial del comando `database update`](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying)

## 7. Verifica las tablas

```bash
docker compose exec postgres \
  psql -U postgres -d inventarioventas -c '\dt'
```

Después levanta la API:

```bash
dotnet run --project src/InventarioVentas.API/InventarioVentas.API.csproj
```

Y prueba:

```bash
curl http://localhost:5011/api/categorias
```

Si no hay categorías registradas, lo esperado sería:

```json
[]
```

## Comandos útiles

Listar migraciones:

```bash
dotnet ef migrations list \
  --context AppDbContext \
  --project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --startup-project src/InventarioVentas.API/InventarioVentas.API.csproj
```

Generar un script SQL para revisarlo:

```bash
dotnet ef migrations script \
  --context AppDbContext \
  --project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --startup-project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --output migrations.sql
```

Eliminar la última migración, únicamente si todavía no fue aplicada:

```bash
dotnet ef migrations remove \
  --context AppDbContext \
  --project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --startup-project src/InventarioVentas.API/InventarioVentas.API.csproj
```

No ejecutes `migrations remove` si la migración ya fue aplicada en una base compartida o productiva; en ese caso se crea una nueva migración correctiva.