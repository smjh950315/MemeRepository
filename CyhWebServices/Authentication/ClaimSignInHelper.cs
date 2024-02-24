using Cyh.Modules.ModAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections;
using System.Security.Claims;

namespace Cyh.WebServices.Authentication
{
    public interface IReadOnlyClaimList : IEnumerable<Claim>
    {
        public string? this[string _claimType] { get; }
    }

    public class ClaimList : IReadOnlyClaimList
    {
        List<Claim>? _Claims;

        public string? this[string claimType] {
            get {
                if (this._Claims == null)
                    return null;
                return this._Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            }
            set {
                if (value == null)
                    return;
                if (this._Claims == null) {
                    this._Claims = new List<Claim>
                    {
                        new Claim(claimType, value)
                    };
                } else if (!this._Claims.Any()) {
                    this._Claims.Add(new Claim(claimType, value));
                } else {
                    Claim? find = this._Claims.Find(c => c.Type == claimType);
                    if (find != null) {
                        if (find.Value != value) {
                            this._Claims.Remove(find);
                        } else { }
                    } else {
                        this._Claims.Add(new Claim(claimType, value));
                    }
                }
            }
        }
        public ClaimList() { }
        public ClaimList(IEnumerable<Claim>? _claims) {
            this._Claims = _claims?.ToList();
        }

        public static implicit operator ClaimList(ClaimsIdentity claims) {
            ClaimList _Ret = new();
            foreach (Claim claim in claims.Claims) {
                _Ret[claim.Type] = claim.Value;
            }
            return _Ret;
        }
        public static implicit operator List<Claim>(ClaimList claims) {
            return claims._Claims ?? new();
        }

        public IEnumerator<Claim> GetEnumerator() {
            return this._Claims?.GetEnumerator() ?? Enumerable.Empty<Claim>().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Claim驗證器
    /// </summary>
    public class ClaimSignInHelper : ISignInHelper
    {
#pragma warning disable
        AuthenticationProperties? _Property;
        ClaimsIdentity? _Identity;
        string? _AuthenticationScheme;
#pragma warning restore

        /// <summary>
        /// 預設用的 Scheme
        /// </summary>
        public const string DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

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
                return this._AuthenticationScheme ?? DefaultScheme;
            }
            set {
                this._AuthenticationScheme = value;
            }
        }

        /// <summary>
        /// Claim 規則
        /// </summary>
        public string? Role {
            get => this.Claims[ClaimDefinitions.ClientRole];
            set => this.Claims[ClaimDefinitions.ClientRole] = value;
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
                if (this._Identity == null)
                    this._Identity = new ClaimsIdentity(this.Scheme);

                List<Claim> claimed = this._Identity.Claims.ToList();

                foreach (Claim? claim in claimed) {
                    this._Identity.RemoveClaim(claim);
                }

                this._Identity.AddClaims((List<Claim>)this.Claims);
                return this._Identity;
            }
        }

        /// <summary>
        /// 以到目前為止加入的資訊取得Principal
        /// </summary>
        public ClaimsPrincipal Principal => new ClaimsPrincipal(this.Identity);

        /// <summary>
        /// 以到目前為止加入的資訊取得Property(就是登入選項)
        /// </summary>
        public AuthenticationProperties Property {
            get {
                return this._Property ??= DefaultProperties;
            }
            set {
                this._Property = value;
            }
        }

        /// <summary>
        /// 設定要用來當作識別的客戶端ID
        /// </summary>
        /// <param name="clientId">要用來當作識別的客戶端ID</param>
        public void SetClientId(string clientId) {
            this.Claims[ClaimDefinitions.ClientId] = clientId;
        }

        public void SetSignInOptions(IAuthorizationOptions options) {
            if (this._Property == null) {
                this._Property = new AuthenticationProperties();
                this._Property.AllowRefresh = options.AllowRefresh;
                this._Property.ExpiresUtc = DateTimeOffset.Now + options.LifeTime;
                this._Property.IsPersistent = options.IsPersistent;
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
                foreach (Claim claim in claims) {
                    this.Claims[claim.Type] = claim.Value;
                }
            }
        }

        /// <summary>
        /// 建構子，如果輸入建構參數 <paramref name="_AppName"/> 則表示將其設定為該應用程式 Cookie 的識別名
        /// </summary>
        public ClaimSignInHelper(string? _AppName = null) {
            if (_AppName != null) {
                this.Claims[ClaimDefinitions.Application] = _AppName;
            }
        }
    }
}
