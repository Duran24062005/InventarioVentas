namespace InventarioVentas.API.Common.Responses;

public class ApiResponse<T>
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public T? Datos { get; set; }

    public static ApiResponse<T> ConExito(T datos, string mensaje = "Operación exitosa")
    {
        return new ApiResponse<T> { 
            Exito = true, 
            Mensaje = mensaje, 
            Datos = datos 
            };
    }


    public static ApiResponse<T> ConError(string mensaje)
    {
        return new ApiResponse<T> { 
            Exito = false, 
            Mensaje = mensaje, 
            Datos = default 
            };
    }

    
}