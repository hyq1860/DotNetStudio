using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract;
using DotNet.CloudFarm.Domain.Contract.SMS;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model;
using DotNet.CloudFarm.Domain.Model.Base;
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

        private CloudFarmDbContext cloudFarmDb;

        public UserService(IUserDataAccess userDataAccess, ISMSService smsService, CloudFarmDbContext dbContext)
        {
            this.userDataAccess = userDataAccess;
            this.smsService = smsService;
            this.cloudFarmDb = dbContext;
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
                userDataAccess.InsertUserCaptcha(userId,mobile, captcha, DateTime.Now, 0);
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

        public UserModel SearchUser(string searchKey)
        {
            return userDataAccess.SearchUser(searchKey);
        }

        public int Insert(UserModel userModel)
        {
            return userDataAccess.Insert(userModel);
        }

        public PagedList<UserModel> GetUserList(int pageIndex, int pageSize)
        {
            return userDataAccess.GetUserList(pageIndex, pageSize);
        }
        public PagedList<UserModel> GetSourceUsers(int pageIndex, int pageSize)
        {
            return userDataAccess.GetSourceUsers(pageIndex, pageSize);
        }
        public PagedList<UserModel> GetUserListBySourceId(string sourceId, int pageIndex, int pageSize)
        {
            return userDataAccess.GetUserListBySourceId(sourceId, pageIndex, pageSize);
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

        public bool UpdateUserCaptchaStatus(int userId,string mobile)
        {
            return userDataAccess.UpdateUserCaptchaStatus(userId, mobile);
        }

        public BackstageLoginUser FindByUserNameAndPassword(string userName, string password)
        {
            return userDataAccess.FindByUserNameAndPassword(userName, password);
        }

        public BackstageLoginUser FindBackstageLoginUserByUserId(int userId)
        {
            return userDataAccess.FindBackstageLoginUserByUserId(userId);
        }

        public bool CheckMobileCaptcha(int userId,string mobile,string captcha)
        {
            return userDataAccess.CheckMobileCaptcha(userId, mobile, captcha);
        }


        public int InsertQRCode(QRCode qr)
        {
            return userDataAccess.InsertQRCode(qr);
        }


        public PagedList<QRCode> GetQRList(int pageIndex, int pageSize)
        {
            return userDataAccess.GetQRList(pageIndex, pageSize);
        }

        public bool InsertPageLog(PageLog pageLog)
        {
            return userDataAccess.InsertPageLog(pageLog);
        }
    }
}
