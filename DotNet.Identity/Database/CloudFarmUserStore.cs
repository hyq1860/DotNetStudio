using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.User;
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
        private IUserService userService;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(CloudFarmIdentityUser user)
        {
            //userService.Insert(new UserModel() {Mobile = user.UserName});
            throw new NotImplementedException();
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

        public async Task<CloudFarmIdentityUser> FindByNameAsync(string userName)
        {
            CloudFarmIdentityUser user = null;
            var userModel = userService.GetUser(userName);
            if (userModel != null && userModel.UserId > 0)
            {
                user= new CloudFarmIdentityUser() {UserName = userModel.Mobile};
            }
            return user;
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
