namespace LabAws.Application.Interfaces
{
    public interface IAwsQueueService
    {
        Task SendMessageAsync(string mensagem);
    }
}
