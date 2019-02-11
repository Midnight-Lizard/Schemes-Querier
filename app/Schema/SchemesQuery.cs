using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Data;
using MidnightLizard.Schemes.Querier.Models;
using MidnightLizard.Schemes.Querier.Schema.Types;
using System;

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
                resolve: async context => await accessor.ReadModelAsync(
                    context.Arguments[this.ToCamelCase(nameof(PublicScheme.Id))] as string));

            this.FieldAsync<SchemesSearchResultsType, SearchResults<PublicScheme>>(
                "search", "Search Midnight Lizard's color schemes",
                new QueryArguments(
                    new QueryArgument<SchemeListEnum> { Name = nameof(SearchOptions.List), DefaultValue = SchemeList.full.ToString() },
                    new QueryArgument<SchemeSideEnum> { Name = nameof(SearchOptions.Side), DefaultValue = SchemeSide.any.ToString() },
                    new QueryArgument<HueFilterEnum> { Name = nameof(SearchOptions.Bg), DefaultValue = HueFilter.any.ToString() },
                    new QueryArgument<StringGraphType> { Name = nameof(SearchOptions.Query), DefaultValue = "" },
                    new QueryArgument<IdGraphType> { Name = nameof(SearchOptions.CurrentUserId), DefaultValue = "" },
                    new QueryArgument<IntGraphType> { Name = nameof(SearchOptions.PageSize), DefaultValue = 10 },
                    new QueryArgument<StringGraphType> { Name = nameof(SearchOptions.Cursor), DefaultValue = "" }
                ),
                resolve: async context => await accessor.SearchSchemesAsync(new SearchOptions(
                   list: Enum.Parse<SchemeList>(context.Arguments[this.ToCamelCase(nameof(SearchOptions.List))].ToString()),
                   side: Enum.Parse<SchemeSide>(context.Arguments[this.ToCamelCase(nameof(SearchOptions.Side))].ToString()),
                   bg: Enum.Parse<HueFilter>(context.Arguments[this.ToCamelCase(nameof(SearchOptions.Bg))].ToString()),
                   query: context.Arguments[this.ToCamelCase(nameof(SearchOptions.Query))] as string,
                   currentUserId: context.Arguments[this.ToCamelCase(nameof(SearchOptions.CurrentUserId))] as string,
                   pageSize: Math.Min((int)context.Arguments[this.ToCamelCase(nameof(SearchOptions.PageSize))], 1000),
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