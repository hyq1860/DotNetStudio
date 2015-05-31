using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.CloudFarm.Domain.Contract;
using DotNet.WebSite.MVC;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    public class HomeController : Controller
    {
        [NinjectService]
        public IUserService UserService { get; set; }

        public ActionResult Index()
        {
            string json = UserService.Login();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}