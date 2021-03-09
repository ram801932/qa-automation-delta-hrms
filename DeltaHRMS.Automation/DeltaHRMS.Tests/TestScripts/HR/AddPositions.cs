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
    [Script("", "", "DeltaHRMS", "HR", "Positions", "Create, View and Delete new Position", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class AddPositions : BaseTest
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

                Step = "Add a New Position";
                string jobTitle = pg_Hrms.AddPosition(TestDataNode["jobTitle"].InnerText, TestDataNode["positionName"].InnerText, TestDataNode["description"].InnerText);

                Step = "Verify the Position";
                pg_Hrms.VerifyPositions(jobTitle, TestDataNode["positionName"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to Employee Configuration => Positions";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.EMPLOYEECONFIGURATION.GetDescription(), SIDEBARSUBMENUNAMES.POSITIONS.GetDescription());

                Step = "Delete the Position";
                pg_Hrms.DeletePositions(jobTitle, TestDataNode["positionName"].InnerText);

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
