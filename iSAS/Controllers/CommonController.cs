using ISas.Entities;
using ISas.Entities.CommonEntities;
using ISas.Repository.Interface;
using ISas.Repository.StudentRegistrationRepository.IRepository;
using ISas.Web.Hubs;
using ISas.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ISas.Web.Controllers
{
    //for acess online registration form state and country
//    [Authorize]
    [ExceptionHandler]
    public class CommonController : Controller
    {
        private IStateRepository _stateRepo;
        private ICityRepository _cityRepo;
        private IRouteStopRepo _routeStopRepo;
        private IStudentSession _studentSessionRepos;
        private IStudentClass _studentClassRepos;
        private ICommonRepo _commonRepo;

        public CommonController(IStateRepository state, ICityRepository city, IRouteStopRepo routeStop, IStudentSession sessionRepo, IStudentClass classRepo,
            ICommonRepo commonRepo)
        {
            this._stateRepo = state;
            this._cityRepo = city;
            this._routeStopRepo = routeStop;
            this._studentSessionRepos = sessionRepo;
            this._studentClassRepos = classRepo;
            this._commonRepo = commonRepo;
        }        //
                 // GET: /Common/

        public JsonResult GetStateListByCountryID(string CountryID)
        {
            List<SelectListItem> stateList = new List<SelectListItem>();
            var states = this._stateRepo.GetStateListByCountryID(CountryID);
            if (states != null && states.Count() > 0)
                stateList = states.OrderByDescending(p => p.PrintOrder).Select(p => new SelectListItem
                {
                    Text = p.StateName,
                    Value = p.StateID
                }).ToList();
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCityListByStateID(string StateID)
        {
            List<SelectListItem> cityList = new List<SelectListItem>();
            var cities = this._cityRepo.GetCityListByStateID(StateID);
            if (cities != null && cities.Count() > 0)
                cityList = cities.OrderByDescending(p => p.PrintOrder).Select(p => new SelectListItem
                {
                    Text = p.CityName,
                    Value = p.CityID
                }).ToList();
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRouteStopByRouteId(int RouteId = 0)
        {
            return Json(this._routeStopRepo.GetRouteStopByRouteId(RouteId), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _StudentSearchPartial()
        {
            StudentSearchModel model = new StudentSearchModel();

            //var sessions = this._studentSessionRepos.GetAllSessions();
            //if (sessions != null && sessions.Count() > 0)
            //    model.SessionList = sessions.OrderByDescending(p => p.PrintOrder).Select(p => new SelectListItem
            //    {
            //        Text = p.SessionDisplayName,
            //        Value = p.SessId,
            //        Selected = p.IsDefault
            //    }).ToList();

            //if (model.SessionList.Where(r => r.Selected).Count() > 0)
            //    model.SessionId = model.SessionList.Where(r => r.Selected).FirstOrDefault().Value;
            model.SessionId = Session["SessionId"].ToString();

            var classes = this._studentClassRepos.GetAllClasses(Session["UserID"].ToString());
            if (classes != null && classes.Count() > 0)
                model.ClassList = classes.OrderBy(r => r.PrintOrder).Select(r => new SelectListItem
                {
                    Text = r.ClassName,
                    Value = r.ClassId,
                }).ToList();

            return PartialView(model);
        }
        public PartialViewResult _StudentSearchPartial_WithAllClasses()
        {
            StudentSearchModel model = new StudentSearchModel();
            //var sessions = this._studentSessionRepos.GetAllSessions();
            //if (sessions != null && sessions.Count() > 0)
            //    model.SessionList = sessions.OrderByDescending(p => p.PrintOrder).Select(p => new SelectListItem
            //    {
            //        Text = p.SessionDisplayName,
            //        Value = p.SessId,
            //        Selected = p.IsDefault
            //    }).ToList();

            //if (model.SessionList.Where(r => r.Selected).Count() > 0)
            //    model.SessionId = model.SessionList.Where(r => r.Selected).FirstOrDefault().Value;

            model.SessionId = Session["SessionId"].ToString();

            var classes = this._studentClassRepos.GetAllClasses(Session["UserId"].ToString());
            if (classes != null && classes.Count() > 0)
                model.ClassList = classes.OrderBy(r => r.PrintOrder).Select(r => new SelectListItem
                {
                    Text = r.ClassName,
                    Value = r.ClassId,
                }).ToList();

            return PartialView(model);
        }


        public JsonResult GetStudentList(string sessionId, string classId, string sectionId)
        {
            List<SelectListItem> studentList = this._commonRepo.GetStudentList(Session["SessionId"].ToString(), classId, sectionId);
            return Json(studentList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentSearchDetails(string SessionId, string ErpNo, string AdmNo)
        {
            return Json(_commonRepo.GetStudentSearchDetails(Session["SessionId"].ToString(), ErpNo, AdmNo, null), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentSearchDetails_ByUser(string SessionId, string ErpNo, string AdmNo)
        {
            return Json(_commonRepo.GetStudentSearchDetails(Session["SessionId"].ToString(), ErpNo, AdmNo, Session["UserId"].ToString()), JsonRequestBehavior.AllowGet);
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewDate = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewDate, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        public static bool CHECKTO_UserAuthorization(string ControllerName, string RequestFor)
        {
            var repo = DependencyResolver.Current.GetService<ICommonRepo>();
            return repo.CHECKTO_UserAuthorization(ControllerName, RequestFor, System.Web.HttpContext.Current.Session["UserID"].ToString());
        }

        [AllowAnonymous]
        public PartialViewResult _ReportHeader(ReportHeaderEntities model, string tempReportName = null)
        {
            if (model == null || string.IsNullOrEmpty(model.Header1))
                model = _commonRepo.ReportHeaderDetails(tempReportName);

            if (System.IO.File.Exists(Server.MapPath("~/" + model.LogoURL + "")))
                ViewBag.ImageData = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("~/" + model.LogoURL + ""))));
            return PartialView(model);
        }

        public PartialViewResult _ReportHeaderWithoutLogo(ReportHeaderEntities model, string tempReportName = null)
        {
            if (model == null || string.IsNullOrEmpty(model.Header1))
                model = _commonRepo.ReportHeaderDetails(tempReportName);

            return PartialView(model);
        }

        public PartialViewResult _PrintPage()
        {
            return PartialView();
        }




        [HttpPost]
        public JsonResult AlertTransaction_CRUD(AlertTransactionModel model)
        {
            if (ModelState.IsValid)
            {
                //model.UserId = Session["UserID"].ToString();
                Tuple<int, string> res = _commonRepo.AlertTransaction_CRUD(model);
                if (res.Item1 == 1)
                {
                    NotificationHub objNotifHub = new NotificationHub();
                    //Notification objNotif = new Notification();
                    //objNotif.SentTo = obj.UserID;
                    //context.Configuration.ProxyCreationEnabled = false;
                    //context.Notifications.Add(objNotif);
                    //context.SaveChanges();

                    objNotifHub.SendNotification(model.AlertForUser);

                    //var query = (from t in context.Notifications  
                    //             select t).ToList();  

                    //   return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
            }
            var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, Error = modelstate.Value.Errors.FirstOrDefault().ErrorMessage };
            var validKeys = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count == 0) select new { Title = modelstate.Key };
            return Json(new { status = "failed", ErrorList = errors, ValidKeyList = validKeys }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult _EventManager_Notification()
        {
            return PartialView();
        }

        public ActionResult EventManager_Notification()
        {
            IAlert_EventManager notificationService = new Alert_EventManager();
            return PartialView("_EventManager_Notification", notificationService.GetAlert_EventList());
        }

    }
}
