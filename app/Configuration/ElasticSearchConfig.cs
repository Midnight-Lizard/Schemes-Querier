namespace MidnightLizard.Schemes.Querier.Configuration
{
    public class ElasticSearchConfig
    {
        public string ELASTIC_SEARCH_CLIENT_URL { get; set; }

        public string ELASTIC_SEARCH_SCHEMES_READ_MODEL_INDEX_NAME { get; set; }
        public string ELASTIC_SEARCH_SCHEMES_READ_MODEL_TYPE_NAME { get; set; }
    }
}