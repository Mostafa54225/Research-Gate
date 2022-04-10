using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using System.Web.Security;
using System.IO;

namespace ResearchGate.Controllers
{
    public class AccountController : Controller
    {
        // GET: Register
        [HttpGet]
        [AllowAnonymous]
        [Route("register")]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Route("register")]
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
                    account.Username = account.Username.ToLower();
                    bool isExist = db.Authors.Where(u => u.Email.ToLower().Equals(account.Email.ToLower())).FirstOrDefault() != null;
                    
                    if (!isExist)
                    {
                        HttpPostedFileBase file = Request.Files["ProfileImage"];
                        if(file != null)
                        {
                            account.Image = Utils.Helper.ConvertToBytes(file);
                        }
                        db.Authors.Add(account);
                        db.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        //ModelState.AddModelError("", "Email is Already Exist!");
                        ViewBag.EmailError = "Email is Already Exist!";
                    }

                }
                return View();
            }

        }

        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [Route("login")]
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
                            FormsAuthentication.SetAuthCookie(query.Username, false);
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
                ViewBag.ErrorMessage = "Error!";
                return View();
            }
        }


        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}