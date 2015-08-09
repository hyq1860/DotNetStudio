using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotNet.CloudFarm.Domain.Contract.SMS;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.DTO.User;
using DotNet.CloudFarm.Domain.Impl.SMS;
using DotNet.CloudFarm.Domain.Impl.User;
using DotNet.CloudFarm.Domain.Model.User;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using DotNet.CloudFarm.WebSite.Models;
using DotNet.Common.Models;
using DotNet.Identity;
using DotNet.Identity.Database;
using System.Web.Configuration;
using log4net;
using DotNet.Common.Utility;
using Senparc.Weixin.MP.CommonAPIs;

namespace DotNet.CloudFarm.WebSite.Controllers
{

    public class AccountController : BaseController
    {
        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];
        /// <summary>
        /// 与微信公众账号后台的AppSecret设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];


        public AccountController()
            : this(new UserManager<CloudFarmIdentityUser>(new CloudFarmUserStore()))
        {
            UserService=new UserService(new UserDataAccess(),new SMSService());
            SmsService=new SMSService();
        }

        public AccountController(UserManager<CloudFarmIdentityUser> userManager)
        {
            UserManager = userManager;
        }

        private ILog logger = LogManager.GetLogger("AccountController");

        public UserManager<CloudFarmIdentityUser> UserManager { get; private set; }

        [Ninject.Inject]
        public IUserService UserService { get; set; }

        public ISMSService SmsService { get; set; }

        [HttpPost]
        public async Task<JsonResult> Login(LoginUser loginUser)
        {
            #if DEBUG
            loginUser.WxOpenId = "oOGoot0O0nEuP4uEHdNLQyNpGnwM";//"oOGootzpwe38CkQSTj00wyHhKSMk";//
            #endif

            var jsonResult = new JsonResult();
            try
            {
                //logger.Info(JsonHelper.ToJson(loginUser));

                //check验证码
                var user = UserService.GetUserByWxOpenId(loginUser.WxOpenId);
                
                if (user == null||user.UserId==0)
                {
                    //创建用户
                    var openId = loginUser.WxOpenId;
                    var accesstoken = AccessTokenContainer.TryGetToken(AppId, AppSecret);
                    var wxUser = CommonApi.GetUserInfo(accesstoken, openId);
                    if (!string.IsNullOrEmpty(wxUser.headimgurl))
                    {
                        wxUser.headimgurl = wxUser.headimgurl.Substring(0, wxUser.headimgurl.Length - 1) + "96";
                    }
                    var userModel = new UserModel()
                    {
                        CreateTime = DateTime.Now,
                        WxOpenId = openId,
                        WxHeadUrl = wxUser.headimgurl,
                        WxNickName = wxUser.nickname,
                        Status = 1
                    };
                    if (UserService.Insert(userModel) > 0)
                    {
                        user = UserService.GetUserByWxOpenId(loginUser.WxOpenId);
                    }
                    else
                    {
                        logger.Error("用户信息insert失败:"+JsonHelper.ToJson(userModel));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.Mobile) && loginUser.Mobile != user.Mobile)
                    {
                        jsonResult.Data = new { IsSuccess = false, Msg = "一个微信账号只能绑定一个手机号。" };
                        return jsonResult;
                    }
                }

                //logger.Info(JsonHelper.ToJson(user));
                if (UserService.CheckMobileCaptcha(user.UserId, loginUser.Mobile, loginUser.Captcha))
                {
                    //更新验证码状态
                    UserService.UpdateUserCaptchaStatus(user.UserId, loginUser.Mobile);
                    //logger.Info(1);

                    //将用户的手机号与weixinid绑定
                    UserService.UpdateMobileUserByWxOpenId(loginUser.Mobile, loginUser.WxOpenId);

                    //logger.Info(2);
                    var result = UserManager.FindByNameAsync(loginUser.Mobile);
                    //logger.Info(3);

                    //用户禁用不让登陆
                    if (result != null && user.Status == 1)
                    {
                        //logger.Info(JsonHelper.ToJson(result));
                        await SignInAsync(result.Result, true);
                        //logger.Info(4);
                        jsonResult.Data = new { IsSuccess = true };
                    }
                    else
                    {
                        jsonResult.Data = new { IsSuccess = false, Msg = "您的用户已被禁用" };
                    }
                }
                else
                {
                    jsonResult.Data = new { IsSuccess = false, Msg = "验证码不正确" };
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return jsonResult;
        }

        /// <summary>
        /// 后台登录
        /// </summary>
        /// <returns></returns>
        public ActionResult BackstageLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> BackstageLogin(BackstageLoginUser loginUser)
        {
            var jsonResult = new JsonResult();

            var user= UserService.FindByUserNameAndPassword(loginUser.UserName, loginUser.Password);
            if (user != null && user.UserId > 0)
            {
                await SignInAsync(new CloudFarmIdentityUser(){UserName = user.UserName,Id = user.UserId.ToString()}, true);
                jsonResult.Data = new { IsSuccess = true };
            }
            else
            {
                jsonResult.Data = new { IsSuccess = false };
            }
            return jsonResult;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult LogOff()
        {
            var jsonResult = new JsonResult();
            AuthenticationManager.SignOut();
            var result = new Result<object>();
            result.Data = new {LoginUrl="/Account/Login"};
            result.Status = new Status() { Code = "1", Message = "退出登录成功。" };
            jsonResult.Data = result;
            return jsonResult;
        }

        public ActionResult BackstageLogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("BackstageLogin","Account");
        }

        public JsonResult GetMobileCaptcha(string mobile,string weixinId)
        {
            #if DEBUG
            weixinId = "oOGoot0O0nEuP4uEHdNLQyNpGnwM"; ;//
            #endif
            //logger.Info("获取验证码："+mobile+"|"+weixinId);            
            //通过微信id获取用户id
            var user = UserService.GetUserByWxOpenId(weixinId);
            //logger.Info(JsonHelper.ToJson(user));
            var userid = user.UserId;
            var result = UserService.GetCaptcha(userid, mobile);

            return Json(result);
        }

        private readonly static string COOKIE_OPENID_KEY = "wx_openId";

        public ActionResult Login()
        {

            if (Request.Cookies[COOKIE_OPENID_KEY]!=null)
            {
                ViewBag.OpenId = Request.Cookies[COOKIE_OPENID_KEY].Value;
            }
            else
            {
                ViewBag.OpenId = "";
            }
            ViewBag.AppId = AppId;
#if DEBUG
            ViewBag.OpenId = "oOGootzpwe38CkQSTj00wyHhKSMk";
#endif
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(CloudFarmIdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
    }
}