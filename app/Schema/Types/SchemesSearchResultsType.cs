using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class SchemesSearchResultsType : SearchResultsType<PublicScheme, PublicSchemeType>
    {
        public SchemesSearchResultsType()
        {
            this.Name = "SchemesSearchResults";
        }
    }
}
