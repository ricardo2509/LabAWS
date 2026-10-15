using LabAws.Application.DTO;
using LabAws.Application.Interfaces.AWS;
using LabAws.Application.Interfaces.Database;
using LabAws.Domain.Entities;

namespace LabAws.Application.UseCases
{
    public class UploadFileUseCase
    {
        private readonly IS3StorageRepository _s3Repository;
        private readonly IMensageriaRepository _awsQueueService;
        private readonly IUploadLogRepository _uploadLogRepository;
        public UploadFileUseCase(IS3StorageRepository s3Repository, IMensageriaRepository queueService, IUploadLogRepository uploadLogRepository)
        {
            _s3Repository = s3Repository;
            _awsQueueService = queueService;
            _uploadLogRepository = uploadLogRepository;
        }

        public async Task ExecutarAsync(ProcessFileCommand command)
        {
            // 1. Validate data 
            var fileUpload = new FileUpload(command.FileName, command.Length);

            // 2. Save on S3 using abstraction
            string fileUrl = await _s3Repository.UploadFileAsync(command);

            // 3. Notify AWS SQS
            await _awsQueueService.SendNotificationAsync(fileUpload.Name, command.BucketName);

            // 4. Log upload on Postgres DB
            var log = new UploadLog(
                fileName: command.FileName,
                bytesLen: command.Length,
                contentType: command.ContentType,
                bucketName: command.BucketName,
                s3Url: fileUrl
            );

            await _uploadLogRepository.AddLogAsync(log);
        }
    }
}