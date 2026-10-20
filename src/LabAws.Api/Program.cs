using LabAws.Api.Middlewares;
using LabAws.Application.DTO;
using LabAws.Application.Interfaces.AWS;
using LabAws.Application.Interfaces.Database;
using LabAws.Application.UseCases;
using LabAws.Domain.Services;
using LabAws.Infrastructure.Configuration;
using LabAws.Infrastructure.Contexts;
using LabAws.Infrastructure.Mocks;
using LabAws.Infrastructure.Repositories.AWS;
using LabAws.Infrastructure.Repositories.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot" 
});

var connectionString = builder.Configuration.GetConnectionString("postgresql");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        b => b.MigrationsAssembly("LabAws.Api")     //  Force migration to be created on API project
    ));

// Add Swagger/OpenAPI services for visual tests
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery();

bool useMock = builder.Configuration.GetValue<bool>("ConfiguracoesAws:UseMock");

if (useMock)
{
    builder.Services.AddScoped<IMensageriaRepository, MockMensageriaRepository>();
    builder.Services.AddScoped<IS3StorageRepository, MockS3StorageRepository>();
}
else
{
    // configurado pelo comando 'aws configure'
    builder.Services.AddInfrastructureAws(builder.Configuration);
    builder.Services.AddScoped<IMensageriaRepository, AwsSqsRepository>();
    builder.Services.AddScoped<IS3StorageRepository, AwsS3Repository>();
}
builder.Services.AddScoped<AwsS3Service>();
builder.Services.AddScoped<UploadFileUseCase>();
builder.Services.AddScoped<CreateBucketUseCase>();

builder.Services.AddScoped<ListLogAWSUsecase>();
builder.Services.AddScoped<ListBucketsUseCase>();
builder.Services.AddScoped<IUploadLogRepository, UploadLogRepository>();
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddCors(opcoes =>
{
    opcoes.AddPolicy("LiberarFront", politica =>
    {
        politica.AllowAnyOrigin() // Em produção colocar a URL do Front, para testes locais liberar tudo
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger Interface for local tests
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAntiforgery();

// Route to create bucket
app.MapPost("/api/createBucket", async (string bucketName, [FromServices] CreateBucketUseCase useCase) =>
{
    await useCase.ExecuteAsync(bucketName);
    return Results.Ok("Bucket created successful");

});

// ENDPOINT (Minimal API): Route to S3 buclets list
app.MapGet("/api/buckets", async (ListBucketsUseCase useCase) =>
{
        var buckets = await useCase.ExecuteAsync();
        return Results.Ok(buckets);
});

app.MapPost("/api/uploadFile", static async (string bucketName, IFormFile file, [FromServices] UploadFileUseCase useCase) =>
{
        var fileStream = file.OpenReadStream();

        ProcessFileCommand processFileCommand = new ProcessFileCommand(fileStream, file.FileName, string.Empty, file.Length, bucketName);
        await useCase.ExecutarAsync(processFileCommand);

        return Results.Ok("File upload successful");
}
).DisableAntiforgery(); ;

app.MapPost("/api/notificar-arquivo", async(string fileName, string bucketName, IMensageriaRepository mensageriaService) =>
{
    try
    {
        var messageId=await mensageriaService.SendNotificationAsync(fileName, bucketName);
        return Results.Ok(new { Message = "Mensagem enviada com sucesso para a fila SQS.", MessageId = messageId });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Amazon SQS error: {ex.Message}");
    }
});

// ENDPOINT (Minimal API): Route to S3 buclets list
app.MapGet("/api/logs", async (ListLogAWSUsecase useCase) =>
{
    try
    {
        var logs = await useCase.ExecuteAsync();
        return Results.Ok(logs);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error to connect to database: {ex.Message}");
    }
});

app.UseCors("LiberarFront");

app.MapFallbackToFile("index.html");

app.Run();
