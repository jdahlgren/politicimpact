using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{
    public class HomeController : Controller
    {
        private readonly CaseItemRepository caseitemRepository = new CaseItemRepository();
        
        
        public ActionResult Index()
        {            
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(caseitemRepository.FindAll());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}
