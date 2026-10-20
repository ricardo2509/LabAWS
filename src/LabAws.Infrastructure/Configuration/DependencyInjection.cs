using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.SQS;
using LabAws.Application.Interfaces.AWS;
using LabAws.Infrastructure.Repositories.AWS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LabAws.Infrastructure.Configuration;
public static class DependencyInjection
{
    // Extension method for .Net interface IServiceCollection 
    public static IServiceCollection AddInfrastructureAws(this IServiceCollection services, IConfiguration configuration)
    {
        var opcoesAws = new Amazon.Extensions.NETCore.Setup.AWSOptions
        {
            Region = RegionEndpoint.USEast1
        };

        if (Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") != null && Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") != null)
        {
            var key = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            var credentials = new BasicAWSCredentials(key, secretKey);
            opcoesAws.Credentials = credentials;
            services.AddAWSService<IAmazonS3>(opcoesAws);
            services.AddAWSService<IAmazonSQS>(opcoesAws);
        }
        else
        {
            // 2. Read "default" from .aws/credentials da máquina
            var chain = new CredentialProfileStoreChain();

            if (chain.TryGetAWSCredentials("default", out AWSCredentials credentials))
            {
                opcoesAws.Credentials = credentials;
                services.AddAWSService<IAmazonS3>(opcoesAws);
                services.AddAWSService<IAmazonSQS>(opcoesAws);
            }
        }
        services.AddDefaultAWSOptions(opcoesAws);

        return services;
    }
}
