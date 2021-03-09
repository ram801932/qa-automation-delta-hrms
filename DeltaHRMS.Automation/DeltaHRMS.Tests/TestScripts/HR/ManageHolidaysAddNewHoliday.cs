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
    [Script("", "", "DeltaHRMS", "HR", "Manage Holidays", "Add, view and Delete New Holiday", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class ManageHolidaysAddNewHoliday : BaseTest
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
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.HOLIDAYMANAGEMENT.GetDescription(), SIDEBARSUBMENUNAMES.MANAGEHOLIDAYS.GetDescription());

                Step = "Add a New Holiday in the Manage Holidays";
                string holidayGroup = pg_Hrms.ManageHoldiaysAddNewHoliday(TestDataNode["HolidayName"].InnerText, TestDataNode["HolidayGroup"].InnerText, DateTime.Now.ToString("dd-MM-yyyy"));

                Step = "Verify New Holiday is added to the list of Holidays";
                pg_Hrms.ManageHolidaysVerifyHoliday(TestDataNode["HolidayName"].InnerText, holidayGroup, DateTime.Now.ToString("dd-MM-yyyy"));

                Step = "Delete the New Holiday from the list of Holidays";
                pg_Hrms.ManageHolidaysDeleteHoliday(TestDataNode["HolidayName"].InnerText, holidayGroup, DateTime.Now.ToString("dd-MM-yyyy"));

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
