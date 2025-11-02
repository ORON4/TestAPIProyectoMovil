using MySqlX.XDevAPI.Common;
using System.Data;
using TestAPI.Modelos;
using TestAPI.Exceptions;
using Org.BouncyCastle.Bcpg.Sig;

namespace TestAPI.Middleware
{
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
                int statusCode = ex switch
                {
                    NoDataException _ => StatusCodes.Status204NoContent,
                    ValidationException _ => StatusCodes.Status400BadRequest,
                    Exception _ => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                var respuesta = new Respuesta(
                    mensaje: ex.Message
                );
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(respuesta);
            }
        }
    }
}

