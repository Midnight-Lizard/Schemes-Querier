namespace MidnightLizard.Schemes.Querier.Models
{
    public class Publisher
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Community { get; set; }

        public Publisher()
        {
        }

        public Publisher(PublicScheme scheme)
        {
            this.Id = scheme.PublisherId;
            this.Name = scheme.PublisherName;
            this.Community = scheme.PublisherCommunity;
        }
    }
}
