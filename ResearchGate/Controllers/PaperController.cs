using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using ResearchGate.Repository;
using ResearchGate.ViewModels;

namespace ResearchGate.Controllers
{
    public class PaperController : Controller
    {
        ResearchGateDBContext db = new ResearchGateDBContext();


        private IPaperRepository _paperRepository;

        public PaperController()
        {
            _paperRepository = new PaperRepository(new ResearchGateDBContext());
        }

        public PaperController(IPaperRepository paperRepository)
        {
            _paperRepository = paperRepository;
        }


        // GET: Paper
        [Authorize]
        public ActionResult Index()
        {

            return View();

            
        }

        [Route("Paper/PaperDetails/{paperId}")]
        public ActionResult PaperDetails(int paperId)
        {
            var author = db.Authors.Where(x => x.Username == User.Identity.Name).SingleOrDefault();

            var paper = db.Papers.Where(x => x.PaperId == paperId).FirstOrDefault();
            var likes = db.Likes.Where(x => x.PaperId == paperId && x.Status == 1).Count();
            var disLikes = db.Likes.Where(x => x.PaperId == paperId && x.Status == -1).Count();

            var comments = db.Comments.Where(c => c.PaperId == paperId).Include(c => c.Author).ToList();
            CommentPaperViewModel model = new CommentPaperViewModel
            {
                Paper = paper,
                Comment = comments
            };
            if (author != null)
            {

                var authorReact = db.Likes.Where(x => x.PaperId == paperId && x.AuthorId == author.AuthorId).SingleOrDefault();
                if(authorReact != null)
                {
                    ViewBag.AuthorReact = authorReact.Status;
                }

            }
            ViewBag.Likes = likes;
            ViewBag.DisLikes = disLikes;
            return View(model);

        }




        [HttpPost]
        [Authorize]
        [Route("Paper/Create")]
        public ActionResult Create(Paper paper)
        {
           
            HttpPostedFileBase file = Request.Files["PaperFile"];
            _paperRepository.Create(paper, file);
            
            return RedirectToAction("Index", "Home");
            
        }



        [Route("papers/{authorId}")]
        public ActionResult AuthorPapers(int authorId)
        {
            var ap = _paperRepository.GetAuthorPapers(authorId);
            if (ap.Count != 0)
                return View(ap);

            return View();


        }


        [HttpGet]
        [Route("Paper/EditPaper/{paperId}")]
        public ActionResult EditPaper(int paperId)
        {
            var getPaper = db.Papers.SingleOrDefault(p => p.PaperId == paperId);
            return View(getPaper);
        }


        [HttpPost]
        [Route("Paper/EditPaper/{paperId}")]
        public ActionResult EditPaper(Paper paper, int paperId)
        {
            HttpPostedFileBase file = Request.Files["PaperFile"];
            var response = _paperRepository.EditPaper(paper, paperId, file);

            if(response == -1)
            {
                TempData["NotAuthorized"] = "You're not have the permession to access it";
                return RedirectToAction("EditPaper/" + paperId);
            }
            
            return RedirectToAction("Index", "Home");


        }



        
        [Authorize]
        [HttpPost]
        [Route("Paper/AddLike")]
        public ActionResult AddLike(Likes like)
        {
            _paperRepository.AddLike(like);
            return RedirectToAction("PaperDetails/" + like.PaperId, "Paper");
        }



        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            var paper = db.Papers.Where(p => p.PaperId == fileId).FirstOrDefault();
            bytes = (byte[])paper.Data;
            contentType = paper.ContentType.ToString();
            fileName = paper.PaperName.ToString();

            return base.File(bytes, contentType);
        }

        [HttpGet]
        public ActionResult GetFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            var paper = db.Papers.Where(p => p.PaperId == fileId).FirstOrDefault();
            bytes = (byte[])paper.Data;
            contentType = paper.ContentType.ToString();
            fileName = paper.PaperName.ToString();

            return base.File(bytes, contentType);
        }
    }
}