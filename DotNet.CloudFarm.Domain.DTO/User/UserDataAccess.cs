using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.DTO.User
{
    /// <summary>
    /// 用户数据访问层
    /// </summary>
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

        public void Login()
        {
            GetUsers();
        }

        public bool Update(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUserByUserId(int userId)
        {
            var user = new UserModel();
            using (var cmd = DataCommandManager.GetDataCommand("GetUserByUserId"))
            {
                cmd.SetParameterValue("@UserId", userId);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        user.UserId = !Convert.IsDBNull(dr["ID"]) ? int.Parse(dr["ID"].ToString()) : 0;
                        user.Mobile = !Convert.IsDBNull(dr["Mobile"]) ? dr["Mobile"].ToString() : string.Empty;
                        user.WxOpenId = !Convert.IsDBNull(dr["WxOpenId"]) ? dr["WxOpenId"].ToString() : string.Empty;
                        user.WxNickName = !Convert.IsDBNull(dr["WxNickName"]) ? dr["WxNickName"].ToString() : string.Empty;
                        user.WxSex = !Convert.IsDBNull(dr["WxSex"]) ? int.Parse(dr["WxSex"].ToString()) : 0;
                        user.WxHeadUrl = !Convert.IsDBNull(dr["WxHeadUrl"]) ? dr["WxHeadUrl"].ToString() : string.Empty;
                        user.WxSubTime = !Convert.IsDBNull(dr["WxSubTime"]) ? Convert.ToDateTime(dr["WxSubTime"]) : DateTime.MinValue;
                        user.WxUnionId = !Convert.IsDBNull(dr["WxUnionId"]) ? dr["WxUnionId"].ToString() : string.Empty;
                        user.WxRemark = !Convert.IsDBNull(dr["WxRemark"]) ? dr["WxRemark"].ToString() : string.Empty;
                        user.WxGroupId = !Convert.IsDBNull(dr["WxGroupId"]) ? int.Parse(dr["WxGroupId"].ToString()) : 0;
                        user.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
                    }
                }
            }
            return user;
        }

        public UserModel GetUser(string userName)
        {
            var user = new UserModel();
            using (var cmd = DataCommandManager.GetDataCommand("GetUserByName"))
            {
                cmd.SetParameterValue("@Mobile", userName);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        user.UserId = !Convert.IsDBNull(dr["ID"]) ? int.Parse(dr["ID"].ToString()) : 0;
                        user.Mobile = !Convert.IsDBNull(dr["Mobile"]) ? dr["Mobile"].ToString() : string.Empty;
                        user.WxOpenId = !Convert.IsDBNull(dr["WxOpenId"]) ? dr["WxOpenId"].ToString() : string.Empty;
                        user.WxNickName = !Convert.IsDBNull(dr["WxNickName"]) ? dr["WxNickName"].ToString() : string.Empty;
                        user.WxSex = !Convert.IsDBNull(dr["WxSex"]) ? int.Parse(dr["WxSex"].ToString()) : 0;
                        user.WxHeadUrl = !Convert.IsDBNull(dr["WxHeadUrl"]) ? dr["WxHeadUrl"].ToString() : string.Empty;
                        user.WxSubTime = !Convert.IsDBNull(dr["WxSubTime"]) ? Convert.ToDateTime(dr["WxSubTime"]) : DateTime.MinValue;
                        user.WxUnionId = !Convert.IsDBNull(dr["WxUnionId"]) ? dr["WxUnionId"].ToString() : string.Empty;
                        user.WxRemark = !Convert.IsDBNull(dr["WxRemark"]) ? dr["WxRemark"].ToString() : string.Empty;
                        user.WxGroupId = !Convert.IsDBNull(dr["WxGroupId"]) ? int.Parse(dr["WxGroupId"].ToString()) : 0;
                        user.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
                    }
                }
            }
            return user;
        }

        public int Insert(UserModel userModel)
        {
            using (var cmd = DataCommandManager.GetDataCommand("InsertUser"))
            {
                cmd.SetParameterValue("@Mobile", userModel.Mobile);
                cmd.SetParameterValue("@WxOpenId", userModel.WxOpenId);
                cmd.SetParameterValue("@WxNickName", userModel.WxNickName);
                cmd.SetParameterValue("@WxSex", userModel.WxSex);
                cmd.SetParameterValue("@WxHeadUrl", userModel.WxHeadUrl);
                cmd.SetParameterValue("@WxSubTime", userModel.WxSubTime);
                cmd.SetParameterValue("@WxUnionId", userModel.WxUnionId);
                cmd.SetParameterValue("@WxRemark", userModel.WxRemark);
                cmd.SetParameterValue("@WxGroupId", userModel.WxGroupId);
                cmd.SetParameterValue("@CreateTime", userModel.CreateTime);

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
        }
    }
}
