namespace InventarioVentas.API.Modules.Productos.DTOs;

public class ProductoResponseDto
{
    public int Id { get; set;}
    public string Nombre {get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public bool Estado { get; set; }

// Información básica de la categoría vinculada.
    public int CategoriaId { get; set; }
    public string NombreCtegoria { get; set; } = string.Empty;
}