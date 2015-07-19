using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.Data;
using System.Data;
using DotNet.Common.Collections;

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
                        user.Status = !Convert.IsDBNull(dr["Status"]) ? int.Parse(dr["Status"].ToString()) : 0;
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
                        user.Status = !Convert.IsDBNull(dr["Status"]) ? int.Parse(dr["Status"].ToString()) : 0;
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
                cmd.SetParameterValue("@Status", userModel.Status);
                cmd.SetParameterValue("@CreateTime", userModel.CreateTime);

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
        }


        public PagedList<UserModel> GetUserList(int pageIndex, int pageSize)
        {
            var userList = new List<UserModel>();
            var count = 0;
            using (var cmd = DataCommandManager.GetDataCommand("GetUserByPageList"))
            {
                cmd.SetParameterValue("@PageIndex", pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);
                using (var ds = cmd.ExecuteDataSet())
                {
                    if (ds.Tables.Count >= 2)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            var user = new UserModel();
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
                            user.Status = !Convert.IsDBNull(dr["Status"]) ? int.Parse(dr["Status"].ToString()) : 0;
                            user.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
                            userList.Add(user);
                        }
                        var countDr = ds.Tables[1].Rows[0][0];
                        count = !Convert.IsDBNull(countDr) ? Convert.ToInt32(countDr) : 0;
                    }
                }
                var result = new PagedList<UserModel>(userList, pageIndex, pageSize, count);
                return result;
            }
        }



        public int UpdateUserStatus(int userId, int status)
        {
            using (var cmd = DataCommandManager.GetDataCommand("UpdateUserStatus"))
            {
                cmd.SetParameterValue("@Status",status);
                cmd.SetParameterValue("@UserId", userId);
               var result = cmd.ExecuteNonQuery();
               if (result != null)
               {
                   return Convert.ToInt32(result);
               }
               return 0;
            }
        }

        public bool InsertUserCaptcha(int userId, string captcha, DateTime sendTime, int status)
        {
            using (var cmd = DataCommandManager.GetDataCommand("InsertUserCaptcha"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@Captcha", captcha);
                cmd.SetParameterValue("@SendTime", sendTime);
                cmd.SetParameterValue("@Status", status);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public string GetUnUsedCaptcha(int userId, string mobile, int expireMinute)
        {
            var captcha = string.Empty;
            using (var cmd = DataCommandManager.GetDataCommand("GetUnUsedCaptcha"))
            {
                cmd.SetParameterValue("@UserId", userId);
                var returnValue = cmd.ExecuteScalar();
                if (returnValue != null)
                {
                    captcha = Convert.ToString(returnValue);
                }
            }

            return captcha;
        }


        public UserModel GetUserByWxOpenId(string openId)
        {
            var user = new UserModel();
            using (var cmd = DataCommandManager.GetDataCommand("GetUserByWxOpenId"))
            {
                cmd.SetParameterValue("@WxOpenId", openId);
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
                        user.Status = !Convert.IsDBNull(dr["Status"]) ? int.Parse(dr["Status"].ToString()) : 0;
                        user.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
                    }
                }
            }
            return user;
        }


        public void updateMobileByWxOpenId(string mobile, string wxOpenId)
        {
            throw new NotImplementedException();
        }

        public bool CheckMobileCaptcha(string mobile, string captcha)
        {
            using (var cmd = DataCommandManager.GetDataCommand("CheckMobileCaptcha"))
            {
                
            }
        }
    }
}
