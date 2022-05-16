using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ResearchGate.Models;
namespace ResearchGate.Repository
{
    public interface IPaperRepository
    {
        void Create(Paper paper, HttpPostedFileBase file);

        List<AuthorPapers> GetAuthorPapers(int authorId);

        void AddLike(Likes like);

        int EditPaper(Paper paper, int paperId, HttpPostedFileBase file);

    }
}
