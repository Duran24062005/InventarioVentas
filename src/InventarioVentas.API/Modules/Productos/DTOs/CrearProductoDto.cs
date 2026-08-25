namespace InventarioVentas.API.Modules.Productos.DTOs;

public class CrearProductoDto
{
    // Nombre comercial del producto (Obligatorio)
    public string Nombre {get; set; } = string.Empty;

    // Código único SKU o código de barras (Obligatorio).
    public string Codigo { get; set; } = string.Empty;

    // Precio de venta del producto. Debe ser mayor a 0.
    public decimal Precio {get; set;}

    // Cantidad de unidades iniciales. Debe ser >= 0.
    public int Stock {get; set; }

    // ID de la categoría a la que pertenece el producto.
    public int CategoriaId {get; set; }
}

