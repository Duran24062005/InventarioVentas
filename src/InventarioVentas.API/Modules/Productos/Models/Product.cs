namespace InventarioVentas.API.Modules.Productos.Models;

using InventarioVentas.API.Modules.Categorias.Models;

public class Producto
{
    public Guid Id { get; set; } 
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public bool Estado { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;


    public Guid CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

}