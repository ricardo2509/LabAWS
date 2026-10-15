namespace LabAws.Application.Interfaces.AWS
{
    public interface IMensageriaRepository
    {
        Task<string> SendNotificationAsync(string fileName, string bucketName);
    }
}
