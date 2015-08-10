using System.Threading.Tasks;
using AzureBits.Core.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace AzureBits.Services
{
    public class QueueService<T> : IQueueService<T> where T : class, new()
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        public QueueService(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }

        public async Task AddMessageToQueueAsync(string messageId, T messageObject)
        {
            var queue = GetQueue();

            // Convert to JSON
            var jsonMessage = JsonConvert.SerializeObject(messageObject);
            
            // Create the actual queue message 
            CloudQueueMessage message = new CloudQueueMessage(jsonMessage);

            // Add the message to the queue
            await queue.AddMessageAsync(message);
        }

        private CloudQueue GetQueue()
        {
            // get the storage account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            // create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // get a reference to the queue
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            // create the queue if it does not exist
            queue.CreateIfNotExists();

            return queue;
        }
    }
}