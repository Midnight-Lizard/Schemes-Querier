using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class PublisherType : ObjectGraphType<Publisher>
    {
        public PublisherType()
        {
            this.Field(x => x.Id);
            this.Field(x => x.Name);
            this.Field(x => x.Community);
        }
    }
}
