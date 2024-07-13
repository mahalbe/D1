using ISas.Entities.DashboardEntities;
using ISas.Repository.DashboardRepository.IRepository;
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
    public class DashBoard_StudentStataticsController : Controller
    {
        private IDashBoard_StudentStataticsRepo _studstataticsRepo;
        private IDashboardRepo _dashRepo;
        public DashBoard_StudentStataticsController(IDashBoard_StudentStataticsRepo studstataticsRepo, IDashboardRepo dashRepo)
        {
            _studstataticsRepo = studstataticsRepo;
            _dashRepo = dashRepo;
        }

        #region Admission Details
        public ActionResult StudentAdmissionDetails()
        {
            return View(_studstataticsRepo.GetStudentAdmissionDetails(Session["SessionId"].ToString())); //
        }
        public PartialViewResult _StudentAdmissionDetails(string ClassSectionId, string StaticMode, string ClassName)
        {
            ViewBag.reportName = ClassName + " :-" + StaticMode;
            return PartialView(_studstataticsRepo._GetStudentAdmissionDetails(ClassSectionId, StaticMode));
        }
        public JsonResult StudentAdmissionDetails_Charts()
        {
            List<StudentAdmissionDetailsSubModel> allChartDetails = _studstataticsRepo.GetStudentAdmissionDetails_Charts();
            var tempStrengtData = allChartDetails.Select(r => new
            {
                className = r.FullClassName,
                strength = r.Strength
            }).ToList();

            var tempnewOrOldAdmChartData = allChartDetails.Select(r => new
            {
                className = r.FullClassName,
                newAdm = r.NewAdm,
                oldAdm = r.OldAdm
            }).ToList();

            var tempgenderWiseChartData = allChartDetails.Select(r => new
            {
                className = r.FullClassName,
                male = (-1) * r.BOY,
                female = r.GIRL
            }).ToList();

            List<Tuple<string, int, string>> tempVal = new List<Tuple<string, int, string>>();
            tempVal.Add(new Tuple<string, int, string>("GENERAL", allChartDetails.Sum(r => r.GEN), "#FF0F00"));
            tempVal.Add(new Tuple<string, int, string>("SC", allChartDetails.Sum(r => r.SC), "#FF6600"));
            tempVal.Add(new Tuple<string, int, string>("ST", allChartDetails.Sum(r => r.ST), "#FF9E01"));
            tempVal.Add(new Tuple<string, int, string>("OBC", allChartDetails.Sum(r => r.OBC), "#FCD202"));
            tempVal.Add(new Tuple<string, int, string>("EWS", allChartDetails.Sum(r => r.EWS), "#F8FF01"));

            var tempcategoryWiseSummaryChartData = tempVal.Select(r => new
            {
                categoryName = r.Item1,
                strength = r.Item2,
                color = r.Item3
            }).ToList();
            var tempcategoryWiseDetailChartData = allChartDetails.Select(r => new
            {
                className = r.FullClassName,
                gen = r.GEN,
                sc = r.SC,
                st = r.ST,
                obc = r.OBC,
                ews = r.EWS
            }).ToList();
            return Json(new { strengthChartData = tempStrengtData, newOrOldAdmChartData = tempnewOrOldAdmChartData, genderWiseChartData = tempgenderWiseChartData, categoryWiseDetailChartData = tempcategoryWiseDetailChartData, categoryWiseSummaryChartData = tempcategoryWiseSummaryChartData }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region FeeCollection Details
        public ActionResult FeeCollectionDetails()
        {
            return View(_studstataticsRepo.GetFeeCollectionDetails(Session["SessionId"].ToString()));
        }
        public JsonResult FeeCollectionDetails_Charts()
        {
            FeeCollectionDetailsModel allChartDetails = _studstataticsRepo.GetFeeCollectionDetails_Charts();
            var tempannualDueReceivedData = allChartDetails.FeeCollectionDetails.Select(r => new
            {
                className = r.Fullclass,
                annualDue = r.AnnualDue,
                annualReceived = r.AnnualReceived
            }).ToList();

            var temppaidDefaulterData = allChartDetails.FeeCollectionDetails.Select(r => new
            {
                className = r.Fullclass,
                paid = (-1) * r.PaidStudent,
                defaulter = r.DefaulterStudent
            }).ToList();
            var tempmonthlyDueReceivedData = allChartDetails.FeeCollectionDetails.Select(r => new
            {
                className = r.Fullclass,
                due = r.Due,
                received = r.Received
            }).ToList();


            return Json(new
            {
                paidDefaulterData = temppaidDefaulterData,
                monthlyDueReceivedData = tempmonthlyDueReceivedData,
                annualDueReceivedData = tempannualDueReceivedData,
                paidDefaulter = allChartDetails.PaidCountPercent,
                monthlyDueReceived = allChartDetails.ReceivedPercent,
                annualyDueReceived = allChartDetails.AReceivedPercent,
            }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _StudentFeeCollectionDetails(string ClassName, string ClassSectionId, string StaticMode)
        {

            ViewBag.StaticMode = StaticMode;

            if (StaticMode == "Strength" || StaticMode == "Total Strength")
            {
                ViewBag.reportName = ClassName + " :-" + StaticMode;
                return PartialView(_studstataticsRepo._GetFeeCollectionDetails(ClassSectionId, StaticMode, Session["SessionId"].ToString()));
            }

            List<_StudentDetails> studDetails = _studstataticsRepo._GetDefaulterOrBalanceDetails(ClassSectionId, Session["SessionId"].ToString());
            ViewBag.reportName = StaticMode + " Balance : " + studDetails.Sum(r => r.Balance);
            return PartialView(studDetails);
        }
        #endregion

        #region Student Dozear
        [EncryptedActionParameter]
        public ViewResult StudentDozear(string erpNo, string viewOnNewWin = "NO")
        {
            int month = DateTime.Now.Month; int year = DateTime.Now.Year;
            StudentDozearModel dozearInfo = _studstataticsRepo.GetStudentDozear(erpNo, Session["SessionId"].ToString());

            List<Tuple<string, string>> attendenceDetails = _dashRepo.GetStudentAttenDetails_ByERPNo(month, year, Session["UserID"].ToString());
            dozearInfo.WeekForMonth = DashboardController.getCalender(month, year, attendenceDetails, erpNo);
            dozearInfo.WeekForMonth.MonthNameWithYear = DateHelpers.GetMonthByName(month) + " " + year.ToString();
            ViewBag.viewOnNewWin = viewOnNewWin;
            return View(dozearInfo);
        }

        public JsonResult GetStudentAttendanceDetails(string erpNo)
        {
            List<AttendanceDetailModel> attenDetails = _studstataticsRepo.StudentAttendanceDetails(erpNo, Session["SessionID"].ToString());
            var result = attenDetails.Select(r => new
            {
                Class = r.MnthName,
                Leave = r.L,
                Absent = r.A,
                Present = r.P,
            }).ToList();
            return Json(new { staffAtten = result }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _ClassStudentDetails(string classSectionId, string filterBy)
        {
            return PartialView(getStudClassDetails(classSectionId, filterBy));
        }

        public List<StudentDetailsModel> getStudClassDetails(string classSectionId, string filterBy)
        {

            if (filterBy == "FEE DEFAULTER")
            {
                return _studstataticsRepo.GetFeeDefaulterDetails(classSectionId, Session["SessionId"].ToString());
            }
            else
            {
                List<StudentDetailsModel> filteredClassList = _studstataticsRepo.GetClassStudentDetails(classSectionId, Session["SessionId"].ToString());
                switch (filterBy)
                {
                    case "BOY":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.Gender == "MALE").ToList();
                        break;
                    case "GIRL":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.Gender == "FEMALE").ToList();
                        break;
                    case "NEWADM":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.NewAdm).ToList();
                        break;
                    case "OLDADM":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.OldAdm).ToList();
                        break;
                    case "TC":
                        filteredClassList = filteredClassList.Where(r => r.TC).ToList();
                        break;
                    case "NSO":
                        filteredClassList = filteredClassList.Where(r => r.NSO).ToList();
                        break;
                    case "GEN":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.Category == "GEN").ToList();
                        break;
                    case "SC":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.Category == "SC").ToList();
                        break;
                    case "ST":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.Category == "ST").ToList();
                        break;
                    case "OBC":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.Category == "OBC").ToList();
                        break;
                    case "EWS":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).Where(r => r.Category == "EWS").ToList();
                        break;
                    case "STRENGTH":
                        filteredClassList = filteredClassList.Where(r => r.TC == false && r.NSO == false).ToList();
                        break;

                }
                return filteredClassList;
            }
        }

        #endregion
    }
}