namespace MidnightLizard.Schemes.Querier.Models
{
    public abstract class VersionedModel
    {
        public string Id { get; set; }
        public string SchemaVersion { get; set; }
    }
}
