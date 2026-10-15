namespace LabAws.Application.Interfaces
{
    public interface IMensageriaRepository
    {
        Task<string> SendNotificationAsync(string fileName, string bucketName);
    }
}
