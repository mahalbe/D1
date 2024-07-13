using ISas.Entities;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace ISas.Web.ViewModels
{
    public class MarksEntryCoScholasticViewModel:BaseViewModel
    {
        
        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _examList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private IEnumerable<SelectListItem> _subjectList;
        private IEnumerable<SelectListItem> _studentList;
        private List<StudentCoScholasticMarkList> _studentCoScholasticMarkList;
        private List<StudentCoScholasticMarkFromTableList> _studentCoScholasticMarkFromTableList;

        public MarksEntryCoScholasticViewModel()
        {
            
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._examList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
            this._subjectList = Enumerable.Empty<SelectListItem>();
            this._studentList = Enumerable.Empty<SelectListItem>();
            this._studentCoScholasticMarkList = new List<StudentCoScholasticMarkList>();
            this._studentCoScholasticMarkFromTableList = new List<StudentCoScholasticMarkFromTableList>();
        }


        //public string ClassTeacherName { get; set; }

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
        public IEnumerable<SelectListItem> StudentList
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
        public string SelectedStudentERPNo { get; set; }
        public IEnumerable<SelectListItem> SubjectList
        {
            get
            {
                return this._subjectList;
            }
            set
            {
                this._subjectList = value;
            }
        }
        public string SelectedSubjectId { get; set; }

        //public IEnumerable<SelectListItem> GradeList
        //{
        //    get
        //    {
        //        return this._gradeList;
        //    }
        //    set
        //    {
        //        this._gradeList = value;
        //    }
        //}
        //public string SelectedGradeId { get; set; }


        //public string SubjectCategory { get; set; }
        //public bool IsStudentAbsent { get; set; }
        //public bool IsStudentonML { get; set; }
        //public bool IsStudentExempt { get; set; }
        //public bool StudentMark { get; set; }
        //public bool StudentGrade { get; set; }
        

        public List<StudentCoScholasticMarkList> StudentsCoScholasticMarkList
        {
            get
            {
                return this._studentCoScholasticMarkList;
            }
            set
            {
                this._studentCoScholasticMarkList = value;
            }

        }
        public List<StudentCoScholasticMarkFromTableList> StudentCoScholasticMarkFromTableList
        {
            get
            {
                return this._studentCoScholasticMarkFromTableList;
            }
            set
            {
                this._studentCoScholasticMarkFromTableList = value;
            }

        }
    }
}