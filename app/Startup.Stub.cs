using Autofac;
using Microsoft.Extensions.Configuration;
using MidnightLizard.Schemes.Querier.Container;

namespace MidnightLizard.Schemes.Querier
{
    public class StartupStub : Startup
    {
        public StartupStub(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<GraphQLSchemaModule>();
            builder.RegisterModule<ModelDeserializationModule>();
        }
    }
}
