﻿using Elasticsearch.Net;
using MidnightLizard.Schemes.Querier.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MidnightLizard.Schemes.Querier.Serialization.Common
{
    public class ModelElasticsearchlSerializer<TModel> : IElasticsearchSerializer where TModel : VersionedModel
    {
        private readonly IModelDeserializer<TModel> modelDeserializer;

        public ModelElasticsearchlSerializer(IModelDeserializer<TModel> modelDeserializer)
        {
            this.modelDeserializer = modelDeserializer;
        }

        public object Deserialize(Type type, Stream stream)
        {
            if (typeof(TModel).IsAssignableFrom(type))
            {
                using (var reader = new StreamReader(stream))
                {
                    var result = this.modelDeserializer.Deserialize(reader.ReadToEnd());
                    return result;
                }
            }
            return null;
        }

        public T Deserialize<T>(Stream stream)
        {
            return (T)this.Deserialize(typeof(T), stream);
        }

        public async Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
        {
            if (typeof(TModel).IsAssignableFrom(type))
            {
                using (var reader = new StreamReader(stream))
                {
                    var result = this.modelDeserializer.Deserialize(await reader.ReadToEndAsync());
                    return result;
                }
            }
            return null;
        }

        public async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            return (T)await this.DeserializeAsync(typeof(T), stream, cancellationToken);
        }

        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.Indented)
        {
            var json = this.SerializeToString(data);
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }
        }

        public async Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.Indented, CancellationToken cancellationToken = default)
        {
            var json = this.SerializeToString(data);
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(json);
            }
        }

        private string SerializeToString<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
