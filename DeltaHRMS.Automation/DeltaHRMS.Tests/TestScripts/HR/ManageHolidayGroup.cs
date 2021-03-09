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
    [Script("", "", "DeltaHRMS", "HR", "Manage Holiday Group", "Create, View and Delete a Holiday Group", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class ManageHolidayGroup : BaseTest
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

                Step = "Navigate to Holiday Management => Manage Holiday Group";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.HOLIDAYMANAGEMENT.GetDescription(), SIDEBARSUBMENUNAMES.MANAGEHOLIDAYGROUP.GetDescription());

                Step = "Add a New Holiday Group";
                pg_Hrms.AddHolidayGroup(TestDataNode["HolidayGroup"].InnerText);

                Step = "Verify if holiday group exists in Manage Holiday Group";
                pg_Hrms.VerifyHolidayGroup(TestDataNode["HolidayGroup"].InnerText);

                Step = "Delete the Holiday Group";
                pg_Hrms.DeleteHolidayGroup(TestDataNode["HolidayGroup"].InnerText);

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
