using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ResearchGate.Models;
using ResearchGate.ViewModels;

namespace ResearchGate.Repository
{
    public class PermissionsRepository: IPermissionsRepository
    {
        private readonly ResearchGateDBContext _db;

        public PermissionsRepository()
        {
            _db = new ResearchGateDBContext();
        }
        public PermissionsRepository(ResearchGateDBContext db)
        {
            _db = db;
        }


        public AuthoPaperViewModel GetPermissions()
        {
            var user = (from obj in _db.Authors
                        where obj.Username.ToLower() == System.Web.HttpContext.Current.User.Identity.Name.ToLower()
                        select obj).FirstOrDefault();
            var requests = _db.Permissions.Where(p => p.AuthorId == user.AuthorId && p.Status == "Pending").ToList();
            var authors = (from i in requests
                           join p in _db.Authors on i.SenderId equals p.AuthorId
                           select p).ToList();
            var papers = (from i in requests
                          join p in _db.Papers on i.PaperId equals p.PaperId
                          select p).ToList();

            var model = new AuthoPaperViewModel { Authors = authors, Papers = papers };
            return model;
        }

        public int RequestAccessToPaper(int paperId, string username)
        {
            var user = _db.Authors.Where(u => u.Username == username).FirstOrDefault();  // the user who request the access
            var getPaper = _db.Papers.SingleOrDefault(paper => paper.PaperId == paperId);        // the paper that user needs to access
            bool isExist = _db.Permissions.Where(x => x.SenderId == user.AuthorId && x.PaperId == paperId).FirstOrDefault() != null;

            if (!isExist)
            {
                IQueryable<AuthorPapers> q = _db.AuthorPapers.Where(a => a.PaperId.Equals(getPaper.PaperId));
                var authorId = q.Where(x => x.CreatedBy != 0).FirstOrDefault().AuthorId;

                Permissions p = new Permissions { SenderId = user.AuthorId, AuthorId = authorId, PaperId = paperId, Status = "Pending" };
                _db.Permissions.Add(p);
                _db.SaveChanges();
                return 1;
            }
            else
            {
                var req = _db.Permissions.Where(x => x.SenderId == user.AuthorId && x.PaperId == paperId).FirstOrDefault();
                if (req.Status == "Refuse")
                {
                    req.Status = "Pending";
                    _db.Entry(req).State = EntityState.Modified;
                    _db.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
        }

        public void Response(Permissions p)
        {
            var per = _db.Permissions.Where(x => x.SenderId == p.SenderId && x.PaperId == p.PaperId).FirstOrDefault();
            per.Status = p.Status;
            _db.Entry(per).State = EntityState.Modified;

            _db.SaveChanges();
        }

    }
}