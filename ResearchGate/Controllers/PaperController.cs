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

        [HttpPost]
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


        [Authorize]
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
            bool isExist = q.Where(x => x.PaperId == paperId).FirstOrDefault() != null;

            if(!isExist)
            {
                authorPapers.AuthorId = user.AuthorId;
                authorPapers.PaperId = paperId;
                db.AuthorPapers.Add(authorPapers);
            }

            db.Entry(getPaper).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index", "Home");


        }


        //public ActionResult GetAuthorsPaper()

        //{
        //    using(ResearchGateDBContext db = new ResearchGateDBContext())
        //    {
        //        var ap = db.AuthorPapers.Include(a => a.Author).Include(a => a.Paper).GroupBy(p => p.PaperId).Select(x => x.FirstOrDefault()).ToList();
        //        return View("Index");
        //    }
        //}

        //public ActionResult GetPapers()
        //{
        //    using(ResearchGateDBContext db = new ResearchGateDBContext())
        //    {
        //        var ap = db.Papers.Include(a => a.AuthorPapers).ToList();
        //        return View("Index");
        //    }
        //}



    }
}