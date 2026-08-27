namespace InventarioVentas.API.Modules.Sales.DTOs;

public class CreateSaleDto
{
	public Guid CustomerId { get; set; }
	public List<CreateSaleDetailDto> Details { get; set; } = new();
}

public class CreateSaleDetailDto
{
	public Guid ProductId { get; set; }
	public int Quantity { get; set; }
}
