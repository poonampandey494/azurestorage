using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerExamples
{
   public class MessageQueue
    {
        CloudStorageAccount cloudStorageAccount = null;
        public void SetUp()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection")
                );
        }
        
        public void CreateQueue(string name)
        {
            var queueClient = cloudStorageAccount.CreateCloudQueueClient();
            var queue= queueClient.GetQueueReference(name);
            queue.CreateIfNotExists();
        }

        public void AddMessage(string name,string message)
        {
            var queueClient = cloudStorageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(name);
            queue.CreateIfNotExists();
            var cloudQueueMessage = new CloudQueueMessage(message);
            queue.AddMessage(cloudQueueMessage);
        }


        public void AddMessageWithExpiration(string name, string message,TimeSpan timeToLive)
        {
            var queueClient = cloudStorageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(name);
            queue.CreateIfNotExists();
            var cloudQueueMessage = new CloudQueueMessage(message);
            queue.AddMessage(cloudQueueMessage, timeToLive);
        }

        public CloudQueueMessage ReadMessage(string name)
        {
            var queueClient = cloudStorageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(name);
            queue.CreateIfNotExists();
            var cloudQueueMessage = queue.GetMessage();
            return cloudQueueMessage;
        }


        public CloudQueueMessage PeekMessage(string name)
        {
            var queueClient = cloudStorageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(name);
            queue.CreateIfNotExists();
            var cloudQueueMessage = queue.PeekMessage();
            return cloudQueueMessage;
        }

        public CloudQueueMessage ReadAndDelete(string name)
        {
            var queueClient = cloudStorageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(name);
            queue.CreateIfNotExists();
            var cloudQueueMessage = queue.GetMessage();
            queue.DeleteMessage(cloudQueueMessage);
            return cloudQueueMessage;
        }

    }
}
