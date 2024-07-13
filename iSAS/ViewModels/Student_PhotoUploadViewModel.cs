using ISas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ISas.Web.ViewModels
{
    public class Student_PhotoUploadViewModel : BaseViewModel
    {
        //private List<StudentAttendance> _studentAttendanceList;
        //private IEnumerable<SelectListItem> _leaveTypeList;
        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private List<ClassPhotoList> _classPhotoList;

        public Student_PhotoUploadViewModel()
        {
            //this._studentAttendanceList = new List<StudentAttendance>();
            //this._leaveTypeList = new List<SelectListItem>();
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
        }

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

        public DateTime? AttendanceDate { get; set; }
        
        public bool IsMarkPresent { get; set; }

        public bool IsMarkAbsent { get; set; }
        
        
        
        
        //public string Leave { get; set; }

       
        
        
       //public LeaveType Leave { get; set; }
        public List<ClassPhotoList> ClassPhotoList
        {
            get
            {
                return this._classPhotoList;
            }
            set
            {
                this._classPhotoList = value;
            }

        }
    }
}