using ISas.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ISas.Web.ViewModels
{
    public class DailyAttendanceViewModel : BaseViewModel
    {
        private List<StudentAttendance> _studentAttendanceList;
        private IEnumerable<SelectListItem> _leaveTypeList;
        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private List<ClassAttendance> _classAttendanceList;

        public DailyAttendanceViewModel()
        {
            this._studentAttendanceList = new List<StudentAttendance>();
            this._leaveTypeList = new List<SelectListItem>();
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
        }

        public bool IsOffToday { get; set; }
        public bool IsSendSMS { get; set; }
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

        [Required(ErrorMessage ="Session is requried..!")]
        [Display(Name = "Session")]
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

        [Required(ErrorMessage = "Class is requried..!")]
        [Display(Name = "Class")]
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

        [Required(ErrorMessage = "Section is requried..!")]
        [Display(Name ="Section")]
        public string SelectedSectionId { get; set; }

        //[DataType(DataType.Date)]
        [Required(ErrorMessage = "Date is requried..!")]
        [Display(Name = "Date")]
        public string AttendanceDate { get; set; }
        
        public bool IsMarkPresent { get; set; }

        public bool IsMarkAbsent { get; set; }
        
        public int TotalStudents
        {
            get
            {
                return _studentAttendanceList.Count;
            }
        }
        
        public int PresentStudents
        {
            get
            {
                return _studentAttendanceList.Count(p => p.StudentAttendanceDetails.MorningAttendnace == "P");
            }
        }
        public int AbsentStudents
        {
            get
            {
                return _studentAttendanceList.Count(p => p.StudentAttendanceDetails.MorningAttendnace == "A");
            }
        }
        public int StudentsOnLeave
        {
            get
            {
                return _studentAttendanceList.Count(p => p.StudentAttendanceDetails.MorningAttendnace == "L");
            }
        }
        //public string Leave { get; set; }

        public IEnumerable<SelectListItem> LeaveTypeList
        {
            get
            {
                return new List<SelectListItem>{   
                       new SelectListItem { Value = LeaveType.NA.ToString()  , Text = LeaveType.NA.ToString()  },
                       new SelectListItem { Value = LeaveType.PL.ToString()  , Text = LeaveType.PL.ToString()  },
                       new SelectListItem{ Value = LeaveType.ML.ToString()  , Text = LeaveType.ML.ToString() },
                       new SelectListItem{ Value = LeaveType.SL.ToString()  , Text = LeaveType.SL.ToString() }
                };
            }
            set
            {
                this._leaveTypeList = value ;
            }
        }
        public string SelectedLeaveType { get; set; }
        public List<StudentAttendance> StudentAttendanceList
        {
            get
            {
                return this._studentAttendanceList;
            }
            set
            {
                this._studentAttendanceList = value;
            }

        }
        public static class Enumeration
        {
            public static IDictionary<int, string> GetAll<TEnum>() where TEnum : struct
            {
                var enumerationType = typeof(TEnum);

                if (!enumerationType.IsEnum)
                    throw new ArgumentException("Enumeration type is expected.");

                var dictionary = new Dictionary<int, string>();

                foreach (int value in Enum.GetValues(enumerationType))
                {
                    var name = Enum.GetName(enumerationType, value);
                    dictionary.Add(value, name);
                }

                return dictionary;
            }
        }
        public enum LeaveType
        {
            NA = 0,
            SL = 1,
            ML = 2,
            PL = 3
        }
        
       //public LeaveType Leave { get; set; }
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