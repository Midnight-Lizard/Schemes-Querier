using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class PublicSchemeType : ObjectGraphType<PublicScheme>
    {
        public PublicSchemeType()
        {
            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.Side);
        }
    }
}
