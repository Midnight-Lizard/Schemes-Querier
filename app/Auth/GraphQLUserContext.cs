using GraphQL.Authorization;
using System.Security.Claims;

namespace MidnightLizard.Schemes.Querier.Auth
{
    public class GraphQLUserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
