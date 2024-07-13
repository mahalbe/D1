using ISas.Entities.DashboardEntities;
using ISas.Repository.DashboardRepository.IRepository;
using ISas.Repository.DashboardRepository.Repository;
using ISas.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISas.Web.Controllers.Dashboard
{
    public class AppliedLeaveNoticBoardController : Controller
    {
        // GET: AppliedLeaveNoticBoard
        private IAppliedLeaveNoticBoard _appliedNBRepo;

        public AppliedLeaveNoticBoardController(AppliedLeaveNoticBoardRepo appliedNBRepo)
        {
            _appliedNBRepo=appliedNBRepo;
        }
        public ActionResult AppliedNBList()
        {
            AppliedLeaveNoticBoardModel model = new AppliedLeaveNoticBoardModel();
            model.fromDate = DateTime.Now.ToShortDateString().Replace("-", "/");
            model.toDate = DateTime.Now.AddMonths(1).ToShortDateString().Replace("-", "/");
            model.authority = Session["Authority"].ToString() ;
            return View(model);
        }

        public PartialViewResult _AppliedNBList(string serachName,string fromDate,string toDate)
        {

            return PartialView(_appliedNBRepo.LeaveApproval_TaskList(Session["UserId"].ToString(), serachName, fromDate, toDate));
        }

        public PartialViewResult _AppliedNBList1(string empid,string empname,string designation,string days,string dates, string serachName, string fromDate, string toDate, string lvid)
        {
            AppliedLeaveNoticBoardModel model = _appliedNBRepo.LeaveApproval_TaskList1(Session["UserId"].ToString(), serachName, fromDate, toDate, empid, lvid);
            AppliedLeaveNoticeBoard appliedLeaveNoticeBoard = new AppliedLeaveNoticeBoard();
            appliedLeaveNoticeBoard.StaffID = empid;
            appliedLeaveNoticeBoard.StaffName = empname;
            appliedLeaveNoticeBoard.desigName = designation;
            appliedLeaveNoticeBoard.DayofLeave = Convert.ToDecimal(days);
            appliedLeaveNoticeBoard.Dates = dates;
            model.appliedLeaveNoticeBoardObject = appliedLeaveNoticeBoard;
            model.staffAvailableLeaveList = _appliedNBRepo.GetStaffAvailableLeaveList(empid);
            model.serachName = serachName;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult AppliedNoticeBoard_CRUD(string staffId, string lvId, string lvDate, string lvStatus, string lvRemark, string approvedLvId)
        //public JsonResult AppliedNoticeBoard_CRUD(AppliedLeaveNoticBoardModel model)
        {
            if (ModelState.IsValid)
            {
                //model.userId = Session["UserId"].ToString();
                Tuple<int, string> res = _appliedNBRepo.HR_LeaveApproval_CRUD(staffId, lvId, lvDate, lvStatus, lvRemark, Session["UserId"].ToString(), approvedLvId);
                return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
            }
            var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, Error = modelstate.Value.Errors.FirstOrDefault().ErrorMessage };
            var validKeys = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count == 0) select new { Title = modelstate.Key };
            return Json(new { status = "failed", ErrorList = errors, ValidKeyList = validKeys }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AppliedNoticeBoard_Forward_CRUD(string staffId, string lvId, string lvDate, int state,string remark)
        {
            if (ModelState.IsValid)
            {
                
                Tuple<int, string> res = _appliedNBRepo.HR_LeaveForward_CRUD(staffId, lvId, lvDate, Convert.ToBoolean(state),Session["UserId"].ToString(), remark);
                return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
            }
            var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, Error = modelstate.Value.Errors.FirstOrDefault().ErrorMessage };
            var validKeys = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count == 0) select new { Title = modelstate.Key };
            return Json(new { status = "failed", ErrorList = errors, ValidKeyList = validKeys }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _LeaveApprovalPopUp(string staffId, string lvId, string lvDate,string title)
        {
            AppliedLeaveNoticBoardModel model = new AppliedLeaveNoticBoardModel();
            model.staffAvailableLeaveList = _appliedNBRepo.GetStaffAvailableLeaveList(staffId);
            model.staffId = staffId;
            model.LvID = lvId;
            model.popupTile = title;
            model.ApprovedLvID = lvId;
            model.LeaveDate = lvDate;
            return PartialView(model);
        }
        
        [EncryptedActionParameter]
        public ActionResult AppliedLeaveNBDetails ( string staffName, string staffId)
        {
            AppliedLeaveNoticBoardModel model = _appliedNBRepo.LeaveApproval_TaskList(Session["UserId"].ToString(), "StaffLvDetail", staffId);
            model.StaffName = staffName;
            return  View(model);
        }
    }
}