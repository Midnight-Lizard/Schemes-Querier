using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class SchemeListEnum : EnumerationGraphType<SchemeList>
    {
        public SchemeListEnum()
        {
            Description = "Splits schemes by background lightness into two sides: dark or light";
        }
    }
}
