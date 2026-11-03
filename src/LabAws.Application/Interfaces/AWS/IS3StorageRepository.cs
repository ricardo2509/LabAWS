using LabAws.Application.DTO;
using LabAws.Application.Model;

namespace LabAws.Application.Interfaces.AWS
{
    public interface IS3StorageRepository
    {
        Task<List<string>> ListBucketsAsync();
        Task<string> CreateBucketAsync(string bucketName);
        Task<string> UploadFileAsync(ProcessFileCommand bucketFileCommand);
        Task<bool> FindBucketAsync(string bucketName);
        Task<string> SecureDownloadURLAsync(string bucketName, string fileKey);
        Task<FileAccessResponse> ValidateAccessDownloadAsync(string bucketName, string fileKey);

    }
}
