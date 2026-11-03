using Amazon.RuntimeDependencies;
using Amazon.S3;
using Amazon.S3.Model;
using LabAws.Application.DTO;
using LabAws.Application.Interfaces.AWS;
using LabAws.Application.Model;
using LabAws.Domain.Exceptions;
using LabAws.Infrastructure.Exceptions;

namespace LabAws.Infrastructure.Repositories.AWS
{
    public class AwsS3Repository : IS3StorageRepository
    {
        private readonly IAmazonS3 _s3Client;

        public AwsS3Repository(IAmazonS3 s3Client)
        {
            this._s3Client = s3Client;
        }

        async public Task<string> CreateBucketAsync(string bucketName)
        {
            try
            {
                Amazon.S3.Model.PutBucketResponse response = new Amazon.S3.Model.PutBucketResponse();

                var request = new Amazon.S3.Model.PutBucketRequest
                {
                    BucketName = bucketName.ToLower().Trim(),
                    UseClientRegion = true
                };
                response = await _s3Client.PutBucketAsync(request);

                var ret = EncryptBucket(bucketName);
                ret = DisablePublicAccessBucket(bucketName);

                return response.HttpStatusCode.ToString();
            }
            catch (AmazonS3Exception ex)
            {
                throw new InfrastructureException(
                    "Fail creating AWS buckets.", ex);
            }
        }

        // Deny public access
        private async Task<bool> DisablePublicAccessBucket(string bucketName)
        {
            var publicAccessBlockRequest = new PutPublicAccessBlockRequest
            {
                BucketName = bucketName,
                PublicAccessBlockConfiguration = new PublicAccessBlockConfiguration
                {
                    BlockPublicAcls = true,
                    BlockPublicPolicy = true,
                    IgnorePublicAcls = true,
                    RestrictPublicBuckets = true
                }
            };

            await _s3Client.PutPublicAccessBlockAsync(publicAccessBlockRequest);
            return true;
        }

        //  Bucket encryption
        private async Task<bool> EncryptBucket(string bucketName)
        {

            var encryptionRequest = new PutBucketEncryptionRequest
            {
                BucketName = bucketName,
                ServerSideEncryptionConfiguration = new ServerSideEncryptionConfiguration
                {

                    ServerSideEncryptionRules = new List<ServerSideEncryptionRule>
                    {
                        new ServerSideEncryptionRule{
                        ServerSideEncryptionByDefault=new ServerSideEncryptionByDefault
                        {
                            ServerSideEncryptionAlgorithm = ServerSideEncryptionMethod.AES256
                        }
                    }
                }

                }
            };
            await _s3Client.PutBucketEncryptionAsync(encryptionRequest);
            return true;
        }

            async public Task<List<string>> ListBucketsAsync()
        {
            try
            {
                List<string> nomesDosBuckets = new List<string>();
                var response = await _s3Client.ListBucketsAsync();

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

                var response = await _s3Client.PutObjectAsync(request);
                return response.HttpStatusCode.ToString();
            }
            catch (AmazonS3Exception ex)
            {
                throw new InfrastructureException(
                    "File upload to AWS failed.", ex);
            }
        }

        public async Task<string> SecureDownloadURLAsync(string bucketName, string fileKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.AddMinutes(10),
                Verb = HttpVerb.GET,
                 
            };

            var response = await _s3Client.GetPreSignedURLAsync(request);
            return response;
        }

        public async Task<FileAccessResponse> ValidateAccessDownloadAsync(string bucketName, string fileKey)
        {
            try
            {
                var resonse = await _s3Client.GetObjectMetadataAsync(bucketName, fileKey);

                return new FileAccessResponse
                {
                    HasAccess = true,
                    Message = "Access granted to the file.",
                };
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new FileAccessResponse
                    {
                        HasAccess = false,
                        Message = "File not found.",
                    };
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return new FileAccessResponse
                    {
                        HasAccess = false,
                        Message = "Access denied to the file.",
                    };
                }
                else
                {
                    throw new InfrastructureException(
                        "Error validating access to the file.", ex);
                }
            }
        }
    }
}