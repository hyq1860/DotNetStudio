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
                        user.SourceId = !Convert.IsDBNull(dr["SourceId"]) ? dr["SourceId"].ToString() : string.Empty;

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
                        user.SourceId = !Convert.IsDBNull(dr["SourceId"]) ? dr["SourceId"].ToString() : string.Empty;

                    }
                }
            }
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public UserModel SearchUser(string searchKey)
        {
            //TODO:暂时不需要，但是可以实现
            return new UserModel();
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
                cmd.SetParameterValue("@SourceId", userModel.SourceId);
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
                            user.SourceId = !Convert.IsDBNull(dr["SourceId"]) ? dr["SourceId"].ToString() : string.Empty;
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

        public PagedList<UserModel> GetSourceUsers(int pageIndex, int pageSize)
        {
            var userList = new List<UserModel>();
            var count = 0;
            using (var cmd = DataCommandManager.GetDataCommand("GetSourceUserByPageList"))
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
                            user.SourceCount = !Convert.IsDBNull(dr["SourceCount"]) ? int.Parse(dr["SourceCount"].ToString()) : 0;
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

        public PagedList<UserModel> GetUserListBySourceId(string sourceId, int pageIndex, int pageSize)
        {
            var userList = new List<UserModel>();
            var count = 0;
            using (var cmd = DataCommandManager.GetDataCommand("GetUserByPageListAndSourceId"))
            {
                cmd.SetParameterValue("@PageIndex", pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);
                cmd.SetParameterValue("@SourceId", sourceId);
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
                            user.SourceId = !Convert.IsDBNull(dr["SourceId"]) ? dr["SourceId"].ToString() : string.Empty;
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

        public bool InsertUserCaptcha(int userId,string mobile, string captcha, DateTime sendTime, int status)
        {
            using (var cmd = DataCommandManager.GetDataCommand("InsertUserCaptcha"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@Mobile", mobile);
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
                cmd.SetParameterValue("@Mobile", mobile);
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
                        user.SourceId = !Convert.IsDBNull(dr["SourceId"]) ? dr["SourceId"].ToString() : string.Empty;
                    }
                }
            }
            return user;
        }


        public void UpdateMobileByWxOpenId(string mobile, string wxOpenId)
        {
            using (var cmd = DataCommandManager.GetDataCommand("updateMobileByWxOpenId"))
            {
                cmd.SetParameterValue("@Mobile", mobile);
                cmd.SetParameterValue("@WxOpenId", wxOpenId);
                cmd.ExecuteNonQuery();
            }
        }

        public bool CheckMobileCaptcha(int userId,string mobile, string captcha)
        {
            using (var cmd = DataCommandManager.GetDataCommand("CheckMobileCaptcha"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@Mobile", mobile);
                cmd.SetParameterValue("@Captcha", captcha);
                var returnValue = cmd.ExecuteScalar();
                return returnValue != null && captcha.ToLower() == returnValue.ToString().ToLower();
            }
        }

        public bool UpdateUserCaptchaStatus(int userId,string mobile)
        {
            using (var cmd = DataCommandManager.GetDataCommand("UpdateUserCaptchaStatus"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@Mobile", mobile);
                return cmd.ExecuteNonQuery()>0;
            }
        }

        public BackstageLoginUser FindByUserNameAndPassword(string userName, string password)
        {
            var backstageLoginUser = new BackstageLoginUser();
            using (var cmd = DataCommandManager.GetDataCommand("FindByUserNameAndPassword"))
            {
                cmd.SetParameterValue("@UserName", userName);
                cmd.SetParameterValue("@Password", password);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        backstageLoginUser.UserId = !Convert.IsDBNull(dr["UserId"]) ? Convert.ToInt32(dr["UserId"]) : 0;
                        backstageLoginUser.UserName = !Convert.IsDBNull(dr["UserName"]) ? dr["UserName"].ToString() : string.Empty;
                    }
                }
            }
            return backstageLoginUser;
        }

        public BackstageLoginUser FindBackstageLoginUserByUserId(int userId)
        {
            var backstageLoginUser = new BackstageLoginUser();
            using (var cmd = DataCommandManager.GetDataCommand("FindBackstageLoginUserByUserId"))
            {
                cmd.SetParameterValue("@UserId", userId);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        backstageLoginUser.UserId = !Convert.IsDBNull(dr["UserId"]) ? Convert.ToInt32(dr["UserId"]) : 0;
                        backstageLoginUser.UserName = !Convert.IsDBNull(dr["UserName"]) ? dr["UserName"].ToString() : string.Empty;
                    }
                }
            }
            return backstageLoginUser;
        }


        public int InsertQRCode(QRCode qr)
        {
            using (var cmd = DataCommandManager.GetDataCommand("InsertQRCode"))
            {
                cmd.SetParameterValue("@QRCodeUrl", qr.QRCodeUrl);
                cmd.SetParameterValue("@SourceCode", qr.SourceCode);
                cmd.SetParameterValue("@SourceName", qr.SourceName);
                cmd.SetParameterValue("@Status", qr.Status);
                cmd.SetParameterValue("@CreateTime", qr.CreateTime);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
        }


        public PagedList<QRCode> GetQRList(int pageIndex, int pageSize)
        {
            var qrList = new List<QRCode>();
            var count = 0;
            using (var cmd = DataCommandManager.GetDataCommand("GetQRListByPage"))
            {
                cmd.SetParameterValue("@PageIndex", pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);
                using (var ds = cmd.ExecuteDataSet())
                {
                    if (ds.Tables.Count >= 2)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            var qrcode = new QRCode();
                            qrcode.Id = !Convert.IsDBNull(dr["Id"]) ? int.Parse(dr["Id"].ToString()) : 0;
                            qrcode.SourceCode = !Convert.IsDBNull(dr["SourceCode"]) ? dr["SourceCode"].ToString() : string.Empty;
                            qrcode.SourceName = !Convert.IsDBNull(dr["SourceName"]) ? dr["SourceName"].ToString() : string.Empty;
                            qrcode.QRCodeUrl = !Convert.IsDBNull(dr["QRCodeUrl"]) ? dr["QRCodeUrl"].ToString() : string.Empty;
                            qrcode.Status = !Convert.IsDBNull(dr["Status"]) ? int.Parse(dr["Status"].ToString()) : 0;
                            qrcode.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
                            qrcode.SourceCount = !Convert.IsDBNull(dr["SourceCount"]) ? int.Parse(dr["SourceCount"].ToString()) : 0;
                            qrList.Add(qrcode);
                        }
                        var countDr = ds.Tables[1].Rows[0][0];
                        count = !Convert.IsDBNull(countDr) ? Convert.ToInt32(countDr) : 0;
                    }
                }
                var result = new PagedList<QRCode>(qrList, pageIndex, pageSize, count);
                return result;
            }
        }
    }
}
