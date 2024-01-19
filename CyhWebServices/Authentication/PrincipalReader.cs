using System.Security.Claims;
using System.Security.Principal;

namespace Cyh.WebServices.Authentication
{
    /// <summary>
    /// Controller 的 User 讀取器
    /// </summary>
    public class PrincipalReader
    {
        ClaimsPrincipal? _Principal;
        IIdentity? _Identity => _Principal?.Identity;
        IEnumerable<Claim>? _Claims => _Principal?.Claims;

        /// <summary>
        /// 當前 USER 是否已授權
        /// </summary>
        public bool IsAuthenticated => _Identity?.IsAuthenticated ?? false;

        /// <summary>
        /// 當前 USER 的識別ID
        /// </summary>
        public string? Id => _Identity?.Name;

        /// <summary>
        /// 當前 USER 的 ROLE 名稱
        /// </summary>
        public string? Role => Claims[ClaimTypes.Role];

        /// <summary>
        /// CLAIM 綁定的應用程式名稱
        /// </summary>
        public string? AppName => Claims["AppName"];

        /// <summary>
        /// 當前 USER 綁定的 CLAIM
        /// </summary>
        public ReadOnlyClaimList Claims => new(_Claims);

        public PrincipalReader(ClaimsPrincipal? user) => _Principal = user;

        public static implicit operator PrincipalReader(ClaimsPrincipal? user) => new PrincipalReader(user);
    }
}
