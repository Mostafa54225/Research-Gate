using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using ResearchGate.Repository;

namespace ResearchGate.Controllers
{
    public class ProfileController : Controller
    {


        private IAuthorRepository _authorRepository;

        public ProfileController()
        {
            _authorRepository = new AuthorRepository(new ResearchGateDBContext());
        }

        public ProfileController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        [Route("profile/{username?}")] 
        public ActionResult Index(string username)
        
        {
            if (username == null)
                username = User.Identity.Name;
            var user = _authorRepository.GetByUsername(username);
                

            if (user != null)
            {
                return View(user);
            }
            else
            {
                ViewBag.ProfileNotFound = "Profile Not Found";
                return View();
            }
        }



        [HttpPost]
        [Authorize]
        [Route("Profile/{currentUser}/Edit")]
        public ActionResult Edit(Author account, string currentUser)
        {
            if (User.Identity.Name == currentUser)
            {
                if (ModelState.IsValid)
                {
                    if (account != null)
                    {
                        HttpPostedFileBase file = Request.Files["ProfileImage"];

                        _authorRepository.Update(account, currentUser, file);
                        _authorRepository.Save();

                        return RedirectToAction("Index");

                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Error";
                    return View("Index");
                }

            }
            return RedirectToAction("Index");
            

        }
    }



}