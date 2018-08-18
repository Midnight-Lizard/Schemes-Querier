using Autofac;
using MidnightLizard.Schemes.Querier.Serialization;
using MidnightLizard.Schemes.Querier.Serialization.Common;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MidnightLizard.Schemes.Querier.Container
{
    public class ModelDeserializationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ModelDeserializer<>))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(ModelDeserializationModule).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IVersinedModelDeserializer<>))
                .As<IVersinedModelDeserializer>()
                .WithMetadata(t =>
                {
                    var msgAttr = t.GetCustomAttribute<ModelAttribute>();
                    return new Dictionary<string, object>
                    {
                        [nameof(IModelMetadata.Type)] = msgAttr.Type ?? t.GetInterfaces().First().GetGenericArguments()[0].Name,
                        [nameof(IModelMetadata.VersionRange)] = msgAttr.VersionRange
                    };
                })
                .SingleInstance();
        }
    }
}
