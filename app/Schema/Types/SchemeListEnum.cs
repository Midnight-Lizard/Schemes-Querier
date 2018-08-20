using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class SchemeListEnum : EnumerationGraphType
    {
        public SchemeListEnum()
        {
            this.Name = nameof(SchemeList);
            this.AddValue(nameof(SchemeList.com), "", nameof(SchemeList.com));
            this.AddValue(nameof(SchemeList.empty), "", nameof(SchemeList.empty));
            this.AddValue(nameof(SchemeList.fav), "", nameof(SchemeList.fav));
            this.AddValue(nameof(SchemeList.full), "", nameof(SchemeList.full));
            this.AddValue(nameof(SchemeList.liked), "", nameof(SchemeList.liked));
            this.AddValue(nameof(SchemeList.ml), "", nameof(SchemeList.ml));
            this.AddValue(nameof(SchemeList.my), "", nameof(SchemeList.my));
        }
    }
}
