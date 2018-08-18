using MidnightLizard.Schemes.Querier.Models;
using Newtonsoft.Json;

namespace MidnightLizard.Schemes.Querier.Serialization
{
    public abstract class AbstractVersionedModelDeserializer<TModel> :
        IVersinedModelDeserializer<TModel> where TModel : VersionedModel
    {
        public virtual TModel DeserializeModel(string payload, JsonSerializerSettings serializerSettings)
        {
            TModel message = JsonConvert.DeserializeObject<TModel>(payload, serializerSettings);
            this.StartAdvancingToTheLatestVersion(message);
            return message;
        }

        public abstract void StartAdvancingToTheLatestVersion(TModel message);

        protected virtual void AdvanceToTheLatestVersion(TModel message) { }
    }
}
