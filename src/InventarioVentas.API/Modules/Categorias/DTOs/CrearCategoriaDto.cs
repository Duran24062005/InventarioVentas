using System.Globalization;

namespace InventarioVentas.API.Modules.Categorias.DTOs
{
    public class CrearCategoriaDto
    {
        public required String Nombre { get; set; } = string.Empty;
        public required String Descripcion { get; set; } = string.Empty;
        public required DateTime FechaCreacion { get; set; }
        public required bool Estado { get; set; }



    }
}
