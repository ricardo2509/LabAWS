namespace LabAws.Application.Model
{
    public class FileAccessResponse
    {
        public bool HasAccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
