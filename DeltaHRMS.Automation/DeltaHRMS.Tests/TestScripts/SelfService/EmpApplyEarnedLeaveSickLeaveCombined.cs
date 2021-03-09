#region Microsoft references

#endregion

#region Automation referencess
using DeltaHRMS.Accelerators.BaseClasses;
using DeltaHRMS.Accelerators.Reporting;
using DeltaHRMS.Accelerators.Utilities;
using DeltaHRMS.Repository.CommonFunctions;
using System;
using static DeltaHRMS.Repository.PageFunctions.Constants;
#endregion

namespace DeltaHRMS.Tests.TestScripts.SelfService
{
    [Script("", "", "DeltaHRMS", "SelfService", "Employee - Create Leave Request", "Apply Earned Leave & Sick Leave Combined", "Regression")]
    /// <summary>
    ///  Employee - Create Leave Request
    /// </summary>
    class EmpApplyEarnedLeaveSickLeaveCombined : BaseTest
    {
        /// <summary>
        ///  overriden Execute TestCase
        /// </summary>
        protected override void ExecuteTestCase()
        {
            try
            {
                Reporter.Add(new Chapter(string.Format("Execute test case- '{0}'", this.GetType().Name)));

                var pg_CommonPage = Page<Common>(Driver, TestDataNode, Reporter);

                Step = "Launch 'Delta HRMS' application";
                var pg_Hrms = pg_CommonPage.NavigateToDeltaHRMSLoginPage();

                var userName = pg_Hrms.ConnectToMySql(SQLQUIRIES.SQLEMPHAVINGELANDSLBAL.GetDescription());

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(userName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave Request Page";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.LEAVEREQUEST.GetDescription());

                Step = "Navigate to Create Leave Request Page";
                pg_Hrms.ClickApplyLeave();

                string date = DateTime.Now.ToString("dd-MM-yyyy");

                Step = "Create Leave Request";
                pg_Hrms.CreateLeaveRequest(LEAVETYPES.EARNEDLEAVE.GetDescription(), date, FULLDAYLEAVE);

                Step = "Verify Leave is created Successfully";
                pg_Hrms.VerifyLeaveApplication(date);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave =>  My Leave Request Page";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.LEAVEREQUEST.GetDescription());

                Step = "Navigate to Create Leave Request Page";
                pg_Hrms.ClickApplyLeave();

                string slDate = pg_Hrms.AddBusinessDays(DateTime.Now, 1).ToString("dd-MM-yyyy");

                Step = "Create a Sick Leave to Verify error Message";
                pg_Hrms.CreateLeaveRequestFailure(LEAVETYPES.SICKCASUALLEAVE.GetDescription(), FULLDAYLEAVE, slDate, COMBINEDLEAVETYPEERRORMSG);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave => My Leave Page";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.MYLEAVE.GetDescription());

                Step = "Delete the Applied Leave before Approval";
                pg_Hrms.DeleteAppliedLeave(date);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
