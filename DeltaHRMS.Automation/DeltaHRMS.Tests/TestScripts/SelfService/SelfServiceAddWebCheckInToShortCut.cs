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
    [Script("", "", "DeltaHRMS", "SelfService", "Add Web CheckIn ToShortCut", "Add, Validate & delete WebCheck In To ShortCuts", "Regression")]
    /// <summary>
    ///  Employee - Create & Cancel Leave Request
    /// </summary>
    class SelfServiceAddWebCheckInToShortCut : BaseTest
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

                var activeEmp = pg_Hrms.ConnectToMySql(SQLQUIRIES.SQLQUERYFORACTIVEEMPLOYEES.GetDescription());

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(activeEmp[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Web Check In page";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.WEBCHECKIN.GetDescription());

                Step = "Add Web Check In option to the shortcuts";
                pg_Hrms.AddRemoveShortcut(SIDEBARSUBMENUNAMES.WEBCHECKIN.GetDescription(), "Add");

                Step = "Verify Check In Option is added to shortcuts";
                pg_Hrms.VerifyShortCutExists(SIDEBARSUBMENUNAMES.WEBCHECKIN.GetDescription());

                Step = "Delete the Web Check In Option from shortcuts";
                pg_Hrms.AddRemoveShortcut(SIDEBARSUBMENUNAMES.WEBCHECKIN.GetDescription(), "Remove");

                Step = "Verify Check In Option is Removed to shortcuts";
                pg_Hrms.VerifyShortCutExists(SIDEBARSUBMENUNAMES.WEBCHECKIN.GetDescription());

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
