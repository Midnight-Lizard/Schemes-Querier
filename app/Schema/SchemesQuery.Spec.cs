using Autofac.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MidnightLizard.Schemes.Querier.Configuration;
using MidnightLizard.Schemes.Querier.Data;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Testing.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MidnightLizard.Schemes.Querier.Schema
{
    public class SchemesQuerySpec
    {
        private readonly TestServer testServer;
        private readonly HttpClient testClient;
        private readonly ISchemesReadModelAccessor testAccessor;
        private readonly PublicScheme testScheme;

        public SchemesQuerySpec()
        {
            this.testAccessor = Substitute.For<ISchemesReadModelAccessor>();
            this.testServer = new TestServer(new WebHostBuilder()
                .ConfigureServices(services => services.AddAutofac())
                .ConfigureTestServices(services => services
                    .AddSingleton<ISchemesReadModelAccessor>(this.testAccessor))
                .UseSetting(nameof(ElasticSearchConfig.ELASTIC_SEARCH_CLIENT_URL), "http://test.com")
                .UseSetting(nameof(CorsConfig.ALLOWED_ORIGINS), "http://localhost")
                .UseSetting(nameof(AuthConfig.NoErrors), true.ToString())
                .UseSetting(nameof(ScreenshotsConfig.SCREENSHOT_SIZES), "test")
                .UseSetting(nameof(ScreenshotsConfig.SCREENSHOT_URL_TITLES), "test")
                .UseSetting(nameof(ScreenshotsConfig.SCREENSHOT_CDN_ID_TEMPLATE), "test")
                .UseSetting(nameof(ScreenshotsConfig.SCREENSHOT_CDN_URL_TEMPLATE), "test")
                .UseSetting(nameof(ElasticSearchConfig.ELASTIC_SEARCH_SCHEMES_READ_MODEL_INDEX_NAME), "test")
                .UseSetting(nameof(ElasticSearchConfig.ELASTIC_SEARCH_SCHEMES_READ_MODEL_TYPE_NAME), "test")
                .UseStartup<StartupStub>());
            this.testClient = this.testServer.CreateClient();
            this.testScheme = new PublicScheme
            {
                Id = "test-public-scheme-id",
                Name = "test-color-scheme",
                PublisherName = "test-publisher",
                PublisherId = "test-publisher-id",
                ColorScheme = new ColorScheme
                {
                    colorSchemeId = "-",
                    colorSchemeName = "-",
                    mode = "auto",
                    scrollbarStyle = "true",
                    backgroundLightnessLimit = 42,
                    hideBigBackgroundImages = true,
                    maxBackgroundImageSize = 500
                }
            };
        }

        protected static async Task CheckErrors(HttpResponseMessage response)
        {
            var errors = JObject.Parse(await response.Content.ReadAsStringAsync())["errors"];
            var message = "none";
            if (errors != null)
            {
                message = ":\n\n--> " + string.Join("\n\n--> ", errors.Select(x => x["message"])) + "\n\n";
            }
            errors.Should().BeNull(message);
        }

        public class DetailsSpec : SchemesQuerySpec
        {
            private readonly StringContent jsonContent;

            public DetailsSpec() : base()
            {
                var csProps = string.Join(' ', typeof(ColorScheme).GetProperties().Select(p => p.Name));
                var testQuery = $@"query ($id: ID) {{
                                     details(id: $id) {{
                                       id
                                       name
                                       colorScheme {{
                                         {csProps}
                                       }}
                                       publisher {{
                                         id
                                         name
                                         community
                                       }}
                                     }}
                                   }}";
                var json = JsonConvert.SerializeObject(new
                {
                    query = testQuery,
                    variables = new { id = this.testScheme.Id }
                });
                this.jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                this.testAccessor.ReadModelAsync(this.testScheme.Id).Returns(this.testScheme);
            }

            [It(nameof(SchemesQuery) + "/details")]
            public async Task Should_return_OK()
            {
                var result = await this.testClient.PostAsync(Routes.Query, this.jsonContent);
                var x = await result.Content.ReadAsStringAsync();
                result.StatusCode.Should().Be(HttpStatusCode.OK, await result.Content.ReadAsStringAsync());
            }

            [It(nameof(SchemesQuery) + "/details")]
            public async Task Should_not_return_errors()
            {
                var response = await this.testClient.PostAsync(Routes.Query, this.jsonContent);
                await CheckErrors(response);
            }

            [It(nameof(SchemesQuery) + "/details")]
            public async Task Should_call_read_model_accessor()
            {
                var response = await this.testClient.PostAsync(Routes.Query, this.jsonContent);
                await CheckErrors(response);
                await this.testAccessor.Received(1).ReadModelAsync(this.testScheme.Id);
            }

            [It(nameof(SchemesQuery) + "/details")]
            public async Task Should_return_cerrect_json_object()
            {
                var response = await this.testClient.PostAsync(Routes.Query, this.jsonContent);

                await CheckErrors(response);

                var result = JObject.Parse(await response.Content.ReadAsStringAsync())["data"]["details"];

                result["id"].Value<string>().Should().Be(this.testScheme.Id);
                result["name"].Value<string>().Should().Be(this.testScheme.Name);
                result["publisher"]["id"].Value<string>().Should().Be(this.testScheme.PublisherId);

                var colorSchemeJson = result["colorScheme"];
                foreach (var prop in typeof(ColorScheme).GetProperties())
                {
                    var jsonValue = colorSchemeJson[prop.Name].Value<string>() ?? "";
                    var objValue = (prop.GetValue(this.testScheme.ColorScheme) ?? "").ToString();
                    jsonValue.Should().Be(objValue);
                }
            }
        }

        public class SearchSpec : SchemesQuerySpec
        {
            private readonly string testNextCursor;
            private readonly StringContent jsonContent;
            private readonly SearchOptions testSearchOptions;

            public SearchSpec() : base()
            {
                var csProps = string.Join(' ', typeof(ColorScheme).GetProperties().Select(p => p.Name));
                var testQuery = $@"query ($query: String, $side: SchemeSide, $list: SchemeList, $publisherId: ID, $cursor: String, $pageSize: Int) {{
                                     search(query: $query, side: $side, list: $list, publisherId: $publisherId, cursor: $cursor, pageSize: $pageSize) {{
                                       cursor
                                       results {{
                                         id
                                         name
                                         publisher {{
                                           id
                                           name
                                           community
                                         }}
                                         colorScheme {{
                                           {csProps}
                                         }}
                                       }}
                                     }}
                                   }}";
                this.testSearchOptions = new SearchOptions(
                    query: "test search query",
                    side: SchemeSide.dark,
                    list: SchemeList.full,
                    cursor: "MSwwMGE3NGZmYi01OTg1LTQzNTQtOTIyZS0xZDU2NTc0NzYwNDM=",
                    publisherId: "ee769863-e4e8-43f6-8de7-500a203dfb87",
                    pageSize: 100);
                var json = JsonConvert.SerializeObject(new
                {
                    query = testQuery,
                    variables = new
                    {
                        query = this.testSearchOptions.Query,
                        side = this.testSearchOptions.Side.ToString(),
                        list = this.testSearchOptions.List.ToString(),
                        cursor = this.testSearchOptions.Cursor,
                        publisherId = this.testSearchOptions.PublisherId,
                        pageSize = this.testSearchOptions.PageSize
                    }
                });
                this.testNextCursor = "next-cursor";
                this.jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                this.testAccessor.SearchSchemesAsync(Arg.Any<SearchOptions>()).Returns(new SearchResults<PublicScheme>
                {
                    Cursor = testNextCursor,
                    Results = new[] { this.testScheme }
                });
            }

            [It(nameof(SchemesQuery) + "/search")]
            public async Task Should_return_OK()
            {
                var result = await this.testClient.PostAsync(Routes.Query, this.jsonContent);
                result.StatusCode.Should().Be(HttpStatusCode.OK, await result.Content.ReadAsStringAsync());
            }

            [It(nameof(SchemesQuery) + "/search")]
            public async Task Should_call_read_model_accessor()
            {
                var response = await this.testClient.PostAsync(Routes.Query, this.jsonContent);
                await CheckErrors(response);
                await this.testAccessor.Received(1).SearchSchemesAsync(Arg.Is<SearchOptions>(opt =>
                    opt.Cursor == this.testSearchOptions.Cursor &&
                    opt.List == this.testSearchOptions.List &&
                    opt.PageSize == this.testSearchOptions.PageSize &&
                    opt.PublisherId == this.testSearchOptions.PublisherId &&
                    opt.Query == this.testSearchOptions.Query &&
                    opt.Side == this.testSearchOptions.Side));
            }

            [It(nameof(SchemesQuery) + "/search")]
            public async Task Should_return_cerrect_json_object()
            {
                var response = await this.testClient.PostAsync(Routes.Query, this.jsonContent);
                await CheckErrors(response);
                var result = JObject.Parse(await response.Content.ReadAsStringAsync())["data"]["search"];
                result["cursor"].Value<string>().Should().Be(this.testNextCursor);
                var scheme = result["results"][0];
                scheme["id"].Value<string>().Should().Be(this.testScheme.Id);
                scheme["name"].Value<string>().Should().Be(this.testScheme.Name);
                scheme["publisher"]["id"].Value<string>().Should().Be(this.testScheme.PublisherId);

                var colorSchemeJson = scheme["colorScheme"];
                foreach (var prop in typeof(ColorScheme).GetProperties())
                {
                    var jsonValue = colorSchemeJson[prop.Name].Value<string>() ?? "";
                    var objValue = (prop.GetValue(this.testScheme.ColorScheme) ?? "").ToString();
                    jsonValue.Should().Be(objValue);
                }
            }
        }
    }
}
