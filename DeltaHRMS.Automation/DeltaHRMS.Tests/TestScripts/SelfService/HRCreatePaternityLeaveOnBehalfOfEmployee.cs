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
    [Script("", "", "DeltaHRMS", "SelfService", "HR - Create Paternity Leave Request", "Create a Paternity Leave Request on Behalf of Employee", "Regression")]
    /// <summary>
    ///  Employee - Create & Cancel Leave Request
    /// </summary>
    class HRCreatePaternityLeaveOnBehalfOfEmployee : BaseTest
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

                var hrUserName = pg_Hrms.ConnectToMySql(SQLQUIRIES.SQLFETCHHREXECUTIVEEMIALID.GetDescription());
                var userName = pg_Hrms.ConnectToMySql(SQLQUIRIES.SQLPERMEMPWITHPATERNITYLEAVES.GetDescription());

                // fetching manager details with the emp details
                string managerUseIdQuery = string.Format(SQLQUIRIES.SQLFETCHEMPIDWITHFULLNAME.GetDescription(), userName[2]);
                var managerName = pg_Hrms.ConnectToMySql(managerUseIdQuery);

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(hrUserName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave => Impersonate Leave";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.IMPERSONATELEAVE.GetDescription());

                string date = DateTime.Now.ToString("dd-MM-yyyy");
                string toDate = pg_Hrms.AddBusinessDays(DateTime.Now, 4).ToString("dd-MM-yyyy");
                
                Step = "Create Leave Request";
                pg_Hrms.CreateImpersonateLeaveRequest(LEAVETYPES.PATERNITYLEAVE.GetDescription(), date, toDate, FULLDAYLEAVE, userName[1]);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(managerName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave => Employee Leaves";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.EMPLOYEELEAVE.GetDescription());

                Step = "Approve the Leave request created by Team Member";
                pg_Hrms.EmpLeaveManageLeaveRequest(userName[1], date, "Reject");

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
