using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExamples
{
    public class Table
    {

        CloudStorageAccount cloudStorageAccount = null;
        public void SetUp()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection")
                );
        }

        public void AddTable(string tableName)
        {
            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
        }

        public void AddRow<T>(string tableName, T rowData) where T : TableEntity
        {
            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            var insert = TableOperation.Insert(rowData);
            table.Execute(insert);
        }

        public T GetRow<T>(string tableName, string partitionKey, string rowKey) where T : TableEntity
        {
            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            var retrieve = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = table.Execute(retrieve);
            return (T)result.Result;
        }


        public IEnumerable<T> GetAllRows<T>(string tableName, string partitionKey) where T : TableEntity, new()
        {
            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            var query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return table.ExecuteQuery(query).ToList<T>();
        }


        public void Update<T>(string tableName, T objectData) where T : TableEntity, new()
        {
            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            var updateQuery = TableOperation.Replace(objectData);
            table.Execute(updateQuery);
        }

        public void Delete<T>(string tableName, T objectData) where T : TableEntity, new()
        {
            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            var deleteQuery = TableOperation.Delete(objectData);
            table.Execute(deleteQuery);
        }


        public void BatchInsert<T>(string tableName, List<T> objectDataList) where T : TableEntity, new()
        {
            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            var batchOperation = new TableBatchOperation();
            foreach(var obj in objectDataList)
            {
                batchOperation.Insert(obj);
            }
            table.ExecuteBatch(batchOperation);
        }
    }
}
