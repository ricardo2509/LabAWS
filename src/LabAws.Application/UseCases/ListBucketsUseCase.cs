using LabAws.Application.Interfaces.AWS;

namespace LabAws.Application.UseCases
{
    public class ListBucketsUseCase
    {
        private IS3StorageRepository _s3StorageRepository;

        public ListBucketsUseCase(IS3StorageRepository s3StorageRepository)
        {
            _s3StorageRepository = s3StorageRepository;
        }
        public async Task<List<string>> ExecuteAsync()
        {
            return await _s3StorageRepository.ListBucketsAsync();
        }
    }
}
