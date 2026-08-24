using System.Globalization;

namespace InventarioVentas.API.Modules.Categorias.DTOs
{
    public class CategoriaReposeDto
    {
        public Guid Id { get; set; }
        public String Nombre { get; set; } = string.Empty;
        public String Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Estado { get; set; }
    }
}
