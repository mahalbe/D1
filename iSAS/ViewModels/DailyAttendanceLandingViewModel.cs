using ISas.Entities;
using System.Collections.Generic;

namespace ISas.Web.ViewModels
{
    public class DailyAttendanceLandingViewModel:BaseViewModel
    {
        private List<DailyAttenanceSummary> _dailyAttenanceSummaryList;
        public List<DailyAttenanceSummary> DailyAttenanceSummaryList
        {
            get
            {
                return this._dailyAttenanceSummaryList;
            }
            set
            {
                this._dailyAttenanceSummaryList = value;
            }
        }
    }
}