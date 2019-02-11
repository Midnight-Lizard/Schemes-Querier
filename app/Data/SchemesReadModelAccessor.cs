using MidnightLizard.Schemes.Querier.Configuration;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Schemes.Querier.Serialization.Common;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidnightLizard.Schemes.Querier.Data
{
    public interface ISchemesReadModelAccessor : IReadModelAccessor<PublicScheme>
    {
        Task<SearchResults<PublicScheme>> SearchSchemesAsync(SearchOptions options);
    }

    public class SchemesReadModelAccessor : ReadModelAccessor<PublicScheme>, ISchemesReadModelAccessor
    {
        protected override string IndexName => this.config.ELASTIC_SEARCH_SCHEMES_READ_MODEL_INDEX_NAME;
        protected override string TypeName => this.config.ELASTIC_SEARCH_SCHEMES_READ_MODEL_TYPE_NAME;

        public SchemesReadModelAccessor(ElasticSearchConfig config, IModelDeserializer<PublicScheme> modelDeserializer) :
            base(config, modelDeserializer)
        {
        }

        public async Task<SearchResults<PublicScheme>> SearchSchemesAsync(SearchOptions options)
        {
            var shoulds = new List<QueryBase>();
            if (!string.IsNullOrWhiteSpace(options.Query))
            {
                shoulds.Add(new SimpleQueryStringQuery
                {
                    Query = options.Query,
                    Fields = new[] {
                        nameof(PublicScheme.Name).ToUpper() + "^3",
                        nameof(PublicScheme.Description).ToUpper() + "^2",
                        nameof(PublicScheme.PublisherName).ToUpper()
                    },
                    Boost = 1.5
                });
            }
            var filters = new List<QueryBase>();
            if (options.Side != SchemeSide.any)
            {
                var rangeQuery = new NumericRangeQuery
                {
                    Field = $"{nameof(PublicScheme.ColorScheme)}.{nameof(ColorScheme.backgroundLightnessLimit)}".ToUpper(),
                };
                switch (options.Side)
                {
                    case SchemeSide.dark:
                        rangeQuery.LessThan = 30;
                        break;
                    case SchemeSide.light:
                        rangeQuery.GreaterThan = 70;
                        break;
                }
                filters.Add(rangeQuery);
            }

            if (options.Bg != HueFilter.any)
            {
                var bgSatField = $"{nameof(PublicScheme.ColorScheme)}.{nameof(ColorScheme.backgroundGraySaturation)}".ToUpper();

                if (options.Bg == HueFilter.gray)
                {
                    var grayFilter = new NumericRangeQuery
                    {
                        Field = bgSatField,
                        LessThan = 10
                    };
                    filters.Add(grayFilter);
                    var noBlueFilter = new NumericRangeQuery
                    {
                        Field = $"{nameof(PublicScheme.ColorScheme)}.{nameof(ColorScheme.blueFilter)}".ToUpper(),
                        LessThan = 5
                    };
                    filters.Add(noBlueFilter);
                }
                else
                {
                    var notGrayFilter = new NumericRangeQuery
                    {
                        Field = bgSatField,
                        GreaterThan = 1
                    };
                    filters.Add(notGrayFilter);

                    if (options.Bg == HueFilter.red)
                    {
                        var field = $"{nameof(PublicScheme.ColorScheme)}.{nameof(ColorScheme.backgroundGrayHue)}".ToUpper();
                        var redFilter = new BoolQuery
                        {
                            MinimumShouldMatch = 1,
                            Should = new[] {
                            new QueryContainer(new NumericRangeQuery {
                                Field = field,
                                GreaterThan = 330
                            }),
                            new QueryContainer(new NumericRangeQuery {
                                Field = field,
                                LessThan = 30
                            })
                        }
                        };
                        filters.Add(redFilter);
                    }
                    else
                    {
                        var rangeQuery = new NumericRangeQuery
                        {
                            Field = $"{nameof(PublicScheme.ColorScheme)}.{nameof(ColorScheme.backgroundGrayHue)}".ToUpper(),
                        };
                        switch (options.Bg)
                        {
                            case HueFilter.yellow:
                                rangeQuery.GreaterThan = 30;
                                rangeQuery.LessThan = 90;
                                break;
                            case HueFilter.green:
                                rangeQuery.GreaterThan = 90;
                                rangeQuery.LessThan = 150;
                                break;
                            case HueFilter.cyan:
                                rangeQuery.GreaterThan = 150;
                                rangeQuery.LessThan = 210;
                                break;
                            case HueFilter.blue:
                                rangeQuery.GreaterThan = 210;
                                rangeQuery.LessThan = 270;
                                break;
                            case HueFilter.purple:
                                rangeQuery.GreaterThan = 270;
                                rangeQuery.LessThan = 330;
                                break;
                            default:
                                break;
                        }
                        filters.Add(rangeQuery);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(options.CurrentUserId))
            {
                switch (options.List)
                {
                    case SchemeList.my:
                        filters.Add(new MatchPhraseQuery
                        {
                            Field = nameof(PublicScheme.PublisherId).ToUpper(),
                            Query = options.CurrentUserId
                        });
                        break;
                    case SchemeList.fav:
                        filters.Add(new MatchPhraseQuery
                        {
                            Field = nameof(PublicScheme.FavoritedBy).ToUpper(),
                            Query = options.CurrentUserId
                        });
                        break;
                    case SchemeList.liked:
                        filters.Add(new MatchPhraseQuery
                        {
                            Field = nameof(PublicScheme.LikedBy).ToUpper(),
                            Query = options.CurrentUserId
                        });
                        break;
                    default:
                        break;
                }
            }
            else if (new[] { SchemeList.my, SchemeList.fav, SchemeList.liked }.Contains(options.List))
            {
                filters.Add(new MatchNoneQuery());
            }
            if (new[] { SchemeList.com, SchemeList.ml }.Contains(options.List))
            {
                filters.Add(new TermQuery
                {
                    Field = nameof(PublicScheme.PublisherCommunity).ToUpper(),
                    Value = options.List == SchemeList.com
                });
            }
            var results = await this.elasticClient.SearchAsync<PublicScheme>(s =>
            {
                var query = s.Query(q => filters.Count > 0 || shoulds.Count > 0
                    ? q.Bool(cs =>
                    {
                        var boolQuery = cs;
                        if (filters.Count > 0)
                        {
                            boolQuery = boolQuery
                                .Filter(filters.Select(f => new QueryContainer(f)).ToArray());
                        }
                        if (shoulds.Count > 0)
                        {
                            boolQuery = boolQuery
                                .Should(shoulds.Select(f => new QueryContainer(f)).ToArray())
                                .MinimumShouldMatch(MinimumShouldMatch.Fixed(1));
                        }
                        return boolQuery;
                    })
                    : q.MatchAll());
                if (!string.IsNullOrWhiteSpace(options.Cursor))
                {
                    query = query.SearchAfter(Encoding.UTF8
                        .GetString(Convert
                        .FromBase64String(options.Cursor))
                        .Split(',')
                        .ToArray());
                }
                return query
                    .Sort(ss => ss
                        .Descending(SortSpecialField.Score)
                        .Descending(x => x.Likes)
                        .Descending(x => x.Favorites)
                        .Ascending(SortSpecialField.DocumentIndexOrder))
                    .Size(options.PageSize);
            });
            if (results.IsValid)
            {
                return new SearchResults<PublicScheme>
                {
                    Done = results.Documents.Count() < options.PageSize,
                    Results = results.Documents,
                    Cursor = Convert
                        .ToBase64String(Encoding.UTF8
                        .GetBytes(string
                        .Join(',', results.Hits.LastOrDefault()?.Sorts ?? new object[] { })))
                };
            }
            throw new ApplicationException("Failed to query color schemes.",
                results.OriginalException ?? new Exception(results.ServerError.Error.Reason));
        }
    }
}
