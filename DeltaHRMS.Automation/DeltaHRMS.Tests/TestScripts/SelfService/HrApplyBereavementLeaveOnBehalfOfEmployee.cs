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
    [Script("", "", "DeltaHRMS", "SelfService", "HR - Apply Bereavement Leave", "HR Apply Bereavement Leave for Employee", "Regression")]
    /// <summary>
    ///  Delta HRMS - Cancel Approved Leave
    /// </summary>
    class HrApplyBereavementLeaveOnBehalfOfEmployee : BaseTest
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
                var userName = pg_Hrms.ConnectToMySql(SQLQUIRIES.SQLPERMEMPWITHELIGIBLEBEREAVEMENT.GetDescription());

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

                Step = "Create Leave Request";
                pg_Hrms.CreateImpersonateLeaveRequest(LEAVETYPES.BEREAVEMENTLEAVE.GetDescription(), date, date, FULLDAYLEAVE, userName[1]);

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
