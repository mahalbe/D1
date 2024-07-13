using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using ISas.Repository.Interface;
using ISas.Repository.Implementation;
using ISas.Repository.ImplementationGetAllClasses;

namespace ISas.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<ILoginData, LoginData>();
            container.RegisterType<IStudentClass, StudentClassRepositroy>();
            container.RegisterType<IStudentSection, StudentSectionRepository>();
            container.RegisterType<IStudentSession, StudentSessionRepository>();
            container.RegisterType<IStudentAttendance, StudentAttendanceRepository>();
            container.RegisterType<IExam, ExamRepository>();
            container.RegisterType<IPtmAttendance, PtmAttendanceRepository>();
            container.RegisterType<IAttendanceRegister, AttendanceRegisterRepository>();
            container.RegisterType<IStudentUpdation, StudentUpdationRepository>();
            container.RegisterType<IStudent_OptionalSubject, Student_OptionalSubjectRepository>();
            container.RegisterType<IExam_AchievementRepository, Exam_AchievementRepository>();
            container.RegisterType<IExam_RemarkRepository, Exam_RemarkRepository>();
            container.RegisterType<IExam_ReportCard, Exam_ReportCardRepository>();
            container.RegisterType<ICountryRepository, CountryRepository>();
            container.RegisterType<IStateRepository, StateRepository>();
            container.RegisterType<ICityRepository, CityRepository>();
            container.RegisterType<IAreaRepository, AreaRepository>();
            container.RegisterType<ISchool_DocumentMasterRepo, School_DocumentMasterRepo>();
            container.RegisterType<Repository.StudentRegistrationRepository.IRepository.IStudentRegistrationRepo, Repository.StudentRegistrationRepository.Repository.StudentRegistrationRepo>();
            container.RegisterType<ISchoolAdmissionCategoryRepo, SchoolAdmissionCategoryRepo>();

            container.RegisterType<Repository.StudentRegistrationRepository.IRepository.IStudent_IdentityCardRepo, Repository.StudentRegistrationRepository.Repository.Student_IdentityCardRepo>();


            container.RegisterType<Repository.StudentRegistrationRepository.IRepository.IStudentAdmissionRepo, Repository.StudentRegistrationRepository.Repository.StudentAdmissionRepo>();
            container.RegisterType<Repository.StudentRegistrationRepository.IRepository.IRouteStopRepo, Repository.StudentRegistrationRepository.Repository.RouteStopRepo>();
            container.RegisterType<Repository.StudentRegistrationRepository.IRepository.IStudent_BoardRegistrationRepo, Repository.StudentRegistrationRepository.Repository.Student_BoardRegistrationRepo>();
            container.RegisterType<Repository.StudentRepository.IRepository.IStudent_BoardRegistration1Repo, Repository.StudentRepository.Repository.Student_BoardRegistration1Repo>();
            container.RegisterType<Repository.StudentRepository.IRepository.IStudent_CertificateRepo, Repository.StudentRepository.Repository.Student_CertificateRepo>();
            container.RegisterType<Repository.StudentRepository.IRepository.IstudentHistoryRepo, Repository.StudentRepository.Repository.studentHistoryRepo>();
            container.RegisterType<Repository.StudentRepository.IRepository.IMyClassRepo, Repository.StudentRepository.Repository.MyClassRepo>();

            container.RegisterType<IStudentTC, StudentTCRepositroy>();
            container.RegisterType<Repository.StaffRepository.IRepository.IStaff_StaffDetailMasterRepo, Repository.StaffRepository.Repository.Staff_StaffDetailMasterRepo>();
            container.RegisterType<Repository.StaffRepository.IRepository.IStaff_AttendanceRegisterRepo, Repository.StaffRepository.Repository.Staff_AttendanceRegisterRepo>();
            container.RegisterType<Repository.StaffRepository.IRepository.IStaffAttendance_TimeGroupMasterRepo, Repository.StaffRepository.Repository.StaffAttendance_TimeGroupMasterRepo>();

            container.RegisterType<IStudent_NSORepo, Student_NSORepo>();
            container.RegisterType<Repository.SMSManagement.IRepository.ISMSManagementRepo, Repository.SMSManagement.Repository.SMSManagementRepo>();
            container.RegisterType<Repository.SMSManagement.IRepository.ISMSTempleteRepo, Repository.SMSManagement.Repository.SMSTempleteRepo>();
            container.RegisterType<Repository.SMSManagement.IRepository.ISMS_AlertSetupRepo, Repository.SMSManagement.Repository.SMS_AlertSetupRepo>();
            

            container.RegisterType<Repository.DashboardRepository.IRepository.IStudentProfileRepo, Repository.DashboardRepository.Repository.StudentProfileRepo>();
            container.RegisterType<Repository.DashboardRepository.IRepository.ICommon_NECNRepo, Repository.DashboardRepository.Repository.Common_NECNRepo>();
            container.RegisterType<Repository.DashboardRepository.IRepository.IDashboardRepo, Repository.DashboardRepository.Repository.DashboardRepo>();
            container.RegisterType<Repository.DashboardRepository.IRepository.IDashBoard_ParentRequestMasterRepo, Repository.DashboardRepository.Repository.DashBoard_ParentRequestMasterRepo>();
            container.RegisterType<Controllers.Dashboard.ParentRequestMasterController, Controllers.Dashboard.ParentRequestMasterController>();
            container.RegisterType<Repository.DashboardRepository.IRepository.IStudentFeeDetailsRepo, Repository.DashboardRepository.Repository.StudentFeeDetailsRepo>();
            container.RegisterType<Repository.DashboardRepository.IRepository.IDashBoard_StudentStataticsRepo, Repository.DashboardRepository.Repository.DashBoard_StudentStataticsRepo>();
            container.RegisterType<Repository.DashboardRepository.IRepository.IToDo_TaskRepo, Repository.DashboardRepository.Repository.ToDo_TaskRepo>();
            container.RegisterType<Repository.DashboardRepository.IRepository.IDashBoard_StaffStataticsRepo, Repository.DashboardRepository.Repository.DashBoard_StaffStataticsRepo>();
            container.RegisterType<Repository.DashboardRepository.IRepository.IAppliedLeaveNoticBoard, Repository.DashboardRepository.Repository.AppliedLeaveNoticBoardRepo>();


            container.RegisterType<ICommonRepo, CommonRepo>();
            container.RegisterType<ICommon_AttachmentRefRepo, Common_AttachmentRefRepo>();
            

            container.RegisterType<IMarksEntry_StudentWiseRepo, MarksEntry_StudentWiseRepo>();
            container.RegisterType<Repository.TransportRepo.IRepository.IAvailTransportRepo, Repository.TransportRepo.Repository.AvailTransportRepo>();

            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_TransactionRepo, Repository.FeeModuleRepo.Repository.Fee_TransactionRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_ConcessionRepo, Repository.FeeModuleRepo.Repository.Fee_ConcessionRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_MiscDuesRepo, Repository.FeeModuleRepo.Repository.Fee_MiscDuesRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_InvoiceCreationRepo, Repository.FeeModuleRepo.Repository.Fee_InvoiceCreationRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_FeeHeadMasterRepo, Repository.FeeModuleRepo.Repository.Fee_FeeHeadMasterRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_FeeStructureMasterRepo, Repository.FeeModuleRepo.Repository.Fee_FeeStructureMasterRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_InstallmentSetupRepo, Repository.FeeModuleRepo.Repository.Fee_InstallmentSetupRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_FinePolicyRepo, Repository.FeeModuleRepo.Repository.Fee_FinePolicyRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_DueSetupRepo, Repository.FeeModuleRepo.Repository.Fee_DueSetupRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_ConcessionPolicyRepo, Repository.FeeModuleRepo.Repository.Fee_ConcessionPolicyRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_ReceiptCancellationRepo, Repository.FeeModuleRepo.Repository.Fee_ReceiptCancellationRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_DefaulterDetailsRepo, Repository.FeeModuleRepo.Repository.Fee_DefaulterDetailsRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_ReceiptHeaderMasterRepo, Repository.FeeModuleRepo.Repository.Fee_ReceiptHeaderMasterRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_FeeBillReportRepo, Repository.FeeModuleRepo.Repository.Fee_FeeBillReportRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_ReportRepo, Repository.FeeModuleRepo.Repository.Fee_ReportRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_AdvanceCollectionRepo, Repository.FeeModuleRepo.Repository.Fee_AdvanceCollectionRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_PaymentGatewayMasterRepo, Repository.FeeModuleRepo.Repository.Fee_PaymentGatewayMasterRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IFee_StudentFeeStructureRepo, Repository.FeeModuleRepo.Repository.Fee_StudentFeeStructureRepo>();
            container.RegisterType<Repository.FeeModuleRepo.IRepository.IExpanseMasterRepo, Repository.FeeModuleRepo.Repository.ExpanseMasterRepo>();


            container.RegisterType<Repository.Academic.IRepository.IAcademic_SectionMasterRepo, Repository.Academic.Repository.Academic_SectionMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_SchoolSetupRepo, Repository.Academic.Repository.Academic_SchoolSetupRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_WingSetupRepo, Repository.Academic.Repository.Academic_WingSetupRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_ClassSetupRepo, Repository.Academic.Repository.Academic_ClassSetupRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_SectionSetupRepo, Repository.Academic.Repository.Academic_SectionSetupRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_HouseMasterRepo, Repository.Academic.Repository.Academic_HouseMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_ProfessionMasterRepo, Repository.Academic.Repository.Academic_ProfessionMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_QualificationMasterRepo, Repository.Academic.Repository.Academic_QualificationMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_DesignationMasterRepo, Repository.Academic.Repository.Academic_DesignationMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_DepartmentMasterRepo, Repository.Academic.Repository.Academic_DepartmentMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_BankMasterRepo, Repository.Academic.Repository.Academic_BankMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_HolidayMasterRepo, Repository.Academic.Repository.Academic_HolidayMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_HolidayDeclarationRepo, Repository.Academic.Repository.Academic_HolidayDeclarationRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_HolidayAllocationRepo, Repository.Academic.Repository.Academic_HolidayAllocationRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_DynamicReportWizardRepo, Repository.Academic.Repository.Academic_DynamicReportWizardRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_PTMOpendDayRepo, Repository.Academic.Repository.Academic_PTMOpendDayRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_DirectoryMasterRepo, Repository.Academic.Repository.Academic_DirectoryMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_HomeWorkMasterRepo, Repository.Academic.Repository.Academic_HomeWorkMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_SyllabusMasterRepo, Repository.Academic.Repository.Academic_SyllabusMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_EventMasterRepo, Repository.Academic.Repository.Academic_EventMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_SessionMasterRepo, Repository.Academic.Repository.Academic_SessionMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_ReportMasterRepo, Repository.Academic.Repository.Academic_ReportMasterRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_RoleSetupRepo, Repository.Academic.Repository.Academic_RoleSetupRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_PromotionSetupRepo, Repository.Academic.Repository.Academic_PromotionSetupRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_WorkflowSetupRepo, Repository.Academic.Repository.Academic_WorkflowSetupRepo>();
            container.RegisterType<Repository.Academic.IRepository.IAcademic_ERPBill_Repo, Repository.Academic.Repository.Academic_ERPBill_Repo>();
            container.RegisterType<Repository.TransportRepo.IRepository.ITransport_RouteMasterRepo, Repository.TransportRepo.Repository.Transport_RouteMasterRepo>();
            container.RegisterType<Repository.TransportRepo.IRepository.ITransport_StopSetupRepo, Repository.TransportRepo.Repository.Transport_StopSetupRepo>();
            container.RegisterType<Repository.TransportRepo.IRepository.ITransport_VehicleSetupRepo, Repository.TransportRepo.Repository.Transport_VehicleSetupRepo>();

            container.RegisterType<IExam_StudentProfileRepo, Exam_StudentProfileRepo>();

            container.RegisterType<Repository.Library.IRepository.ILibrary_BookTitleMasterRepo, Repository.Library.Repository.Library_BookTitleMasterRepo>();
            container.RegisterType<Repository.Library.IRepository.ILibrary_SetupRepo, Repository.Library.Repository.Library_SetupRepo>();
            container.RegisterType<Repository.Library.IRepository.ILibrary_AuthorMasterRepo, Repository.Library.Repository.Library_AuthorMasterRepo>();
            container.RegisterType<Repository.Library.IRepository.ILibrary_SupplierMasterRepo, Repository.Library.Repository.Library_SupplierMasterRepo>();
            container.RegisterType<Repository.Library.IRepository.ILibrary_PublisherMasterRepo, Repository.Library.Repository.Library_PublisherMasterRepo>();
            container.RegisterType<Repository.Library.IRepository.ILibrary_SubjectMasterRepo, Repository.Library.Repository.Library_SubjectMasterRepo>();
            container.RegisterType<Repository.Library.IRepository.ILibrary_BookMasterRepo, Repository.Library.Repository.Library_BookMasterRepo>();
            container.RegisterType<Repository.Library.IRepository.ILibrary_FineSetupRepo, Repository.Library.Repository.Library_FineSetupRepo>();
            container.RegisterType<Repository.Library.IRepository.ILibrary_GenerateBarcodeRepo, Repository.Library.Repository.Library_GenerateBarcodeRepo>();

            container.RegisterType<Repository.StudentRegistrationRepository.IRepository.IStudent_ReportRepo, ISas.Repository.StudentRegistrationRepository.Repository.Student_ReportRepo>();
            container.RegisterType<IExam_MarksVerificationRepo, Exam_MarksVerificationRepo>();
            container.RegisterType<IStudent_AttendanceReportRepo, Student_AttendanceReportRepo>();

            container.RegisterType<Repository.StaffRepository.IRepository.IStaff_ReportsRepo, Repository.StaffRepository.Repository.Staff_ReportsRepo>();

            container.RegisterType<Repository.SMSManagement.IRepository.ISMS_ReportRepo, Repository.SMSManagement.Repository.SMS_ReportRepo>();
            

            container.RegisterType<Repository.Library.IRepository.ILibrary_TransactionRepo, Repository.Library.Repository.Library_TransactionRepo>();

            container.RegisterType<Repository.StaffRepository.IRepository.IStaff_AttendanceReportsRepo, Repository.StaffRepository.Repository.Staff_AttendanceReportsRepo>();
            container.RegisterType<Repository.TransportRepo.IRepository.ITransport_ReportRepo, Repository.TransportRepo.Repository.Transport_ReportRepo>();

            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_LeaveMasterRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_LeaveMasterRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_LeaveRegisterRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_LeaveRegisterRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_SalaryHeadRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_SalaryHeadRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_PayBandMasterRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_PayBandMasterRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_CTC_SetupRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_CTC_SetupRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_StaffEnrollmentRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_StaffEnrollmentRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_FinalCTSRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_FinalCTSRepo >();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_AttendanceProcessRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_AttendanceProcessRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_SalaryRegisterRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_SalaryRegisterRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_SalaryRegister1Repo, Repository.HR_PayrollRepo.Repository.HR_Payroll_SalaryRegister1Repo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_Payroll_LeavePolicySetupRepo, Repository.HR_PayrollRepo.Repository.HR_Payroll_LeavePolicySetupRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_PolicyConfiguration, Repository.HR_PayrollRepo.Repository.HR_PolicyConfigurationRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRPayroll_SalaryRegisterRepo, Repository.HR_PayrollRepo.Repository.HRPayroll_SalaryRegisterRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHR_CompentatoryLeave, Repository.HR_PayrollRepo.Repository.HR_CompentatorRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRPayrollCreateOpenDaysRepo, Repository.HR_PayrollRepo.Repository.HRPayrollCreateOpenDaysRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRPayroll_FinalCTCRepo, Repository.HR_PayrollRepo.Repository.HRPayroll_FinalCTCRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRPayroll_MiscTransRepo, Repository.HR_PayrollRepo.Repository.HRPayroll_MiscTransRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRPayroll_SalarySheetRepo, Repository.HR_PayrollRepo.Repository.HRPayroll_SalarySheetRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IBankSalaryRegisterRepo, Repository.HR_PayrollRepo.Repository.BankSalaryRegisterRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRPayroll_PromotionRepo, Repository.HR_PayrollRepo.Repository.HRPayroll_PromotionRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IPayroll_PromotionRepo, Repository.HR_PayrollRepo.Repository.Payroll_PromotionRepo>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRPayroll_MiscTransRepo_V1, Repository.HR_PayrollRepo.Repository.HRPayroll_MiscTransRepo_V1>();
            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRPayroll_SalarySlipRepo, Repository.HR_PayrollRepo.Repository.HRPayroll_SalarySlipRepo>();

            container.RegisterType<Repository.UserRepo.IRepository.IUserCreationRepo, Repository.UserRepo.Repository.UserCreationRepo>();
            container.RegisterType<Repository.UserRepo.IRepository.IUserPermissionRepo, Repository.UserRepo.Repository.UserPermissionRepo>();
            container.RegisterType<Repository.StudentRegistrationRepository.IRepository.ISiblingAllotmentRepo, Repository.StudentRegistrationRepository.Repository.SiblingAllotmentRepo>();

            container.RegisterType<Repository.TimeTable_Repo.IRepository.ITimeTable_PeriodMatrixRepo, Repository.TimeTable_Repo.Repository.TimeTable_PeriodMatrixRepo>();
            container.RegisterType<Repository.TimeTable_Repo.IRepository.ITimeTable_StaffWorkLoadSetupRepo, Repository.TimeTable_Repo.Repository.TimeTable_StaffWorkLoadSetupRepo>();
            container.RegisterType<Repository.TimeTable_Repo.IRepository.ITimeTable_SetupRepo, Repository.TimeTable_Repo.Repository.TimeTable_SetupRepo>();
            container.RegisterType<Repository.TimeTable_Repo.IRepository.ITimeTable_PeriodTimingSetupRepo, Repository.TimeTable_Repo.Repository.TimeTable_PeriodTimingSetupRepo>();
            container.RegisterType<Repository.TimeTable_Repo.IRepository.ITimeTable_AdjustmentRepo, Repository.TimeTable_Repo.Repository.TimeTable_AdjustmentRepo>();

            container.RegisterType<Repository.ExaminationRepository.IRepository.IExam_TemplateSetupRepo, Repository.ExaminationRepository.Repository.Exam_TemplateSetupRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_SubjectMasterRepo, Repository.ExaminationRepository.Repository.Examination_SubjectMasterRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_AssessmentSetupRepo, Repository.ExaminationRepository.Repository.Examination_AssessmentSetupRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_ChildSubjectSetupRepo, Repository.ExaminationRepository.Repository.Examination_ChildSubjectSetupRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_MarksEntryRepo, Repository.ExaminationRepository.Repository.Examination_MarksEntryRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_RemarksCenterRepo, Repository.ExaminationRepository.Repository.Examination_RemarksCenterRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_RemarksEntryRepo, Repository.ExaminationRepository.Repository.Examination_RemarksEntryRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_AchievementEntryRepo, Repository.ExaminationRepository.Repository.Examination_AchievementEntryRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_ProfileEntryRepo, Repository.ExaminationRepository.Repository.Examination_ProfileEntryRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_ReportCardRepo, Repository.ExaminationRepository.Repository.Examination_ReportCardRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_ResultRepo, Repository.ExaminationRepository.Repository.Examination_ResultRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_GreenSheetRepo, Repository.ExaminationRepository.Repository.Examination_GreenSheetRepo>();
            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_RemarksEntryRepo_V1, Repository.ExaminationRepository.Repository.Examination_RemarksEntryRepo_V1>();
            container.RegisterType<Repository.ExpenseRepository.IRepository.IExpense_PettyCashManagementRepo, Repository.ExpenseRepository.Repository.Expense_PettyCashManagementRepo>();

            


            container.RegisterType<Repository.MobilePhoneRepository.IRepository.IFeeCollectionRepo, Repository.MobilePhoneRepository.Repository.FeeCollectionRepo>();


            container.RegisterType<Repository.StaffModuleRepository.IRepository.IStaffModule_DetailMasterRepo, Repository.StaffModuleRepository.Repository.StaffModule_DetailMasterRepo>();


            container.RegisterType<Repository.StaffRepository.IRepository.IStaff_LeaveApplyRepo, Repository.StaffRepository.Repository.Staff_LeaveApplyRepo>();

            container.RegisterType<Repository.StudentRepository.IRepository.IstudentProfileRepo, Repository.StudentRepository.Repository.studentProfileRepo>();

            container.RegisterType<Repository.FeeModuleRepo.IRepository.IonlinePaymentSearchRepo, Repository.FeeModuleRepo.Repository.onlinePaymentSearchRepo>();

            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IHRReportRepo, Repository.HR_PayrollRepo.Repository.HRReportRepo>();


            container.RegisterType<Repository.HR_PayrollRepo.IRepository.IdaArrearSheetRepo, Repository.HR_PayrollRepo.Repository.daArrearSheetRepo>();

            container.RegisterType<Repository.SMSManagement.IRepository.InoticeBoardRepo, Repository.SMSManagement.Repository.noticeBoardRepo>();

            container.RegisterType<Repository.ExaminationRepository.IRepository.IExamination_ReportRepo, Repository.ExaminationRepository.Repository.Examination_ReportRepo>();


            return container;
        }
    }
}