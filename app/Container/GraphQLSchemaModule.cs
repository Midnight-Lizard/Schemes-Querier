using Autofac;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using MidnightLizard.Schemes.Querier.Schema;
using MidnightLizard.Schemes.Querier.Schema.Types;

namespace MidnightLizard.Schemes.Querier.Container
{
    public class GraphQLSchemaModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SchemesQuerierSchema>().As<ISchema>();
            builder.RegisterType<SchemesQuery>().AsSelf();
            builder.RegisterType<PublicSchemeType>().AsSelf();
            builder.RegisterType<PublisherType>().AsSelf();
            builder.RegisterType<ColorSchemeType>().AsSelf();
            builder.RegisterType<SchemeSideEnum>().AsSelf();
            builder.RegisterType<SchemeListEnum>().AsSelf();
            builder.RegisterType<SchemesSearchResultsType>().AsSelf();
        }
    }
}
