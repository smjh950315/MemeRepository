using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 基本的 Cookie 認證選項，主要要實作的只有 AgeMinutes、Name、Path、HttpOnly，剩下是內建要用的
    /// </summary>
    public interface ICookieAuthenticOptions
    {
        public int AgeMinutes { get; }
        public string Name { get; }
        public string Path { get; }
        public bool HttpOnly { get; }

        public TimeSpan MaxCookieAge => TimeSpan.FromMinutes(AgeMinutes);
        public bool SlidingExpiration => true;
        public SameSiteMode SameSite => SameSiteMode.Lax;
        public CookieSecurePolicy SecurePolicy => CookieSecurePolicy.SameAsRequest;
        public bool IsEssential => true;
        public ChunkingCookieManager CookieManager => new ChunkingCookieManager();
        public ISecureDataFormat<AuthenticationTicket> TicketDataFormat(IDataProtectionProvider _DataProtectProvider) {
            var _DataProtecter = _DataProtectProvider.CreateProtector(CookieAuthenticationDefaults.AuthenticationScheme);
            return new TicketDataFormat(_DataProtecter);
        }
    }
}
