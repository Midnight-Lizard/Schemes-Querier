﻿using MidnightLizard.Schemes.Querier.Configuration;
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
        Task<SearchResult<PublicScheme>> SearchSchemesAsync(SearchOptions options);
    }

    public class SchemesReadModelAccessor : ReadModelAccessor<PublicScheme>, ISchemesReadModelAccessor
    {
        protected override string IndexName => this.config.ELASTIC_SEARCH_SCHEMES_READ_MODEL_INDEX_NAME;
        protected override string TypeName => this.config.ELASTIC_SEARCH_SCHEMES_READ_MODEL_TYPE_NAME;

        public SchemesReadModelAccessor(ElasticSearchConfig config, IModelDeserializer<PublicScheme> modelDeserializer) :
            base(config, modelDeserializer)
        {
        }

        public async Task<SearchResult<PublicScheme>> SearchSchemesAsync(SearchOptions options)
        {
            var filters = new List<QueryBase>();
            if (!string.IsNullOrWhiteSpace(options.Query))
            {
                filters.Add(new SimpleQueryStringQuery
                {
                    Query = options.Query,
                    Fields = new[] {
                        nameof(PublicScheme.Name).ToUpper(),
                        nameof(PublicScheme.Description).ToUpper(),
                        nameof(PublicScheme.PublisherName).ToUpper()
                    }
                });
            }
            if (options.Side != SchemeSide.none)
            {
                filters.Add(new TermQuery
                {
                    Field = nameof(PublicScheme.Side).ToUpper(),
                    Value = options.Side.ToString()
                });
            }
            if (!string.IsNullOrWhiteSpace(options.PublisherId))
            {
                switch (options.List)
                {
                    case SchemeList.my:
                        filters.Add(new MatchPhraseQuery
                        {
                            Field = nameof(PublicScheme.PublisherId).ToUpper(),
                            Query = options.PublisherId
                        });
                        break;
                    case SchemeList.fav:
                        break;
                    case SchemeList.liked:
                        break;
                    default:
                        break;
                }
            }
            else if (new[] { SchemeList.my, SchemeList.fav, SchemeList.liked }.Contains(options.List))
            {
                filters.Add(new MatchNoneQuery());
            }
            var results = await this.elasticClient.SearchAsync<PublicScheme>(s =>
            {
                var query = s.Query(q => q.Bool(cs => cs
                    .Filter(filters.Select(f => new QueryContainer(f)).ToArray())));
                if (!string.IsNullOrWhiteSpace(options.Cursor))
                {
                    query = query.SearchAfter(Encoding.UTF8
                        .GetString(Convert
                        .FromBase64String(options.Cursor))
                        .Split(',')
                        .ToArray());
                }
                return query.Sort(ss => ss.Descending(SortSpecialField.Score).Ascending("_id")).Size(options.PageSize);
            });
            if (results.IsValid)
            {
                return new SearchResult<PublicScheme>
                {
                    Models = results.Documents,
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