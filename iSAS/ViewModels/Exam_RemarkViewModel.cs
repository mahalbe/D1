using ISas.Entities;
using ISas.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISas.Web.ViewModels
{
    public class Exam_RemarkViewModel : BaseViewModel
    {

        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _examList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private IEnumerable<SelectListItem> _remarkList;
        private List<StudentRemarkList> _studentRemarkList;


        public Exam_RemarkViewModel()
        {
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._examList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
            this._remarkList = Enumerable.Empty<SelectListItem>();
            this._studentRemarkList = new List<StudentRemarkList>();
        }

        public string TermName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }

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

        public IEnumerable<SelectListItem> ExamList
        {
            get
            {
                return this._examList;
            }
            set
            {
                this._examList = value;
            }
        }
        public string SelectedExamId { get; set; }

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

        public IEnumerable<SelectListItem> RemarkList
        {
            get
            {
                return this._remarkList;
            }
            set
            {
                this._remarkList = value;
            }
        }
        public string SelectedRemarkId { get; set; }

        public List<StudentRemarkList> StudentRemarkList
        {
            get
            {
                return this._studentRemarkList;
            }
            set
            {
                this._studentRemarkList = value;
            }

        }
        public string SelectedRemarkOption { get; set; }
    }
}