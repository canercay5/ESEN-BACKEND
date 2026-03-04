using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ESEN.Domain.Entities
{
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public bool IsDeleted { get; protected set; } = false;
        public void Delete() => IsDeleted = true;
    }
}