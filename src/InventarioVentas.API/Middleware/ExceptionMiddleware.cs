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
            await HandleExceptionAsync(context, ex);
        }
    }




    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,           // HTTP 404     https://http.cat/status/404
            BusinessException => HttpStatusCode.BadRequest,         // HTTP 400     https://http.cat/status/400
            ValidationException => HttpStatusCode.BadRequest,       // HTTP 400     https://http.cat/status/400
            _ => HttpStatusCode.InternalServerError                 // HTTP 500     https://http.cat/status/500
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            mensaje = exception.Message
        };


        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }


    
}
