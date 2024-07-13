using ISas.Entities.DashboardEntities;
using ISas.Repository.DashboardRepository.IRepository;
using ISas.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ISas.Web.Controllers.Dashboard
{
    [Authorize]
    [ExceptionHandler]
    public class ParentRequestMasterController : Controller
    {
        private IDashBoard_ParentRequestMasterRepo _requestMstRepo;
        public ParentRequestMasterController(IDashBoard_ParentRequestMasterRepo requestMaster)
        {
            this._requestMstRepo = requestMaster;
        }

        public List<DashBoard_ParentRequestMasterModel> GetParentRequestMasterList(string UserID, string Category)
        {
            return this._requestMstRepo.GetParentRequestMasterList(UserID, Category);
        }

        public JsonResult ParentRequestMaster_CRUD(DashBoard_ParentRequestMasterModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserID = Session["UserID"].ToString();
                string messege = this._requestMstRepo.ParentRequestMaster_CRUD(model);
                return Json(new { status = "success", Msg = messege }, JsonRequestBehavior.AllowGet);
            }

            var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, Error = modelstate.Value.Errors.FirstOrDefault().ErrorMessage };
            var validKeys = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count == 0) select new { Title = modelstate.Key };
            return Json(new { status = "failed", ErrorList = errors, ValidKeyList = validKeys }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _RequestDetails(string Category)
        {
            return PartialView(this._requestMstRepo.GetParentRequestMasterList(Session["UserID"].ToString(), Category));
        }

        public PartialViewResult _RequestCommunicationDetails(string RequestID)
        {
            DashBoard_ParentRequestMasterModel model = new DashBoard_ParentRequestMasterModel();
            model.RequestId = RequestID;
            model.Function = "COMM";
            model.CommunicationDetailsList = this._requestMstRepo.GetCommunicationDetails(RequestID, Session["UserID"].ToString());
            return PartialView(model);
        }

        public PartialViewResult _RequestCommunication_DisplayMsg(string RequestID)
        {
            return PartialView(this._requestMstRepo.GetCommunicationDetails(RequestID, Session["UserID"].ToString()));
        }
    }
}
