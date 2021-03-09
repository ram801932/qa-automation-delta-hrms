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
    [Script("", "", "DeltaHRMS", "HR", "Positions", "Add, Remove and verify Position Pin to Shortcut", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class PinToShortCutPositions : BaseTest
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

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["EmpUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to Employee Configuration => Positions";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.EMPLOYEECONFIGURATION.GetDescription(), SIDEBARSUBMENUNAMES.POSITIONS.GetDescription());

                Step = "Add Web Check In option to the shortcuts";
                pg_Hrms.AddRemoveShortcut(SIDEBARSUBMENUNAMES.POSITIONS.GetDescription(), "Add");

                Step = "Verify Check In Option is added to shortcuts";
                pg_Hrms.VerifyShortCutExists(SIDEBARSUBMENUNAMES.POSITIONS.GetDescription());

                Step = "Delete the Web Check In Option from shortcuts";
                pg_Hrms.AddRemoveShortcut(SIDEBARSUBMENUNAMES.POSITIONS.GetDescription(), "Remove");

                Step = "Verify Check In Option is Removed to shortcuts";
                pg_Hrms.VerifyShortCutExists(SIDEBARSUBMENUNAMES.POSITIONS.GetDescription());

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
