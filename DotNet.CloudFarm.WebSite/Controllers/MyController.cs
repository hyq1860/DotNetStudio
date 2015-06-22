using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    public class MyController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}