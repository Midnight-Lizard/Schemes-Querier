using GraphQL.Types;
using Microsoft.Extensions.Options;
using MidnightLizard.Schemes.Querier.Configuration;
using MidnightLizard.Schemes.Querier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class PublicSchemeType : ObjectGraphType<PublicScheme>
    {
        private readonly ScreenshotsConfig config;
        private readonly IEnumerable<(string text, string kebab)> titles;
        private readonly Dictionary<string, string> sizes;

        public PublicSchemeType(IOptions<ScreenshotsConfig> screenshotsConfig)
        {
            this.config = screenshotsConfig.Value;
            this.titles = this.GetTitles();
            this.sizes = this.GetSizes();

            this.Field(x => x.Id);
            this.Field(x => x.Name);
            this.Field(x => x.Description);
            this.Field(x => x.Generation);
            this.Field(x => x.Likes, nullable: true);
            this.Field(x => x.LikedBy, nullable: true);
            this.Field(x => x.Favorites, nullable: true);
            this.Field(x => x.FavoritedBy, nullable: true);

            this.Field<BooleanGraphType>("liked", "True if liked by the specified user", new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "by", DefaultValue = null }
                ), resolve: context =>
            {
                var userId = context.Arguments["by"] as string;
                return !string.IsNullOrEmpty(userId) &&
                    context.Source.LikedBy != null &&
                    context.Source.LikedBy.Contains(userId);
            });

            this.Field<BooleanGraphType>("favorited", "True if favorited by the specified user", new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "by", DefaultValue = null }
                ), resolve: context =>
            {
                var userId = context.Arguments["by"] as string;
                return !string.IsNullOrEmpty(userId) &&
                    context.Source.FavoritedBy != null &&
                    context.Source.FavoritedBy.Contains(userId);
            });

            this.Field<ColorSchemeType>(nameof(ColorScheme), "Color scheme",
                resolve: context => context.Source.ColorScheme);

            this.Field<PublisherType>(nameof(Publisher), "Color scheme publisher",
                resolve: context => new Publisher(context.Source));

            this.Field<ListGraphType<ScreenshotType>>(nameof(Screenshot) + "s", "Color scheme screenshots",
                resolve: context => this.CreateScreenshots(context.Source));
        }

        public Screenshot[] CreateScreenshots(PublicScheme publicScheme)
        {
            return (
                from title in this.titles
                select new Screenshot
                {
                    Title = title.text,
                    Urls = new ScreenshotUrls
                    {
                        xs = this.GetScreenshotUrl(publicScheme, title.kebab, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.xs))),
                        sm = this.GetScreenshotUrl(publicScheme, title.kebab, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.sm))),
                        md = this.GetScreenshotUrl(publicScheme, title.kebab, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.md))),
                        lg = this.GetScreenshotUrl(publicScheme, title.kebab, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.lg))),
                        xl = this.GetScreenshotUrl(publicScheme, title.kebab, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.xl)))
                    }
                }
            ).ToArray();
        }

        private string GetScreenshotUrl(PublicScheme publicScheme, string title, string size)
        {
            return this.config.SCREENSHOT_CDN_URL_TEMPLATE
                .Replace("{id}", this.config.SCREENSHOT_CDN_ID_TEMPLATE
                .Replace("{id}", publicScheme.Id)
                .Replace("{title}", title)
                .Replace("{size}", size));
        }

        private Dictionary<string, string> GetSizes()
        {
            return Regex.Matches(this.config.SCREENSHOT_SIZES, "(\\w{2}):(\\d+x\\d+)x\\d+,?")
                .ToDictionary(x => x.Groups[1].Value, x => x.Groups[2].Value);
        }

        private IEnumerable<(string text, string kebab)> GetTitles()
        {
            return this.config.SCREENSHOT_URL_TITLES
                .Split(',', '~', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => (x, Regex.Replace(x, "\\s", "-").ToLower()));
        }
    }
}
