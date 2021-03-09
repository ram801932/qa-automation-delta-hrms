using System.ComponentModel;

namespace DeltaHRMS.Repository.PageFunctions
{
    public static class Constants
    {
        /// <summary>
        /// Modules Enum
        /// </summary>
        public enum Modules
        {
            [Description("DeltaHRMS")]
            DELTAHRMS
        }

        /// <summary>
        /// Pages Enum
        /// </summary>
        public enum PAGE
        {
            [Description("Generic")]
            GENERIC,
            [Description("Login")]
            LOGIN,
            [Description("Home")]
            HOME,
            [Description("Hr")]
            HR,
            [Description("SelfService")]
            SELFSERVICE,
            [Description("Appraisals")]
            APPRAISALS,
            [Description("Recruitments")]
            RECRUITMENTS,
            [Description("Organization")]
            ORGANIZATION

        }

        public enum GENERICOBJECTS
        {
            [Description("SideBarMenuHideShow")]
            SIDEBARMENUHIDESHOW,
            [Description("ShortcutButtons")]
            SHORTCUTBUTTONS,
            [Description("SideBarMenu")]
            SIDEBARMENUBTN,
            [Description("SideBarSubMenu")]
            SIDEBARSUBMENUBTN,
            [Description("DisplayedMessage")]
            DISPLAYEDMESSAGE,
            [Description("LoadingCircle")]
            LOADINGCIRCLE,
            [Description("SideBarMenuActive")]
            SIDEBARMENUACTIVE,
            [Description("SideBarMenuHidden")]
            SIDEBARMENUHIDDEN,
            [Description("CreateLeaveDatePicker")]
            DATEPICKER,
            [Description("CreateLeaveCalanderPrevMonth")]
            CALENDERPREVMONTH,
            [Description("CreateLeaveCalanderNextMonth")]
            CALANDERNEXTMONTH,
            [Description("CreateLeaveCalanderSelectMonth")]
            CALANDERSELECTMONTH,
            [Description("CreateLeaveCalanderSelectYear")]
            CALANDERSELECTYEAR,
            [Description("CreateLeaveCalanderTable")]
            CALANDERTABLE,
            [Description("CreateLeaveCalanderDates")]
            CALANDERDATES,
            [Description("CreateShortCutBtn")]
            CREATESHORTCUTBTN,
            [Description("SubmitButton")]
            SUBMITBUTTON,
            [Description("PopUpOkButton")]
            POPUPOKBUTTON
        }
        public enum LOGINOBJECTS
        {
            [Description("UserName")]
            USERNAME,
            [Description("Password")]
            PASSWORD,
            [Description("LoginSubmit")]
            LOGINSUBMIT
        }

        public enum HOMEOBJECTS
        {
            [Description("HomeMenuBar")]
            HOMEMENUBAR,
            [Description("NotificationAlertPopUp")]
            NOIFICATIONALERTPOPUP,
            [Description("NotificationAlertPopUpCloseBtn")]
            NOIFICATIONALERTPOPUPCLOSEBTN,
            [Description("NavToDashboardBtn")]
            NAVTODASHBOARDBTN,
            [Description("NavToSelfServiceBtn")]
            NAVTOSELFSERVICEBTN,
            [Description("NavToHRBtn")]
            NAVTOHRBTN,
            [Description("NavToAppraisalsBtn")]
            NAVTOAPPRAISALSBTN,
            [Description("NavToRecruitmentsBtn")]
            NAVTORECRUITMENTSBTN,
            [Description("NavToOrganizationBtn")]
            NAVTOORGANIZATIONBTN,
            [Description("DBViewMyTeamBtn")]
            DBVIEWMYTEAMBTN,
            [Description("DBMyTeamTotal")]
            DBMYTEAMTOTAL,
            [Description("DBMyTeamActive")]
            DBMYTEAMACTIVE,
            [Description("DBMyTeamInactive")]
            DBMYTEAMINACTIVE,
            [Description("DBAvailableLeaves")]
            DBAVAILABLELEAVES,
            [Description("DBApplyLeavebtn")]
            DBAPPLYLEAVEBTN,
            [Description("DBViewAllPendingLeavesBtn")]
            DBVIEWALLPENDINGLEAVESBTN,
            [Description("DBPendingLeaves")]
            DBPENDINGLEAVES,
            [Description("DBApproveLeavesBtn")]
            DBAPPROVELEAVESBTN,
            [Description("DBPendingLeavesForApproval")]
            DBPENDINGLEAVESFORAPPROVAL,
            [Description("DBViewHolidaysBtn")]
            DBVIEWHOLIDAYSBTN,
            [Description("DBViewHolidaysRows")]
            DBVIEWHOLIDAYSROWS,
            [Description("DBMyDetailsBtn")]
            DBMYDETAILSBTN,
            [Description("DBMyDetailsRows")]
            DBMYDETAILSROWS,
            [Description("CreateNewRequestBtn")]
            HOMEPAGECREATENEWREQUESTBTN,
            [Description("CreateNewRequestDropDownList")]
            HOMEPAGECREATENEWREQDROPDOWNLIST,
            [Description("UserProfileBtn")]
            HOMEPAGEUSERPROFILEBTN,
            [Description("UserProfileSubMenuBtns")]
            HOMEPAGEUSERPROFILESUBMENU

        }

        public enum HRPAGEOBJECTS
        {
            [Description("SelectSearchCategory")]
            SELECTSEARCHCATEGORY,
            [Description("SearchBtn")]
            SEARCHBTN,
            [Description("ClearSearchBtn")]
            CLEARSEARCHBTN,
            [Description("ViewMoreBtn")]
            VIEWMOREBTN,
            [Description("EmployeeName")]
            EMPLOYEENAME,
            [Description("EmployeeDesignation")]
            EMPLOYEEDESIGNATION,
            [Description("EmployeeDept")]
            EMPLOYEEDEPT,
            [Description("EmployeeId")]
            EMPLOYEEID,
            [Description("EmployeeEmail")]
            EMPLOYEEEMAIL,
            [Description("EmployeePhone")]
            EMPLOYEEPHONE,
            [Description("AddEmployeeBtn")]
            ADDEMPLOYEEBTN,
            [Description("ImportFormatBtn")]
            IMPORTFORMATBTN,
            [Description("ImportEmployeesBtn")]
            IMPORTEMPLOYEESBTN,
            [Description("HrAddEmployeeSubModules")]
            HRADDEMPLOYEESUBFIELDSMENU,
            [Description("OfficialTab")]
            OFFICIALTAB,
            [Description("OffEmpCode")]
            OFFEMPCODE,
            [Description("OffEmpId")]
            OFFEMPID,
            [Description("OffPrefix")]
            OFFPREFIX,
            [Description("OffSelectOptions")]
            OFFSELECTOPTIONS,
            [Description("OffFirstName")]
            OFFFIRSTNAME,
            [Description("OffLastName")]
            OFFLASTNAME,
            [Description("OffModeofEmployment")]
            OFFMODEOFEMPLOYMENT,
            [Description("OffRole")]
            OFFROLE,
            [Description("OffEmail")]
            OFFEMAIL,
            [Description("OffBusinessUnit")]
            OFFBUSINESSUNIT,
            [Description("OffDepartment")]
            OFFDEPARTMENT,
            [Description("OffReportingManager")]
            OFFREPORTINGMANAGER,

            [Description("HolidaysTab")]
            HOLIDAYSTAB,
            [Description("HdayHolidayGroup")]
            HDAYHOLIDAYGROUP,
            [Description("HdaySelectOptions")]
            HDAYSELECTOPTIONS,
            [Description("HdayHolidaysTable")]
            HDAYHOLIDAYSTABLE,
            [Description("HdaySaveBtn")]
            HDAYSAVEBTN,
            [Description("HdayCancelBtn")]
            HDAYCANCELBTN,

            [Description("ContactTab")]
            CONTACTTAB,
            [Description("CntPersonalEmail")]
            CNTPERSONALEMAIL,
            [Description("CntPermanentStreetAddress")]
            CNTPERMANENTSTREETADDRESS,
            [Description("CntPermanentCountry")]
            CNTPERMANENTCOUNTRY,
            [Description("CntSelectOptions")]
            CNTSELECTOPTIONS,
            [Description("CntPermanentState")]
            CNTPERMANENTSTATE,
            [Description("CntPermanentCity")]
            CNTPERMANENTCITY,
            [Description("CntPermanentPostalCode")]
            CNTPERMANENTPOSTALCODE,
            [Description("CntCheckbox")]
            CNTCHECKBOX,
            [Description("CntCurrentStreetAddress")]
            CNTCURRENTSTREETADDRESS,
            [Description("CntCurrentCountry")]
            CNTCURRENTCOUNTRY,
            [Description("CntCurrentState")]
            CNTCURRENTSTATE,
            [Description("CntCurrentCity")]
            CNTCURRENTCITY,
            [Description("CntCurrentPostalCode")]
            CNTCURRENTPOSTALCODE,
            [Description("CntName")]
            CNTNAME,
            [Description("CntEmail")]
            CNTEMAIL,
            [Description("CntNumber")]
            CNTNUMBER,
            [Description("CntSubmitBtn")]
            CNTSUBMITBTN,

            [Description("SkillsTab")]
            SKILLSTAB,
            [Description("SkillsTable")]
            SKILLSTABLE,
            [Description("SkillsAddBtn")]
            SKILLSADDBTN,
            [Description("SklSkill")]
            SKLSKILL,
            [Description("SklYearsOfExperience")]
            SKLYEARSOFEXPERIENCE,
            [Description("SklCompetencyLevel")]
            SKLCOMPETENCYLEVEL,
            [Description("SklSelectOptions")]
            SKLSELECTOPTIONS,
            [Description("SklSkillLastUsedYear")]
            SKLSKILLLASTUSEDYEAR,
            [Description("SklSaveBtn")]
            SKLSAVEBTN,
            [Description("SklCancelBtn")]
            SKLCANCELBTN,
            [Description("SklCloseBtn")]
            SKLCLOSEBTN,

            [Description("MedicalClaimsTab")]
            MEDICALCLAIMSTAB,
            [Description("MedicalClaimsTable")]
            MEDICALCLAIMSTABLE,
            [Description("MedicalClaimsAddBtn")]
            MEDICALCLAIMSADDBTN,
            [Description("MedicalClaimsType")]
            MEDICALCLAIMSTYPE,
            [Description("MedClaimsSelectOptions")]
            MEDCLAIMSSELECTOPTIONS,
            [Description("MedicalClaimsCloseBtn")]
            MEDICALCLAIMSCLOSEBTN,

            [Description("DependencyTab")]
            DEPENDENCYTAB,
            [Description("DependencyTable")]
            DEPENDENCYTABLE,
            [Description("DependencyAddBtn")]
            DEPENDENCYADDBTN,
            [Description("DpdDependentName")]
            DPDDEPENDENTNAME,
            [Description("DpdDependentRelation")]
            DPDDEPENDENTRELATION,
            [Description("DpdSelectOptions")]
            DPDSELECTOPTIONS,
            [Description("DpdDependentCustodyCode")]
            DPDDEPENDENTCUSTODYCODE,
            [Description("DpdDependentDOB")]
            DPDDEPENDENTDOB,
            [Description("DpdDependentAge")]
            DPDDEPENDENTAGE,
            [Description("DpdSaveBtn")]
            DPDSAVEBTN,
            [Description("DpdCancelBtn")]
            DPDCANCELBTN,
            [Description("DpdCloseBtn")]
            DPDCLOSEBTN,

            [Description("VisaImmigrationTab")]
            VISAIMMIGRATIONTAB,
            [Description("VisaImmigrationTable")]
            VISAIMMIGRATIONTABLE,
            [Description("VisaImmigrationAddBtn")]
            VISAIMMIGRATIONADDBTN,
            [Description("VisaImmPassportNumber")]
            VISAIMMPASSPORTNUMBER,
            [Description("VisaImmPassportIssueDate")]
            VISAIMMPASSPORTISSUEDATE,
            [Description("VisaImmPassportExpiryDate")]
            VISAIMMPASSPORTEXPIRYDATE,
            [Description("VisaImmVisaTypeCode")]
            VISAIMMVISATYPECODE,
            [Description("VisaImmVisaNumber")]
            VISAIMMVISANUMBER,
            [Description("VisaImmVisaIssueDate")]
            VISAIMMVISAISSUEDATE,
            [Description("VisaImmVisaExpiryDate")]
            VISAIMMVISAEXPIRYDATE,
            [Description("VisaImmI9Status")]
            VISAIMMI9STATUS,
            [Description("VisaImmI9ReviewDate")]
            VISAIMMI9REVIEWDATE,
            [Description("VisaImmIssuingAuthority")]
            VISAIMMISSUINGAUTHORITY,
            [Description("VisaImmI94Status")]
            VISAIMMI94STATUS,
            [Description("VisaImmI94ExpiryDate")]
            VISAIMMI94EXPIRYDATE,
            [Description("VisaImmSaveBtn")]
            VISAIMMSAVEBTN,
            [Description("VisaImmCancelBtn")]
            VISAIMMCANCELBTN,
            [Description("VisaImmCloseBtn")]
            VISAIMMCLOSEBTN,

            [Description("AdditonalDetailsTab")]
            ADDITONALDETAILSTAB,
            [Description("AdditonalDetailsTable")]
            ADDITONALDETAILSTABLE,
            [Description("AdditonalDetailsAddBtn")]
            ADDITONALDETAILSADDBTN,
            [Description("AddDtlServedinMilitary")]
            ADDDTLSERVEDINMILITARY,
            [Description("AddDtlSelectOptions")]
            ADDDTLSELECTOPTIONS,
            [Description("AddDtlCountriesServed")]
            ADDDTLCOUNTRIESSERVED,
            [Description("AddDtlBranchofService")]
            ADDDTLBRANCHOFSERVICE,
            [Description("AddDtlRankAchieved")]
            ADDDTLRANKACHIEVED,
            [Description("AdDtlFrom")]
            ADDTLFROM,
            [Description("AddDtlTo")]
            ADDDTLTO,
            [Description("AddDtlSpecialTrainings")]
            ADDDTLSPECIALTRAININGS,
            [Description("AddDtlAwardsHonorsRecieved")]
            ADDDTLAWARDSHONORSRECIEVED,
            [Description("AddDtlStatusofDischarge")]
            ADDDTLSTATUSOFDISCHARGE,
            [Description("AddDtlMilitaryServiceNumber")]
            ADDDTLMILITARYSERVICENUMBER,
            [Description("AddDtlCurrentEndingRank")]
            ADDDTLCURRENTENDINGRANK,
            [Description("AddDtlMilitaryVerificationReport")]
            ADDDTLMILITARYVERIFICATIONREPORT,
            [Description("AddDtlMilitaryServiceType")]
            ADDDTLMILITARYSERVICETYPE,
            [Description("AddDtlVeteranStatus")]
            ADDDTLVETERANSTATUS,
            [Description("AddDtlSaveBtn")]
            ADDDTLSAVEBTN,
            [Description("AddDtlCancelBtn")]
            ADDDTLCANCELBTN,
            [Description("AddDtlCloseBtn")]
            ADDDTLCLOSEBTN,

            [Description("OffJobtitle")]
            OFFJOBTITLE,
            [Description("OffEmpStatus")]
            OFFEMPSTATUS,
            [Description("Offdateofjoining")]
            OFFDATEOFJOINING,
            [Description("dateofconfo")]
            DATEOFCONFO,
            [Description("dateofleaving")]
            DATEOFLEAVING,
            [Description("Yearsofexperience")]
            YEARSOFEXPERIENCE,
            [Description("WorkTelPhonenum")]
            WORKTELPHONENUM,
            [Description("Extension")]
            EXTENSION,
            [Description("Fax")]
            FAX,
            [Description("OffSaveBtn")]
            OFFSAVEBTN,
            [Description("OffCancelBtn")]
            OFFCANCELBTN,
            [Description("Docname")]
            DOCNAME,
            [Description("UploadAttachment")]
            UPLOADATTACHMENT,
            [Description("Leavecancelbtn")]
            LEAVECANCELBTN,
            [Description("Leavesavebtn")]
            LEAVESSAVEBTN,
            [Description("Leave")]
            LEAVE,
            [Description("PersonalGender")]
            PERSONALGENDER,
            [Description("PersonalGenderSelect")]
            PERSONALGENDERSELECT,
            [Description("PersonalMartialstatus")]
            PERSONALMARTIALSTATUS,
            [Description("PersonalMartialSelect")]
            PERSONALMARTIALSELECT,
            [Description("PersonalNationality")]
            PERSONALNATIONALITY,
            [Description("PersonalEthiniccode")]
            PERSONALETHINICCODE,
            [Description("Personalraceccode")]
            PERSONALRACECCODE,
            [Description("PersonalLanguage")]
            PERSONALLANGUAGE,
            [Description("PersonalDateofbirth")]
            PERSONALDATEOFBIRTH,
            [Description("PersonalBloodgroup")]
            BLOODGROUP,
            [Description("JobHistoryDepartment")]
            JOBHISTORYDEPARTMENT,
            [Description("JobHistorySelect")]
            JOBHISTORYSELECT,
            [Description("JobHistoryJobtittle")]
            JOBHISTORYJOBTITTLE,
            [Description("JobHistorySelectposition")]
            JOBHISTORYSELECTPOSITION,
            [Description("JobHistoryFromdate")]
            JOBHISTORYFROMDATE,
            [Description("JobHistoryTodate")]
            JOBHISTORYTODATE,
            [Description("JobHistoryClient")]
            JOBHISTORYCLIENT,
            [Description("JobHistoryVendor")]
            JOBHISTORYVENDOR,
            [Description("JobHistoryAmntreceived")]
            JOBHISTORYAMNTRECEIVED,
            [Description("JobHistoryAmntpaid")]
            JOBHISTORYAMNTPAID,
            [Description("ExperienceCompname")]
            EXPERIENCECOMPNAME,
            [Description("ExperienceCompwebsite")]
            EXPERIENCECOMPWEBSITE,
            [Description("ExperienceDesignation")]
            EXPERIENCEDESIGNATION,
            [Description("ExperienceFromdate")]
            EXPERIENCEFROMDATE,
            [Description("ExperienceTodate")]
            EXPERIENCETODATE,
            [Description("ExperienceReasonforleaving")]
            EXPERIENCEREASONFORLEAVING,
            [Description("ExperienceReferrername")]
            EXPERIENCEREFERRERNAME,
            [Description("ExperienceReferenceContact")]
            EXPERIENCEREFERENCECONTACT,
            [Description("ExperienceFererenceEmail")]
            EXPERIENCEFERERENCEEMAIL,
            [Description("EducationLevel")]
            EDUCATIONLEVEL,
            [Description("EducationInstititutionName")]
            EDUCATIONINSTITITUTIONNAME,
            [Description("EducationCourse")]
            EDUCATIONCOURSE,
            [Description("EducationFromDate")]
            EDUCATIONFROMDATE,
            [Description("EducationToDate")]
            EDUCATIONTODATE,
            [Description("EducationPercentage")]
            EDUCATIONPERCENTAGE,
            [Description("TrainingCertificationCoursename")]
            TRAININGCERTIFICATIONCOURSENAME,
            [Description("TrainingCertificationCourseLevel")]
            TRAININGCERTIFICATIONCOURSELEVEL,
            [Description("TrainingCertificationCourseOffBy")]
            TRAINING_CERTIFICATIONCOURSEOFFBY,
            [Description("TrainingCertificationDescription")]
            TRAININGCERTIFICATIONDESCRIPTION,
            [Description("TrainingCertificationCerfName")]
            TRAININGCERTIFICATIONCERFNAME,
            [Description("TrainingCertificationIssuedDate")]
            TRAININGCERTIFICATIONISSUEDDATE,
            [Description("OffModeofEmploymentOptions")]
            OFFMODEOFEMPLOYMENTOPTIONS,
            [Description("OffRoleOptions")]
            OFFROLEOPTIONS,
            [Description("OffBusinessUnitOptions")]
            OFFBUSINESSUNITOPTIONS,
            [Description("OffDepartmentOptions")]
            OFFDEPARTMENTOPTIONS,
            [Description("OffReportingManagerOptions")]
            OFFREPORTINGMANAGEROPTIONS,
            [Description("OffEmpStatusOptions")]
            OFFEMPSTATUSOPTIONS,
            [Description("PersonalNationalityOptions")]
            SELECTNATIONALITYOPTIONS,
            [Description("PersonalEthiniccodeOptions")]
            SELECTETHINICCODEOPTIONS,
            [Description("PersonalraceccodeOptions")]
            PERSONALRACECODEOPTIONS,
            [Description("PersonalLanguageOptions")]
            PERSONALLANGUAGEOPTIONS,
            [Description("PersonalSaveBtn")]
            PERSONALSAVEBTN,
            [Description("CntPermanentStateOptions")]
            CNTPERMANENTSTATEOPTIONS,
            [Description("CntPermanentCityOptions")]
            CNTPERMANENTCITYOPTIONS,
            [Description("ExperienceAddBtn")]
            EXPERIENCEADDBTN,
            [Description("ExperienceSaveBtn")]
            EXPERIENCESAVEBTN,
            [Description("EducationLevelOptions")]
            EDUCATIONLEVELOPTIONS,
            [Description("EducationSavebtn")]
            EDUCATIONSAVEBTN,
            [Description("EducationAddBtn")]
            EDUCATIONADDBTN,
            [Description("SearchValueField")]
            SEARCHVALUEFIELD,
            [Description("SearchSelectRole")]
            SEARCHSELECTROLE,
            [Description("AddHolidayGroupBtn")]
            ADDHOLIDAYGROUPBTN,
            [Description("AddHolidayGrpGroupName")]
            ADDHOLIDAYGRPGROUPNAME,
            [Description("AddHolidayGrpDescription")]
            ADDHOLIDAYGRPDESCRIPTION,
            [Description("AddHolidayGrpSaveBtn")]
            ADDHOLIDAYGRPSAVEBTN,
            [Description("ManageHolidayGroupTable")]
            MANAGEHOLIDAYGROUPTABLE,
            [Description("ManageHolidaysTable")]
            MANAGEHOLIDAYSTABLE,
            [Description("ManageHolidaysAddBtn")]
            MANAGEHOLIDAYSADDBTN,
            [Description("ManageHolidaysAddHolidayName")]
            MANAGEHOLIDAYSADDHOLIDAYNAME,
            [Description("ManageHolidaysAddHolidayDate")]
            MANAGEHOLIDAYSADDHOLIDAYDATE,
            [Description("AddHolidayDescription")]
            ADDHOLIDAYDESCRIPTION,
            [Description("AddHolidayGroup")]
            ADDHOLIDAYGROUP,
            [Description("AddHolidayGroupOptions")]
            ADDHOLIDAYGROUPOPTIONS,
            [Description("AddHolidaySaveBtn")]
            ADDHOLIDAYSAVEBTN,
            [Description("EmployeeActionsBtn")]
            EMPLOYEEACTIONSBTN,
            [Description("EmployeeActionsEdit")]
            EMPLOYEEACTIONSEDIT,
            [Description("EmployeeActionsView")]
            EMPLOYEEACTIONSVIEW,
            [Description("EmployeeViewFirstName")]
            EMPLOYEEVIEWFIRSTNAME,
            [Description("ManageHolidaysConfirmationYes")]
            MANAGEHOLIDAYCONFIRMATIONYES,
            [Description("AddHolidayName")]
            ADDHOLIDAYNAME,
            [Description("AddHolidayDateBtn")]
            ADDHOLIDAYDATEBTN,
            [Description("ManageHolidaysLastPageBtn")]
            MANAGEHOLIDAYSLASTPAGEBTN,
            [Description("LeaveManagementOptionsTable")]
            LEAVEMANAGEMENTOPTIONSTABLE,
            [Description("LeaveManagementOptionsLastPage")]
            LEAVEMANAGEMENTOPTINSLASTPAGE,
            [Description("LeaveManagementAddBussUnitName")]
            LEAVEMANAGEMENTADDBUSSUNITNAME,
            [Description("LeaveManagementAddDepartment")]
            LEAVEMANAGEMENTADDDEPARTMENT,
            [Description("LeaveManagementAddCalenderStartMnth")]
            LEAVEMANAGEMENTADDCALENDERSTARTMONTH,
            [Description("LeaveManagementAddWeekendDay1")]
            LEAVEMANAGEMENTADDWEEKENDDAY1,
            [Description("LeaveManagementAddWeekendDay2")]
            LEAVEMANAGEMENTADDWEEKENDDAY2,
            [Description("LeaveManagementAddHalfDayRequests")]
            LEAVEMANAGEMENTADDHALFDAYREQUESTS,
            [Description("LeaveManagementAddAllowLeaveTransfers")]
            LEAVEMANAGEMENTADDALLOWLEAVETRANSFERS,
            [Description("LeaveManagementAddSkipHolidays")]
            LEAVEMANAGEMENTADDSKIPHOLIDAYS,
            [Description("LeaveManagementAddHrManager")]
            LEAVEMANAGEMENTADDHRMANAGER,
            [Description("LeaveManagementAddDescription")]
            LEAVEMANAGEMENTADDDESCRIPTION,
            [Description("LeaveManagementAddWorkingHrs")]
            LEAVEMANGEMENTADDWORKINGHRS,
            [Description("LeaveManagementAddSubmitBtn")]
            LEAVEMANAGEMENTADDSUBMITBTN,
            [Description("LeaveManagementOptionsNextPage")]
            LEAVEMANAGEMENTOPTIONSNEXTPAGE,
            [Description("ManageHolidaysNextPageBtn")]
            MANAGEHOLDIAYSNEXTPAGEBTN,
            [Description("JobTitlesAddBtn")]
            JOBTITLESADDBTN,
            [Description("JobTitlesTable")]
            JOBTITLESTABLE,
            [Description("JobTitlesAddJobTitleCode")]
            JOBTITLESADDJOBTITLECODE,
            [Description("JobTitlesAddJobTitle")]
            JOBTITLESADDJOBTITLE,
            [Description("JobTitlesAddJobDescription")]
            JOBTITLESADDJOBDESCRIPTION,
            [Description("JobTitlesAddJobMinExpRequired")]
            JOBTITLESADDJOBMINEXPREQUIRED,
            [Description("JobTitlesAddJobPayGrade")]
            JOBTITLESADDJOBPAYGRADE,
            [Description("JobTitlesAddJobPayFrequency")]
            JOBTITLESADDJOBPAYFREQUENCY,
            [Description("JobTitlesAddJobComments")]
            JOBTITLESADDJOBCOMMENTS,
            [Description("JobTitlesAddJobsubmitBtn")]
            JOBTITLESADDJOBSUBMITBTN,
            [Description("PositionsAddBtn")]
            POSITIONSADDBTN,
            [Description("PositionsTable")]
            POSITIONSTABLE,
            [Description("PositionsTableLastPage")]
            POSITIONSTABLELASTPAGE,
            [Description("PositionsTableNextPage")]
            POSITIONSTABLENEXTPAGE,
            [Description("PositionsAddJobTitle")]
            POSITIONSADDJOBTITLE,
            [Description("PositionsAddPositionName")]
            POSITIONSADDPOSITIONNAME,
            [Description("PositionsAddPositionDescription")]
            POSITIONSADDDESCRIPTION,
            [Description("JobTitlesAddPositionSubmitBtn")]
            POSITIONSADDSUBMITBTN,
            [Description("RolesPrivilegesAddCheckBoxes")]
            ROLESPRIVILEGESADDCHECKBOXES,
            [Description("RolesPrivilegesAddRolesSideBarSelection")]
            ROLESPRIVILEGESSIDEBARSELECTION,
            [Description("RolesPrivilegesAddSubmitBtn")]
            ROLESPRIVILEGESADDSUBMITBTN,
            [Description("RolesPrivilegesAddBtn")]
            ROLESPRIVILEGESADDBTN,
            [Description("RolesPrivilegesTable")]
            ROLESPRIVILEGESTABLE,
            [Description("RolesPrivilegesAddRoleName")]
            ROLESPRIVILEGESADDROLENAME,
            [Description("RolesPrivilegesAddRoleType")]
            ROLESPRIVILEGESADDROLETYPE,
            [Description("RolesPrivilegesAddRoleDescription")]
            ROLESPRIVILEGESADDROLEDESCRIPTION
        }

        public enum SELFSERVICEOBJECTS
        {
            [Description("LeaveRequestApplyLeaveButton")]
            APPLYLEAVEBUTTON,
            [Description("CreateLeaveRequestLeaveType")]
            REQUESTLEAVETYPE,
            [Description("CreateLeaveSelectLeaveType")]
            SELECTREQUESTLEAVETYPE,
            [Description("CreateLeaveCompOffDate")]
            COMPOFFDATE,
            [Description("CreateLeaveFromDate")]
            LEAVEFROMDATE,
            [Description("CreateLeaveToDate")]
            LEAVETODATE,
            [Description("CreateLeaveLeaveForField")]
            LEAVEFORFIELD,
            [Description("CreateLeaveSelectLeaveFor")]
            SELECTLEAVEFOR,
            [Description("CreateLeaveLeavesDaysCount")]
            LEAVESDAYS,
            [Description("CreateLeaveReportingManager")]
            REPORTINGMANAGER,
            [Description("CreateLeaveReason")]
            LEAVEREASON,
            [Description("CreateLeaveSubmitBtn")]
            SUBMITBTN,
            [Description("CreateLeaveCancelBtn")]
            CANCELBTN,
            [Description("CreateLeaveRequestCloseBtn")]
            CREATELEAVECLOSEBTN,
            [Description("MyLeaveAllLeaves")]
            MYLEAVESALLLEAVES,
            [Description("MyLeavePendingLeaves")]
            MYLEAVESPENDINGLEAVES,
            [Description("MyLeaveCancelledLeaves")]
            MYLEAVESCANCELLEDLEAVES,
            [Description("MyLeaveApprovedLeaves")]
            MYLEAVESAPPROVEDLEAVES,
            [Description("MyLeaveRejectedLeaves")]
            MYLEAVESREJECTEDLEAVES,
            [Description("MyLeavePendingLeavesTable")]
            MYLEAVEPENDINGLEAVESTABLE,
            [Description("MyLeaveAllotmentLeaveTable")]
            MYLEAVEALLOTMENTLEAVETABLE,
            [Description("EmployeeLeaveTable")]
            EMPLOYEELEAVETABLE,
            [Description("MyLeaveCancelPopUpYes")]
            MYLEAVECANCELPOPUPYES,
            [Description("CreateLeaveErrorLeaveType")]
            LEAVETYPEERROR,
            [Description("EmployeeLeaveEditStatus")]
            EMPLEAVEEDITSTATUS,
            [Description("EmployeeLeaveEditStatusSelect")]
            EMPLEAVEEDITSTATUSSELECT,
            [Description("EmployeeLeaveEditStatusSelectOptions")]
            EMPLEAVEEDITSTATUSSELECTOPTIONS,
            [Description("EmployeeLeaveEditCommentsText")]
            EMPLEAVEEDITCOMMENTSTEXT,
            [Description("EmployeeLeaveEditSave")]
            EMPLEAVEEDITSAVE,
            [Description("EmployeeLeaveEditCancel")]
            EMPLEAVEEDITCANCEL,
            [Description("EmployeeLeaveNextbtn")]
            EMPLOYEELEAVENEXTBTN,
            [Description("EmployeeLeaveLastPagebtn")]
            EMPLOYEELEAVELASTPAGEBTN,
            [Description("ImpersonateLeaveSelectEmployee")]
            IMPERSONATELEAVESELECTEMPLOYEE,
            [Description("ImpersonateLeaveSelectEmployeeSelect")]
            IMPERSONATELEAVESELECTEMPLOYEESELECT,
            [Description("MyDetailsEmpCode")]
            MYDETAILSEMPCODE,
            [Description("MyDetailsCandidateName")]
            MYDETAILSCANDIDATENAME,
            [Description("MyDetailsBusinessUnit")]
            MYDETAILSBUSINESSUNITNAME,
            [Description("MyDetailsReportingManager")]
            MYDETAILSREPORTINGMANAGER,
            [Description("MyDetailsEmploymentStatus")]
            MYDETAILSEMPSTATUS,
            [Description("MyDetailsDateOfJoining")]
            MYDETAILSDATEOFJOINING,
            [Description("MyDetailsEmail")]
            MYDETAILSEMAIL,
            [Description("MyDetailsPosition")]
            MYDETAILSPOSITION,
            [Description("MyDetailsOfficialTab")]
            MYDETAILSOFFICIALTAB,
            [Description("MyDetailsJobTitle")]
            MYDETAILSJOBTITLE,
            [Description("WebCheckInTable")]
            WEBCHECKINTABLE,
            [Description("WebCheckInDate")]
            WEBCHECKINDATE,
            [Description("WebCheckInCheckInCheckOutBtn")]
            WEBCHECKINCHECKOUTBTN,
            [Description("WebCheckInTimer")]
            WEBCHECKINTIMER,
            [Description("MyRegularizationRegularizationDay")]
            REGULARIZATIONDAY,
            [Description("MyRegularizationCheckInTime")]
            MYREGULARIZATIONCHECKINTIME,
            [Description("MyRegularizationCheckOutDay")]
            MYREGULARIZATIONCHECKOUTDAY,
            [Description("MyRegularizationCheckOutTime")]
            MYREGULARIZATIONCHECKOUTTIME,
            [Description("MyRegularizationDescription")]
            MYREGULARIZATIONDESCRIPTION,
            [Description("MyRegularizationHours")]
            MYREGULARIZATIONHOURS,
            [Description("MyRegularizationSubmitBtn")]
            MYREGULARIZATIONSUBMITBTN,
            [Description("MyRegularizationReqTable")]
            MYREGULARIZATIONREQUESTTABLE,
            [Description("MyRegularizationTimeHours")]
            MYREGULARIZATIONTIMEHOURS,
            [Description("MyRegularizationTimeMinutes")]
            MYREGULARIZATIONTIMEMINUTES,
            [Description("EmployeeRegularizationRequestTable")]
            EMPLOYEEREGULARIZATIONREQUESTTABLE,
            [Description("EmployeeRegularizationEditStatusField")]
            EMPREGULARIZATIONEDITSTATUSFIELD,
            [Description("EmployeeRegularizationEditStatusSelectOptions")]
            EMPREGULARIZATIONEDITSTATUSSELECTOPTIONS,
            [Description("EmployeeRegularizationSaveBtn")]
            EMPREGULARIZATIONSAVEBTN,
            [Description("AssignWebCheckInSelectEmpName")]
            ASSIGNWEBCHECKINSELECTEMPNAME,
            [Description("AssignWebCheckInSelectBy")]
            ASSIGNWEBCHECKINSELECTBY,
            [Description("AssignWebCheckInStatus")]
            ASSIGNWEBCHECKINSTATUS,
            [Description("AssignWebCheckInSubmitBtn")]
            ASSIGNWEBCHECKINSUBMITBTN


        }

        /// <summary>
        /// Constants for Leave types
        /// </summary>
        public enum LEAVETYPES
        {
            [Description("Bereavement Leave")]
            BEREAVEMENTLEAVE,
            [Description("Earned Leave")]
            EARNEDLEAVE,
            [Description("Paternity Leave")]
            PATERNITYLEAVE,
            [Description("Sick Leave/Casual Leave")]
            SICKCASUALLEAVE,
            [Description("Compensatory Off")]
            COMPOFF,
            [Description("Maternity Leave")]
            MATERNITYLEAVE
        }

        public enum APPRAISALPAGEOBJECTS
        {
            [Description("AppaddBtn")]
            APPADDBTN,
            [Description("Step1Initlize")]
            STEP1INITILIZE,
            [Description("AppSelBussUnitDrpDwn")]
            APPSELBUSSUNITDRPDWN,
            [Description("AppSelBussUnit")]
            APPSELBUSSUNIT,
            [Description("AppSelDepDrpDwn")]
            APPSELDEPDRPDWN,
            [Description("AppSelDep")]
            APPSELDEP,
            [Description("AppYrRngFrmDrpDwn")]
            APPYRRNGFRMDRPDWN,
            [Description("AppYrRngFrm")]
            APPYRRNGFRM,
            [Description("AppYrRngToDrpDwn")]
            APPYRRNGTODRPDWN,
            [Description("AppYrTo")]
            APPYRTO,
            [Description("AppModeDrpDwn")]
            APPMODEDRPDWN,
            [Description("AppModeList")]
            APPMODELIST,
            [Description("AppPeriod")]
            APPPERIOD,
            [Description("AppEnableToDrpDwn")]
            APPENABLETODRPDWN,
             [Description("AppStatusDrpDwn")]
            APPSTATUSDRPDWN,
            [Description("AppStatusList")]
            APPSTATUSLIST,
            
            [Description("AppEnableToList")]
            APPENABLETOLIST,
            [Description("AppMngrDueDate")]
            APPMNGRDUEDATE,
            [Description("AppEmployeeDueDate")]
            APPEMPLOYEERDUEDATE,
            
            [Description("AppElgibltyDrpDwn")]
            APPELGIBLTYDRPDWN,
            [Description("AppElgibltyList")]
            APPELGIBLTYLIST,
            
            [Description("AppElgibltySelectAll")]
            APPELGIBLTYSELECTALL,

            [Description("AppParameters")]
            APPPARAMETERS,
            [Description("AppRatingsDrpDwn")]
            APPRATINDSDRPDWN,
            [Description("AppRatingsDrpDwnList")]
            APPRATINDSDRPDWNLIST,
            [Description("AppSaveBtn")]
            APPSAVEBTN,
            [Description("Step2ConfLinMngr")]
            STEP2CONFLINMNGR,
            [Description("AppDetls")]
            APPDETLS,
            [Description("ChoseByOrgHierchy")]
            CHOSEBYORGHIERCHY,
            [Description("ConfPopUp")]
            CONFPOPUP,
            [Description("L1MngrTbl")]
            L1MNGRTBL,
            [Description("SaveBtn")]
            SAVEBTN,
            [Description("Step3ConfgAppParm")]
            STEP3CONFGAPPPARM,
            [Description("AllEmplyessBtn")]
            ALLEMPLYESSBTN,
            [Description("AllQustnTbl")]
            ALLQUSTNTBL,
            [Description("SubmitandInitlizeBtn")]
            SUBMITANDINITLIZEBTN,
            [Description("AppraisalParametersAdd")]
            APPRAISALPARAMETERSADD,
            [Description("AppraisalParametersParameter")]
            APPRAISALPARAMETERSPARAMETER,
            [Description("AppraisalParametersDescription")]
            APPRAISALPARAMETERSDESCRIPTION,
            [Description("AppraisalParametersWeightage")]
            APPRAISALPARAMETERSWEIGHTAGE,
            [Description("AppraisalParameterssave")]
            APPRAISALPARAMETERSSAVE,
            [Description("AppraisalParametersTable")]
            APPRAISALPARAMETERSTABLE,
            [Description("AppraisalParametersCancel")]
            APPRAISALPARAMETERSCANCEL,
            [Description("AppraisalQuestionsAddSelectParam")]
            APPRAISALQUESTIONSADDSELECTPARAM,
            [Description("AppraisalQuestionsAddSelectParamOption")]
            APPRAISALQUESTIONSADDSELECTPARAMOPTION,
            [Description("AppraisalQuestionsQuestion")]
            APPRAISALQUESTIONSQUESTION,
            [Description("AppraisalQuestionsDescription")]
            APPRAISALQUESTIONSDESCRIPTION,
            [Description("AppraisalQuestionsAddQuesBtn")]
            APPRAISALQUESTIONSADDQUESBTN,
            [Description("AppraisalQuestionsAddQuesQuestion")]
            APPRAISALQUESTIONSADDQUESQUESTION,
            [Description("AppraisalQuestionsAddQuesDescription")]
            APPRAISALQUESTIONSADDQUESDESCRIPTION,
            [Description("AppraisalQuestionsSave")]
            APPRAISALQUESTIONSSAVE, 
            [Description("AppraisalQuestionsTable")]
            APPRAISALQUESTIONSTABLE,
            [Description("Appraisal_Questions_Cancel")]
            APPRAISAL_QUESTIONS_CANCEL,
            [Description("SkillsTable")]
            SKILLSTABLE,
            [Description("SkillsAddBtn")]
            SKILLSADDBTN,
            [Description("SkillsAddSkill")]
            SKILLSADDSKILL,
            [Description("SkillsAddDescription")]
            SKILLSADDDESCRIPTION,
            [Description("SkillsAddSaveBtn")]
            SKILLSADDSAVEBTN,
            [Description("SkillsAddCancelBtn")]
            SKILLSADDCANCELBTN,
            [Description("RatingsTable")]
            RATINGSTABLE,
            [Description("RatingsAddBtn")]
            RATINGSADDBTN,
            [Description("RatingsAddBusinessUnit")]
            RATINGSADDBUSINESSUNIT,
            [Description("RatingsAddSelectOptions")]
            RATINGSADDSELECTOPTIONS,
            [Description("RatingsAddDepartment")]
            RATINGSADDDEPARTMENT,
            [Description("EmployeeSelfAppraisalsParametersList")]
            EMPLOYEESELFAPPRAISALSPARAMETERSLIST,
            [Description("EmployeeSelfAppraisalsSendToL1ManagerBtn")]
            EMPLOYEESELFAPPRAISALSSENDTOL1MANAGERBTN,
            [Description("ManagerAppraisalRatingExpandBtn")]
            MANAGERAPPRAISALRATINGEXPANDBTN,
            [Description("ManagerAppraisalsParametersList")]
            MANAGERAPPRAISALSPARAMETERSLIST,
            [Description("ManagerAppraisalsOverallRating")]
            MANAGERAPPRAISALSOVERALLRATING,
            [Description("ManagerAppraisalsSubmitBtn")]
            MANAGERAPPRAISALSSUBMITBTN,
            [Description("AppMangSearchEmp")]
            APPMANGSEARCHEMP,
            [Description("AppMangSearchGoBtn")]
            APPMANGSEARCHGOBTN,

            [Description("ManagerAppraisalsPrintBtn")]
            MANAGERAPPRAISALSPRINTBTN,
            [Description("ConfAppModeOptions")]
            CONFAPPMODEOPTIONS
            


        }
        /// <summary>
        /// Constants for Recruitments
        /// </summary>
        public enum RECRUITMENTSPAGEOBJECTS
        {
            [Description("RecruitOpenaddBtn")]
            RECRUITOPENADDBTN,
            [Description("RecruitOpenBusinessUnitDrpDwn")]
            RECRUITOPENBUSINESSUNITDRPDWN,
            [Description("RecruitOpenDepDrpDwn")]
            RECRUITOPENDEPDRPDWN,
            [Description("RecruitOpenRepMngr")]
            RECRUITOPENREPMNGR,
            [Description("RecruitOpenApprove1")]
            RECRUITOPENAPPROVER1,
            [Description("RecruitOpenDueDate")]
            RECRUITOPENDUEDATE,
            [Description("RecruitOpenJobTitle")]
            RECRUITOPENJOBTITLE,
            [Description("RecruitOpenPosition")]
            RECRUITOPENPOSITION,
            [Description("RecruitOpenNoofPostn")]
            RECRUITOPENNOOPOSTN,
            [Description("RecruitOpenBillingType")]
            RECRUITOPENBILLINGTYPE,
            [Description("RecruitOpenPositionType")]
            RECRUITOPENPOSITIONTYPE,
            [Description("RecruitOpenMinReqExp")]
            RECRUITOPENMINREQEXP,
            [Description("RecruitOpenEmpStatus")]
            RECRUITOPENEMPSTATUS,
            [Description("RecruitOpenUploadFile")]
            RECRUITOPENUPLOADFILE,
            [Description("RecruitOpenSubBtn")]
            RECRUITOPENSUBBTN,
            [Description("RecruitCandAddBtn")]
            RECRUITCANDADDBTN,
            [Description("RecruitCandReqIdDrpDwn")]
            RECRUITCANDREQIDDRPDWN,
            [Description("RecruitCandFirstName")]
            RECRUITCANDFIRSTNAME,
            [Description("RecruitCandLastName")]
            RECRUITCANDLASTNAME,
            [Description("RecruitCandSource")]
            RECRUITCANDSOURCE,
            [Description("RecruitCandEmail")]
            RECRUITCANDEMAIL,
            [Description("RecruitCandContNum")]
            RECRUITCANDCONTNUM,
            [Description("RecruitCandSkillSet")]
            RECRUITCANDSKILLSET,
            [Description("RecruitCandSav")]
            RECRUITCANDSAV,
            [Description("RecruitCandSavSchd")]
            RECRUITCANDSAVSCHD,
            [Description("RecruitInterviewAddBtn")]
            RECRUITINTERVIEWADDBTN,
            [Description("RecruitInterviewCandNam")]
            RECRUITINTERVIEWCANDNAM,
            [Description("RecruitInterviewLocation")]
            RECRUITINTERVIEWLOCATION,
            [Description("RecruitInterviewType")]
            RECRUITINTERVIEWTYPE,
            [Description("RecruitInterviewDate")]
            RECRUITINTERVIEWDATE,
            [Description("RecruitInterviewName")]
            RECRUITINTERVIEWNAME,
            [Description("RecruitInterviewSubmitBtn")]
            RECRUITINTERVIEWSUBMITBTN,
            [Description("RecruitInterviewRoundActnEdit")]
            RECRUITINTERVIEWROUNDACTNEDIT,
            [Description("RecruitInterviewRoundFeedback")]
            RECRUITINTERVIEWROUNDFEEDBACK,
            [Description("RecruitInterviewRoundStatus")]
            RECRUITINTERVIEWROUNDSTATUS,
            [Description("RecruitInterviewUpdateBtn")]
            RECRUITINTERVIEWUPDATEBTN,
            [Description("RecruitInterviewSelectDrpDwn")]
            RECRUITINTERVIEWSELECTDRPDWN,
            [Description("RecruitInterviewSchdulNexRound")]
            RECRUITINTERVIEWSCHDULNEXTROUND,
            [Description("RecruitAddRequisitionCode")]
            RECRUITADDREQUISITIONCODE,
            [Description("RecruitmentOpeningsPositionsTable")]
            RECRUITMENTOPENINGSPOSITIONSTABLE,
            [Description("RecruitmentOpeningsPositionsApproverStatus")]
            RECRUITMENTSOPENINGSPOSITIONSAPPROVERSTATUS,
            [Description("RecruitInterviewAddReqId")]
            RECRUITINTERVIEWADDREQID,
            [Description("RecruitmentInterviewsTable")]
            RECRUITMENTINTERVIEWSTABLE,
            [Description("RecruitmentInterviewRoundsTable")]
            RECRUITMENTINTERVIEWROUNDSTABLE,
            [Description("RecruitmentInterviewDetailsStatus")]
            RECRUITMENTINTERVIEWDETAILSSTATUS,
            [Description("RecruitmentInterviewDetailsCandidateStatus")]
            RECRUITMENTINTERVIEWDETAILSCANDIDATESTATUS,
            [Description("RecruitInterviewRoundInterviewerComments")]
            RECRUITINTERVIEWROUNDINTERVIEWERCOMMENTS,
            [Description("RecruitmentShortlistedSelectedCandidatesTable")]
            SHORTLISTEDSELECTEDCANDIDATESTABLE,
            [Description("RecruitmentShortlistedSelectedCandidatesCandidateStatus")]
            SHORTLISTEDSELECTEDCANDIDATESCANDIDATESELCIONSTATUS,
            [Description("RecruitmentAddCandidatesEnterCandateDetails")]
            ADDCANDIDATEDETAILSENTERCANDIDATEDETAILS,
            [Description("RecruitmentAddCandidatesEnterCandateDetailsQualification")]
            ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSQUALIFICATION,
            [Description("RecruitmentAddCandidatesEnterCandateDetailsExperince")]
            ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSEXPERINCE,
            [Description("RecruitmentAddCandidatesEnterCandateDetailsCustLocation")]
            ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCUSTLOCATION,
            [Description("RecruitmentAddCandidatesEnterCandateDetailsCountry")]
            ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCOUNTRY,
            [Description("RecruitmentAddCandidatesEnterCandateDetailsState")]
            ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSSTATE,
            [Description("RecruitmentAddCandidatesEnterCandateDetailsCity")]
            ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCITY,
            [Description("RecruitmentAddCandidatesEnterCandateDetailsPostalCode")]
            ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSPOSTALCODE

        }

        public enum ORGANIZATIONPAGEOBJECTS
        {
            [Description("BusinessUnitsTable")]
            BUSINESSUNITSTABLE,
            [Description("AddBusinessUnitBtn")]
            ADDBUSINESSUNITBTN,
            [Description("AddBusinessUnitName")]
            ADDBUSINESSUNITNAME,
            [Description("AddBusinessUnitUnitCode")]
            ADDBUSINESSUNITUNITCODE,
            [Description("AddBusinessUnitDescription")]
            ADDBUSINESSUNITDESCRIPTION,
            [Description("AddBusinessUnitStartedOn")]
            ADDBUSINESSUNITSTARTEDON,
            [Description("AddBusinessUnitTimeZone")]
            ADDBUSINESSUNITTIMEZONE,
            [Description("AddBusinessUnitCountry")]
            ADDBUSINESUNITCOUNTRY,
            [Description("AddBusinessUnitState")]
            ADDBUSINESSUNITSTATE,
            [Description("AddBusinessUnitCity")]
            ADDBUSINESSUNITCITY,
            [Description("AddBusinessUnitStreetAdd1")]
            ADBUSINESSUNTISTREETADD1,
            [Description("DepartmentsTable")]
            DEPARTMENTSTABLE,
            [Description("DepatmentsTableLastPageBtn")]
            DEPARTMENTSTABLELASTPAGEBTN,
            [Description("DepatmentsTableNextPageBtn")]
            DEPARTMENTSTABLENEXTPAGEBTN,
            [Description("AddDepatmentsDepName")]
            ADDDEPARTMENTSDEPNAME,
            [Description("AddDepatmentsBusinessUnit")]
            ADDDEPARTMENTSBUSINESSUNIT,
            [Description("AddDepatmentsDepCode")]
            ADDDEPARTMENTSDEPCODE,
            [Description("AddDepatmentsDescription")]
            ADDDEPARTMENTSDESCRIPTION,
            [Description("AddDepatmentsStartedOn")]
            ADDDEPARTMENTSSTARTEDON,
            [Description("AddDepatmentsTimeZone")]
            ADDDEPARTMENTSTIMEZONE,
            [Description("AddDepatmentsCountry")]
            ADDDEPARTMENTSCOUNTRY,
            [Description("AddDepatmentsState")]
            ADDDEPARTMENTSSTATE,
            [Description("AddDepatmentsCity")]
            ADDDEPARTMENTSCITY,
            [Description("AddDepatmentsStreetAdd1")]
            ADDDEPARTMENTSSTREETADD1,
            [Description("DepatmentsAddDepartmentBtn")]
            DEPARTMENTSADDDEPARTMENTBTN,
            [Description("BusinessUnitsTableLastPageBtn")]
            BUSINESSUNITSTABLELASTPAGEBTN,
            [Description("BusinessUnitsTableNextPageBtn")]
            BUSINESSUNITSTABLENEXTPAGEBTN
        }

        /// <summary>
        /// Used for Menu Names
        /// </summary>
        public enum SIDEBARMENUNAMES
        {
            [Description("Employee Configuration")]
            EMPLOYEECONFIGURATION,
            [Description("Leave Management")]
            LEAVEMANAGEMENT,
            [Description("Holiday Management")]
            HOLIDAYMANAGEMENT,
            [Description("Configuration")]
            CONFIGURATION,
            [Description("Leaves")]
            LEAVES
        }

        /// <summary>
        /// used for sub menu names
        /// </summary>
        public enum SIDEBARSUBMENUNAMES
        {
            [Description("Job Titles")]
            JOBTITLES,
            [Description("Leave Management Options")]
            LEAVEMANAGEMENTOPTIONS,
            [Description("Roles & Privileges")]
            ROLESANDPRIVILEGES,
            [Description("Positions")]
            POSITIONS,
            [Description("Manage Holiday Group")]
            MANAGEHOLIDAYGROUP,
            [Description("Manage Holidays")]
            MANAGEHOLIDAYS,
            [Description("My Team Appraisal")]
            MYTEAMAPPRAISAL,
            [Description("Parameters")]
            PARAMETERS,
            [Description("Questions")]
            QUESTIONS,
            [Description("Skills")]
            SKILLS,
            [Description("Business Units")]
            BUSINESSUNITS,
            [Description("Departments")]
            DEPARTMENTS,
            [Description("Candidates")]
            CANDIDATES,
            [Description("Interviews")]
            INTERVIEWS,
            [Description("Shortlisted & Selected Candidates")]
            SHORTLISTEDANDSELECTEDCANDIDATES,
            [Description("Leave Request")]
            LEAVEREQUEST,
            [Description("My Leave")]
            MYLEAVE,
            [Description("Employee Leave")]
            EMPLOYEELEAVE,
            [Description("My Regularization")]
            MYREGULARIZATION,
            [Description("Employee Regularization")]
            EMPLOYEEREGULARIZATION,
            [Description("Impersonate Leave")]
            IMPERSONATELEAVE,
            [Description("Web Checkin")]
            WEBCHECKIN,
            [Description("My Details")]
            MYDETAILS,
            [Description("Assign Web Checkin")]
            ASSIGNWEBCHECKIN,
            [Description("Openings/Positions")]
            OPENINGSANDPOSITIONS,
            [Description("Employees")]
            EMPLOYEES

        }

        public enum SQLQUIRIES
        {
            [Description("SELECT a.emailaddress FROM deltahrmsqa.main_employees_summary as a join deltahrmsqa.main_employeeleaves as b on a.user_id = b.user_id" +
                          " where a.emp_status_name = 'Probation' and a.isactive = '1' and b.emp_leave_limit <> b.used_leaves and a.userfullname <> 'Employee Automation' " +
                " and a.createdby_name = 'System Administrator' order by RAND() limit 1;")]
            SQLEMPLOYEEONPROBATION,
            [Description("SELECT a.emailaddress, a.userfullname, a.reporting_manager_name FROM deltahrmsqa.main_employees_summary as a " +
                           "join deltahrmsqa.main_employeeleaves_allotment as b on a.user_id = b.user_id where a.emp_status_name = 'Permanent' and " +
                            "a.isactive = '1' and b. leavecode = 'EL' and b.leave_limit <> b.used_leaves and a.userfullname <> 'Employee Automation' " +
                "and a.createdby_name = 'System Administrator' order by RAND() limit 1;")]
            SQLPERMEMPWITHEARNEDLEAVES,
            [Description("SELECT emailaddress FROM deltahrmsqa.main_employees_summary where userfullname = '{0}';")]
            SQLFETCHEMPIDWITHFULLNAME,
            [Description("SELECT a.emailaddress, a.userfullname, a.reporting_manager_name FROM deltahrmsqa.main_employees_summary as a " +
                           "join deltahrmsqa.main_employeeleaves_allotment as b on a.user_id = b.user_id where a.emp_status_name = 'Permanent' and " +
                            "a.isactive = '1' and b. leavecode = 'CO' and b.leave_limit <> b.used_leaves and a.userfullname <> 'Employee Automation' " +
                "and a.createdby_name = 'System Administrator' order by RAND() limit 1;")]
            SQLPERMEMPWITHELIGIBLECOMPOFF,
            [Description("SELECT emailaddress FROM deltahrmsqa.main_employees_summary where position_name ='Executive - Human Resource';")]
            SQLFETCHHREXECUTIVEEMIALID,
            [Description("SELECT a.emailaddress, a.userfullname, a.reporting_manager_name FROM deltahrmsqa.main_employees_summary as a " +
                           "join deltahrmsqa.main_employeeleaves_allotment as b on a.user_id = b.user_id where a.emp_status_name = 'Permanent' and " +
                            "a.isactive = '1' and b. leavecode = 'ML' and b.leave_limit <> b.used_leaves and a.userfullname <> 'Employee Automation' " +
                "and a.createdby_name = 'System Administrator' order by RAND() limit 1;")]
            SQLPERMEMPWITHMATERNITYLEAVES,
            [Description("SELECT a.emailaddress, a.userfullname, a.reporting_manager_name FROM deltahrmsqa.main_employees_summary as a " +
                           "join deltahrmsqa.main_employeeleaves_allotment as b on a.user_id = b.user_id where a.emp_status_name = 'Permanent' and " +
                            "a.isactive = '1' and b. leavecode = 'PL' and b.leave_limit <> b.used_leaves and a.userfullname <> 'Employee Automation' " +
                "and a.createdby_name = 'System Administrator' order by RAND() limit 1;")]
            SQLPERMEMPWITHPATERNITYLEAVES,
            [Description("SELECT employeeId, userfullname, businessunit_name, reporting_manager_name, position_name, date_of_joining, emailaddress FROM " +
                        "deltahrmsqa.main_employees_summary where userfullname = '{0}';")]
            SQLEMPDETAILS,
            [Description("SELECT emailaddress, userfullname, reporting_manager_name, position_name FROM deltahrmsqa.main_employees_summary where isactive = '1' and userfullname <> 'Employee Automation' " +
                "and createdby_name = 'System Administrator' order by rand() limit 1;")]
            SQLQUERYFORACTIVEEMPLOYEES,
            [Description("SELECT a.emailaddress, a.userfullname, a.reporting_manager_name FROM deltahrmsqa.main_employees_summary as a join deltahrmsqa.main_employeeleaves_allotment as b on " +
                "a.user_id = b.user_id where a.emp_status_name = 'Permanent' and a.isactive = '1' and b. leavecode = 'EL' and b.leave_limit <> b.used_leaves and a.userfullname <> 'Employee Automation' and a.createdby_name = 'System Administrator' " +
                "and b.user_id in (SELECT user_id FROM deltahrmsqa.main_employeeleaves_allotment where leavecode = 'EL' and leave_limit > used_leaves and isactive = '1') order by rand() limit 1;")]
            SQLEMPHAVINGELANDSLBAL,
            [Description("SELECT a.emailaddress, a.userfullname, a.reporting_manager_name FROM deltahrmsqa.main_employees_summary as a " +
                           "join deltahrmsqa.main_employeeleaves_allotment as b on a.user_id = b.user_id where a.emp_status_name = 'Permanent' and " +
                            "a.isactive = '1' and b. leavecode = 'BL' and b.leave_limit <> b.used_leaves and a.userfullname <> 'Employee Automation' " +
                "and a.createdby_name = 'System Administrator' order by RAND() limit 1;")]
            SQLPERMEMPWITHELIGIBLEBEREAVEMENT
        }

        public const string REGISTEREDANDAPPROVED = "Registered and Approved";
        public const string FULLDAYLEAVE = "Full Day";
        public const string APPLYLEAVECOMMENT = "Leave - Automation Test";
        public const string COMBINEDLEAVETYPEERRORMSG = "Leave types cannot be combined.";
        public const string SQLDATABASECONNECTION = "HOST=192.168.100.253;DATABASE=deltahrmsqa; UID=deltaqaautomation;PASSWORD=Automation@2020";
    }
}