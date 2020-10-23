using Broker.Models;

namespace Broker.Services.Interfaces
{
    public interface IMessageStoregeService
    {
        void Add(Message message);

        Message GetNext();

        bool IsEmpty();
    }
}
