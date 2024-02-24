using System.Security.Claims;

namespace Cyh.WebServices.Authentication
{
    public static class ClaimDefinitions
    {
        public const string ClientId = ClaimTypes.Name;
        public const string ClientRole = ClaimTypes.Role;
        public const string Application = ClaimTypes.NameIdentifier;
    }
}
