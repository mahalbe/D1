using ISas.Entities.CommonEntities;
using ISas.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ISas.Web.Models
{
    public class Alert_EventManagerHelper
    {
        public static List<Alert_EventManagerModel> GetStudentRecords()
        {
            var lstStudentRecords = new List<Alert_EventManagerModel>();
            string dbConnectionSettings = ConfigurationManager.ConnectionStrings["iSASDB"].ConnectionString;

            using (var dbConnection = new SqlConnection(dbConnectionSettings))
            {
                dbConnection.Open();

                var sqlCommandText = @"SELECT [EventDateTime],[EventDiscription] FROM [dbo].[Alert_EventManager]";

                using (var sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    AddSQLDependency(sqlCommand);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    lstStudentRecords = GetStudentRecords(reader);
                }
            }
            return lstStudentRecords;
        }
        /// <summary>
        /// Adds SQLDependency for change notification and passes the information to Student Hub for broadcasting
        /// </summary>
        /// <param name="sqlCommand"></param>
        private static void AddSQLDependency(SqlCommand sqlCommand)
        {
            sqlCommand.Notification = null;

            var dependency = new SqlDependency(sqlCommand);

            dependency.OnChange += (sender, sqlNotificationEvents) =>
            {
                if (sqlNotificationEvents.Type == SqlNotificationType.Change)
                {
                    NotificationHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
                }
            };
        }

        /// <summary>
        /// Fills the Student Records
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<Alert_EventManagerModel> GetStudentRecords(SqlDataReader reader)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iSASDB"].ToString()))
            {
                DataSet ds = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_Alert_EventManager_Transaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", HttpContext.Current.Session["UserId"].ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                return ds.Tables[0].AsEnumerable().Select(r => new Alert_EventManagerModel
                {
                     EventDate = r.Field<string>("EventDate"),
                     EventDiscription = r.Field<string>("EventDiscription"),
                     EventTime = r.Field<string>("EventTime"),
                }).ToList();
            }
        }
    }
}