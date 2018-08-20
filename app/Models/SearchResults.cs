using System.Collections.Generic;

namespace MidnightLizard.Schemes.Querier.Models
{
    public class SearchResults<TModel> where TModel : VersionedModel
    {
        public bool Done { get; set; }
        public string Cursor { get; set; }
        public IEnumerable<TModel> Results { get; set; }
    }
}
