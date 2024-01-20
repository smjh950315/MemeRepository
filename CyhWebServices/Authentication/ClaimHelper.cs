using Cyh.Modules.ModAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Cyh.WebServices.Authentication
{
    public class ReadOnlyClaimList
    {
        IEnumerable<Claim>? _Claims;
        public string? this[string _ClaimType] {
            get => _Claims?.FirstOrDefault(c => c.Type == _ClaimType)?.Value;
        }
        public ReadOnlyClaimList(IEnumerable<Claim>? _claims) {
            _Claims = _claims;
        }
    }
    public class ClaimList
    {
        List<Claim> _Claims = new();
        public string? this[string _ClaimType] {
            get => _Claims.FirstOrDefault(c => c.Type == _ClaimType)?.Value;
            set {
                if (value == null)
                    return;
                Claim? _Fnd = _Claims.Find(c => c.Type == _ClaimType);
                if (_Fnd != null) {
                    if (_Fnd.Value != value) {
                        _Claims.Remove(_Fnd);
                    }
                } else {
                    _Claims.Add(new Claim(_ClaimType, value));
                }
            }
        }

        public static implicit operator ClaimList(ClaimsIdentity claims) {
            ClaimList _Ret = new();
            foreach (Claim claim in claims.Claims) {
                _Ret[claim.Type] = claim.Value;
            }
            return _Ret;
        }

        public static implicit operator List<Claim>(ClaimList claims) {
            return claims._Claims;
        }
    }

    /// <summary>
    /// Claim驗證器
    /// </summary>
    public class ClaimHelper : ISignInHelper
    {
#pragma warning disable
        AuthenticationProperties? _Property;
        ClaimsIdentity? _Identity;
        string? _AuthenticationScheme;
#pragma warning restore

        /// <summary>
        /// 預設用的 Scheme
        /// </summary>
        public static string DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        /// <summary>
        /// 預設的認證相關設定
        /// </summary>
        public static AuthenticationProperties DefaultProperties {
            get {
                return new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };
            }
        }

        /// <summary>
        /// 取得設定的認證方式，若無則使用預設(COOKIE)
        /// </summary>
        public string Scheme {
            get {
                return _AuthenticationScheme ?? DefaultScheme;
            }
            set {
                _AuthenticationScheme = value;
            }
        }

        /// <summary>
        /// Claim 規則
        /// </summary>
        public string? Role {
            get => Claims[ClaimTypes.Role];
            set => Claims[ClaimTypes.Role] = value;
        }

        /// <summary>
        /// 已加入的 Claim 清單
        /// </summary>
        public ClaimList Claims = new();

        /// <summary>
        /// 以到目前為止加入的資訊取得Identity
        /// </summary>
        public ClaimsIdentity Identity {
            get {
                if (_Identity == null)
                    _Identity = new ClaimsIdentity(Scheme);

                var claimed = _Identity.Claims.ToList();

                foreach (var claim in claimed) {
                    _Identity.RemoveClaim(claim);
                }

                _Identity.AddClaims((List<Claim>)Claims);
                return _Identity;
            }
        }

        /// <summary>
        /// 以到目前為止加入的資訊取得Principal
        /// </summary>
        public ClaimsPrincipal Principal => new ClaimsPrincipal(Identity);

        /// <summary>
        /// 以到目前為止加入的資訊取得Property(就是登入選項)
        /// </summary>
        public AuthenticationProperties Property {
            get {
                return _Property ??= DefaultProperties;
            }
            set {
                _Property = value;
            }
        }

        /// <summary>
        /// 設定要用來當作識別的使用者ID
        /// </summary>
        /// <param name="_userId">要用來當作識別的使用者ID</param>
        public void SetClientId(string clientId) {
            Claims[ClaimTypes.Name] = clientId;
        }

        /// <summary>
        /// 設定與登入有關的資訊
        /// </summary>
        /// <param name="_options">與登入有關的資訊</param>
        public void SetLoginOptions(ILoginOptions _options) {
            if (_Property == null) {
                _Property = new AuthenticationProperties();
                _Property.AllowRefresh = _options.AllowRefresh;
                _Property.ExpiresUtc = DateTimeOffset.Now.AddMinutes(_options.ExpireTime);
                _Property.IsPersistent = _options.IsPersist;
            }
        }

        /// <summary>
        /// 加入額外訊息作為登入資訊
        /// </summary>
        /// <param name="objects">額外訊息</param>
        public void AddAdditionalInfos(params object?[] objects) {
            if (objects.IsNullOrEmpty())
                return;
            if (objects is Claim[]) {
                Claim[]? claims = objects as Claim[];
                if (claims.IsNullOrEmpty())
                    return;
                foreach (var claim in claims) {
                    Claims[claim.Type] = claim.Value;
                }
            }
        }

        /// <summary>
        /// 建構子，如果輸入建構參數 <paramref name="_AppName"/> 則表示將其設定為該應用程式 Cookie 的識別名
        /// </summary>
        public ClaimHelper(string? _AppName = null) {
            if (_AppName != null)
                Claims[ClaimTypes.NameIdentifier] = _AppName;
        }
    }
}
