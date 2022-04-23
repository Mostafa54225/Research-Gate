using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;


namespace ResearchGate.Controllers
{
    public class CommentController : Controller
    {

        [Authorize]
        [Route("Comment/Add/{paperId}/")]
        public ActionResult Add(Comment comment, int paperId)
        {

            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                if (ModelState.IsValid)
                {
                    var user = db.Authors.SingleOrDefault(c => c.Username == User.Identity.Name);

                    Comment newComment = new Comment
                    {
                        AuthorId = user.AuthorId,
                        CommentContent = comment.CommentContent,
                        PaperId = paperId
                    };
                    db.Comments.Add(newComment);
                    db.SaveChanges();
                }
            }
            return Redirect("/Paper/PaperDetails/" + paperId);
        }

        public ActionResult Delete(int id)
        {
            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                Comment comment = db.Comments.Find(id);
                db.Comments.Remove(comment);
                db.SaveChanges();
                return Redirect("/Paper/PaperDetails/" + comment.PaperId);
            }
        }

        public ActionResult Edit(int id, string content)
        {
            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                var comment = db.Comments.SingleOrDefault(c => c.CommentId == id);
                comment.CommentContent = content;
                db.SaveChanges();
                return Redirect("/Paper/PaperDetails/" + comment.PaperId);
            }
        }
    }
}