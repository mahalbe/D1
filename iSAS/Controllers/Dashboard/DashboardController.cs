using ISas.Entities.DashboardEntities;
using ISas.Repository.DashboardRepository.IRepository;
using ISas.Repository.Interface;
using ISas.Repository.StaffRepository.IRepository;
using ISas.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;

namespace ISas.Web.Controllers.Dashboard
{
    [Authorize]
    [ExceptionHandler]
    public class DashboardController : Controller
    {
        private IStudentProfileRepo _StudentProfileRepo;
        private IDashboardRepo _dashRepo;
        private IStudentSession _sessionRepos;
        private IStaff_StaffDetailMasterRepo _staffRepo;
        private IDashBoard_ParentRequestMasterRepo _requestMstRepo;
        private ParentRequestMasterController _requestMstController;
        private IDashBoard_StudentStataticsRepo _studstataticsRepo;
        public DashboardController(IStudentProfileRepo studentProfile, IDashboardRepo dashRepo, IStudentSession sessionRepo, ParentRequestMasterController RequestMstController, IDashBoard_ParentRequestMasterRepo requestMaster, ParentRequestMasterController requestMstController, IStaff_StaffDetailMasterRepo staffRepo, IDashBoard_StudentStataticsRepo staticRepo)
        {
            _StudentProfileRepo = studentProfile;
            _dashRepo = dashRepo;
            _sessionRepos = sessionRepo;
            _requestMstRepo = requestMaster;
            _requestMstController = requestMstController;
            _staffRepo = staffRepo;
            _studstataticsRepo = staticRepo;
        }
        
        public ActionResult Dashboard()
        {
            string authType1 = User.Identity.AuthenticationType;
            bool isAuth1 = User.Identity.IsAuthenticated;
            string authName1 = User.Identity.Name;

            if (User.IsInRole("Student"))
            {
                Dashboard_StudentModel studModel = new Dashboard_StudentModel();
                Session["LoginStudentERPNo"] = studModel.ERPNo;
                if (studModel.AttenDetails == null)
                    studModel.AttenDetails = new AttendanceSummaryModel();
                return View("Dashboard_Student", studModel);
            }
            else if (User.IsInRole("Admin") || User.IsInRole("Superadmin"))
            {
                DashboardModel_Admin dashboardModel_Admin = new DashboardModel_Admin();
                return View("Dashboard_Admin", dashboardModel_Admin);
            }
            else if (User.IsInRole("Principal") || User.IsInRole("Coordinator") || User.IsInRole("Class Teacher")
                || User.IsInRole("User") || User.IsInRole("Teacher"))
            {

                Dashboard_StaffModel dashboard_StaffModel = new Dashboard_StaffModel();
                return View("Dashboard_Staff", dashboard_StaffModel);
            }
            return View();
        }

        public JsonResult GetEvents()
        {
            List<Event> events = new List<Event>();
            if (Session["EventsList"] == null)
            {
                events.Add(new Event { EventID = 1, Subject = "Half Day", Start = DateTime.Now, End = DateTime.Now, Description = "Testing", IsFullDay = false, ThemeColor = "green" });
                events.Add(new Event { EventID = 2, Subject = "Full Day", Start = DateTime.Now, End = null, Description = "Testing", IsFullDay = true, ThemeColor = "green" });
                Session["EventsList"] = events;
            }
            else
            {
                events = Session["EventsList"] as List<Event>;
            }
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            List<Event> events = Session["EventsList"] as List<Event>;
            e.EventID = events.Max(r => r.EventID + 1);
            events.Add(e);
            Session["EventsList"] = null;
            Session["EventsList"] = events;
            status = true;
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            List<Event> events = Session["EventsList"] as List<Event>;
            Event evt = events.Where(r => r.EventID == eventID).FirstOrDefault();
            if (evt != null)
                events.Remove(evt);
            status = true;
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult UnderConstruction()
        {
            return View();
        }

        public ActionResult StudentProfile()
        {
            return View(_StudentProfileRepo.GetStudentProfileById(Session["UserID"].ToString()));
        }

        public ActionResult StudentAttendanceDetails()
        {
            AttnDetails_ParentModel model = new AttnDetails_ParentModel();
            model.AppliedLeaveDetails = this._requestMstController.GetParentRequestMasterList(Session["UserID"].ToString(), "AT");
            model.AttenDetails = _dashRepo.AttendanceDetails(Session["LoginStudentERPNo"].ToString(), Session["SessionId"].ToString());
            model.ApplyNewLeave.Function = "SAVE";
            var sessions = this._sessionRepos.GetAllSessions();
            if (sessions != null && sessions.Count() > 0)
                model.SessionList = sessions.OrderByDescending(p => p.PrintOrder).Select(p => new SelectListItem
                {
                    Text = p.SessionDisplayName,
                    Value = p.SessId,
                    Selected = p.IsDefault
                }).ToList();

            if (model.SessionList != null && model.SessionList.Where(r => r.Selected).Count() > 0)
                model.SessionID = Convert.ToInt32(model.SessionList.Where(r => r.Selected).FirstOrDefault().Value);

            return View(model);
        }

        public ActionResult _StudentAttenMonthCalenderPartial()
        {
            return View();
        }

        public JsonResult GetDataForStudentAttn(int sessionId)
        {
            List<AttnDetailsModel> attenDetails = _dashRepo.GetStudentAttenDetails_BySession(sessionId, Session["LoginStudentERPNo"].ToString());
            var result = attenDetails.Select(r => new
            {
                Month = r.MonthName,
                Leave = r.LeaveCount,
                Absent = r.AbsentCount,
                Present = r.PresentCount
            }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApplyLeave_CRUD(AttnDetails_ParentModel allData)
        {
            DashBoard_ParentRequestMasterModel model = allData.ApplyNewLeave;
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

        //
        public JsonResult getAdminDashboard_GraphData()
        {
            DataSet ds = _dashRepo.GetAdminDashboard_GraphData(DateTime.Now.ToShortDateString(), Session["SessionID"].ToString());

            var attenSummary = ds.Tables[0].AsEnumerable().Select(r => new
            {
                AttenStatus = r.Field<string>("AttendanceStatus"),
                NoOfStudent = r.Field<int>("NoofCount"),
            }).ToArray();

            #region School Strength
            DashboardModel_Admin model = new DashboardModel_Admin();
            for (int i = 2; i < ds.Tables[1].Columns.Count; i++)
            {
                model.SectionList.Add(ds.Tables[1].Columns[i].ColumnName);
            }
            var array = DataTableX.ToDynamic(ds.Tables[1]).ToArray();

            int totalStrength = 0;
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                for (int j = 2; j < ds.Tables[1].Columns.Count; j++)
                {
                    int classStrength = 0;
                    int.TryParse(ds.Tables[1].Rows[i][j].ToString(), out classStrength);
                    totalStrength += classStrength;
                }
            }
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });
            var json = serializer.Serialize(array);
            var sectionList = model.SectionList.Select(r => new
            {
                balloonText = "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
                fillAlphas = 0.8,
                labelText = "[[value]]",
                lineAlpha = 0.3,
                title = r,
                type = "column",
                color = "#000000",
                valueField = r
            }).ToArray();
            #endregion

            var staffAtten = ds.Tables[2].AsEnumerable().Select(r => new
            {
                Class = r.Field<string>("Department"),
                Leave = r.Field<int>("L"),
                Absent = r.Field<int>("A"),
                Present = r.Field<int>("P"),
            }).ToList();
            return Json(new { Strength = json, Sections = sectionList, TotalStrength = totalStrength , AttnSummary = attenSummary, StaffAttn = staffAtten }, JsonRequestBehavior.AllowGet);
        }


        //for ajax request
        public ActionResult AsyncUpdateCalender(int month, int year, string erpNo)
        {
            List<Tuple<string, string>> attendenceDetails = this._dashRepo.GetStudentAttenDetails(month, year, erpNo);
            WeekForMonth model = getCalender(month, year, attendenceDetails, erpNo);
            model.MonthNameWithYear = DateHelpers.GetMonthByName(month) + " " + year.ToString();
            return PartialView("_StudentAttenMonthCalenderPartial", model);
        }
        public PartialViewResult _StudentAttenMonthCalenderPartial_ForPrint(int month, int year, string userId)
        {
            List<Tuple<string, string>> attendenceDetails = this._dashRepo.GetStudentAttenDetails_ByERPNo(month, year, userId);
            WeekForMonth model = getCalender(month, year, attendenceDetails, userId);
            model.MonthNameWithYear = DateHelpers.GetMonthByName(month) + " " + year.ToString();
            return PartialView(model);
        }
        //main function to arrange each details of day into a month, function returns a list of weeks for a month
        public static WeekForMonth getCalender(int month, int year, List<Tuple<string, string>> attendenceDetails, string erpNo)
        {
            WeekForMonth weeks = new WeekForMonth();
            weeks.Week1 = new List<Day>();
            weeks.Week2 = new List<Day>();
            weeks.Week3 = new List<Day>();
            weeks.Week4 = new List<Day>();
            weeks.Week5 = new List<Day>();
            weeks.Week6 = new List<Day>();

            List<DateTime> dt = new List<DateTime>();

            dt = DateHelpers.GetDates(year, month);
            foreach (DateTime day in dt)
            {
                switch (DateHelpers.GetWeekOfMonth(day))
                {
                    case 1:
                        Day dy1 = new Day();

                        dy1.Date = day;
                        dy1._Date = day.ToShortDateString();
                        dy1.dateStr = day.ToString("MM/dd/yyyy");
                        dy1.dtDay = day.Day;
                        dy1.daycolumn = DateHelpers.GetDateInfo(dy1.Date);
                        weeks.Week1.Add(dy1);
                        dy1.AttendenceStatus = attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault() == null ? "NA" : attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault().Item2;
                        break;
                    case 2:
                        Day dy2 = new Day();
                        dy2.Date = day;
                        dy2._Date = day.ToShortDateString();
                        dy2.dateStr = day.ToString("MM/dd/yyyy");
                        dy2.dtDay = day.Day;
                        dy2.daycolumn = DateHelpers.GetDateInfo(dy2.Date);
                        weeks.Week2.Add(dy2);
                        dy2.AttendenceStatus = attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault() == null ? "NA" : attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault().Item2;
                        break;
                    case 3:
                        Day dy3 = new Day();
                        dy3.Date = day;
                        dy3._Date = day.ToShortDateString();
                        dy3.dateStr = day.ToString("MM/dd/yyyy");
                        dy3.dtDay = day.Day;
                        dy3.daycolumn = DateHelpers.GetDateInfo(dy3.Date);
                        weeks.Week3.Add(dy3);
                        dy3.AttendenceStatus = attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault() == null ? "NA" : attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault().Item2;
                        break;
                    case 4:
                        Day dy4 = new Day();
                        dy4.Date = day;
                        dy4._Date = day.ToShortDateString();
                        dy4.dateStr = day.ToString("MM/dd/yyyy");
                        dy4.dtDay = day.Day;
                        dy4.daycolumn = DateHelpers.GetDateInfo(dy4.Date);
                        weeks.Week4.Add(dy4);
                        dy4.AttendenceStatus = attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault() == null ? "NA" : attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault().Item2;
                        break;
                    case 5:
                        Day dy5 = new Day();
                        dy5.Date = day;
                        dy5._Date = day.ToShortDateString();
                        dy5.dateStr = day.ToString("MM/dd/yyyy");
                        dy5.dtDay = day.Day;
                        dy5.daycolumn = DateHelpers.GetDateInfo(dy5.Date);
                        weeks.Week5.Add(dy5);
                        dy5.AttendenceStatus = attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault() == null ? "NA" : attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault().Item2;
                        break;
                    case 6:
                        Day dy6 = new Day();
                        dy6.Date = day;
                        dy6._Date = day.ToShortDateString();
                        dy6.dateStr = day.ToString("MM/dd/yyyy");
                        dy6.dtDay = day.Day;
                        dy6.daycolumn = DateHelpers.GetDateInfo(dy6.Date);
                        weeks.Week6.Add(dy6);
                        dy6.AttendenceStatus = attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault() == null ? "NA" : attendenceDetails.Where(r => r.Item1 == day.ToShortDateString()).FirstOrDefault().Item2;
                        break;
                };
            }

            while (weeks.Week1.Count < 7) // not starting from sunday
            {
                Day dy = null;
                weeks.Week1.Insert(0, dy);
            }

            if (month == 12)
            {
                weeks.nextMonth = (01).ToString() + "/" + (year + 1).ToString() + "/" + erpNo;
                weeks.prevMonth = (month - 1).ToString() + "/" + (year).ToString() + "/" + erpNo;
            }
            else if (month == 1)
            {
                weeks.nextMonth = (month + 1).ToString() + "/" + (year).ToString() + "/" + erpNo;
                weeks.prevMonth = (12).ToString() + "/" + (year - 1).ToString() + "/" + erpNo;
            }
            else
            {
                weeks.nextMonth = (month + 1).ToString() + "/" + (year).ToString() + "/" + erpNo;
                weeks.prevMonth = (month - 1).ToString() + "/" + (year).ToString() + "/" + erpNo;
            }


            //Extra code for not getting next month value if it was more than current month
            List<string> nextMontAndYear = weeks.nextMonth.Split('/').ToList();
            if (nextMontAndYear.Count == 3)
            {
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;

                int _Nmonth = Convert.ToInt32(nextMontAndYear[0]);
                int _Nyear = Convert.ToInt32(nextMontAndYear[1]);

                if (_Nmonth > currentMonth && _Nyear >= currentYear) //
                    weeks.nextMonth = "";

            }

            return weeks;
        }
    }
}

