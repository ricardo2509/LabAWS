namespace LabAws.Application.DTO
{
    public record ProcessFileCommand(Stream FileStream, string FileName, string ContentType, long Length, string BucketName);
}