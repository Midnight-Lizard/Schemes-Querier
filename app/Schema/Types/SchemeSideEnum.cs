using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class SchemeSideEnum : EnumerationGraphType<SchemeSide>
    {
        public SchemeSideEnum()
        {
            this.Description = "Splits schemes by background lightness into two sides: dark or light";
            this.AddValue(nameof(SchemeSide.none), "", nameof(SchemeSide.none));
            this.AddValue(nameof(SchemeSide.dark), "", nameof(SchemeSide.dark));
            this.AddValue(nameof(SchemeSide.light), "", nameof(SchemeSide.light));
        }
    }
}
