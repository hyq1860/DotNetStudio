using System;
using System.Collections.Generic;
using System.Web;

namespace DotNet.CloudFarm.WebSite.WeixinPay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}