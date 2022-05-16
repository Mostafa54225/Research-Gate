using ResearchGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearchGate.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ResearchGateDBContext _db;
        public CommentRepository()
        {
            _db = new ResearchGateDBContext();
        }
        public CommentRepository(ResearchGateDBContext db)
        {
            _db = db;
        }

        public void AddComment(Comment comment, int paperId)
        {
            var user = _db.Authors.SingleOrDefault(c => c.Username ==  System.Web.HttpContext.Current.User.Identity.Name);

            Comment newComment = new Comment
            {
                AuthorId = user.AuthorId,
                CommentContent = comment.CommentContent,
                PaperId = paperId
            };
            _db.Comments.Add(newComment);
            _db.SaveChanges();

        }

        public Comment DeleteComment(int id)
        {
            Comment comment = _db.Comments.Find(id);
            _db.Comments.Remove(comment);
            _db.SaveChanges();
            return comment;
        }

        public Comment EditComment(int id, string content)
        {
            var comment = _db.Comments.SingleOrDefault(c => c.CommentId == id);
            comment.CommentContent = content;
            _db.SaveChanges();
            return comment;
        }
    }
}