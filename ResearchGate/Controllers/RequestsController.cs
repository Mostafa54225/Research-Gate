using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using ResearchGate.ViewModels;
using ResearchGate.Repository;
namespace ResearchGate.Controllers
{
    public class RequestsController : Controller
    {

        private IPermissionsRepository _permissionRepository;
        public RequestsController()
        {
            _permissionRepository = new PermissionsRepository(new ResearchGateDBContext());
        }
        public RequestsController(IPermissionsRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        ResearchGateDBContext db = new ResearchGateDBContext();

        [Authorize]
        public ActionResult Index()
        {
            var model = _permissionRepository.GetPermissions();
            return View(model);
        }



        [HttpPost]
        [Route("Requests/RequestAccess/{paperId}/{username}")]
        public ActionResult RequestAccess(int paperId, string username)
        {
            var response = _permissionRepository.RequestAccessToPaper(paperId, username);
            if(response == 1)
                TempData["ResponseToRequest"] = "Request Access has successfully sent";
            else
                TempData["ResponseToRequest"] = "You have already sent a request. Please wait";

            return RedirectToAction("EditPaper/" + paperId, "Paper");
        }


        [Route("Requests/ResponseToRequest")]
        public ActionResult ResponseToRequest(Permissions p)
        {
            _permissionRepository.Response(p);
            return RedirectToAction("Index");

        }
    }
}