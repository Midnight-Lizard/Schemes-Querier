using FluentAssertions;
using Microsoft.Extensions.Options;
using MidnightLizard.Schemes.Querier.Configuration;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Testing.Utilities;
using NSubstitute;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class PublicSchemeTypeSpec
    {
        private readonly PublicSchemeType publicSchemeType;
        private readonly PublicScheme testPublicScheme = new PublicScheme { Id = "test-id" };
        private readonly ScreenshotsConfig screenshotsConfig = new ScreenshotsConfig
        {
            SCREENSHOT_SIZES = "xl:1280x800x200,md:960x600x200,xs:640x400x200,",
            SCREENSHOT_URL_TITLES = "Google Search,Google Search Images,",
            SCREENSHOT_CDN_ID_TEMPLATE = "test/{id}/{title}/{size}",
            SCREENSHOT_CDN_URL_TEMPLATE = "https://test.com/xxx/{id}.png"
        };

        public PublicSchemeTypeSpec()
        {
            var screenshotsConfigOptions = Substitute.For<IOptions<ScreenshotsConfig>>();
            screenshotsConfigOptions.Value.Returns(this.screenshotsConfig);

            this.publicSchemeType = new PublicSchemeType(screenshotsConfigOptions);
        }

        public class CreateScreenshotsSpec : PublicSchemeTypeSpec
        {
            [It(nameof(PublicSchemeType.CreateScreenshots))]
            public void Should_return_screenshots_for_each_title()
            {
                var results = this.publicSchemeType.CreateScreenshots(this.testPublicScheme);
                results.Should().HaveCount(2);
            }

            [It(nameof(PublicSchemeType.CreateScreenshots))]
            public void Should_return_screenshots_with_correct_urls()
            {
                var results = this.publicSchemeType.CreateScreenshots(this.testPublicScheme);
                results[0].Urls.xl.Should().Be("https://test.com/xxx/test/test-id/google-search/1280x800.png");
            }
        }
    }
}
