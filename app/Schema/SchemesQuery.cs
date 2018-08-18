using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Data;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Schemes.Querier.Schema.Types;

namespace MidnightLizard.Schemes.Querier.Schema
{
    public class SchemesQuery : ObjectGraphType
    {
        public SchemesQuery(IReadModelAccessor<PublicScheme> accessor)
        {
            this.Name = "Schemes";
            this.FieldAsync<PublicSchemeType, PublicScheme>(
                "details", "Color scheme details",
                new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }),
                resolve: async context => await accessor.ReadModelAsync(context.Arguments["id"].ToString()));
        }
    }
}