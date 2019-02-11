using System;

namespace MidnightLizard.Schemes.Querier.Models
{
    public class SearchOptions
    {
        public SchemeList List { get; }
        public SchemeSide Side { get; }
        public HueFilter Bg { get; }
        public string Query { get; }
        public string CurrentUserId { get; }
        [Obsolete(nameof(CurrentUserId))]
        public string PublisherId { get; }
        public int PageSize { get; }
        public string Cursor { get; }

        public SearchOptions(
            SchemeList list,
            SchemeSide side,
            HueFilter bg,
            string query,
            string currentUserId,
            int pageSize,
            string cursor)
        {
            this.List = list;
            this.Side = side;
            this.Bg = bg;
            this.Query = query;
            this.CurrentUserId = currentUserId;
            this.PageSize = pageSize;
            this.Cursor = cursor;
        }
    }
}
