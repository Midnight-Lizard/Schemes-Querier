namespace MidnightLizard.Schemes.Querier.Models
{
    public class SearchOptions
    {
        public SchemeList List { get; }
        public SchemeSide Side { get; }
        public string Query { get; }
        public string PublisherId { get; }
        public int PageSize { get; }
        public string Cursor { get; }

        public SearchOptions(
            SchemeList list,
            SchemeSide side,
            string nameFilter,
            string publisherId,
            int pageSize,
            string cursor)
        {
            this.List = list;
            this.Side = side;
            this.Query = nameFilter;
            this.PublisherId = publisherId;
            this.PageSize = pageSize;
            this.Cursor = cursor;
        }
    }
}
