using IRepairman.Application.Models;

namespace IRepairman.Application.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
