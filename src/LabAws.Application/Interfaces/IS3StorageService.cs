using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Application.Interfaces
{
    public interface IS3StorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string bucketName);
    }
}
