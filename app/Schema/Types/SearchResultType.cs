using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class SearchResultType<TModel, TModelType> : ObjectGraphType<SearchResult<TModel>>
        where TModel : VersionedModel
        where TModelType : ObjectGraphType<TModel>
    {
        public SearchResultType()
        {
            this.Field(x => x.Cursor, nullable: true);
            this.Field<ListGraphType<TModelType>>(
                nameof(SearchResult<TModel>.Models),
                resolve: context => context.Source.Models);
        }
    }
}
