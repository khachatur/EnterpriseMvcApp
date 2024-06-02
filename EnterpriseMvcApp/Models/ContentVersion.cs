namespace EnterpriseMvcApp.Models
{
    public class ContentVersion
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
