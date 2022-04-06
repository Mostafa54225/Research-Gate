using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;



namespace ResearchGate.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            

            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                return View(db.Authors.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Register(Author account)
        {
            if (ModelState.IsValid)
            {
                using (ResearchGateDBContext db = new ResearchGateDBContext())
                {
                    db.Authors.Add(account);
                    db.SaveChanges();   
                }
                ModelState.Clear(); 
                ViewBag.Message = account.FirstName + " " + account.LastName+ "Account successfully registered!";

            }

            return View();
        }

    }
}