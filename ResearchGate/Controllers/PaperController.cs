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
using ResearchGate.ViewModels;

namespace ResearchGate.Controllers
{
    public class PaperController : Controller
    {

        ResearchGateDBContext db = new ResearchGateDBContext();

        // GET: Paper
        [Authorize]
        public ActionResult Index()
        {
            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                var user = (from obj in db.Authors
                            where obj.Username.ToLower() == User.Identity.Name.ToLower()
                            select obj).FirstOrDefault();

                return View(user);


            }
        }

        [Route("Paper/PaperDetails/{paperId}")]
        public ActionResult PaperDetails(int paperId)
        {
            var author = db.Authors.Where(x => x.Username == User.Identity.Name).SingleOrDefault();

            var paper = db.Papers.Where(x => x.PaperId == paperId).FirstOrDefault();
            var likes = db.Likes.Where(x => x.PaperId == paperId && x.Status == 1).Count();
            var disLikes = db.Likes.Where(x => x.PaperId == paperId && x.Status == -1).Count();

            var comments = db.Comments.Where(c => c.PaperId == paperId).Include(c => c.Author).ToList();

            if (author != null)
            {
                CommentPaperViewModel model = new CommentPaperViewModel
                {
                    Paper = paper,
                    Comment = comments

                };
                var authorReact = db.Likes.Where(x => x.PaperId == paperId && x.AuthorId == author.AuthorId).SingleOrDefault();
                if(authorReact != null)
                {
                    ViewBag.AuthorReact = authorReact.Status;
                }
                ViewBag.Likes = likes;
                ViewBag.DisLikes = disLikes;
                return View(model);
            }

            return View();
            
        }



        [HttpPost]
        [Authorize]
        [Route("Paper/Create/{authorId}")]
        public ActionResult Create(Paper paper, int authorId)
        {
            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {

                byte[] bytes;
                HttpPostedFileBase file = Request.Files["PaperFile"];

                if (file.ContentLength != 0)
                {
                    using (BinaryReader br = new BinaryReader(file.InputStream))
                    {
                        bytes = br.ReadBytes(file.ContentLength);
                        paper.ContentType = file.ContentType;
                        paper.Data = bytes;
                    }
                }

                db.Papers.Add(paper);
                AuthorPapers authorPapers = new AuthorPapers();
                authorPapers.AuthorId = authorId;
                authorPapers.PaperId = paper.PaperId;
                authorPapers.CreatedBy = authorId;
                db.AuthorPapers.Add(authorPapers);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
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

            return File(bytes, contentType, fileName);
        }

        [Route("papers/{authorId}")]
        public ActionResult AuthorPapers(int authorId)
        {
            var ap = db.AuthorPapers.Include(a => a.Author).Include(a => a.Paper).Where(x => x.AuthorId == authorId).ToList();
            //var ap = db.AuthorPapers.Include(a => a.Author).Include(a => a.Paper).GroupBy(p => p.PaperId).Select(x => x.FirstOrDefault()).ToList();
            //var ap = query.Where(x => x.AuthorId == authorId).ToList();
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
        [Route("Paper/EditPaper/{paperId}/{username}")]
        public ActionResult EditPaper(Paper paper, int paperId, string username)
        {
            byte[] bytes;
            var user = db.Authors.Where(u => u.Username == username).FirstOrDefault();
            var getPaper = db.Papers.SingleOrDefault(p => p.PaperId == paperId);
            getPaper.PaperName = paper.PaperName;
            getPaper.PaperDescription = paper.PaperDescription;

            HttpPostedFileBase file = Request.Files["PaperFile"];

            if (file.ContentLength != 0)
            {
                using (BinaryReader br = new BinaryReader(file.InputStream))
                {
                    bytes = br.ReadBytes(file.ContentLength);
                    getPaper.ContentType = file.ContentType;
                    getPaper.Data = bytes;
                }
            }


            AuthorPapers authorPapers = new AuthorPapers();

            //var isExist = db.AuthorPapers.Where(a => a.AuthorId.Equals(user.AuthorId)).ToList();
            IQueryable<AuthorPapers> q = db.AuthorPapers.Where(a => a.AuthorId.Equals(user.AuthorId));
            bool isCreator = q.Where(x => x.PaperId == paperId && x.CreatedBy == user.AuthorId).FirstOrDefault() != null;

            if(isCreator)
            {
                authorPapers.AuthorId = user.AuthorId;
                authorPapers.PaperId = paperId;
                bool i = db.AuthorPapers.Where(x => x.AuthorId == user.AuthorId && x.PaperId == paperId).FirstOrDefault() != null;
                if(!i)
                    db.AuthorPapers.Add(authorPapers);

                db.Entry(getPaper).State = EntityState.Modified;

                db.SaveChanges();
            } else
            {
                bool isAllow = db.Permissions.Where(x => x.SenderId == user.AuthorId && x.PaperId == paperId && x.Status == "Approve").FirstOrDefault() != null;
                if(isAllow)
                {
                    authorPapers.AuthorId = user.AuthorId;
                    authorPapers.PaperId = paperId;
                    bool i = db.AuthorPapers.Where(x => x.AuthorId == user.AuthorId && x.PaperId == paperId).FirstOrDefault() != null;
                    if (!i)
                        db.AuthorPapers.Add(authorPapers);

                    db.Entry(getPaper).State = EntityState.Modified;

                    db.SaveChanges();
                } else
                {
                    TempData["NotAuthorized"] = "You're not have the permession to access it";
                    return RedirectToAction("EditPaper/"+ paperId);
                }
            }

            
            return RedirectToAction("Index", "Home");


        }



        
        [Authorize]
        [HttpPost]
        [Route("Paper/AddLike")]
        public ActionResult AddLike(Likes like)
        {
            var authorId = db.Authors.SingleOrDefault(x => x.Username == User.Identity.Name).AuthorId;
            var paperId = db.Papers.SingleOrDefault(p => p.PaperId == like.PaperId).PaperId;
            bool isLikeExist = db.Likes.Where(x => x.AuthorId == authorId && x.PaperId == like.PaperId).SingleOrDefault() != null;
            
            if (isLikeExist)
            {
                var l = db.Likes.Where(x => x.AuthorId == authorId && x.PaperId == like.PaperId).SingleOrDefault();
                if(l.Status == like.Status)
                    l.Status = 0;
                else
                    l.Status = like.Status;
                db.Entry(l).State = EntityState.Modified;
            } else
            {
                like.AuthorId = authorId;
                like.PaperId = paperId;
                db.Likes.Add(like);
            }
            db.SaveChanges();
            return RedirectToAction("PaperDetails/" + like.PaperId, "Paper");
        }
    }
}