namespace LabAws.Application.Interfaces.AWS
{
    public interface IAwsQueueService
    {
        Task SendMessageAsync(string mensagem);
    }
}
