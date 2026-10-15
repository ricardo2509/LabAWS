using LabAws.Domain.Exceptions;

namespace LabAws.Domain.Services
{
    public class AwsS3Service
    {
        public AwsS3Service() {}

        // Obsolete, using FileUpload class instead
        // Class stands for future needs
        public void ValidateBucketName(string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new DomainException("Bucket name is mandatory.");
        }
    }
}
