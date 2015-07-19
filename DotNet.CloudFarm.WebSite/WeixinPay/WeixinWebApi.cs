using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DotNet.CloudFarm.WebSite.WeixinPay
{
    public static class WeixinWebApi
    {
         /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];
        /// <summary>
        /// 与微信公众账号后台的AppSecret设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];

        /// <summary>
        /// 刷新oAuth的token
        /// </summary>
        /// <param name="refreshToken"></param>
        public static string  RefreshAuthToken(string refreshToken)
        {
            var refreshtoken = OAuthApi.RefreshToken(AppId,refreshToken);
            return refreshtoken.access_token;
        }


        public static OAuthUserInfo GetUserInfoByAceessToken(string accessToken, string openId)
        {
            return  OAuthApi.GetUserInfo(accessToken, openId);
        }
    }
}