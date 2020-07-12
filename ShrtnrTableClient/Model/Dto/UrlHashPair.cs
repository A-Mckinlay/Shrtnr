using System;
namespace ShrtnrTableClient.Model.Dto
{
    public class UrlHashPair
    {
        public string Url { get; set; }
        public string Hash { get; set; }

        public UrlHashPair FromShrtUrlEntity(ShrtUrlEntity shrtUrlEntity)
        {
            return new UrlHashPair()
            {
                Hash = shrtUrlEntity.RowKey,
                Url = shrtUrlEntity.Url
            };
        }
    }
}
