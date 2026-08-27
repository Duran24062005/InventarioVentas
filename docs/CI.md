# Integración continua

El repositorio ejecuta GitHub Actions mediante [`.github/workflows/ci.yml`](../.github/workflows/ci.yml).

## Cuándo se ejecuta

El workflow se ejecuta en:

- Cada `push` a `main` o `develop`.
- Cada `pull request` dirigida a `main` o `develop`.

## Qué verifica

El job usa .NET 10 y ejecuta, en este orden:

```bash
dotnet restore InventarioVentas.slnx
dotnet build InventarioVentas.slnx --configuration Release --no-restore
dotnet test InventarioVentas.slnx --configuration Release --no-restore --no-build
```

La solución contiene un proyecto de pruebas con cobertura inicial de validaciones, servicios de ventas y el modelo EF. La cobertura de integración HTTP y PostgreSQL todavía está pendiente; el detalle está en [`Testing.md`](Testing.md).

## Ejecución local equivalente

Desde la raíz del repositorio:

```bash
dotnet restore InventarioVentas.slnx
dotnet build InventarioVentas.slnx --configuration Release --no-restore
dotnet test InventarioVentas.slnx --configuration Release --no-restore --no-build
```

El CI no aplica migraciones ni requiere credenciales de PostgreSQL. La verificación de base de datos y de los endpoints permanece en el flujo de pruebas de integración.
