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
    [Script("", "", "DeltaHRMS", "SelfService", "Employee - Cancel Approved Leave", "Cancel Leave Approved by Manager", "Regression")]
    /// <summary>
    ///  Delta HRMS - Cancel Approved Leave
    /// </summary>
    class EmployeeCancelApprovedLeave : BaseTest
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

                //fetching Emp details
                var userName = pg_Hrms.ConnectToMySql(SQLQUIRIES.SQLPERMEMPWITHEARNEDLEAVES.GetDescription());

                // fetching manager details with the emp details
                string managerUseIdQuery = string.Format(SQLQUIRIES.SQLFETCHEMPIDWITHFULLNAME.GetDescription(), userName[2]);
                var managerName = pg_Hrms.ConnectToMySql(managerUseIdQuery);

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(userName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave Request Page";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.LEAVEREQUEST.GetDescription());

                Step = "Navigate to Create Leave Request Page";
                pg_Hrms.ClickApplyLeave();

                string date = pg_Hrms.AddBusinessDays(DateTime.Now, 1).ToString("dd-MM-yyyy");

                Step = "Create Leave Request";
                pg_Hrms.CreateLeaveRequest(LEAVETYPES.EARNEDLEAVE.GetDescription(), date, FULLDAYLEAVE);

                Step = "Verify Leave is created Successfully";
                pg_Hrms.VerifyLeaveApplication(date);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(managerName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave => Employee Leaves";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.EMPLOYEELEAVE.GetDescription());

                Step = "Approve the Leave request created by Team Member";
                pg_Hrms.EmpLeaveManageLeaveRequest(userName[1], date, "Approve");

                Step = "Verify Leave Status for the Approved Leave";
                pg_Hrms.EmpLeaveVerifyLeaveStatus(userName[1], date, "Approved");
                
                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with Employee credentials";
                pg_Hrms.LoginToDeltaHRMS(userName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave => My Leave Page";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.MYLEAVE.GetDescription());

                Step = "Navigate to Approved Leaves Tab";
                pg_Hrms.NavToMyLeavesApprovedLeaves();

                Step = "Delete the Leave Approved by the Reporting Manager";
                pg_Hrms.DeleteAppliedLeave(date);

                Step = "Logout from Delta HRMS application";
                pg_Hrms.LogoutFromDeltaHRMS();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
