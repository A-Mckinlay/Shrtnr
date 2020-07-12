namespace ShrtnrTableClient.Model
{
    using Microsoft.Azure.Cosmos.Table;

    public class ShrtUrlEntity : TableEntity
    {
        public ShrtUrlEntity()
        {
        }

        public ShrtUrlEntity(string shrtId)
        {
            PartitionKey = "findabetterpartitionkey";
            RowKey = shrtId;
        }

        public string Url { get; set; }
        public int Clicks { get; set; }
    }

}

