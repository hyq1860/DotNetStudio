using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace DotNet.Identity.Database
{
    /// <summary>
    /// yangke用户认证的数据库实现
    /// </summary>
    public class CloudFarmUserStore : IUserStore<CloudFarmIdentityUser>, IUserPasswordStore<CloudFarmIdentityUser>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(CloudFarmIdentityUser user)
        {
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

        public Task<CloudFarmIdentityUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
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
