using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.DTO.User;
using DotNet.CloudFarm.Domain.Impl.SMS;
using DotNet.CloudFarm.Domain.Impl.User;
using DotNet.CloudFarm.Domain.Model;
using DotNet.CloudFarm.Domain.Model.User;
using Microsoft.AspNet.Identity;

namespace DotNet.Identity.Database
{
    /// <summary>
    /// yangke用户认证的数据库实现
    /// </summary>
    public class CloudFarmUserStore : IUserStore<CloudFarmIdentityUser>, IUserPasswordStore<CloudFarmIdentityUser>
    {
        [Ninject.Inject]
        private IUserService userService=new UserService(new UserDataAccess(),new SMSService(),new CloudFarmDbContext());

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(CloudFarmIdentityUser user)
        {
            //userService.Insert(new UserModel() {Mobile = user.UserName});
            return Task.Factory.StartNew(() =>
            {
                userService.Insert(new UserModel() {Mobile = user.UserName});
            });
        }

        public Task UpdateAsync(CloudFarmIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(CloudFarmIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<CloudFarmIdentityUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<CloudFarmIdentityUser> FindByNameAsync(string userName)
        {
            CloudFarmIdentityUser user = null;
            
            return Task.Factory.StartNew(() =>
            {
                var userModel = userService.GetUser(userName);
                if (userModel != null && userModel.UserId > 0)
                {
                    user = new CloudFarmIdentityUser() { UserName = userModel.Mobile,Id = userModel.UserId.ToString()};
                }
                return user;
            });
        }

        public Task SetPasswordHashAsync(CloudFarmIdentityUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(CloudFarmIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(CloudFarmIdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}
