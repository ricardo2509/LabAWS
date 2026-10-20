using Amazon.S3;
using LabAws.Application.DTO;
using LabAws.Application.Interfaces.AWS;
using LabAws.Domain.Exceptions;
using LabAws.Infrastructure.Exceptions;

namespace LabAws.Infrastructure.Repositories.AWS
{
    public class AwsS3Repository : IS3StorageRepository
    {
        private readonly IAmazonS3 s3Client;

        public AwsS3Repository(IAmazonS3 s3Client)
        {
            this.s3Client = s3Client;
        }

        async public Task<string> CreateBucketAsync(string nomeDoBucket)
        {
            try
            {
                Amazon.S3.Model.PutBucketResponse response = new Amazon.S3.Model.PutBucketResponse();

                var request = new Amazon.S3.Model.PutBucketRequest
                {
                    BucketName = nomeDoBucket.ToLower().Trim(),
                    UseClientRegion = true
                };
                response = await s3Client.PutBucketAsync(request);
                return response.HttpStatusCode.ToString();
            }
            catch (AmazonS3Exception ex)
            {
                throw new InfrastructureException(
                    "Fail creating AWS buckets.", ex);
            }
        }

        async public Task<List<string>> ListBucketsAsync()
        {
            try
            {
                List<string> nomesDosBuckets = new List<string>();
                var response = await s3Client.ListBucketsAsync();

                if (response.Buckets == null)
                    return new List<string>();

                return response.Buckets.Select(b => b.BucketName).ToList();
            }
            catch (AmazonS3Exception ex)
            {
                throw new InfrastructureException(
                    "Fail accessing AWS buckets.", ex);
            }
        }
        async public Task<bool> FindBucketAsync(string bucketName)
        {
            var ret = await ListBucketsAsync();
            return ret.Contains(bucketName);
        }

        async public Task<string> UploadFileAsync(ProcessFileCommand bucketFileCommand)
        {
            try
            {
                var request = new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = bucketFileCommand.BucketName,
                    Key = string.Format("{0}_{1}.{2}", bucketFileCommand.FileName.Substring(0, bucketFileCommand.FileName.IndexOf(".")),
                                                    DateTime.Now.ToShortTimeString(),
                                                    bucketFileCommand.FileName.Substring(bucketFileCommand.FileName.IndexOf(".") + 1, 
                                                    bucketFileCommand.FileName.Length - bucketFileCommand.FileName.IndexOf(".") - 1)).Replace(":", "_"),
                    InputStream = bucketFileCommand.FileStream
                };
                request.AutoCloseStream = false;

                var response = await s3Client.PutObjectAsync(request);
                return response.HttpStatusCode.ToString();
            }
            catch (AmazonS3Exception ex)
            {
                throw new InfrastructureException(
                    "File upload to AWS failed.", ex);
            }
        }
    }
}