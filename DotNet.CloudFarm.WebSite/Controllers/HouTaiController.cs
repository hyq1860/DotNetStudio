using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    /// <summary>
    /// 后台
    /// </summary>
    public class HouTaiController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}