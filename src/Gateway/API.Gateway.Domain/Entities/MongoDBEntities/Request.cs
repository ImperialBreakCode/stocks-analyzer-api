using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Entities.MongoDBEntities
{
    public class Request
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Controller { get; set; }
        public string? Ip { get; set; }
        public string? Username { get; set; }
        public string? Route { get; set; }
    }
}
