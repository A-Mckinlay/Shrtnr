using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using ShrtnrTableClient.Model;
using ShrtnrTableClient.Model.Dto;

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

        public async Task<ShrtUrlEntity> AddShrtUrl(UrlHashPair urlHashPair)
        {
            if (urlHashPair == null) throw new ArgumentNullException("shrtUrlEntity");

            var shrtUrlEntity = new ShrtUrlEntity(urlHashPair.Hash)
            {
                Url = urlHashPair.Url
            };

            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(shrtUrlEntity);

            TableResult result = await _shrtUrlTable.ExecuteAsync(insertOrMergeOperation);
            ShrtUrlEntity insertedShrtUrlEntity = result.Result as ShrtUrlEntity;

            return insertedShrtUrlEntity;
        }

        public async Task<ShrtUrlEntity> LookupShrtUrl(string code)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<ShrtUrlEntity>("findabetterpartitionkey", code);
            TableResult result = await _shrtUrlTable.ExecuteAsync(retrieveOperation);
            ShrtUrlEntity shrtUrlEntity = result.Result as ShrtUrlEntity;

            return shrtUrlEntity;
        }

        public async Task<ShrtUrlEntity> IncrementClicks(ShrtUrlEntity shrtUrlEntity)
        {
            shrtUrlEntity.Clicks += 1;

            TableOperation updateOperation = TableOperation.Merge(shrtUrlEntity);
            TableResult result = await _shrtUrlTable.ExecuteAsync(updateOperation);
            ShrtUrlEntity mergedShrtUrlEntity = result.Result as ShrtUrlEntity;

            return mergedShrtUrlEntity;
        }
    }
}
