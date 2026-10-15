public class UploadLogDto
{
    public Guid Id { get; set; }
    public string NomeArquivo { get; set; } = string.Empty;
    public long TamanhoBytes { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string S3Url { get; set; } = string.Empty;
    public DateTime DataUpload { get; set; }
}