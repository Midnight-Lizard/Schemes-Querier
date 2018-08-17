using GraphQL;

namespace MidnightLizard.Schemes.Querier.Schema
{
    public class SchemesQuerierSchema : GraphQL.Types.Schema
    {
        public SchemesQuerierSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            this.Query = resolver.Resolve<SchemesQuery>();
        }
    }
}
