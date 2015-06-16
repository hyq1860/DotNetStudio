using DotNet.CloudFarm.WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNet.CloudFarm.Domain.Contract.User;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    public class WeiXinController : ApiController
    {
        log4net.ILog logger = log4net.LogManager.GetLogger("WeixinController");

        [Ninject.Inject]
        public IUserService UserService { get; set; }

        // GET: api/WeiXin
        public IEnumerable<string> Get()
        {
            //logger.Info("test");
            //var user=UserService.GetUserByUserId(3);
            return new string[] { "value1", "value2" };
        }

        // GET: api/WeiXin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WeiXin
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/WeiXin/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WeiXin/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 用于给微信提供服务器验证的方法
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns>直接返回请求参数echostr即可以验证通过</returns>
        public IHttpActionResult GetSign(string signature, string timestamp, string nonce, string echostr)
        {
            logger.Info("signature" + signature + "|" + "timestamp" + timestamp + "|" + "nonce" + nonce + "|" + "signature" + echostr);
            return new TextResult(echostr, Request);
            }

      
    }
}
