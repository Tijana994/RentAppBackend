using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RentApp.Repo;
using RentApp.Persistance;
using RentApp.Models.Entities;

namespace RentApp.Hubs
{
    //[Authorize(Roles = "Admin, Manager")]
    [HubName("notifications")]
    public class NotificationHub : Hub
    {
        private RADBContext db = new RADBContext();
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        private static Timer t = new Timer();
       
        public void Hello()
        {
            Clients.All.hello("Hello from server");
        }

        public static void Notify(string msg)
        {
            //hubContext.Clients.Group("Admins").msgNotification(msg); 
        }

        public void RegisterForNotification(string userId, string userRole)
        {
            if (userRole.Equals("Admin"))
            {
                hubContext.Groups.Add(Context.ConnectionId, "Admins");
            }
        }

        public void UnsubscribeForNotification(string userId, string userRole)
        {
            if (userRole.Equals("Admin"))
            {
                hubContext.Groups.Remove(Context.ConnectionId, "Admins");
            }
        }

        /// <summary>
        ///     notifikacija admina da je kreiran smestaj
        /// </summary>
        /// <param name="id"></param>
        public static void SendNotification(string msg)
        {
            //hubContext.Clients.Group("Admins").clickNotification(accommodation);
            //List<Accommodation> accommodationToBeApproved = db.AppAccommodations.Where(p => p.Approved == false) as List<Accommodation>;

            hubContext.Clients.Group("Admins").clickNotification(msg);
        }

        public void GetNotification()
        {
            List<AppUser> listOfUser = db.AppUsers.ToList();
            foreach (var user in listOfUser)
            {
                if (!user.Approved)
                {
                    hubContext.Clients.Group("Admins").clickNotification("There are unapproved users. Check them in admin panel.");
                    break;
                }
            }

            List<Service> listOfServices = db.Services.ToList();
            foreach (var service in listOfServices)
            {
                if (!service.Approved)
                {
                    hubContext.Clients.Group("Admins").clickNotification("There are unapproved services. Check them in admin panel.");
                    break;
                }
            }
        }

        public override Task OnConnected()
        {
            //Ako vam treba pojedinacni User
            //var identityName = Context.User.Identity.Name;

            //Groups.Add(Context.ConnectionId, "Admins");

           /* if (Context.User.IsInRole("Admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");
            }*/

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //Groups.Remove(Context.ConnectionId, "Admins");

            /*if (Context.User.IsInRole("Admin"))
            {
                Groups.Remove(Context.ConnectionId, "Admins");
            }*/

            return base.OnDisconnected(stopCalled);
        }
    }
}