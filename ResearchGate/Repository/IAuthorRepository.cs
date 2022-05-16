using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ResearchGate.Models;
namespace ResearchGate.Repository
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> GetAll();
        Author GetByUsername(string username);
        void Insert(Author author);
        void Update(Author author, string currentUser, HttpPostedFileBase file);
        void Delete(int authorId);
        IEnumerable<Author> Search(string option, string search);
        bool CheckHashState(Author author, string currentUser);

        int Register(Author account, HttpPostedFileBase file);
        int Login(string email, string password);



        List<AuthorPapers> GetAllAuthorPaper();
        void Save();
    }
}
