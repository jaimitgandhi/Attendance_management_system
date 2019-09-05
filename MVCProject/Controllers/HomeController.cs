using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        MyDataContext db = new MyDataContext();
        public ActionResult Index()
        {
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

        public ActionResult Logout()
        {
            if (Session["myid"] != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Index", "Home");

        }

    }
}