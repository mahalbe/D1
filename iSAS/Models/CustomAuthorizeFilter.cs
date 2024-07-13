using ISas.Repository.Interface;
using ISas.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ISas.Web.Models
{
    public class CustomAuthorizeFilter : ActionFilterAttribute
    {
        private string LookingFor;
        
        public CustomAuthorizeFilter(string _LookingFor)
        {
            LookingFor = _LookingFor;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsAuthorized(filterContext))
            {
                filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary(new { controller = "ErrorHandler", action = "UnauthorizeAccess" }));
            }
        }

        private bool IsAuthorized(ActionExecutingContext filterContext)
        {
            return CommonController.CHECKTO_UserAuthorization(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, LookingFor);
        }
    }
}