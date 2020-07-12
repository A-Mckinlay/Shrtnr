using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using ShrtnrTableClient.Model;

namespace ShrtnrTableClient.Repository
{
    public class ShrtUrlRepo : IShrtUrlRepo
    {
        private readonly CloudTable _shrtUrlTable;

        public ShrtUrlRepo(CloudStorageAccount storageAccount)
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 3);
            _shrtUrlTable = tableClient.GetTableReference("ShrtnrTable");
        }

        public async Task<ShrtUrlEntity> AddShrtUrl(ShrtUrlEntity shrtUrlEntity)
        {
            if (shrtUrlEntity == null) throw new ArgumentNullException("shrtUrlEntity");

            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(shrtUrlEntity);

            TableResult result = await _shrtUrlTable.ExecuteAsync(insertOrMergeOperation);
            ShrtUrlEntity insertedShrtUrlEntity = result.Result as ShrtUrlEntity;

            return insertedShrtUrlEntity;
        }
    }
}
