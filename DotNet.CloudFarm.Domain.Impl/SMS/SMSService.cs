﻿using DotNet.CloudFarm.Domain.Contract.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using DotNet.CloudFarm.Domain.Model.SMS;
using DotNet.Common.Utility;
using System.Security.Cryptography;
using log4net;

namespace DotNet.CloudFarm.Domain.Impl.SMS
{
    public class SMSService:ISMSService
    {

        private static string oAuthUrl = ConfigurationManager.AppSettings["SMS_URL_Token"];
        private static string templateUrl = ConfigurationManager.AppSettings["SMS_URL_Template"];
        private static string appId = ConfigurationManager.AppSettings["SMS_AppId"];
        private static string appSecret = ConfigurationManager.AppSettings["SMS_AppSecret"];
        private static string orderTempId = ConfigurationManager.AppSettings["SMS_TemplateId_Order"];
        private static string regTempId = ConfigurationManager.AppSettings["SMS_TemplateId_Reg"];
        private static string smsSendOnOff = ConfigurationManager.AppSettings["SMS_SendOnOff"];
        private static ILog logger = LogManager.GetLogger("SMSService");

        /// <summary>
        /// TOKEN
        /// </summary>
        private static string _accessToken;

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="expireMinute">过期时间（分钟）</param>
        /// <returns>0为成功</returns>
        public int SendSMSUserCaptcha(string mobile, string code,int expireMinute)
        {
            try
            {
                //开关，避免测试发送过多短信
                if (smsSendOnOff == "0")
                {
                    return 0;
                }
                var tempParamModel = new
                {
                    tel = "羊客",
                    code = code,
                    lifetime = expireMinute.ToString()
                };
                var token = getToken();
                var smsRequestModel = new SMSRequestModel()
                {
                    acceptor_tel = mobile,
                    access_token = token,
                    app_id = appId,
                    template_id = regTempId,
                    template_param = tempParamModel.ToJson(),
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var requestParamList = new List<string>();
                requestParamList.Add("acceptor_tel=" + smsRequestModel.acceptor_tel);
                requestParamList.Add("access_token=" + smsRequestModel.access_token);
                requestParamList.Add("app_id=" + smsRequestModel.app_id);
                requestParamList.Add("template_id=" + smsRequestModel.template_id);
                requestParamList.Add("template_param=" + smsRequestModel.template_param);
                requestParamList.Add("timestamp=" + smsRequestModel.timestamp);
                var sign = getSign(requestParamList);
                requestParamList.Add("sign=" + sign);
                var content = string.Join("&", requestParamList);
                logger.Info("登录短信接口content：" + content);
                var returnJson = Post(templateUrl, content);
                logger.Info("登录短信接口返回：" + returnJson);
                var smsResponseModel = JsonHelper.FromJson<SMSResponseModel>(returnJson);
                return smsResponseModel.res_code;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return 1;
            }
           
        }

        /// <summary>
        /// 发送订单提交成功短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="orderId">订单金额</param>
        /// <param name="totalPrice">总价</param>
        /// <returns>0为成功</returns>
        public int SendSMSOrderCreated(string mobile,long orderId, decimal totalPrice)
        {
            try
            {
                //开关，避免测试发送过多短信
                if (smsSendOnOff == "0")
                {
                    return 0;
                }
                var tempParamModel = new
                {
                    order_sn = orderId,
                    price = totalPrice
                };
                var token = getToken();
                var smsRequestModel = new SMSRequestModel()
                {
                    acceptor_tel = mobile,
                    access_token = token,
                    app_id = appId,
                    template_id = orderTempId,
                    template_param = tempParamModel.ToJson(),
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var requestParamList = new List<string>();
                requestParamList.Add("acceptor_tel=" + smsRequestModel.acceptor_tel);
                requestParamList.Add("access_token=" + smsRequestModel.access_token);
                requestParamList.Add("app_id=" + smsRequestModel.app_id);
                requestParamList.Add("template_id=" + smsRequestModel.template_id);
                requestParamList.Add("template_param=" + smsRequestModel.template_param);
                requestParamList.Add("timestamp=" + smsRequestModel.timestamp);
                var sign = getSign(requestParamList);
                requestParamList.Add("sign=" + sign);
                var content = string.Join("&", requestParamList);
                content = System.Web.HttpUtility.UrlEncode(content);
                
                var returnJson = Post(templateUrl, content);
                var smsResponseModel = JsonHelper.FromJson<SMSResponseModel>(returnJson);
                logger.Info("订单短信接口返回：" + returnJson);
                return smsResponseModel.res_code;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return 1;
            }
            
        }


        /// <summary>
        /// 获取参数签名
        /// </summary>
        /// <param name="paramList"></param>
        /// <returns></returns>
        private string getSign(List<string> paramList)
        {
            paramList.Sort();
            var text = string.Join("&", paramList);
            return HmacSha1Sign(text, appSecret);
        }
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string HmacSha1Sign(string text, string key)
        {
            var input_charset = "utf-8";
            Encoding encode = Encoding.GetEncoding(input_charset);
            byte[] byteData = encode.GetBytes(text);
            byte[] byteKey = encode.GetBytes(key);
            HMACSHA1 hmac = new HMACSHA1(byteKey);
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            cs.Write(byteData, 0, byteData.Length);
            cs.Close();
            return Convert.ToBase64String(hmac.Hash);
        }

        private string getToken()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                var requestModel = new oAuthRequestModel()
                {
                    app_id = appId,
                    app_secret = appSecret,
                    grant_type = EnumGrantType.client_credentials
                };
                var content = new StringBuilder();
                content.Append("app_id=");
                content.Append(requestModel.app_id);
                content.Append("&app_secret=");
                content.Append(requestModel.app_secret);
                content.Append("&grant_type=");
                content.Append(requestModel.grant_type.ToString());
                var returnVal = Post(oAuthUrl, content.ToString());
                var responseModel = new SMSResponseModel();
                responseModel = JsonHelper.FromJson<SMSResponseModel>(returnVal);
                if (responseModel.res_code == 0)
                {
                    _accessToken = responseModel.access_token;
                }
            }
            return _accessToken;

        }

        private string Post(string url,string content)
        {
            var result="";
            var reqeust = HttpWebRequest.Create(url);
            var bytes = Encoding.UTF8.GetBytes(content);
            reqeust.Method = "POST";
            reqeust.ContentType="application/x-www-form-urlencoded";
            reqeust.ContentLength = bytes.Length;
            using (var stream = reqeust.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            var response = reqeust.GetResponse();
            var sr = new StreamReader(response.GetResponseStream());
            result = sr.ReadToEnd();
            return result;
        }



        /// <summary>
        /// 发送预售商城短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int SendSMSPreOrderCreated(string mobile,string date)
        {
            try
            {
                //开关，避免测试发送过多短信
                if (smsSendOnOff == "0")
                {
                    return 0;
                }
              
                var token = getToken();
                var smsRequestModel = new SMSRequestModel()
                {
                    acceptor_tel = mobile,
                    access_token = token,
                    app_id = appId,
                    template_id = "91550062",
                    template_param = "{}",//tempParamModel.ToJson(),
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var requestParamList = new List<string>();
                requestParamList.Add("acceptor_tel=" + smsRequestModel.acceptor_tel);
                requestParamList.Add("access_token=" + smsRequestModel.access_token);
                requestParamList.Add("app_id=" + smsRequestModel.app_id);
                requestParamList.Add("template_id=" + smsRequestModel.template_id);
                requestParamList.Add("template_param=" + smsRequestModel.template_param);
                requestParamList.Add("timestamp=" + smsRequestModel.timestamp);
                var sign = getSign(requestParamList);
                requestParamList.Add("sign=" + sign);
                var content = string.Join("&", requestParamList);
                //content = System.Web.HttpUtility.UrlEncode(content);
                logger.Info("预售订单短信接口返回content：" + content);
                var returnJson = Post(templateUrl, content);
                var smsResponseModel = JsonHelper.FromJson<SMSResponseModel>(returnJson);
                logger.Info("预售订单短信接口返回：" + returnJson);
                return smsResponseModel.res_code;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return 1;
            }
        }


        public int SendSMSPreOrderSendProduct(string mobile, string id)
        {
            try
            {
                //开关，避免测试发送过多短信
                if (smsSendOnOff == "0")
                {
                    return 0;
                }
                var tempParamModel = new
                {
                    id = id
                };
                var token = getToken();
                var smsRequestModel = new SMSRequestModel()
                {
                    acceptor_tel = mobile,
                    access_token = token,
                    app_id = appId,
                    template_id = "91550003",
                    template_param = tempParamModel.ToJson(),
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var requestParamList = new List<string>();
                requestParamList.Add("acceptor_tel=" + smsRequestModel.acceptor_tel);
                requestParamList.Add("access_token=" + smsRequestModel.access_token);
                requestParamList.Add("app_id=" + smsRequestModel.app_id);
                requestParamList.Add("template_id=" + smsRequestModel.template_id);
                requestParamList.Add("template_param=" + smsRequestModel.template_param);
                requestParamList.Add("timestamp=" + smsRequestModel.timestamp);
                var sign = getSign(requestParamList);
                requestParamList.Add("sign=" + sign);
                var content = string.Join("&", requestParamList);
                //content = System.Web.HttpUtility.UrlEncode(content);
                logger.Info("预售订单发货短信接口返回content：" + content);
                var returnJson = Post(templateUrl, content);
                var smsResponseModel = JsonHelper.FromJson<SMSResponseModel>(returnJson);
                logger.Info("预售订单发货短信接口返回：" + returnJson);
                return smsResponseModel.res_code;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return 1;
            }
        }
    }
}
