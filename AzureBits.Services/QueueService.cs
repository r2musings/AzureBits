using System.Threading.Tasks;
using AzureBits.Core.Extensions;
using AzureBits.Core.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureBits.Web.Services
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

        public Task AddMessageToQueueAsync(string messageId, T messageObject)
        {
            var queue = GetQueue();

            // serialize the payload for the message
            var serializedMessage = messageObject.SerializeToByteArray();
            
            // Create the actual queue message 
            CloudQueueMessage message = new CloudQueueMessage(serializedMessage);

            // Add the message to the queue
            return queue.AddMessageAsync(message);
        }

        public T GetNextMessageFromQueue()
        {
            var queue = GetQueue();

            // Get the next message
            CloudQueueMessage retrievedMessage = queue.GetMessage();

            // deserialize to uploadedImage
            T message = retrievedMessage.AsBytes.Deserialize<T>();

            // delete the message from the queue
            queue.DeleteMessage(retrievedMessage);

            return message;
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