using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentGateway.Api.Models
{
    public class Currency
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string CC { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}