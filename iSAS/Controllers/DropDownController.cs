using ISas.Repository.Academic.IRepository;
using ISas.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISas.Web.Controllers
{
    [Authorize]
    [ExceptionHandler]
    public class DropDownController : Controller
    {
        private IAcademic_SectionMasterRepo _sectionMst;
        public DropDownController(IAcademic_SectionMasterRepo sectionMst)
        {
            _sectionMst = sectionMst;
        }

        public JsonResult GetSectionsForClass(string classId)
        {
            return Json(_sectionMst.getAllSectionByClass(classId), JsonRequestBehavior.AllowGet);
        }
    }
}