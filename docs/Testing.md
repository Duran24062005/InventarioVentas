# Pruebas

La solución contiene el proyecto `tests/InventarioVentas.API.Tests`, registrado en `InventarioVentas.slnx`.

## Ejecutar las pruebas

Desde la raíz del repositorio:

```bash
dotnet restore InventarioVentas.slnx
dotnet build InventarioVentas.slnx --configuration Release --no-restore
dotnet test InventarioVentas.slnx --configuration Release --no-restore --no-build
```

También se puede ejecutar únicamente el proyecto de pruebas:

```bash
dotnet test tests/InventarioVentas.API.Tests/InventarioVentas.API.Tests.csproj --configuration Release
```

## Cobertura actual

Las pruebas actuales cubren:

- `CreateSaleValidator`: venta sin detalles, cantidades no positivas y productos repetidos.
- `SaleService`: uso del precio persistido, cálculo de total, descuento de stock y rechazo por stock insuficiente sin guardar una venta.
- `AppDbContext`: relación única `Category 1:N Product`, eliminación restrictiva, índice único de código y ausencia de la propiedad accidental `Categories.ProductId`.

El proyecto usa xUnit y SQLite en memoria para aislar cada prueba. SQLite permite probar el flujo transaccional del service sin depender de una instancia local de PostgreSQL, pero no sustituye la verificación del proveedor PostgreSQL.

## Alcance pendiente

Todavía falta agregar pruebas de integración HTTP y pruebas contra PostgreSQL real para validar migraciones, tipos específicos de Npgsql, códigos HTTP, middleware y el flujo completo de la API.
