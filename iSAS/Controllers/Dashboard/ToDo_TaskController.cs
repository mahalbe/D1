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
    public class ToDo_TaskController : Controller
    {
        private IToDo_TaskRepo _todoRepo;
        public ToDo_TaskController(IToDo_TaskRepo todoRepo)
        {
            _todoRepo = todoRepo;
        }

        // GET: ToDo_Task

        public PartialViewResult _CRUD(string staffId, int Id = 0)
        {
            if (Id > 0)
                return PartialView(_todoRepo.GetToDo_TaskById(Id));

            return PartialView(new ToDo_TaskEntitiesModel {ReferenceId = staffId, ToDoDate = DateTime.Now.ToShortDateString().Replace("-", "/") });
        }


        [HttpPost]
        public JsonResult ToDo_Task_CRUD(ToDo_TaskEntitiesModel model)
        {
            if (ModelState.IsValid)
            {
                model.CRUDMode = "SAVE";
                Tuple<int, string> res = _todoRepo.ToDo_Task_CRUD(model);
                return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
            }
            var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, Error = modelstate.Value.Errors.FirstOrDefault().ErrorMessage };
            var validKeys = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count == 0) select new { Title = modelstate.Key };
            return Json(new { status = "failed", ErrorList = errors, ValidKeyList = validKeys }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ToDo_Task_Delete(int Id)
        {
            Tuple<int, string> res = _todoRepo.ToDo_Task_CRUD(Id);
            return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _DisplayToDoList(int staffId)
        {
            return PartialView(_todoRepo.GetToDo_TaskList(staffId));
        }
    }
}