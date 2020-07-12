namespace ShrtnrTableClient.Model
{
    using Microsoft.Azure.Cosmos.Table;

    public class ShrtUrlEntity : TableEntity
    {
        public ShrtUrlEntity(string url, string shrtId)
        {
            PartitionKey = url;
            RowKey = shrtId;
        }

        public int clicks { get; set; }
    }
}
