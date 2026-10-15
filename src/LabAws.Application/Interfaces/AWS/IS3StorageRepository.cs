using LabAws.Application.DTO;

namespace LabAws.Application.Interfaces.AWS
{
    public interface IS3StorageRepository
    {
        Task<List<string>> ListBucketsAsync();
        Task<string> CreateBucketAsync(string bucketName);

        Task<string> UploadFileAsync(ProcessFileCommand bucketFileCommand);

        Task<bool> FindBucketAsync(string bucketName);
    }
}
