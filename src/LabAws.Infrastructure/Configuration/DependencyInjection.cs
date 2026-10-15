using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.SQS;
using LabAws.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LabAws.Application.Interfaces;

namespace LabAws.Infrastructure.Configuration;
public static class DependencyInjection
{
    // Criamos um método de extensão para a interface IServiceCollection do .NET
    public static IServiceCollection AddInfrastructureAws(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Criamos o objeto de opções definindo a região
        var opcoesAws = new Amazon.Extensions.NETCore.Setup.AWSOptions
        {
            Region = RegionEndpoint.USEast1
        };

        // 2. Buscamos o perfil "default" direto do arquivo físico .aws/credentials da máquina
        // (O SDK vai ler a pasta do usuário do Windows ou a pasta do container Docker automaticamente)
        var chain = new CredentialProfileStoreChain();
        if (chain.TryGetAWSCredentials("default", out AWSCredentials credentials))
        {
            opcoesAws.Credentials = credentials;
            // 2. Injeta os clientes nativos da AWS aqui dentro, escondidos do mundo exterior
            services.AddAWSService<IAmazonS3>(opcoesAws);
            services.AddAWSService<IAmazonSQS>(opcoesAws);

            // 3. Registra o seu serviço de mensageria
            services.AddScoped<IMensageriaRepository, AwsSqsRepository>();
        }
        return services;
    }
}
