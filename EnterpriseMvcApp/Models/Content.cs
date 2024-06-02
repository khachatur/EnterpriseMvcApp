using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseMvcApp.Models
{
    public class Content
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
