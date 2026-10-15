using LabAws.Application.DTO;
using LabAws.Application.Interfaces;
using LabAws.Domain.Exceptions;
using LabAws.Domain.Services;

namespace LabAws.Application.UseCases
{
    public class UploadFileUseCase
    {
        private readonly IS3StorageRepository _s3Repository;
        private readonly IMensageriaRepository _awsQueueService;

        public UploadFileUseCase(IS3StorageRepository s3Repository, IMensageriaRepository queueService)
        {
            _s3Repository = s3Repository;
            _awsQueueService = queueService;
        }

        public async Task ExecutarAsync(ProcessFileCommand command)
        {
            var fileUpload = new Domain.Entities.FileUpload(command.FileName, command.Length);
            
            // 2. Save on S3 using abstraction
            string fileUrl = await _s3Repository.UploadFileAsync(command);

            // 3. Notify AWS SQS
            await _awsQueueService.SendNotificationAsync(fileUpload.Name, command.BucketName);
        }
    }
}
