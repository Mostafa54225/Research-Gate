using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using System.Web.Security;

namespace ResearchGate.Controllers
{
    public class AccountController : Controller
    {
        // GET: Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(Author account)
        {

            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                if (ModelState.IsValid)
                {
                    var salt = Utils.PasswordHashing.GeneratePassword(10);
                    var password = Utils.PasswordHashing.EncodePassword(account.Password, salt);
                    account.Password = password;
                    account.Salt = salt;
                    db.Authors.Add(account);
                    bool isExist = db.Authors.Where(u => u.Email.ToLower().Equals(account.Email.ToLower())).FirstOrDefault() != null;
                    
                    if (!isExist)
                    {
                        db.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email is Already Exist!");
                        //return RedirectToAction("Register", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                    }

                }
                return View();
            }

        }

        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                using (ResearchGateDBContext db = new ResearchGateDBContext())
                {
                    var getUser = (from obj in db.Authors where obj.Email == email select obj).FirstOrDefault();
                    if (getUser != null)
                    {
                        var hashCode = getUser.Salt;
                        var hashedPassword = Utils.PasswordHashing.EncodePassword(password, hashCode);

                        var query = (from obj in db.Authors
                                     where (obj.Email == email && obj.Password.Equals(hashedPassword)
                                     )
                                     select obj).FirstOrDefault();

                        if (query != null)
                        {
                            string username = query.Email;
                            FormsAuthentication.SetAuthCookie(username, false);
                            return RedirectToAction("Index", "Home");
                        }
                        ViewBag.ErrorMessage = "Invalid email or password";
                        return View();
                    }
                    ViewBag.ErrorMessage = "Invalid email or password";
                    return View();
                }
            }
            catch (Exception error)
            {
                ViewBag.ErrorMessage = "Error: " + error;
                return View("Index");
            }
        }

        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}