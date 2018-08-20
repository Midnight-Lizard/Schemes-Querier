using Autofac.Features.Metadata;
using MidnightLizard.Schemes.Querier.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SemVer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MidnightLizard.Schemes.Querier.Serialization.Common
{
    public interface IModelDeserializer<TModel> where TModel : VersionedModel
    {
        TModel Deserialize(string message);
    }

    public class ModelDeserializer<TModel> : IModelDeserializer<TModel> where TModel : VersionedModel
    {
        private readonly IEnumerable<Meta<Lazy<IVersinedModelDeserializer>>> versionedDeserializers;
        private readonly string modelTypeName;
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        public ModelDeserializer(IEnumerable<Meta<Lazy<IVersinedModelDeserializer>>> deserializers)
        {
            this.versionedDeserializers = deserializers;
            this.modelTypeName = typeof(TModel).Name;
        }

        public virtual TModel Deserialize(string message)
        {
            try
            {
                var version = JsonConvert.DeserializeObject<VersionedModel>(message).SchemaVersion;
                var deserializer = this.versionedDeserializers.FirstOrDefault(x =>
                    x.Metadata[nameof(Type)] as string == this.modelTypeName &&
                    (x.Metadata["VersionRange"] as Range).IsSatisfied(version));
                if (deserializer != null)
                {
                    return (deserializer.Value.Value as IVersinedModelDeserializer<TModel>)
                        .DeserializeModel(message, this.serializerSettings);
                }
                throw new ApplicationException(
                    $"Deserializer for message type {this.modelTypeName} and version {version} is not found.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to deserialize read model", ex);
            }
        }
    }
}
