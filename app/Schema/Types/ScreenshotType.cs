using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class ScreenshotType : ObjectGraphType<Screenshot>
    {
        public ScreenshotType()
        {
            this.Field(x => x.Title);
            this.Field(x => x.Urls, type: typeof(ScreenshotUrlsType));
        }
    }

    public class ScreenshotUrlsType : ObjectGraphType<ScreenshotUrls>
    {
        public ScreenshotUrlsType()
        {
            this.Field(x => x.xs);
            this.Field(x => x.sm);
            this.Field(x => x.md);
            this.Field(x => x.lg);
            this.Field(x => x.xl);
        }
    }
}
