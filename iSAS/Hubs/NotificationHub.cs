using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using ISas.Entities;
using ISas.Repository.Implementation;
using ISas.Repository.Interface;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ISas.Web.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, UserHubModels> Users =
           new ConcurrentDictionary<string, UserHubModels>(StringComparer.InvariantCultureIgnoreCase);

        //Logged Use Call
        public void GetNotification()
        {
            try
            {
                string loggedUser = Context.User.Identity.Name;

                //Get TotalNotification
                string totalNotif = LoadNotifData(loggedUser);

                //Send To
                UserHubModels receiver;
                if (Users.TryGetValue(loggedUser, out receiver))
                {
                    var cid = receiver.ConnectionIds.FirstOrDefault();
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    context.Clients.Client(cid).broadcaastNotif(totalNotif);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //Specific User Call
        public void SendNotification(string SentTo)
        {
            try
            {
                //Get TotalNotification
                string totalNotif = LoadNotifData(SentTo);

                //Send To
                UserHubModels receiver;
                if (Users.TryGetValue(SentTo, out receiver))
                {
                    var cid = receiver.ConnectionIds.FirstOrDefault();
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    context.Clients.Client(cid).broadcaastNotif(totalNotif);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private string LoadNotifData(string userId)
        {
            int total = 0;
            ICommonRepo _commonRepo = new CommonRepo();
            total = _commonRepo.GetNotificationCount(userId);
            return total.ToString();
        }

        public override Task OnConnected()
        {
            string userRole = Roles.GetRolesForUser().FirstOrDefault();
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userName, _ => new UserHubModels
            {
                UserName = userName,
                UserRole = userRole,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
                if (user.ConnectionIds.Count == 1)
                {
                    Clients.Others.userConnected(userName);
                }
            }

            ShowUsersOnLine();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            UserHubModels user;
            Users.TryGetValue(userName, out user);

            if (user != null)
            {
                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
                    if (!user.ConnectionIds.Any())
                    {
                        UserHubModels removedUser;
                        Users.TryRemove(userName, out removedUser);
                        Clients.Others.userDisconnected(userName);
                    }
                }
            }
            ShowUsersOnLine();
            return base.OnDisconnected(stopCalled);
        }



        [HubMethodName("sendUptodateInformation")]
        public static void SendUptodateInformation(string action)
        {
            List<string> connectionIds = Users.Where(r => r.Value.UserRole == "Admin" || r.Value.UserRole == "Principal").SelectMany(r => r.Value.ConnectionIds).Distinct().ToList();
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            // the updateStudentInformation method will update the connected client about any recent changes in the server data
            // context.Clients.All.updateStudentInformation(action);
            context.Clients.Clients(connectionIds).updateStudentInformation(action);
        }

        //Get Current Loggin User Count
        public void ShowUsersOnLine()
        {
            Clients.All.showUsersOnLine(Users.Count);
        }
    }
}