using Autofac;
using MidnightLizard.Schemes.Querier.Data;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Container
{
    public class DataAccessModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SchemesReadModelAccessor>()
                .As<IReadModelAccessor<PublicScheme>>()
                .SingleInstance();
        }
    }
}
