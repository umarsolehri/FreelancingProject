using FFYP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFYP
{
    public class RegisterComponent
    {
        public void RegisterNotification(DateTime currentTime)
        {
            currentTime = DateTime.Now;
            //string ct;
            //string constring = ConfigurationManager.ConnectionStrings["SignalRContext"].ConnectionString;
            FFYPContext db = new FFYPContext();
            var cons = db.Database.Connection.ConnectionString;
            string sqlCmd = @"SELECT [BidingID], [SiteUserID],[ProjectID] from [dbo].[Biding] where [BidingDate] < @AddedTime";
            using (SqlConnection con = new SqlConnection(cons))
            {
                SqlCommand cmd = new SqlCommand(sqlCmd, con);
                cmd.Parameters.AddWithValue("@AddedTime", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        private void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;
                var notifiactionHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notifiactionHub.Clients.All.notify("added");
                RegisterNotification(DateTime.Now);
            }
        }

        public List<Biding> GetBidings(DateTime afterdate)
        {
            FFYPContext db = new FFYPContext();

            var list = db.Biding.Where(a => a.BidingDate < afterdate).OrderByDescending(a => a.BidingDate).ToList();
            return list;

        }
    }
}