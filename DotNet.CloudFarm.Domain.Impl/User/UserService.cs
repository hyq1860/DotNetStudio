using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract;
using DotNet.CloudFarm.Domain.Contract.SMS;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.Message;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.Common.Collections;
using DotNet.Common.Models;
using DotNet.Common.Utility;

namespace DotNet.CloudFarm.Domain.Impl.User
{
    public class UserService:IUserService
    {
        private IUserDataAccess userDataAccess;

        private ISMSService smsService;
        public UserService(IUserDataAccess userDataAccess, ISMSService smsService)
        {
            this.userDataAccess = userDataAccess;
            this.smsService = smsService;
        }

        public Result<LoginUser> Login(LoginUser loginUser)
        {
            userDataAccess.Login();
            return new Result<LoginUser>();
        }

        public bool GetCaptcha(int userId,string mobile)
        {
            var unUsedCaptcha = userDataAccess.GetUnUsedCaptcha(userId, mobile, 5);
            var captcha = string.Empty;
            if (string.IsNullOrEmpty(unUsedCaptcha))
            {
                //计算出验证码
                captcha = StringHelper.GetRandomInt(6, DateTime.Now.Millisecond);
                //写入验证码发送表
                userDataAccess.InsertUserCaptcha(userId, captcha, DateTime.Now, 0);
            }
            else
            {
                captcha = unUsedCaptcha;
            }
            //调用验证码发送接口
            var returnCode = smsService.SendSMSUserCaptcha(mobile, captcha, 5);
            if (returnCode == 0)
            {
                return true;
            }
            else
            {
                //失败再调一遍
                returnCode = smsService.SendSMSUserCaptcha(mobile, captcha, 5);
                if (returnCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public PagedList<MessageModel> GetMessages(int userId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUserByUserId(int userId)
        {
            return userDataAccess.GetUserByUserId(userId);
        }

        public UserModel GetUser(string userName)
        {
            return userDataAccess.GetUser(userName);
        }

        public int Insert(UserModel userModel)
        {
            return userDataAccess.Insert(userModel);
        }


        public PagedList<UserModel> GetUserList(int pageIndex, int pageSize)
        {
            return userDataAccess.GetUserList(pageIndex, pageSize);
        }


        public int UpdateUserStatus(int userId, int status)
        {
             return userDataAccess.UpdateUserStatus(userId, status);
        }




        public UserModel GetUserByWxOpenId(string wxOpenId)
        {
            return userDataAccess.GetUserByWxOpenId(wxOpenId);
        }



        public void UpdateMobileUserByWxOpenId(string mobile, string wxOpenId)
        {
             userDataAccess.UpdateMobileByWxOpenId(mobile, wxOpenId);
        }

        public bool UpdateUserCaptchaStatus(int userId)
        {
            return userDataAccess.UpdateUserCaptchaStatus(userId);
        }

        public bool CheckMobileCaptcha(int userId,string mobile,string captcha)
        {
            return userDataAccess.CheckMobileCaptcha(userId, mobile, captcha);
        }
    }
}
