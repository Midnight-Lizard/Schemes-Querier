using MidnightLizard.Schemes.Querier.Models;
using Newtonsoft.Json;

namespace MidnightLizard.Schemes.Querier.Serialization
{
    public interface IVersinedModelDeserializer
    {

    }

    public interface IVersinedModelDeserializer<out TModel> : IVersinedModelDeserializer where TModel : VersionedModel
    {
        TModel DeserializeModel(string payload, JsonSerializerSettings serializerSettings);
    }
}
