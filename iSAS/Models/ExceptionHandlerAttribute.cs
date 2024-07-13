using ISas.Entities;
using ISas.Repository.Implementation;
using ISas.Repository.Interface;
using System.Web.Mvc;

namespace ISas.Web.Models
{
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        private ICommonRepo _commonRepo;
        public ExceptionHandlerAttribute()
        {
            _commonRepo = new CommonRepo();
        }
       
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                ExceptionLogger logger = new ExceptionLogger()
                {
                    ExceptionMsg = filterContext.Exception.Message,
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                    ActionName = filterContext.RouteData.Values["action"].ToString(),
                };
                _commonRepo.ExceptionLoggingToDataBase(logger);
                filterContext.ExceptionHandled = false;
            }
        }
    }
}