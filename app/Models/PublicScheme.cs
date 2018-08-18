namespace MidnightLizard.Schemes.Querier.Models
{
    public class PublicScheme : VersionedModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Side { get; set; }
        public ColorScheme ColorScheme { get; set; }

        public string PublisherId { get; set; }
        public string PublisherName { get; set; }
        public bool PublisherCommunity { get; set; }
    }
}
