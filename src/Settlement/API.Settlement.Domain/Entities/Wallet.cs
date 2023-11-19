using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Settlement.Domain.Entities
{
    public class Wallet
    {
        [BsonId]
        [BsonElement("_id")]
        public string WalletId { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }

		[BsonElement("stocks")]
		public IEnumerable<Stock> Stocks { get; set; } = Enumerable.Empty<Stock>();
    }
}