using ISas.Entities.CommonEntities;
using ISas.Entities.DashboardEntities;
using ISas.Repository.DashboardRepository.IRepository;
using ISas.Repository.Interface;
using ISas.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISas.Web.Controllers.Dashboard
{
    [Authorize]
    [ExceptionHandler]
    public class DashBoard_StaffStataticsController : Controller
    {
        private IDashBoard_StaffStataticsRepo _staffStatics;
        private ICommonRepo _commonRepo;
        public DashBoard_StaffStataticsController(IDashBoard_StaffStataticsRepo staffStatics, ICommonRepo commonRepo)
        {
            _staffStatics = staffStatics;
            _commonRepo = commonRepo;
        }
        public ViewResult MyClassInfo()
        {
            return View(_staffStatics.GetClassInfo(Session["SessionId"].ToString(), Session["UserId"].ToString()));
        }

        public ViewResult SalryDetails()
        {
            return View(_staffStatics.GetSalaryDetails(Session["SessionId"].ToString(), Session["UserId"].ToString()));
        }

        //public JsonResult GetLeaveBalanceSummary()
        //{
        //    List<LeaveBalanceDetailsModel> leaveBalances = _staffStatics.GetStaffLeaveBalanceDetails(Session["UserId"].ToString(), DateTime.Now.Month, DateTime.Now.Year);
        //    var graphData = leaveBalances.Select(r => new SelectListItem
        //    {
        //        Text = r.LvCode,
        //        Value = r.LeaveAvailed.ToString(),
        //    }).ToList();

        //    if (graphData != null)
        //        graphData.Add(new SelectListItem { Text = "Balance", Value = leaveBalances.Sum(r => r.LeaveAvailable).ToString() });

        //    return Json(graphData, JsonRequestBehavior.AllowGet);
        //}

        public ViewResult BookHistory()
        {
            return View(_staffStatics.GetBookHistory(Session["SessionId"].ToString(), Session["UserId"].ToString()));
        }

        public ViewResult SMSDetails()
        {
            return View(_staffStatics.GetSMSDetails(Session["SessionId"].ToString(), Session["UserId"].ToString()));
        }


        public ViewResult AttendanceDetails()
        {
            Staff_AttendanceDetailsModel model = _staffStatics.GetStaffAttendanceInfo_FormLoad(Session["UserId"].ToString(), DateTime.Now.Month, DateTime.Now.Year);
            if (model != null)
            {
                model.FromDate = DateTime.Now.ToShortDateString().Replace("-", "/");
                model.ToDate = DateTime.Now.ToShortDateString().Replace("-", "/");
            }
            return View(model);
        }

        public PartialViewResult _StaffAttenMonthCalenderPartial()
        {
            return PartialView();
        }

        public ActionResult AsyncUpdateCalender(int month, int year)
        {
            List<Tuple<string, string>> attendenceDetails = _staffStatics.GetStaffAttenDetails(month, year, Session["UserId"].ToString());
            WeekForMonth model = DashboardController.getCalender(month, year, attendenceDetails, Session["UserId"].ToString());
            model.MonthNameWithYear = DateHelpers.GetMonthByName(month) + " " + year.ToString();
            return PartialView("_StaffAttenMonthCalenderPartial", model);
        }


        public JsonResult CancelLeave(string transid)
        {
            Tuple<int, string> res = _staffStatics.LeaveCancel_CRUD(Convert.ToInt32(transid),Session["UserId"].ToString());
            return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DashBoard_StaffStatatics_CRUD(Staff_AttendanceDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = Session["UserID"].ToString();
                Tuple<int, string> res = _staffStatics.LeaveApply_CRUD(model);
                return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
            }
            var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, Error = modelstate.Value.Errors.FirstOrDefault().ErrorMessage };
            var validKeys = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count == 0) select new { Title = modelstate.Key };
            return Json(new { status = "failed", ErrorList = errors, ValidKeyList = validKeys }, JsonRequestBehavior.AllowGet);
        }

        //Birthday Messege Send From Staff to Student
        public PartialViewResult _Staff_BirthdayMessege(string sendToImgURL, string sendToName, string sendToId)
        {
            AlertTransactionModel model = new AlertTransactionModel();
            model.UserId = Session["UserId"].ToString();
            model.AlertForUser = sendToId;
            model.IsCancelled = false;model.MarkRead = false;

            model.AlertFor = "Student BirthDay";

            model.Temp_SendToImageURL = sendToImgURL;
            model.Temp_SendToName = sendToName;
            
            return PartialView(model);
        }

        public PartialViewResult _NotificationList()
        {
            return PartialView(_commonRepo.GetNotificationList(Session["UserId"].ToString()));
        }
    }
}