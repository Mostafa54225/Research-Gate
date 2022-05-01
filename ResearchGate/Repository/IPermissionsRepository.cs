using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResearchGate.Models;
using ResearchGate.ViewModels;
namespace ResearchGate.Repository
{
    public interface IPermissionsRepository
    {
        AuthoPaperViewModel GetPermissions();
        int RequestAccessToPaper(int paperId, string username);

        void Response(Permissions p);

    }
}
