namespace MidnightLizard.Schemes.Querier.Models
{
    public class PublicScheme
    {
        public string Id { get; set; }
        public string SchemaVersion { get; set; }
        public string Name { get; set; }
        public string Side { get; set; }
        public ColorScheme ColorScheme { get; set; }

        public string PublisherId { get; set; }
        public string PublisherName { get; set; }
        public string PublisherCommunity { get; set; }
    }
}
