using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using ResearchGate.Models;
namespace ResearchGate.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ResearchGateDBContext _db;
        public AuthorRepository()
        {
            _db = new ResearchGateDBContext();
        }
        public AuthorRepository(ResearchGateDBContext db)
        {
            _db = db;
        }

        public IEnumerable<Author> GetAll()
        {
            return _db.Authors.ToList();
        }
        public Author GetByUsername(string username)
        {
            return _db.Authors.SingleOrDefault(c => c.Username.ToLower() == username.ToLower());
        }
        public void Insert(Author author)
        {
            _db.Authors.Add(author);
        }

        public void Update(Author account, string currentUser, HttpPostedFileBase file)
        {
            Author user = GetByUsername(currentUser);

            if (CheckHashState(account, currentUser))
            {
                var salt = Utils.PasswordHashing.GeneratePassword(10);
                var hp = Utils.PasswordHashing.EncodePassword(account.Password, salt);
                user.Password = hp;
                user.Salt = salt;
            }
            else
            {

                account.Password = user.Password;
                account.Salt = user.Salt;
            }
            user.FirstName = account.FirstName;
            user.LastName = account.LastName;
            user.Email = account.Email;
            user.Department = account.Department;
            user.University = account.University;
            user.Username = account.Username;
            user.Mobile = account.Mobile;
            if (file.ContentLength != 0)
            {
                user.Image = Utils.Helper.ConvertToBytes(file);
            }

            _db.Entry(user).State = EntityState.Modified;
            FormsAuthentication.SetAuthCookie(user.Username, false);
        }

        public bool CheckHashState(Author account, string currentUser)
        {
            var user = GetByUsername(currentUser);
            var hashedPassword = Utils.PasswordHashing.EncodePassword(account.Password, user.Salt);
            if (hashedPassword != user.Password)
                return true;
            return false;
        }


        public void Delete(int authorId)
        {
            Author author = _db.Authors.Find(authorId);
            _db.Authors.Remove(author);
        }



        public int Register(Author account, HttpPostedFileBase file)
        {

            var salt = Utils.PasswordHashing.GeneratePassword(10);
            var password = Utils.PasswordHashing.EncodePassword(account.Password, salt);
            account.Password = password;
            account.Salt = salt;
            account.Username = account.Username.ToLower();
            bool isExist = _db.Authors.Where(u => u.Email.ToLower().Equals(account.Email.ToLower())).FirstOrDefault() != null;

            if (!isExist)
            {
                //HttpPostedFileBase file = Request.Files["ProfileImage"];
                if (file.ContentLength != 0)
                {
                    account.Image = Utils.Helper.ConvertToBytes(file);
                }
                _db.Authors.Add(account);
                _db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
                //ModelState.AddModelError("", "Email is Already Exist!");

            }
        }
        public int Login(string email, string password)
        {

            var getUser = (from obj in _db.Authors where obj.Email == email select obj).FirstOrDefault();
            if (getUser != null)
            {
                var hashCode = getUser.Salt;
                var hashedPassword = Utils.PasswordHashing.EncodePassword(password, hashCode);

                var query = (from obj in _db.Authors
                                where (obj.Email == email && obj.Password.Equals(hashedPassword)
                                )
                                select obj).FirstOrDefault();

                if (query != null)
                {
                    FormsAuthentication.SetAuthCookie(query.Username, false);
                    return 1;
                }
                return 0;
            }
            return 0;
        }



        public void Save()
        {
            _db.SaveChanges();
        }
        private bool dispose = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.dispose)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.dispose = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}