using LabAws.Application.Exceptions;
using LabAws.Application.Interfaces.AWS;

namespace LabAws.Application.UseCases
{
    public class CreateBucketUseCase
    {
        private IS3StorageRepository _s3StorageRepository;
        public CreateBucketUseCase(IS3StorageRepository s3StorageRepository) { 
        
            _s3StorageRepository = s3StorageRepository;
        }

        public async Task ExecuteAsync(string bucketName)
        {

            if (string.IsNullOrEmpty(bucketName))
                throw new ValidationBucketNameException("Bucket name can't be empty.");

            //  Call Infra S3 repository to validate bucket name
            if (await _s3StorageRepository.FindBucketAsync(bucketName))
                throw new ValidationBucketNameException("Bucket name already exists.");

            await _s3StorageRepository.CreateBucketAsync(bucketName);
        }
    }
}