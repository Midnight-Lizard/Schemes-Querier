﻿using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Data;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Schemes.Querier.Schema.Types;
using System.Linq;

namespace MidnightLizard.Schemes.Querier.Schema
{
    public class SchemesQuery : ObjectGraphType
    {
        public SchemesQuery(ISchemesReadModelAccessor accessor)
        {
            this.Name = "Schemes";

            this.FieldAsync<PublicSchemeType, PublicScheme>(
                "details", "Get Midnight Lizard's color scheme details",
                new QueryArguments(new QueryArgument<IdGraphType> { Name = nameof(PublicScheme.Id) }),
                resolve: async context => await accessor.ReadModelAsync(context.Arguments.Values.First() as string));

            this.FieldAsync<SearchResultType<PublicScheme, PublicSchemeType>, SearchResult<PublicScheme>>(
                "search", "Search Midnight Lizard's color schemes",
                new QueryArguments(
                    new QueryArgument<SchemeListEnum> { Name = nameof(SearchOptions.List), DefaultValue = SchemeList.full },
                    new QueryArgument<SchemeSideEnum> { Name = nameof(SearchOptions.Side), DefaultValue = SchemeSide.none },
                    new QueryArgument<StringGraphType> { Name = nameof(SearchOptions.Query), DefaultValue = "" },
                    new QueryArgument<IdGraphType> { Name = nameof(SearchOptions.PublisherId), DefaultValue = "" },
                    new QueryArgument<IntGraphType> { Name = nameof(SearchOptions.PageSize), DefaultValue = 10 },
                    new QueryArgument<StringGraphType> { Name = nameof(SearchOptions.Cursor), DefaultValue = "" }
                ),
                resolve: async context => await accessor.SearchSchemesAsync(new SearchOptions(
                   list: (SchemeList)context.Arguments[this.ToCamelCase(nameof(SearchOptions.List))],
                   side: (SchemeSide)context.Arguments[this.ToCamelCase(nameof(SearchOptions.Side))],
                   nameFilter: context.Arguments[this.ToCamelCase(nameof(SearchOptions.Query))] as string,
                   publisherId: context.Arguments[this.ToCamelCase(nameof(SearchOptions.PublisherId))] as string,
                   pageSize: (int)context.Arguments[this.ToCamelCase(nameof(SearchOptions.PageSize))],
                   cursor: context.Arguments[this.ToCamelCase(nameof(SearchOptions.Cursor))] as string
                )));
        }

        private string ToCamelCase(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
    }
}