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
    [Script("", "", "DeltaHRMS", "HR", "Leave Management", "Create, View and Delete new Leave Mangement Options", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class AddLeaveManagementOptions : BaseTest
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

                Step = "Login to Delta HRMS with Management Credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["EmpUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate from Home Page => Organization Page";
                pg_Hrms.NavigateToOrganizationPage();

                Step = "Navigate to Business Units Sub Menu in Organization Tab";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.BUSINESSUNITS.GetDescription());

                Step = "Test Data Set Up - Verify & Add Business Unit in Organization";
                pg_Hrms.AddNewBusinessUnitInOrganization(TestDataNode["businessUnit"].InnerText);

                Step = "Navigate from Home Page => Organization Page";
                pg_Hrms.NavigateToOrganizationPage();

                Step = "Navigate to Departments Sub Menu in Organization Tab";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.DEPARTMENTS.GetDescription());

                Step = "Test Data Set Up - Verify & Add department in Organization";
                pg_Hrms.AddNewDepartmentInOrganization(TestDataNode["dept"].InnerText, TestDataNode["businessUnit"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to Leave Management => Leave Management Options";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVEMANAGEMENT.GetDescription(), SIDEBARSUBMENUNAMES.LEAVEMANAGEMENTOPTIONS.GetDescription());

                Step = "Add a New Leave Management Group";
                pg_Hrms.AddLeaveManagementOptions(TestDataNode["businessUnit"].InnerText, TestDataNode["dept"].InnerText, TestDataNode["startMnth"].InnerText,
                                                   TestDataNode["weekend1"].InnerText, TestDataNode["weekend2"].InnerText, TestDataNode["wrkingHrs"].InnerText,
                                                   TestDataNode["hlfDayReq"].InnerText, TestDataNode["leaveTransfer"].InnerText, TestDataNode["skipHolidays"].InnerText,
                                                   TestDataNode["hrManager"].InnerText, TestDataNode["description"].InnerText);

                Step = "Verify the Leave Management Options";
                pg_Hrms.VerifyLeaveManagementOptions(TestDataNode["dept"].InnerText, TestDataNode["weekend1"].InnerText, TestDataNode["weekend2"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to Leave Management => Leave Management Options";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.LEAVEMANAGEMENT.GetDescription(), SIDEBARSUBMENUNAMES.LEAVEMANAGEMENTOPTIONS.GetDescription());

                Step = "Delete the Leave Manegement Options";
                pg_Hrms.DeleteLeaveManagementOptions(TestDataNode["dept"].InnerText, TestDataNode["weekend1"].InnerText, TestDataNode["weekend2"].InnerText);

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
