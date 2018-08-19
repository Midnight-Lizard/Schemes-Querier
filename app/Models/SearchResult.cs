using System.Collections.Generic;

namespace MidnightLizard.Schemes.Querier.Models
{
    public class SearchResult<TModel> where TModel : VersionedModel
    {
        public string Cursor { get; set; }
        public IEnumerable<TModel> Models { get; set; }
    }
}
