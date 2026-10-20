using LabAws.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Application.Interfaces.Database
{
    public interface IErrorLogRepository
    {
        Task AddLogAsync(ErrorLog log);
        Task<List<ErrorLog>> GetAllLogAsync();
    }
}
