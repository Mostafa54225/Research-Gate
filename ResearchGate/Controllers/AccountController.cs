using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using System.Web.Security;
using System.IO;
using ResearchGate.Repository;

namespace ResearchGate.Controllers
{
    public class AccountController : Controller
    {


        private IAuthorRepository _authorRepository;

        public AccountController()
        {
            _authorRepository = new AuthorRepository(new ResearchGateDBContext());
        }

        public AccountController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }


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
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["ProfileImage"];
                int response = _authorRepository.Register(account, file);

                if (response == 1)
                {
                    ModelState.Clear();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.EmailError = "Email is Already Exist!";
                }
            }
            return View();

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
            int response = _authorRepository.Login(email, password);
            if(response == 1)
                return RedirectToAction("Index", "Home");
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password";
            }
            return View();

        }


        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}