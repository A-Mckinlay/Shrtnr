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

        public async Task<ShrtUrlDto> AddShrtUrl(ShrtUrlDto shrtUrlDto)
        {
            if (shrtUrlDto == null) throw new ArgumentNullException("shrtUrlDto");

            var shrtUrlEntity = new ShrtUrlEntity(shrtUrlDto.Code)
            {
                Url = shrtUrlDto.Url
            };

            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(shrtUrlEntity);

            TableResult result = await _shrtUrlTable.ExecuteAsync(insertOrMergeOperation);
            ShrtUrlEntity insertedShrtUrlEntity = result.Result as ShrtUrlEntity;

            return new ShrtUrlDto().FromShrtUrlEntity(insertedShrtUrlEntity);
        }

        public async Task<ShrtUrlEntity> LookupShrtUrl(string code)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<ShrtUrlEntity>("findabetterpartitionkey", code);
            TableResult result = await _shrtUrlTable.ExecuteAsync(retrieveOperation);
            ShrtUrlEntity shrtUrlEntity = result.Result as ShrtUrlEntity;

            return shrtUrlEntity;
        }

        public async Task<ShrtUrlDto> IncrementClicks(ShrtUrlEntity shrtUrlEntity)
        {
            shrtUrlEntity.Clicks += 1;

            TableOperation updateOperation = TableOperation.Merge(shrtUrlEntity);
            TableResult result = await _shrtUrlTable.ExecuteAsync(updateOperation);
            ShrtUrlEntity mergedShrtUrlEntity = result.Result as ShrtUrlEntity;

            return new ShrtUrlDto().FromShrtUrlEntity(mergedShrtUrlEntity);
        }
    }
}
