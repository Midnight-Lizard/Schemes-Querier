using Elasticsearch.Net;
using MidnightLizard.Schemes.Querier.Configuration;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Schemes.Querier.Serialization.Common;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MidnightLizard.Schemes.Querier.Data
{
    public interface IReadModelAccessor<TModel> where TModel : VersionedModel
    {
        Task<TModel> ReadModelAsync(string modelId, CancellationToken cancellationToken = default);
    }

    public abstract class ReadModelAccessor<TModel> : IReadModelAccessor<TModel> where TModel : VersionedModel
    {
        protected readonly ElasticSearchConfig config;
        protected readonly IModelDeserializer<TModel> modelDeserializer;
        protected readonly ElasticClient elasticClient;
        protected abstract string IndexName { get; }
        protected abstract string TypeName { get; }

        public ReadModelAccessor(ElasticSearchConfig config, IModelDeserializer<TModel> modelDeserializer)
        {
            this.config = config;
            this.modelDeserializer = modelDeserializer;
            this.elasticClient = this.CreateElasticClient();
        }

        protected virtual ElasticClient CreateElasticClient()
        {
            var node = new Uri(this.config.ELASTIC_SEARCH_CLIENT_URL);
            return new ElasticClient(this.InitDefaultMapping(new ConnectionSettings(
                new SingleNodeConnectionPool(node),
                (builtin, settings) => new ModelElasticsearchlSerializer<TModel>(this.modelDeserializer))));
        }

        protected virtual ConnectionSettings InitDefaultMapping(ConnectionSettings connectionSettings)
        {
            return connectionSettings
                .DefaultFieldNameInferrer(i => i/*.ToUpper()*/)
                .DefaultMappingFor<TModel>(map => map
                     .IdProperty(to => to.Id)
                     .RoutingProperty(x => x.Id)
                     .IndexName(this.config.ELASTIC_SEARCH_SCHEMES_READ_MODEL_INDEX_NAME)
                     .TypeName(this.TypeName));
        }

        public async Task<TModel> ReadModelAsync(string modelId, CancellationToken cancellationToken = default)
        {
            var result = await this.elasticClient.GetAsync<TModel>(
                new DocumentPath<TModel>(modelId),
                cancellationToken: cancellationToken);
            if (!result.IsValid)
            {
                throw new ApplicationException("Failed to read model from the store",
                    result.OriginalException ?? new Exception(result.ServerError.Error.Reason));
            }
            return result.Source;
        }
    }
}
