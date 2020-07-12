using System;
namespace ShrtnrTableClient.Model.Dto
{
    public class ShrtUrlDto
    {
        public string Url { get; set; }
        public string Code { get; set; }
        public int Clicks { get; set; }

        public ShrtUrlDto FromShrtUrlEntity(ShrtUrlEntity shrtUrlEntity)
        {
            return new ShrtUrlDto()
            {
                Code = shrtUrlEntity.RowKey,
                Url = shrtUrlEntity.Url,
                Clicks = shrtUrlEntity.Clicks
            };
        }

        public ShrtUrlEntity ToShrtUrlEntity()
        {
            return new ShrtUrlEntity(Code)
            {
                Clicks = Clicks,
                Url = Url
            };
        } 
    }
}
