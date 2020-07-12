namespace ShrtnrTableClient.Model
{
    using System.Text;
    using Microsoft.Azure.Cosmos.Table;

    public class ShrtUrlEntity : TableEntity
    {
        public ShrtUrlEntity(string url, string shrtId)
        {
            PartitionKey = EncodePartitionKey(url);
            RowKey = shrtId;
        }

        public int Clicks { get; set; }

        private string EncodePartitionKey(string url)
        {
            var bytes = Encoding.UTF8.GetBytes(url);
            return System.Convert.ToBase64String(bytes);
        }

        public string DecodePartitionKey(string partKey)
        {
            var b64Bytes = System.Convert.FromBase64String(partKey);
            return Encoding.UTF8.GetString(b64Bytes);
        }
    }

}

