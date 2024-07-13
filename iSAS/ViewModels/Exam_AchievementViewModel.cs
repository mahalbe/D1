using ISas.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ISas.Web.ViewModels
{
    public class Exam_AchievementViewModel : BaseViewModel
    {
        
        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _examList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private IEnumerable<SelectListItem> _studentList;
        private List<StudentAchievementList> _studentAchievementList;


        public string TermName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }

        public Exam_AchievementViewModel()
        {
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._examList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
            this._studentList = Enumerable.Empty<SelectListItem>();
            this._studentAchievementList = new List<StudentAchievementList>();
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

        public List<StudentAchievementList> StudentAchievementList
        {
            get
            {
                return this._studentAchievementList;
            }
            set
            {
                this._studentAchievementList = value;
            }

        }
    }
}