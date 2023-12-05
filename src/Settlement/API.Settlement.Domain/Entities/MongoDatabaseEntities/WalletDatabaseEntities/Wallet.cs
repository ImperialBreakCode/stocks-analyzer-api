using API.Settlement.Domain.Enums;
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

        [BsonElement("user_email")]
        public string UserEmail { get; set; }

        [BsonElement("user_rank")]
		public UserRank UserRank { get; set; }

		[BsonElement("stocks")]
		public IEnumerable<Stock> Stocks { get; set; } = Enumerable.Empty<Stock>();
    }
}