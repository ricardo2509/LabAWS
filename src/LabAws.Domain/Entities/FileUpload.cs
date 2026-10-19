using LabAws.Domain.Exceptions;

namespace LabAws.Domain.Entities
{
    public class FileUpload
    {
        private const int MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".txt" };

        public string Name { get; private set; }
        public long BytesLen { get; private set; }
        public string Extension { get; private set; }

        public FileUpload(string name, long bytesLen)
        {
            Name     = name ?? throw new ArgumentNullException(nameof(name));
            BytesLen = bytesLen;
            Extension = Path.GetExtension(name).ToLowerInvariant();

            ValidateName();
            ValidateLength();
            ValidateExtension();         
        }

        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new DomainException("File name cannot be null or empty.");
        }
        
        private void ValidateLength()
        {
            if (BytesLen <= 0 || BytesLen > MaxFileSizeInBytes)
                throw new DomainException( $"File size must be between 1 byte and {MaxFileSizeInBytes} bytes.");
        }

        private void ValidateExtension()
        {
            var extension = System.IO.Path.GetExtension(Name).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new DomainException($"File extension '{extension}' is not allowed. Allowed extensions are: {string.Join(", ", AllowedExtensions)}");
        }
    }
}
