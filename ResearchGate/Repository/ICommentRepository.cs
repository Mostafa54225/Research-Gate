using ResearchGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchGate.Repository
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment, int paperId);
        Comment DeleteComment(int id);
        Comment EditComment(int id, string content);
    }
}
