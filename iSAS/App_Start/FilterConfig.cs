using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using ISas.Web.Models;

namespace ISas.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new RedirectFilter());
            filters.Add(new ExceptionHandlerAttribute());
        }

        public class RedirectFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (!IsAuthorized(filterContext))
                {
                    filterContext.Result =
                        new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action="Login" }));
                }
            }

            private bool IsAuthorized(ActionExecutingContext filterContext)
            {
                var descriptor = filterContext.ActionDescriptor;
                var authorizeAttr = descriptor.GetCustomAttributes(typeof(AuthorizeAttribute), false).FirstOrDefault() as AuthorizeAttribute;

                if (authorizeAttr != null)
                {
                    if (!authorizeAttr.Users.Contains(filterContext.HttpContext.User.ToString()))
                        return false;
                }
                return true;

            }
        }
    }
}