using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.DTO.User
{
    public class UserDataAccess : IUserDataAccess
    {
        public void GetUsers()
        {
            using (var cmd = DataCommandManager.GetDataCommand("GetUsers"))
            {
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        
                    }
                }
            }
            
        }
    }
}
