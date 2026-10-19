namespace AwsLab.Web.DTO
{
    public class UploadLogDto
    {
        public Guid Id { get; set; }
        public string QueueId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long BytesLen { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string S3Url { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
    }
}