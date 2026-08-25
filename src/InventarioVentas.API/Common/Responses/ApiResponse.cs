using System.Text.Json.Serialization;

namespace InventarioVentas.API.Common.Responses;

public class ApiResponse<T>
{
    [JsonPropertyName("exito")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("mensaje")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("datos")]
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }


    public static ApiResponse<T> Error(string message)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Data = default
        };
    }

    
}
