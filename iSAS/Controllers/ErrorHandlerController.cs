using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISas.Web.Controllers
{
    public class ErrorHandlerController : Controller
    {
        // GET: ErrorHandler

        //[ActionName("")]
        public ActionResult GenericError()
        {
            return View();
        }

        //[ActionName("400")]
        public ActionResult BadRequest()
        {
            return View();
        }

        //[ActionName("401")]
        public ActionResult UnauthorizeAccess()
        {
            return View();
        }

        //[ActionName("403")]
        public ActionResult Forbidden()
        {
            return View();
        }

        //[ActionName("404")]
        public ActionResult NotFound()
        {
            return View();
        }

        //[ActionName("500")]
        public ActionResult InternalServerError()
        {
            return View();
        }

        //[ActionName("502")]
        public ActionResult BadGateway()
        {
            return View();
        }

        //[ActionName("503")]
        public ActionResult ServiceUnavailable()
        {
            return View();
        }

        //[ActionName("504")]
        public ActionResult GatewayTimeout()
        {
            return View();
        }
    }
}