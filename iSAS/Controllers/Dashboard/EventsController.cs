using ISas.Entities;
using ISas.Entities.DashboardEntities;
using ISas.Repository.DashboardRepository.IRepository;
using ISas.Repository.Interface;
using ISas.Repository.SMSManagement.IRepository;
using ISas.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISas.Web.Controllers.Dashboard
{
    [Authorize]
    [ExceptionHandler]
    public class EventsController : Controller
    {
        private ICommon_NECNRepo _noticeRepo;
        private ISMSManagementRepo _smsManagementRepo;
        private ICommonRepo _commonRepo;
        private string _uploadType = "";
        public EventsController(ICommon_NECNRepo noticerepo, ISMSManagementRepo smsMangement, ICommonRepo commonRepo)
        {
            this._noticeRepo = noticerepo;
            this._smsManagementRepo = smsMangement;
            this._commonRepo = commonRepo;
            _uploadType = "EV";
        }

        public ActionResult LandingPage()
        {
            return View("~/Views/Common_NECN/Common_NECN_LandingPage.cshtml", this._noticeRepo.LandingPageDetails(_uploadType).ToList());
        }

        public ActionResult New()
        {
            Session["CurrentUploadDocList"] = null;
            Common_NECN_MainModel model = new Common_NECN_MainModel();
            model.UploadType = _uploadType;
            model.Function = "SAVE";
            model.SelectionGroups = this._noticeRepo.GetSelectionGroupDetails("NT",Session["UserId"].ToString());

            string _today = DateTime.Now.ToShortDateString().Replace("-", "/");
            model.BasicDetails.CreationDate = _today;
            model.BasicDetails.UploadStartDate = _today;

            return View("~/Views/Common_NECN/Common_NECN_New.cshtml", model);
        }

        [EncryptedActionParameter]
        public ActionResult Updation(string UploadId)
        {
            Session["CurrentUploadDocList"] = null;
            Common_NECN_MainModel model = this._noticeRepo.GetDetailsById(_uploadType, UploadId);
            model.Function = "UPDATE";
            model.UploadType = _uploadType;

            if (model != null && model.UploadDocList != null && model.UploadDocList.Count > 0)
                Session["CurrentUploadDocList"] = model.UploadDocList.ToList();

            return View("~/Views/Common_NECN/Common_NECN_Updation.cshtml", model);
        }

        public ActionResult _DispalyToUser(string UploadId)
        {
            return PartialView("~/Views/Common_NECN/_Common_NECN_DispalyToUser.cshtml", this._noticeRepo.GetDetailsById(_uploadType, UploadId));
        }


        public ActionResult DisplayList()
        {
            string current_user_role = System.Web.Security.Roles.GetRolesForUser(User.Identity.Name).FirstOrDefault();
            string userID = Session["UserID"].ToString();
            return View("~/Views/Common_NECN/Common_NECN_DisplayList.cshtml", this._noticeRepo.GetNewAndOld_NECNList(userID, current_user_role, _uploadType));
        }

        public ActionResult _UploadedDocDetails(string UploadID)
        {
            string myFullFileName = "";
            string mySavetoFileName = "";
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                    ext = ext.ToLower();
                    if (file.ContentLength > 10500000) //2100000
                    {
                        return Json(new { status = "Failled", Msg = "File size is too big..!max file size is allowed is approx 10mb" }, JsonRequestBehavior.AllowGet);
                    }
                    if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".txt"
                        || ext == ".xlsx" || ext == ".xls" || ext == ".docx" || ext == ".doc" || ext == ".ppt"
                        || ext == ".pptx" || ext == ".pdf"
                        )
                    {
                        Common_NECN_MainModel model = new Common_NECN_MainModel();
                        //model.UploadDocList = new List<Common_NECN_PhotoUploads>();
                        if (Session["CurrentUploadDocList"] != null)
                            model.UploadDocList = Session["CurrentUploadDocList"] as List<Common_NECN_PhotoUploads>;

                        if (model.UploadDocList == null)
                            model.UploadDocList = new List<Common_NECN_PhotoUploads>();

                        myFullFileName = Path.GetFileName(file.FileName);
                        //fileExtension = Path.GetExtension(file.FileName);
                        myFullFileName = Guid.NewGuid() + "@" + myFullFileName;
                        mySavetoFileName = "Images/Common_NECN_Docs/" + myFullFileName;
                        myFullFileName = "~/Images/Common_NECN_Docs/" + myFullFileName;
                        file.SaveAs(Server.MapPath(myFullFileName));

                        model.UploadDocList.Add(new Common_NECN_PhotoUploads { FileName = file.FileName, UploadID = UploadID, AttachPath = mySavetoFileName });

                        Session["CurrentUploadDocList"] = model.UploadDocList.ToList();
                        return PartialView("~/Views/Common_NECN/_Common_NECN_UploadedDocDetails.cshtml", model);
                    }
                    else
                    {
                        return Json(new { status = "Failled", Msg = "Only supports Image/Text/Word/Excel/Ppt/Pdf formats ..!" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { status = "Failled", Msg = "No file selected ..!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveDocument(string DocId, string UploadID)
        {
            string path = Server.MapPath("~/" + DocId);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                Common_NECN_MainModel model = new Common_NECN_MainModel();
                if (Session["CurrentUploadDocList"] != null)
                    model.UploadDocList = Session["CurrentUploadDocList"] as List<Common_NECN_PhotoUploads>;

                if (model.UploadDocList == null)
                    model.UploadDocList = new List<Common_NECN_PhotoUploads>();

                bool IsFileDeletedFromDb = true; // 
                if (!string.IsNullOrEmpty(UploadID) && UploadID != "TEMPUPLOADID")
                {
                    int effectedRows = this._noticeRepo.DeleteUploadedDocument(DocId, UploadID);
                    if (effectedRows == 0)
                        IsFileDeletedFromDb = false;
                }

                if (IsFileDeletedFromDb)
                {
                    file.Delete();

                    Common_NECN_PhotoUploads currentSelectedDoc = model.UploadDocList.Where(r => r.AttachPath == DocId).FirstOrDefault();
                    if (currentSelectedDoc != null)
                        model.UploadDocList.Remove(currentSelectedDoc);

                    ViewBag.trnMsg = "File Removed Successfully..!";
                    Session["CurrentUploadDocList"] = model.UploadDocList.ToList();
                    return PartialView("~/Views/Common_NECN/_Common_NECN_UploadedDocDetails.cshtml", model);
                }
            }
            return Json(new { status = "Failled", Msg = "Can not find this document please try again after some time..!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _StudentDetails(string ClassIds)
        {
            Common_NECN_MainModel model = new Common_NECN_MainModel();
            model.SelectionGroups.StudentDetailsList = this._smsManagementRepo.GetStudentDetails(ClassIds, "", Session["SessionID"].ToString()).Select(r => new Common_NECN_StudentDetails
            {
                ClassName = r.Class,
                ERPNo = r.ERP,
                Selected = false,
                StudentName = r.Student
            }).ToList();
            return PartialView("~/Views/Common_NECN/_Common_NECN_StudentDetails.cshtml", model);
        }

        [HttpPost]
        public JsonResult Common_NECN_CRUD(Common_NECN_MainModel model)
        {
            if (model.WingIds != null && model.WingIds.Count() > 0 ||
                  model.ClassIds != null && model.ClassIds.Count() > 0 ||
                  model.DeprtmentIds != null && model.DeprtmentIds.Count() > 0 ||
                    model.StaffIds != null && model.StaffIds.Count() > 0 ||
                     model.SelectionGroups.StudentDetailsList.Where(r => r.Selected).Count() > 0
                         ) { }
            else
                return Json(new { status = "failed_NoSelect", Msg = "No one selected to send messege ..!" }, JsonRequestBehavior.AllowGet);

            if (ModelState.IsValid)
            {
                model.UploadType = _uploadType;
                model.UserID = Session["UserID"].ToString();
                Tuple<int, string> res = this._noticeRepo.Common_NECN_CRUD(model);
                if (res.Item1 == 1 && model.Function == "SAVE")
                {
                    string wingIds = ""; string studentIds = "";
                    string keymode = "";
                    string param = "";

                    if (model.SelectionGroups != null && model.SelectionGroups.StudentDetailsList != null)
                        studentIds = string.Join(",", model.SelectionGroups.StudentDetailsList.Where(r => r.Selected).Select(r => r.ERPNo).ToList());

                    if (model.WingIds != null)
                        wingIds = string.Join(",", model.WingIds);

                    if (!string.IsNullOrEmpty(studentIds))
                    {
                        keymode = "STUD";
                        param = studentIds;
                    }
                    else
                    {
                        keymode = "WING";
                        param = wingIds;
                    }
                    NotificationModel model1 = new NotificationModel();
                    notification notification = new notification();
                    notification.title = "School Event";
                    notification.body = "Dear Parents, school event details is uploaded. Kindly check the communication";
                    notification.device_id = "";
                    model1.registration_ids = _commonRepo.getFireBaseKey(keymode, param);
                    model1.notification = notification;
                    if (model1.registration_ids.Length > 0)
                        _commonRepo.SendNotification(model1, "https://fcm.googleapis.com/fcm/send");
                }
                return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
            }
            var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, Error = modelstate.Value.Errors.FirstOrDefault().ErrorMessage };
            var validKeys = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count == 0) select new { Title = modelstate.Key };
            return Json(new { status = "failed", ErrorList = errors, ValidKeyList = validKeys }, JsonRequestBehavior.AllowGet);

        }
    }
}

