using LabAws.Domain.Exceptions;
using System.Net;
using System.Text.Json;
using LabAws.Application.Exceptions;

namespace LabAws.Api.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is DomainException || ex is ValidationBucketNameException)
        {
            //  handle the exception caught by domain
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var response = new { erro = ex.Message };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        catch (Exception)
        {
            // Unexpected (ex: S3 down) handle Internal Server Error (500)
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var resposta = new { erro = "Ocorreu um erro interno inesperado no servidor." };
            var json = JsonSerializer.Serialize(resposta);

            await context.Response.WriteAsync(json);
        }
    }
}