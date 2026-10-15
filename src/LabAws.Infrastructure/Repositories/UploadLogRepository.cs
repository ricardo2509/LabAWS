using LabAws.Application.Interfaces.Database;
using LabAws.Domain.Entities;
using LabAws.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LabAws.Infrastructure.Repositories
{
    public class UploadLogRepository : IUploadLogRepository
    {
        private readonly ApplicationDbContext _context;
        public UploadLogRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task AddLogAsync(UploadLog log)
        {
            await _context.UploadLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UploadLog>> GetAllLogAsync()
        {
            return await _context.UploadLogs
                        .AsNoTracking()
                        .OrderByDescending(log => log.UploadDate)
                        .ToListAsync();
        }
    }
}
