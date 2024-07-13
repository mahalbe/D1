using ISas.Entities;
using ISas.Repository.Implementation;
using ISas.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ISas.Web.ViewModels
{

    public class BaseViewModel
    {
        private List<Menu> _menuList;
        private List<Notification> _notificationList;
        private Login _loginUser;
        public string UserId = HttpContext.Current.Session != null ? Convert.ToString(HttpContext.Current.Session["UserID"]) : null;
        private string _userId;
        public BaseViewModel()
        {
            this._menuList = new List<Menu>();
            this._loginUser = new Login();
            this._notificationList = new List<Notification>();
            this._userId="";
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                var objILoginData = new LoginData();
                _userId = HttpContext.Current.Session != null ? Convert.ToString(HttpContext.Current.Session["UserID"]) : null;
                if (!string.IsNullOrEmpty(_userId))
                {
                    var roles = objILoginData.GetAllRoles(Convert.ToInt32(_userId)).ToList();
                    this._notificationList = objILoginData.GetAllNotification(Convert.ToInt32(_userId)).ToList();
                    var mainMenus = roles.Select(p => p.Module).Distinct().ToList();

                    this._menuList = mainMenus.Select(menu =>
                    {
                        return new Menu()
                        {
                            MainMenu = menu,
                            SubMenus = roles.Where(role => role.Module.ModuleId == menu.ModuleId).ToList()
                        };
                    }).ToList();

                    

                }
                else
                {
                //    RequestContext requestContext = new RequestContext();
                //    requestContext.HttpContext.Response.Clear();
                //    requestContext.HttpContext.Response.Redirect("../Account/Login");
                //    requestContext.HttpContext.Response.End();

                    var context = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
                    var urlHelper = new UrlHelper(context);
                    var url = urlHelper.Action("Login", "Account");
                    HttpContext.Current.Response.Redirect(url);
                }
            }
        }
        public List<Menu> MenuList { get { return _menuList; } }
        public List<Notification> notificationList { get { return _notificationList; } }
        public string _userDisplayname = HttpContext.Current.Session["DisplayName"] == null ? "": HttpContext.Current.Session["DisplayName"].ToString();
        public string _userDisplayImage = HttpContext.Current.Session["DisplayImage"] == null ? "" : HttpContext.Current.Session["DisplayImage"].ToString();
        public string _schoolName= HttpContext.Current.Session["SchoolName"] == null ? "" : HttpContext.Current.Session["SchoolName"].ToString();
        public string _sessionName = HttpContext.Current.Session["SessionName"] == null ? "" : HttpContext.Current.Session["SessionName"].ToString();
        public int _newAlertCount = HttpContext.Current.Session["NewAlertCount"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["NewAlertCount"]);
    }
}