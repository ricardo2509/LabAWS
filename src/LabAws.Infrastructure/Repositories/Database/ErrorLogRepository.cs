using LabAws.Application.Interfaces.Database;
using LabAws.Domain.Entities;
using LabAws.Domain.Exceptions;
using LabAws.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LabAws.Infrastructure.Repositories.Database
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ErrorLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(ErrorLog log)
        {
            await _context.ErrorLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ErrorLog>> GetAllLogAsync()
        {
            return await _context.ErrorLogs.ToListAsync();
        }
    }
}
