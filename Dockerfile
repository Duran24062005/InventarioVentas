# Imagen de compilación: restaura y publica la API .NET 10.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["src/InventarioVentas.API/InventarioVentas.API.csproj", "src/InventarioVentas.API/"]
RUN dotnet restore "src/InventarioVentas.API/InventarioVentas.API.csproj"

COPY . .
WORKDIR "/src/src/InventarioVentas.API"
RUN dotnet publish "InventarioVentas.API.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Imagen de ejecución: contiene únicamente el runtime y la aplicación publicada.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "InventarioVentas.API.dll"]
