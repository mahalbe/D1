using ISas.Entities.CommonEntities;
using ISas.Repository.Interface;
using ISas.Web.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ISas.Web.Controllers
{
    [Authorize]
    [ExceptionHandler]
    public class Common_AttachmentRefrenceController : Controller
    {
        private ICommon_AttachmentRefRepo _attchRef;
        public Common_AttachmentRefrenceController(ICommon_AttachmentRefRepo attchRef)
        {
            _attchRef = attchRef;
        }

        [HttpPost]
        public JsonResult UploadFile(Common_AttachemntRefrence model)
        {
            string myFullFileName = "";
            string mySavetoFileName = "";
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string ext = Path.GetExtension(file.FileName).ToLower();
                    if (file.ContentLength > 4200000)
                    {
                        return Json(new { Msg = "File size is too big..! max file size is allowed is approx 4mb", Color = "Warning" }, JsonRequestBehavior.AllowGet);
                    }
                    if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".pdf")
                    {
                        myFullFileName = Path.GetFileName(file.FileName);
                        myFullFileName = Guid.NewGuid() + myFullFileName;
                        mySavetoFileName = model.filePath + myFullFileName;
                        myFullFileName = "/" + model.filePath + myFullFileName;
                        file.SaveAs(Server.MapPath(myFullFileName));

                        model.fileName = Path.GetFileName(file.FileName);
                        model.filePath = myFullFileName;

                        model.updatedBy = Session["UserID"].ToString();
                    }
                    else
                    {
                        return Json(new { Msg = "Only supports jpg, png, gif, jpeg formats ..!", Color = "Warning" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            Tuple<int, string> res = _attchRef.Common_AttachmentRef_ADD(model);
            return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult _attachments(string refId, string refId1, string filterBy)
        {
            return PartialView(_attchRef.Common_AttachmentRef_List(refId, refId1, filterBy)); //(staffId, "STAFF_OFFICIAL_DETAILS", "FORM"););
        }

        public PartialViewResult _attachments_Readonly_WithoutRemarks(string refId, string refId1, string filterBy)
        {
            return PartialView(_attchRef.Common_AttachmentRef_List(refId, refId1, filterBy)); //(staffId, "STAFF_OFFICIAL_DETAILS", "FORM"););
        }

        public PartialViewResult _attachments_Student(string refId, string refId1, string filterBy)
        {
            return PartialView(_attchRef.Common_AttachmentRef_Stud_List(refId, refId1, filterBy)); //(staffId, "STAFF_OFFICIAL_DETAILS", "FORM"););
        }

        public JsonResult removeAttachment(string key, string filePath)
        {
            Tuple<int, string> res = _attchRef.Common_AttachmentRef_DELETE(key);
            if (res.Item1 == 1)
            {
                FileInfo file = new FileInfo(Server.MapPath("~" + filePath));
                if (file.Exists)
                    file.Delete();
            }
            return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
        }
    }
}