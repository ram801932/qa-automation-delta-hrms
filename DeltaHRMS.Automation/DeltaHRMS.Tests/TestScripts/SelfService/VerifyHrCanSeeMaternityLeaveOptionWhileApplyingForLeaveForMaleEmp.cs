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
    [Script("", "", "DeltaHRMS", "SelfService", "HR - Verify Leave Options For Male Employee", "Verify Hr Can See Maternity Leave Option While Applying For Leave For Male Employee", "Regression")]
    /// <summary>
    ///  Delta HRMS - Cancel Approved Leave
    /// </summary>
    class VerifyHrCanSeeMaternityLeaveOptionWhileApplyingForLeaveForMaleEmp : BaseTest
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
                
                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(hrUserName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Leave => Impersonate Leave";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVES.GetDescription(), SIDEBARSUBMENUNAMES.IMPERSONATELEAVE.GetDescription());
                
                Step = "Verify Maternity Leave Exists for Male Employee";
                pg_Hrms.VerifyLeaveTypesForEmployee(userName[1], LEAVETYPES.MATERNITYLEAVE.GetDescription(), "Doesnt Exists");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
