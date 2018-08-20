using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class PublicSchemeType : ObjectGraphType<PublicScheme>
    {
        public PublicSchemeType()
        {
            this.Field(x => x.Id);
            this.Field(x => x.Name);
            this.Field(x => x.Description);
            this.Field<SchemeSideEnum>(nameof(PublicScheme.Side),
                "Splits schemes by background lightness into two sides: dark or light",
                resolve: context => context.Source.Side);
            this.Field<ColorSchemeType>(nameof(ColorScheme), "Color scheme",
                resolve: context => context.Source.ColorScheme);
            this.Field<PublisherType>(nameof(Publisher), "Color scheme publisher",
                resolve: context => new Publisher(context.Source));
            this.Field<ListGraphType<ScreenshotType>>(nameof(Screenshot) + "s", "Color scheme screenshots",
                resolve: context => new[] { new Screenshot {
                    Title = "Fake screenshot",
                    Urls = new ScreenshotUrls(randomScreenshots: true)
                } });
        }
    }
}
