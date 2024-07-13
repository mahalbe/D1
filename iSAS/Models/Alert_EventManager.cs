using ISas.Entities.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISas.Web.Models
{
    public class Alert_EventManager : IAlert_EventManager
    {
        public List<Alert_EventManagerModel> GetAlert_EventList()
        {
            return Alert_EventManagerHelper.GetStudentRecords();
        }
    }
}