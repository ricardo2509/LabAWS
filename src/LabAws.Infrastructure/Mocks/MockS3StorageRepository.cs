using LabAws.Application.DTO;
using LabAws.Application.Interfaces.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Infrastructure.Mocks
{
    public class MockS3StorageRepository : IS3StorageRepository
    {

        private static readonly List<string> _inMemoryBuckets = new() { "shered-bucket-test", "documents" };
        public Task<string> CreateBucketAsync(string bucketName)
        {
            if (!_inMemoryBuckets.Contains(bucketName))
                _inMemoryBuckets.Add(bucketName);

            return Task.FromResult(@"Bucket {bucketName} create succesfully");
        }

        public Task<bool> FindBucketAsync(string bucketName)
        {
            bool exists = _inMemoryBuckets.Contains(bucketName);
            return Task.FromResult(exists);
        }

        public Task<List<string>> ListBucketsAsync()
        {
            return Task.FromResult(_inMemoryBuckets);
        }

        public Task<string> UploadFileAsync(ProcessFileCommand bucketFileCommand)
        {
            string fakeURL = $"https://amazonaws.com{bucketFileCommand.BucketName}/{Guid.NewGuid()}_{bucketFileCommand.FileName}";
            return Task.FromResult(fakeURL);
        }
    }
}
