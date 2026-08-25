namespace InventarioVentas.API.Modules.Categorias.Models
{
    public class Categoria
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Estado { get; set; }

    }
}
