using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;

namespace ResearchGate.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [Route("profile/{username}")]
        public ActionResult Index(string username = "")
        {
            
            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                var user = (from obj in db.Authors where obj.Username.ToLower() == username.ToLower() select obj).FirstOrDefault();
                if(user != null)
                {
                    return View(user);
                } else
                {
                    return View();
                }
                
            }
        }
    }
}