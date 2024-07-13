using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using ISas.Web.Models;
using System.Data;
using ISas.Repository.Interface;
using ISas.Repository.StaffRepository.IRepository;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using ISas.Entities.CommonEntities;

namespace ISas.Web.Controllers
{
    
    [Authorize]
    [ExceptionHandler]
    public class AccountController : Controller
    {
        private IStaff_StaffDetailMasterRepo _staffRepo;
        ILoginData objILoginData;
        private IStudentSession _studentSessionRepos;
        private ICommonRepo _commonRepo;

        public string _userName = System.Web.HttpContext.Current.User.Identity.Name.ToString() != null ? System.Web.HttpContext.Current.User.Identity.Name.ToString() : null;
        public AccountController(ILoginData loginRepos, IStudentSession loginSession, IStaff_StaffDetailMasterRepo staffRepo, ICommonRepo commonRepo)
        {
            objILoginData = loginRepos;
            this._studentSessionRepos = loginSession;
            this._staffRepo = staffRepo;
            _commonRepo = commonRepo;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            string authType1 = User.Identity.AuthenticationType;
            bool isAuth1 = User.Identity.IsAuthenticated;
            string authName1 = User.Identity.Name;

            var model = new Login();
            model.SessionList = _studentSessionRepos.GetAllSessions().OrderByDescending(p => p.PrintOrder)
                .Select(p => new SelectListItem
                {
                    Text = p.SessionDisplayName,
                    Value = p.SessId,
                    Selected = p.IsDefault
                }).ToList();

            string clientWiseLoginEnabled = System.Configuration.ConfigurationManager.AppSettings["ClientWiseLoginEnabled"];
            if (clientWiseLoginEnabled == "YES")
            {
                string loginPageName = System.Configuration.ConfigurationManager.AppSettings["ClientWise_LoginPageName"];
                return View(loginPageName, model);
            }

            ViewBag.HeadDetails = _commonRepo.ReportHeaderDetails("");
            ViewBag.ClientInfo = _commonRepo.get_ClientInfo();
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Keepalive()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            var model = new LocalPasswordModel();
            model.UserName = _userName;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(LocalPasswordModel loginuser)
        {
            if (ModelState.IsValid)
            {
                bool success = WebSecurity.ChangePassword(_userName, loginuser.OldPassword, loginuser.NewPassword);
                if (success == true)
                    return RedirectToAction("Logout");

                else
                {
                    ModelState.AddModelError("OldPassword", "The current password is incorrect.");
                    var model = new LocalPasswordModel();
                    model.UserName = _userName;
                    return View(model);
                }
            }
            else
            {
                var model = new LocalPasswordModel();
                model.UserName = _userName;
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Login login , string schoolName )
        {
            if (ModelState.IsValid)
            {
                bool success =  WebSecurity.Login(login.username, login.password, false);
                //bool success = true;
                if (success == true)
                {
                    Tuple<string, string, string,string> UserIDs = objILoginData.GetUserID_By_UserName(login.username);
                    var LoginType = objILoginData.GetRoleByUserID(UserIDs.Item1);
                    var SessId = login.SelectedSessionId; //objILoginData.GetAllSessions().OrderByDescending(m => m.SessId).First().SessId;
                    var SessName = objILoginData.GetAllSessions().Where(r => r.SessId == SessId).FirstOrDefault().SessionDisplayName; //objILoginData.GetAllSessions().OrderByDescending(m => m.SessId).First().SessionDisplayName;
                    if (string.IsNullOrEmpty(Convert.ToString(LoginType)))
                    {
                        ModelState.AddModelError("Error", "Rights to User are not Provide Contact to Admin");

                        string clientWiseLoginEnabled = System.Configuration.ConfigurationManager.AppSettings["ClientWiseLoginEnabled"];
                        if (clientWiseLoginEnabled == "YES")
                        {
                            string loginPageName = System.Configuration.ConfigurationManager.AppSettings["ClientWise_LoginPageName"];
                            return View(loginPageName, login);
                        }
                        return View();
                    }
                    else
                    {
                        Session["Name"] = login.username;
                        Session["UserID"] = UserIDs.Item1;
                        Session["LoginType"] = LoginType;
                        Session["SessionId"] = SessId;
                        Session["DisplayName"] = UserIDs.Item2;
                        Session["DisplayImage"] = UserIDs.Item3;
                        Session["Authority"] = UserIDs.Item4;
                        Session["SessionName"] = SessName;
                        Session["SchoolName"] = schoolName;
                        ClinetInfoEntities clientInfo =  _commonRepo.get_ClientInfo();
                        if (clientInfo != null)
                        {
                            Session["CopyRightsYear"] = clientInfo.CopyRightYear;
                            Session["CopyRightsName"] = clientInfo.CopyRightName;
                            Session["CopyRightsLink"] = clientInfo.Comp_WebLink; ;
                        }
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("loginFailedError", "Please enter valid Username and Password");
                    login.SessionList = _studentSessionRepos.GetAllSessions().OrderByDescending(p => p.PrintOrder)
                .Select(p => new SelectListItem
                {
                    Text = p.SessionDisplayName,
                    Value = p.SessId,
                    Selected = p.IsDefault
                }).ToList();


                    string clientWiseLoginEnabled = System.Configuration.ConfigurationManager.AppSettings["ClientWiseLoginEnabled"];
                    if (clientWiseLoginEnabled == "YES")
                    {
                        string loginPageName = System.Configuration.ConfigurationManager.AppSettings["ClientWise_LoginPageName"];
                        return View(loginPageName, login);
                    }


                    ViewBag.HeadDetails = _commonRepo.ReportHeaderDetails("");
                    ViewBag.ClientInfo = _commonRepo.get_ClientInfo();
                    return View(login);
                }
            }
            else
            {
                ModelState.AddModelError("loginFailedError", "Please enter Username and Password");
                var model = new Login();
                model.SessionList = _studentSessionRepos.GetAllSessions().OrderByDescending(p => p.PrintOrder)
                .Select(p => new SelectListItem
                {
                    Text = p.SessionDisplayName,
                    Value = p.SessId,
                    Selected = p.IsDefault
                }).ToList();

                //return View(model);
                string clientWiseLoginEnabled = System.Configuration.ConfigurationManager.AppSettings["ClientWiseLoginEnabled"];
                if (clientWiseLoginEnabled == "YES")
                {
                    string loginPageName = System.Configuration.ConfigurationManager.AppSettings["ClientWise_LoginPageName"];
                    return View(loginPageName, model);
                }

                ViewBag.HeadDetails = _commonRepo.ReportHeaderDetails("");
                ViewBag.ClientInfo = _commonRepo.get_ClientInfo();
                return View(model);
            }
        }

        public ActionResult EditUserProfile()
        {
            if (User.IsInRole("Student"))
                return RedirectToAction("StudentProfile", "Dashboard");

            return View(_staffRepo.GetStaffProfileDetails(Session["UserID"].ToString()));
        }
        #region Forgot Password
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ViewBag.msg = null;
            ViewBag.msgColor = null;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(string UserName)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.UserExists(UserName))
                {
                    string To = UserName, UserID, Password, SMTPPort, Host;
                    string token = WebSecurity.GeneratePasswordResetToken(UserName, 30);
                    if (token == null)
                    {
                        // If user does not exist or is not confirmed.  
                        ViewBag.msg = "User not exist..!";
                        ViewBag.msgColor = "Danger";
                        return View();
                    }
                    else
                    {
                        //Create URL with above token  
                        var lnkHref = "<a href='" + Url.Action("ResetPassword", "Account", new { email = UserName, code = token }, "http") + "'>Reset Password</a>";
                        //HTML Template for Send email  
                        string subject = "Your changed password";
                        string body = "<b>Please find the Password Reset Link. </b><br/>";
                        body += "<b>This link is valid for 30 min. </b><br/>" + lnkHref;
                        //Get and set the AppSettings using configuration manager.  
                        EmailManager.AppSettings(out UserID, out Password, out SMTPPort, out Host);
                        //Call send email methods. 

                        string email = objILoginData.getUserEmailByUserName(UserName);
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            try
                            {
                                EmailManager.SendEmail(UserID, subject, body, email, UserID, Password, SMTPPort, Host);
                                ViewBag.msg = "Email send successfully, Please check your registred email.";
                                ViewBag.msgColor = "Success";
                                return View();
                            }
                            catch
                            {
                                ViewBag.msg = "Failed to send email, contact your administrator.";
                                ViewBag.msgColor = "Warning";
                                return View();
                            }
                        }

                        ViewBag.msg = "Email is not registred for this user, please contact your administrator";
                        ViewBag.msgColor = "Danger";
                        return View();
                    }
                }
            }

            ViewBag.msg = "User not exist..!";
            ViewBag.msgColor = "Danger";
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string email)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.ReturnToken = code;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken, model.Password);
                if (resetResponse)
                {
                    ViewBag.msg = "Password changed successfully, go back to login page ..!";
                    ViewBag.msgColor = "Success";
                    return View(model);
                }
                else
                {
                    ViewBag.msg = "Something went horribly wrong, contact your administrator!";
                    ViewBag.msgColor = "Danger";
                    return View(model);
                }
            }
            ViewBag.msg = "Something went wrong, contact your administrator!";
            ViewBag.msgColor = "Danger";
            return View(model);
        }
        #endregion
    }

    public class EmailManager
    {
        public static void AppSettings(out string UserID, out string Password, out string SMTPPort, out string Host)
        {
            UserID = ConfigurationManager.AppSettings.Get("FromMailId");
            Password = ConfigurationManager.AppSettings.Get("Password");
            SMTPPort = ConfigurationManager.AppSettings.Get("PortNo");
            Host = ConfigurationManager.AppSettings.Get("PortName");
        }
        public static void SendEmail(string From, string Subject, string Body, string To, string UserID, string Password, string SMTPPort, string Host)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(From);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Host;
            smtp.Port = Convert.ToInt16(SMTPPort);
            smtp.Credentials = new NetworkCredential(UserID, Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
