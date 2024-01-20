using Cyh.Modules.ModAuthentication;
using Cyh.Modules.ModIdentity;
using Cyh.Modules.ModRoleSystem;
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
        public ILoginOptions? LoginOptions => this._AppConfigurations?.LoginOptions;

        public ILoginModel? LoginModel => this.LoginOptions?.LoginModel;

        public IRoleValidator? RoleValidator { get; set; }

        public IUserValidator? UserValidator { get; set; }

        public ISignInHelper? SignInHelper { get; set; }

        public bool AlwaysRelogin { get; set; }

        /// <summary>
        /// 驗證器是否都有效
        /// </summary>
        public bool ValidatorIsReady {
            get => this.SignInHelper != null
                && this.UserValidator != null
                && this.RoleValidator != null;
        }

        protected MyAuthenticControllerBase(
            IWebAppConfigurations webAppConfigurations,
            IRoleValidator? roleValidator,
            IUserValidator? userValidator,
            ISignInHelper? signInHelper,
            bool alwaysRelogin) : base(webAppConfigurations) {
            this.RoleValidator = roleValidator;
            this.UserValidator = userValidator;
            this.SignInHelper = signInHelper;
            this.AlwaysRelogin = alwaysRelogin;
        }

        /// <summary>
        /// 用 ILoginModel 介面提供的資訊將客戶端登入，如果想自訂驗證方式(帳號與密碼)，可以用 override 對此函數複寫
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>是否成功登入</returns>
        protected virtual bool ValidateAndSignIn(ILoginModel? loginModel) {
            if (loginModel == null)
                return false;

            if (!this.ValidatorIsReady)
                return false;

            var user = this.UserValidator?
                .FindUserByLoginModel(loginModel);

            if (user == null)
                return false;

            if (this.LogUserIn(user))
                return true;

            return false;
        }

        /// <summary>
        /// 用 IUser 介面提供的資訊將客戶端登入，如果想自訂登入流程，可以用 override 對此函數複寫
        /// </summary>
        /// <param name="user"></param>
        /// <param name="_claims"></param>
        /// <returns>是否成功登入</returns>
        protected virtual bool LogUserIn(IUser? user, params Claim[]? _claims) {
            if (HasNull(this.LoginOptions, user)) { return false; }
            if (this.SignInHelper == null) { return false; }
#pragma warning disable
            if (this.SignInHelper.PrepareSignInAndReady(this, user.ID.ToString(), false)) {
                this.SignInHelper.SetClientId(user.ID.ToString());
                this.SignInHelper.AddAdditionalInfos(_claims);
                this.SignInHelper.SetLoginOptions(this.LoginOptions);
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
