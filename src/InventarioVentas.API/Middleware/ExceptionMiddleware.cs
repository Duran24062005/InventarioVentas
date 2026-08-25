using System.Net;
using System.Text.Json;
using InventarioVentas.API.Common.Exceptions;

namespace InventarioVentas.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }




    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ManejarExcepcionAsync(context, ex);
        }
    }




    private static Task ManejarExcepcionAsync(HttpContext context, Exception excepcion)
    {
        context.Response.ContentType = "application/json";

        var codigoEstado = excepcion switch
        {
            NotFoundException => HttpStatusCode.NotFound,           // HTTP 404     https://http.cat/status/404
            BusinessException => HttpStatusCode.BadRequest,         // HTTP 400     https://http.cat/status/400
            ValidationException => HttpStatusCode.BadRequest,       // HTTP 400     https://http.cat/status/400
            _ => HttpStatusCode.InternalServerError                 // HTTP 500     https://http.cat/status/500
        };

        context.Response.StatusCode = (int)codigoEstado;

        var respuesta = new
        {
            mensaje = excepcion.Message
        };


        var json = JsonSerializer.Serialize(respuesta);
        return context.Response.WriteAsync(json);
    }


    
}