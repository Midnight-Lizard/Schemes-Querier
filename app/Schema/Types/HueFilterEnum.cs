using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class HueFilterEnum : EnumerationGraphType
    {
        public HueFilterEnum()
        {
            this.Name = nameof(HueFilter);
            this.Description = "Filters color schemes by background hue segments";
            this.AddValue(nameof(HueFilter.any), "", nameof(HueFilter.any));
            this.AddValue(nameof(HueFilter.red), "", nameof(HueFilter.red));
            this.AddValue(nameof(HueFilter.yellow), "", nameof(HueFilter.yellow));
            this.AddValue(nameof(HueFilter.green), "", nameof(HueFilter.green));
            this.AddValue(nameof(HueFilter.cyan), "", nameof(HueFilter.cyan));
            this.AddValue(nameof(HueFilter.blue), "", nameof(HueFilter.blue));
            this.AddValue(nameof(HueFilter.purple), "", nameof(HueFilter.purple));
            this.AddValue(nameof(HueFilter.gray), "", nameof(HueFilter.gray));
        }
    }
}
