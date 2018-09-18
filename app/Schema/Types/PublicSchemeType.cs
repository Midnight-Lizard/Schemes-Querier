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
        private readonly string[] titles;
        private readonly Dictionary<string, string> sizes;

        public PublicSchemeType(IOptions<ScreenshotsConfig> screenshotsConfig)
        {
            this.config = screenshotsConfig.Value;
            this.titles = this.GetTitles();
            this.sizes = this.GetSizes();

            this.Field(x => x.Id);
            this.Field(x => x.Name);
            this.Field(x => x.Description);
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
                    Title = title,
                    Urls = new ScreenshotUrls
                    {
                        xs = this.GetScreenshotUrl(publicScheme, title, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.xs))),
                        sm = this.GetScreenshotUrl(publicScheme, title, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.sm))),
                        md = this.GetScreenshotUrl(publicScheme, title, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.md))),
                        lg = this.GetScreenshotUrl(publicScheme, title, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.lg))),
                        xl = this.GetScreenshotUrl(publicScheme, title, this.sizes.GetValueOrDefault(nameof(ScreenshotUrls.xl)))
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

        private string[] GetTitles()
        {
            return this.config.SCREENSHOT_URL_TITLES
                .Split(',', '~', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Regex.Replace(x, "\\s", "-").ToLower())
                .ToArray();
        }
    }
}
