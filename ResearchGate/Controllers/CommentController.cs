using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using ResearchGate.Repository;

namespace ResearchGate.Controllers
{
    public class CommentController : Controller
    {

        private ICommentRepository _commentRepository;

        public CommentController()
        {
            _commentRepository = new CommentRepository(new ResearchGateDBContext());
        }

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }


        [Authorize]
        [Route("Comment/Add/{paperId}/")]
        public ActionResult Add(Comment comment, int paperId)
        {

            if (ModelState.IsValid)
            {
                _commentRepository.AddComment(comment, paperId);
            }
            return Redirect("/Paper/PaperDetails/" + paperId);
        }

        public ActionResult Delete(int id)
        {
            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                var comment = _commentRepository.DeleteComment(id);
                return Redirect("/Paper/PaperDetails/" + comment.PaperId);
            }
        }

        public ActionResult Edit(int id, string content)
        {
            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                var comment = _commentRepository.EditComment(id, content);
                return Redirect("/Paper/PaperDetails/" + comment.PaperId);
            }
        }
    }
}