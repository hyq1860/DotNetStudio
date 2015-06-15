using DotNet.CloudFarm.WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    public class WeiXinController : ApiController
    {
        log4net.ILog logger = log4net.LogManager.GetLogger("WeixinController");

        // GET: api/WeiXin
        public IEnumerable<string> Get()
        {
            //logger.Info("test");

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


        public IHttpActionResult GetSign(string signature, string timestamp, string nonce, string echostr)
        {
            logger.Info("signature" + signature + "|" + "timestamp" + timestamp + "|" + "nonce" + nonce + "|" + "signature" + echostr);
            return new TextResult(echostr, Request);
            }

      
    }
}
