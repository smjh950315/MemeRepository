﻿using Microsoft.AspNetCore.Authentication.Cookies;
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
        /// <summary>
        /// 要使用的Cookie名稱(通常與APP名稱相同)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ????
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 是否僅允許HTTP協定
        /// </summary>
        bool HttpOnly { get; }

        /// <summary>
        /// Cookie生命週期(秒)
        /// </summary>
        int CookieAge { get; }

        /// <summary>
        /// 是否在每次呼叫時重置Cookie生命週期
        /// </summary>
        bool SlidingExpiration { get; }

        /// <summary>
        /// 不知道幹嘛的，預設SameSiteMode.Lax，想了解的請看<see cref="SameSiteMode"/>
        /// </summary>
        SameSiteMode SameSite { get; }

        /// <summary>
        /// Cookie的安全性策略，預設CookieSecurePolicy.SameAsRequest，想了解的請看<see cref="CookieSecurePolicy"/>
        /// </summary>
        CookieSecurePolicy SecurePolicy { get; }

        /// <summary>
        /// 此Cookie對於此APP的功能是否為必須(有用到的話要設定為TRUE)
        /// </summary>
        bool IsEssential { get; }

        /// <summary>
        /// 不知道幹嘛的，預設 <see cref="ChunkingCookieManager"/> 
        /// </summary>
        ChunkingCookieManager CookieManager { get; }
    }
}
