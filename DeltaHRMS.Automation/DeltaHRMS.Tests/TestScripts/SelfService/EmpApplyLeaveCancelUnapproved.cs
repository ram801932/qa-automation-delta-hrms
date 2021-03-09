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
    [Script("", "", "DeltaHRMS", "SelfService", "Employee - Create & Cancel Leave Request", "Create a Leave Request & Cancel it unapproved", "Regression")]
    /// <summary>
    ///  Employee - Create & Cancel Leave Request
    /// </summary>
    class EmpApplyLeaveCancelUnapproved : BaseTest
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

                var userName = pg_Hrms.ConnectToMySql(SQLQUIRIES.SQLPERMEMPWITHEARNEDLEAVES.GetDescription());

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
