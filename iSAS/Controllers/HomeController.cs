using ISas.Web.Models;
using System.Web.Mvc;

namespace ISas.Web.Controllers
{
    [Authorize]
    [ExceptionHandler]
    public class HomeController : Controller
    {      
        [HttpGet]
        public ActionResult Index()
        {
            //var model = new DashboardViewModel();
            //return View("DashBoard", model);
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            //var objILoginData = new LoginData();
            //var submenumodules = objILoginData.GetAllSubMenu(1).ToList();
            //return View("DashBoard", submenumodules);
            ////return new JsonResult { Data = submenumodules };
            return View();
        }



        //[NonAction]
        //public List<SelectListItem> GetAll_Sesssion()
        //{
        //    var Sessionlist = objILoginData.GetAllSession();
        //    List<SelectListItem> listsesssion = new List<SelectListItem>();
        //    listsesssion.Add(new SelectListItem { Text = "Select", Value = "0" });
        //    foreach (var item in Sessionlist)
        //    {
        //        listsesssion.Add(new SelectListItem { Text = item.SessName, Value = item.SessID });
        //    }
        //    return listsesssion;
        //}

    }

}
