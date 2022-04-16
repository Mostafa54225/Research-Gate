using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using ResearchGate.ViewModels;

namespace ResearchGate.Controllers
{
    public class RequestsController : Controller
    {

        ResearchGateDBContext db = new ResearchGateDBContext();
        

        [Authorize]
        public ActionResult Index()
        {
            var user = (from obj in db.Authors
                        where obj.Username.ToLower() == User.Identity.Name.ToLower()
                        select obj).FirstOrDefault();
            var requests = db.Permissions.Where(p => p.AuthorId == user.AuthorId && p.Status == 0).ToList();
            var authors = (from i in requests
                         join p in db.Authors on i.SenderId equals p.AuthorId select p).ToList();
            var papers = (from i in requests    
                          join p in db.Papers on i.PaperId equals p.PaperId
                          select p).ToList();

            var model = new AuthoPaperViewModel { Authors = authors, Papers = papers };
            return View(model);
        }



        [HttpPost]
        [Route("Requests/RequestAccess/{paperId}/{username}")]
        public ActionResult RequestAccess(int paperId, string username)
        {
            var user = db.Authors.Where(u => u.Username == username).FirstOrDefault();  // the user who request the access
            var getPaper = db.Papers.SingleOrDefault(paper => paper.PaperId == paperId);        // the paper that user needs to access
            bool isExist = db.Permissions.Where(x => x.SenderId == user.AuthorId && x.PaperId == paperId).FirstOrDefault() != null;

            if (!isExist)
            {
                IQueryable<AuthorPapers> q = db.AuthorPapers.Where(a => a.PaperId.Equals(getPaper.PaperId));
                var authorId = q.Where(x => x.CreatedBy != 0).FirstOrDefault().AuthorId;

                Permissions p = new Permissions { SenderId = user.AuthorId, AuthorId = authorId, PaperId = paperId };
                db.Permissions.Add(p);
                db.SaveChanges();
                TempData["ResponseToRequest"] = "Request Access has successfully sent";
            } else
            {
                var req = db.Permissions.Where(x => x.SenderId == user.AuthorId && x.PaperId == paperId).FirstOrDefault();
                if(req.Status == -1)
                {
                    req.Status = 0;
                    db.Entry(req).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["ResponseToRequest"] = "Request Access has successfully send";
                } else
                {
                    TempData["ResponseToRequest"] = "You have already sent a request. Please wait";
                }
                
            }
            

            return RedirectToAction("EditPaper/" + paperId, "Paper");
        }


        [Route("Requests/ResponseToRequest")]
        public ActionResult ResponseToRequest(Permissions p)
        {
            var per = db.Permissions.Where(x => x.SenderId == p.SenderId && x.PaperId == p.PaperId).FirstOrDefault();
            per.Status = p.Status;
            db.Entry(per).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}