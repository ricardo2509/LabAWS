namespace LabAws.Domain.Entities;

public class UploadLog
{
    public Guid Id { get; private set; }
    public string FileName { get; private set; }
    public long BytesLen { get; private set; }
    public string ContentType { get; private set; }
    public string BucketName { get; private set; }
    public string S3Url { get; private set; }
    public DateTime UploadDate { get; private set; }

    public UploadLog(string fileName, long bytesLen, string contentType, string bucketName, string s3Url)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Filename can't be null.");

        if (string.IsNullOrWhiteSpace(s3Url))
            throw new ArgumentException("S3 URL can't be null.");

        Id = Guid.NewGuid();
        FileName = fileName;
        BytesLen = bytesLen;
        ContentType = contentType;
        BucketName = bucketName;
        S3Url = s3Url;
        UploadDate = DateTime.UtcNow;
    }
}
