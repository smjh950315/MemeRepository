using Cyh.Modules.ModAuthentication;
using Cyh.Modules.ModIdentity;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Cyh.WebServices.Controller
{
    /// <summary>
    /// 基本的登入用 Controller
    /// </summary>
    public abstract class MyAuthenticControllerBase : MyControllerBase, IAuthenticController
    {
        public IAuthorizationOptions? LoginOptions { get; set; }

        public IUserValidator? UserValidator { get; set; }

        public ISignInHelper? SignInHelper { get; set; }

        public bool AlwaysRelogin { get; set; }

        /// <summary>
        /// 驗證器是否都有效
        /// </summary>
        public bool ValidatorIsReady {
            get => this.SignInHelper != null
                && this.UserValidator != null;
        }

        protected MyAuthenticControllerBase(
            IWebAppConfigurations webAppConfigurations,
            IUserValidator? userValidator,
            ISignInHelper? signInHelper,
            bool alwaysRelogin) : base(webAppConfigurations) {
            this.UserValidator = userValidator;
            this.SignInHelper = signInHelper;
            this.AlwaysRelogin = alwaysRelogin;
        }

        protected MyAuthenticControllerBase(
            IWebAppConfigurations webAppConfigurations,
            IWebAuthorizationOptions authorizationOptions,
            IUserValidator? userValidator,
            ISignInHelper? signInHelper,
            bool alwaysRelogin) : base(webAppConfigurations) {
            this.UserValidator = userValidator;
            this.SignInHelper = signInHelper;
            this.AlwaysRelogin = alwaysRelogin;
            this.LoginOptions = authorizationOptions;
        }

        /// <summary>
        /// 用 ILoginModel 介面提供的資訊將客戶端登入，如果想自訂驗證方式(帳號與密碼)，可以用 override 對此函數複寫
        /// </summary>
        /// <returns>是否成功登入</returns>
        protected virtual bool ValidateAndSignIn(string? account, string? password) {
            string? user_id = this.UserValidator?
                .GetUserIdIfValid(account, password);

            return this.Login(user_id);
        }

        /// <summary>
        /// 用 IUser 介面提供的資訊將客戶端登入，如果想自訂登入流程，可以用 override 對此函數複寫
        /// </summary>
        /// <param name="user"></param>
        /// <param name="_claims"></param>
        /// <returns>是否成功登入</returns>
        protected virtual bool Login(string? client_id, params Claim[]? _claims) {
            if (this.LoginOptions == null || client_id.IsNullOrEmpty()) { return false; }
            if (this.SignInHelper == null) { return false; }
#pragma warning disable
            if (this.SignInHelper.PrepareSignInAndReady(this, client_id, false)) {
                this.SignInHelper.SetClientId(client_id);
                this.SignInHelper.AddAdditionalInfos(_claims);
                this.SignInHelper.SetSignInOptions(this.LoginOptions);
                this.SignInHelper.SignInAsync(this);
                return true;
            }
#pragma warning restore
            return false;
        }

        [NonAction]
        public void LogUserout() {
            this.SignInHelper?.SignOutAsync(this);
        }
    }
}
