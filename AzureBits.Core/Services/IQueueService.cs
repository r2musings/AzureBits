
using System.Threading.Tasks;

namespace AzureBits.Core.Services
{
    public interface IQueueService<T> where T: new()
    {
        Task AddMessageToQueueAsync(string messageId, T messageObject);
    }
}
