using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public abstract class SearchResultsType<TModel, TModelType> : ObjectGraphType<SearchResults<TModel>>
        where TModel : VersionedModel
        where TModelType : ObjectGraphType<TModel>
    {
        public SearchResultsType()
        {
            this.Field(x => x.Done);
            this.Field(x => x.Cursor, nullable: true);
            this.Field<ListGraphType<TModelType>>(
                nameof(SearchResults<TModel>.Results),
                resolve: context => context.Source.Results);
        }
    }
}
