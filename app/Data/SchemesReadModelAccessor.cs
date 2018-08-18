using MidnightLizard.Schemes.Querier.Configuration;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Schemes.Querier.Serialization.Common;

namespace MidnightLizard.Schemes.Querier.Data
{
    public class SchemesReadModelAccessor : ReadModelAccessor<PublicScheme>
    {
        protected override string IndexName => this.config.ELASTIC_SEARCH_SCHEMES_READ_MODEL_INDEX_NAME;
        protected override string TypeName => this.config.ELASTIC_SEARCH_SCHEMES_READ_MODEL_TYPE_NAME;

        public SchemesReadModelAccessor(ElasticSearchConfig config, IModelDeserializer<PublicScheme> modelDeserializer) :
            base(config, modelDeserializer)
        {
        }
    }
}
