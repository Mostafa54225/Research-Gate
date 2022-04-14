using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
namespace ResearchGate.Controllers
{
    public class HomeController : Controller
    {

        ResearchGateDBContext db = new ResearchGateDBContext();
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


        [HttpGet]
        
        public ActionResult search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult search(string option, string search)
        {

            var data = (dynamic)null;

            search = search.ToLower();

            switch (option)
            {
                case "name":
                    data = db.Authors.Where(x => x.FirstName.ToLower() == search || x.LastName.ToLower() == search || x.FirstName.ToLower() + 
                    x.LastName.ToLower() == search || search == null).ToList();
                    break;
                case "University":
                    data = db.Authors.Where(x => x.University.ToLower() == search || search == null).ToList();
                    break;
                case "email":
                    data = db.Authors.Where(x => x.Email.ToLower() == search || search == null).ToList();
                    break;
            }

            if(data.Count == 0)
            {
                ViewBag.Authors = "Not Found";
            }


            return View("search", data);
        }
    }
}