using ResearchGate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace ResearchGate.Repository
{
    public class PaperRepository : IPaperRepository
    {

        private readonly ResearchGateDBContext _db;
        public PaperRepository()
        {
            _db = new ResearchGateDBContext();
        }
        public PaperRepository(ResearchGateDBContext db)
        {
            _db = db;
        }

        public void AddLike(Likes like)
        {
            var authorId = _db.Authors.SingleOrDefault(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name).AuthorId;
            var paperId = _db.Papers.SingleOrDefault(p => p.PaperId == like.PaperId).PaperId;
            bool isLikeExist = _db.Likes.Where(x => x.AuthorId == authorId && x.PaperId == like.PaperId).SingleOrDefault() != null;

            if (isLikeExist)
            {
                var l = _db.Likes.Where(x => x.AuthorId == authorId && x.PaperId == like.PaperId).SingleOrDefault();
                if (l.Status == like.Status)
                    l.Status = 0;
                else
                    l.Status = like.Status;
                _db.Entry(l).State = EntityState.Modified;
            }
            else
            {
                like.AuthorId = authorId;
                like.PaperId = paperId;
                _db.Likes.Add(like);
            }
            _db.SaveChanges();
        }

        public void Create(Paper paper, HttpPostedFileBase file)
        {
            var user = (from obj in _db.Authors
                        where obj.Username.ToLower() == System.Web.HttpContext.Current.User.Identity.Name.ToLower()
                        select obj).FirstOrDefault();
            byte[] bytes;

            if (file.ContentLength != 0)
            {
                using (BinaryReader br = new BinaryReader(file.InputStream))
                {
                    bytes = br.ReadBytes(file.ContentLength);
                    paper.ContentType = file.ContentType;
                    paper.Data = bytes;
                }
            }

            _db.Papers.Add(paper);
            AuthorPapers authorPapers = new AuthorPapers();
            authorPapers.AuthorId = user.AuthorId;
            authorPapers.PaperId = paper.PaperId;
            authorPapers.CreatedBy = user.AuthorId;
            _db.AuthorPapers.Add(authorPapers);
            _db.SaveChanges();
        }

        public List<AuthorPapers> GetAuthorPapers(int authorId)
        {
            return _db.AuthorPapers.Include(a => a.Author).Include(a => a.Paper).Where(x => x.AuthorId == authorId).ToList();
        }


        public int EditPaper(Paper paper, int paperId, HttpPostedFileBase file)
        {
            byte[] bytes;
            var user = _db.Authors.Where(u => u.Username == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            var getPaper = _db.Papers.SingleOrDefault(p => p.PaperId == paperId);
            getPaper.PaperName = paper.PaperName;
            getPaper.PaperDescription = paper.PaperDescription;


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
            IQueryable<AuthorPapers> q = _db.AuthorPapers.Where(a => a.AuthorId.Equals(user.AuthorId));
            bool isCreator = q.Where(x => x.PaperId == paperId && x.CreatedBy == user.AuthorId).FirstOrDefault() != null;

            if (isCreator)
            {
                authorPapers.AuthorId = user.AuthorId;
                authorPapers.PaperId = paperId;
                bool i = _db.AuthorPapers.Where(x => x.AuthorId == user.AuthorId && x.PaperId == paperId).FirstOrDefault() != null;
                if (!i)
                    _db.AuthorPapers.Add(authorPapers);

                _db.Entry(getPaper).State = EntityState.Modified;

                _db.SaveChanges();
                return 1;
            }
            else
            {
                bool isAllow = _db.Permissions.Where(x => x.SenderId == user.AuthorId && x.PaperId == paperId && x.Status == "Approve").FirstOrDefault() != null;
                if (isAllow)
                {
                    authorPapers.AuthorId = user.AuthorId;
                    authorPapers.PaperId = paperId;
                    bool i = _db.AuthorPapers.Where(x => x.AuthorId == user.AuthorId && x.PaperId == paperId).FirstOrDefault() != null;
                    if (!i)
                        _db.AuthorPapers.Add(authorPapers);

                    _db.Entry(getPaper).State = EntityState.Modified;

                    _db.SaveChanges();
                    return 1;
                }
                else
                {
                    return -1;
                    //TempData["NotAuthorized"] = "You're not have the permession to access it";
                    //return RedirectToAction("EditPaper/" + paperId);
                }
            }
        }
    }
}