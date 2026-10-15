using LabAws.Application.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Application.UseCases
{
    public class ListLogAWSUsecase
    {
        private IUploadLogRepository _uploadLogRepository;

        public ListLogAWSUsecase(IUploadLogRepository uploadLogRepository)
        {
            _uploadLogRepository = uploadLogRepository;
        }

        public async Task<List<Domain.Entities.UploadLog>> ExecuteAsync()
        {
            return await _uploadLogRepository.GetAllLogAsync();
        }
    }
}
