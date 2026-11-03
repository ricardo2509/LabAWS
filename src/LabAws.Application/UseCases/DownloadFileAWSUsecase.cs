using LabAws.Application.Interfaces.AWS;
using LabAws.Application.Model;

namespace LabAws.Application.UseCases
{
    public class DownloadFileAWSUsecase
    {
        public IS3StorageRepository _s3StorageRepository { get; set; }
        public DownloadFileAWSUsecase(IS3StorageRepository s3StorageRepository)
        {
            _s3StorageRepository = s3StorageRepository;
        }

        public async Task<string> Execute(string bucketName, string fileKey)
        {
            var ret = await _s3StorageRepository.SecureDownloadURLAsync(bucketName, fileKey);
            return ret;
        }
        public async Task<FileAccessResponse> ValidateAccessExecute(string bucketName, string fileKey)
        {
            var ret = await _s3StorageRepository.ValidateAccessDownloadAsync(bucketName, fileKey);
            return ret;
        }
    }
}