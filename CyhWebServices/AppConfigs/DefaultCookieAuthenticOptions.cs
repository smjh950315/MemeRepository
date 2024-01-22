﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 基本的 Cookie 認證選項
    /// </summary>
    public partial class DefaultCookieAuthenticOptions : ICookieAuthenticOptions
    {
        /// <summary>
        /// 要使用的Cookie名稱(通常與APP名稱相同)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ????
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 是否僅允許HTTP協定
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        /// Cookie生命週期(秒)
        /// </summary>
        public int CookieAge { get; set; }

        /// <summary>
        /// 是否在每次呼叫時重置Cookie生命週期
        /// </summary>
        public bool SlidingExpiration { get; set; }

        /// <summary>
        /// 不知道幹嘛的，預設SameSiteMode.Lax，想了解的請看<see cref="SameSiteMode"/>
        /// </summary>
        public SameSiteMode SameSite { get; set; }

        /// <summary>
        /// Cookie的安全性策略，預設CookieSecurePolicy.SameAsRequest，想了解的請看<see cref="CookieSecurePolicy"/>
        /// </summary>
        public CookieSecurePolicy SecurePolicy { get; set; }

        /// <summary>
        /// 此Cookie對於此APP的功能是否為必須(有用到的話要設定為TRUE)
        /// </summary>
        public bool IsEssential { get; set; }

        /// <summary>
        /// 不知道幹嘛的，預設 <see cref="ChunkingCookieManager"/> 
        /// </summary>
        public ChunkingCookieManager CookieManager { get; set; }
    }
}