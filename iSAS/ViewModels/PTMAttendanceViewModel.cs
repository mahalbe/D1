using ISas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ISas.Web.ViewModels
{
    public class PTMAttendanceViewModel : BaseViewModel
    {
        private List<StudentPTMAttendance> _studentPTMAttendanceList;
        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private List<ClassAttendance> _classAttendanceList;
        private IEnumerable<SelectListItem> _ptmDateList;
        private IEnumerable<SelectListItem> _ptmCategoryList; 
        public PTMAttendanceViewModel()
        {
            this._studentPTMAttendanceList = new List<StudentPTMAttendance>();
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
            this._ptmDateList = Enumerable.Empty<SelectListItem>();
            this._ptmCategoryList = Enumerable.Empty<SelectListItem>();
        }

        public string ClassTeacherName { get; set; }
        public string AttendanceMessage { get; set; }
        public IEnumerable<SelectListItem> SessionList
        {
            get
            {
                return this._sessionList;
            }
            set
            {
                this._sessionList = value;
            }
        }

        public string SelectedSessionId { get; set; }
        
        public IEnumerable<SelectListItem> ClassList
        {
            get
            {
                return this._classList;
            }
            set
            {
                this._classList = value;
            }
        }

        public string SelectedClassId { get; set; }

        public IEnumerable<SelectListItem> SectionList
        {
            get
            {
                return this._sectionList;
            }
            set
            {
                this._sectionList = value;
            }
        }
        
        public string SelectedSectionId { get; set; }
        public IEnumerable<SelectListItem> PTMDateList
        {
            get
            {
                return this._ptmDateList;
            }
            set
            {
                this._ptmDateList = value;
            }
        }
        public string SelectedPtmDate { get; set; }

        public IEnumerable<SelectListItem> PTMCategoryList
        {
            get
            {
                return this._ptmDateList;
            }
            set
            {
                this._ptmDateList = value;
            }
        }
        public string SelectedPtmCategoryId { get; set; }
        
        public DateTime? AttendanceDate { get; set; }
        
        public bool IsMarkPresent { get; set; }

        public bool IsMarkAbsent { get; set; }
        
        public int TotalStudents
        {
            get
            {
                return _studentPTMAttendanceList.Count;
            }
        }
        
        public int PresentStudents
        {
            get
            {
                return _studentPTMAttendanceList.Count(p => p.StudentPTMAttendanceDetail.Student == "P");
            }
        }
        public int AbsentStudents
        {
            get
            {
                return _studentPTMAttendanceList.Count(p => p.StudentPTMAttendanceDetail.Student == "A");
            }
        }
        public int StudentsOnLeave
        {
            get
            {
                return _studentPTMAttendanceList.Count(p => p.StudentPTMAttendanceDetail.Student == "L");
            }
        }
        //public string Leave { get; set; }

        //public List<SelectListItem> LeaveList
        //{
        //    get
        //    {
        //        return new List<SelectListItem>{   
        //             new SelectListItem { Value = LeaveType.NA.ToString()  , Text = LeaveType.NA.ToString()  },
        //               new SelectListItem { Value = LeaveType.PL.ToString()  , Text = LeaveType.PL.ToString()  },
        //               new SelectListItem{ Value = LeaveType.ML.ToString()  , Text = LeaveType.ML.ToString() },
        //               new SelectListItem{ Value = LeaveType.SL.ToString()  , Text = LeaveType.SL.ToString() }
        //        };
            
            
        //    }
        //}
       
        public List<StudentPTMAttendance> StudentPTMAttendanceList
        {
            get
            {
                return this._studentPTMAttendanceList;
            }
            set
            {
                this._studentPTMAttendanceList = value;
            }

        }

        public enum AttendanceType
        {
            PTM = 0,
            ORIENTATION = 1
        }
        //public AttendanceType AttendanceType { get; set; }


        public List<ClassAttendance> ClassAttendanceList
        {
            get
            {
                return this._classAttendanceList;
            }
            set
            {
                this._classAttendanceList = value;
            }

        }
    }
}