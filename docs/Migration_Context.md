En tu proyecto, las migraciones deben generarse usando `AppDbContext`, porque es el contexto principal que contiene:

- `Categories`
- `Products`
- `Customers`
- `Sales`
- `SaleDetails`
- Las configuraciones de entidades y sus relaciones

`AppDbContext` es el único contexto de la aplicación y el propietario de las migraciones versionadas. Los contextos auxiliares anteriores fueron eliminados para evitar modelos y unidades de trabajo divergentes.

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
export ConnectionStrings__DefaultConnection='Host=localhost;Port=5455;Database=inventarioventas;Username=postgres;Password=TU_PASSWORD'
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

## 6. Migración de `Customers`

La migración `20260826194808_AddCustomers` agrega la tabla `Customers` al esquema de `AppDbContext`. También contiene cambios adicionales detectados por EF Core en las relaciones y columnas de productos y categorías; revisa el archivo antes de aplicarlo en una base compartida.

Para crearla:

```bash
dotnet ef migrations add AddCustomers \
  --context AppDbContext \
  --project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --startup-project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --output-dir Data/Migrations
```

El código generado actualmente es:

```csharp
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioVentas.API.Data.Migrations
{
    public partial class AddCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Code",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreCompleto = table.Column<string>(type: "text", nullable: false),
                    Documento = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProductId",
                table: "Categories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Products_ProductId",
                table: "Categories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Products_ProductId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProductId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Categories");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Products",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
```

## 7. Aplica la migración a PostgreSQL

```bash
dotnet ef database update \
  --context AppDbContext \
  --project src/InventarioVentas.API/InventarioVentas.API.csproj \
  --startup-project src/InventarioVentas.API/InventarioVentas.API.csproj
```

Este comando crea o actualiza el esquema y registra la migración aplicada en `__EFMigrationsHistory`. [Referencia oficial del comando `database update`](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying)

## 8. Verifica las tablas

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

## 9. Comandos útiles

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
