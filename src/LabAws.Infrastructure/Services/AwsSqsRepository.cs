using Amazon.SQS;
using Amazon.SQS.Model;
using LabAws.Application.Interfaces.AWS;
using System.Text.Json;

namespace LabAws.Infrastructure.Services
{
    public class AwsSqsRepository : IMensageriaRepository
    {
        private readonly IAmazonSQS sqsClient;

        public AwsSqsRepository(IAmazonSQS sqsClient)
        {
            this.sqsClient = sqsClient;
        }

        async Task<string> IMensageriaRepository.SendNotificationAsync(string fileName, string bucketName)
        {
            string sqsQueue = "fila-importacao-contatos";

            var eventData = new
            {
                file = fileName,
                Bucket = bucketName,
                createDate = DateTime.UtcNow
            };

            string bodyMessage = JsonSerializer.Serialize(eventData);

            var request = new SendMessageRequest
            {
                QueueUrl = sqsQueue,
                MessageBody = bodyMessage
            };

            var response = await sqsClient.SendMessageAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.MessageId;
            }

            throw new Exception("Fail to send message to SQS queue");
        }
    }
}
