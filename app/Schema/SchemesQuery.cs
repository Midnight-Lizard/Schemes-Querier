using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Schemes.Querier.Schema.Types;

namespace MidnightLizard.Schemes.Querier.Schema
{
    public class SchemesQuery : ObjectGraphType
    {
        public SchemesQuery()
        {
            this.Name = "Schemes";
            this.Field<PublicSchemeType>(
                "details", "Color scheme details",
                new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }),
                resolve: context => new PublicScheme() { Id = "123", Name = "Test", Side = "DARK" });
        }
    }
}