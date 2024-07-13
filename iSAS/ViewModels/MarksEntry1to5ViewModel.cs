using ISas.Entities;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace ISas.Web.ViewModels
{
    public class MarksEntry1to5ViewModel //:BaseViewModel
    {
        private List<StudentsMarkList> _studenstMarkList;
        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _examList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private IEnumerable<SelectListItem> _assessmentList;
        private IEnumerable<SelectListItem> _subjectList;
        private IEnumerable<SelectListItem> _gradeList;
        private List<StudentsMarksFromTableList> _studentsMarksFromTableList;

        public MarksEntry1to5ViewModel()
        {
            this._studenstMarkList = new List<StudentsMarkList>();
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._examList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
            this._assessmentList = Enumerable.Empty<SelectListItem>();
            this._subjectList = Enumerable.Empty<SelectListItem>();
            this._gradeList = Enumerable.Empty<SelectListItem>();
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
        public IEnumerable<SelectListItem> AssessmentList
        {
            get
            {
                return this._assessmentList;
            }
            set
            {
                this._assessmentList = value;
            }
        }
        public string SelectedAssessmentId { get; set; }
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

        public IEnumerable<SelectListItem> GradeList
        {
            get
            {
                return this._gradeList;
            }
            set
            {
                this._gradeList = value;
            }
        }
        public string SelectedGradeId { get; set; }

        //Selected Drop Down Values
        public string SessionName { get; set; }
        public string TermName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public string AssessmentName { get; set; }



        public string SubjectCategory { get; set; }
        //public bool IsStudentAbsent { get; set; }
        //public bool IsStudentonML { get; set; }
        //public bool IsStudentExempt { get; set; }
        //public bool StudentMark { get; set; }
        //public bool StudentGrade { get; set; }
        

        public List<StudentsMarkList> StudentsMarkList
        {
            get
            {
                return this._studenstMarkList;
            }
            set
            {
                this._studenstMarkList = value;
            }

        }
        public List<StudentsMarksFromTableList> StudentsMarksFromTableList
        {
            get
            {
                return this._studentsMarksFromTableList;
            }
            set
            {
                this._studentsMarksFromTableList = value;
            }

        }
    }
}