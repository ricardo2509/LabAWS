using LabAws.Domain.Entities;

namespace LabAws.Application.Interfaces.Database
{
    public interface IUploadLogRepository
    {
        Task AddLogAsync(UploadLog log);
        Task<List<UploadLog>> GetAllLogAsync();
    }
}