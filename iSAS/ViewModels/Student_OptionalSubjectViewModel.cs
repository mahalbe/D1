using ISas.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ISas.Web.ViewModels
{
    public class Student_OptionalSubjectViewModel : BaseViewModel
    {
        
        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private IEnumerable<SelectListItem> _optionalSubjectList;
        private IEnumerable<SelectListItem> _optionalSubjectParametersList;
        private List<StudentList> _studentList;


        public Student_OptionalSubjectViewModel()
        {
            this._studentList = new List<StudentList>();
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
            this._optionalSubjectList = Enumerable.Empty<SelectListItem>();
            this._optionalSubjectParametersList = Enumerable.Empty<SelectListItem>();
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

        public IEnumerable<SelectListItem> OptionalSubjectList
        {
            get
            {
                return this._optionalSubjectList;
            }
            set
            {
                this._optionalSubjectList = value;
            }
        }
        public string SelectedOptioanSubjectId { get; set; }

        
        public IEnumerable<SelectListItem> OptionalSubjectParametersList
        {
            get
            {
                return this._optionalSubjectParametersList;
            }
            set
            {
                this._optionalSubjectParametersList = value;
            }
        }
        public string SelectedParameterId { get; set; }
        
        public List<StudentList> StudentList
        {
            get
            {
                return this._studentList;
            }
            set
            {
                this._studentList = value;
            }

        }
    }
}