using ISas.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ISas.Web.ViewModels
{
    public class Student_UpdationViewModel : BaseViewModel
    {
        
        private IEnumerable<SelectListItem> _sessionList;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _sectionList;
        private IEnumerable<SelectListItem> _religionList;
        private IEnumerable<SelectListItem> _categoryList;
        private IEnumerable<SelectListItem> _houseList;
        private IEnumerable<SelectListItem> _bloodGroupList;
        private IEnumerable<SelectListItem> _modeOfTransportList;
        private IEnumerable<SelectListItem> _pickedUpByList;
        private IEnumerable<SelectListItem> _professionList;
        private IEnumerable<SelectListItem> _updateParametersList;
        private List<ClassofJoiningList> _classofjoiningList;
        

        public Student_UpdationViewModel()
        {
            this._classofjoiningList = new List<ClassofJoiningList>();
            this._sessionList = Enumerable.Empty<SelectListItem>();
            this._classList = Enumerable.Empty<SelectListItem>();
            this._sectionList = Enumerable.Empty<SelectListItem>();
            this._religionList = Enumerable.Empty<SelectListItem>();
            this._categoryList = Enumerable.Empty<SelectListItem>();
            this._houseList = Enumerable.Empty<SelectListItem>();
            this._bloodGroupList = Enumerable.Empty<SelectListItem>();
            this._modeOfTransportList = Enumerable.Empty<SelectListItem>();
            this._professionList = Enumerable.Empty<SelectListItem>();
            this._updateParametersList = Enumerable.Empty<SelectListItem>();

            Paramether1_DDList = new List<SelectListItem>();
            Paramether2_DDList = new List<SelectListItem>();
            Paramether3_DDList = new List<SelectListItem>();
        }

        public string ColumnType { get; set; }
        public string Print { get; set; }
        public string SelectedClassSectionName { get; set; }
      
        public List<SelectListItem> Paramether1_DDList { get; set; }
        public List<SelectListItem> Paramether2_DDList { get; set; }
        public List<SelectListItem> Paramether3_DDList { get; set; }

        //public string AttendanceMessage { get; set; }
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
        
        public IEnumerable<SelectListItem> ReligionList
        {
            get
            {
                return this._religionList;
            }
            set
            {
                this._religionList = value;
            }
        }
        public string SelectedReligionId { get; set; }
        
        public IEnumerable<SelectListItem> CategoryList
        {
            get
            {
                return this._categoryList;
            }
            set
            {
                this._categoryList = value;
            }
        }
        public string SelectedCategoryId { get; set; }
       
        public IEnumerable<SelectListItem> HouseList
        {
            get
            {
                return this._houseList;
            }
            set
            {
                this._houseList = value;
            }
        }
        public string SelectedHouseId { get; set; }

        public IEnumerable<SelectListItem> BloodGroupList
        {
            get
            {
                return this._bloodGroupList;
            }
            set
            {
                this._bloodGroupList = value;
            }
        }
        public string SelectedBloodGroupId { get; set; }

        public IEnumerable<SelectListItem> ModeOfTransportList
        {
            get
            {
                return this._modeOfTransportList;
            }
            set
            {
                this._modeOfTransportList = value;
            }
        }
        public string SelectedModeofTransportId { get; set; }

        public IEnumerable<SelectListItem> PickedUpByList
        {
            get
            {
                return this._pickedUpByList;
            }
            set
            {
                this._pickedUpByList = value;
            }
        }
        public string SelectedPickedUpById { get; set; }

        public IEnumerable<SelectListItem> ProfessionList
        {
            get
            {
                return this._professionList;
            }
            set
            {
                this._professionList = value;
            }
        }
        public string SelectedProfessionId { get; set; }

        
        public IEnumerable<SelectListItem> UpdationParametersList
        {
            get
            {
                return this._updateParametersList;
            }
            set
            {
                this._updateParametersList = value;
            }
        }
        public string SelectedParameterId { get; set; }
        //public int TotalStudents
        //{
        //    get
        //    {
        //        return _studentAttendanceList.Count;
        //    }
        //}
        public List<ClassofJoiningList> ClassofJoiningList
        {
            get
            {
                return this._classofjoiningList;
            }
            set
            {
                this._classofjoiningList = value;
            }

        }
    }
}