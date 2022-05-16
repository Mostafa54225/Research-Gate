using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using System.Data.Entity;
using ResearchGate.Repository;

namespace ResearchGate.Controllers
{
    public class HomeController : Controller
    {
        private IAuthorRepository _authorRepository;

        public HomeController()
        {
            _authorRepository = new AuthorRepository(new ResearchGateDBContext());
        }

        public HomeController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public ActionResult Index()
        {
            List<AuthorPapers> ap = _authorRepository.GetAllAuthorPaper();

            return View(ap);
        }

        [HttpPost]
        public ActionResult search(string option, string search)
        {

            var data = (dynamic)null;
            data = _authorRepository.Search(option, search);

            if(data.Count == 0)
            {
                ViewBag.Authors = "Not Found";
            }


            return View("search", data);
        }
    }
}