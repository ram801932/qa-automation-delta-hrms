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
    [Script("", "", "DeltaHRMS", "HR", "View Edit Employee Details", "Login as HR, View / Edit Employee Details", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class ViewEditEmployeeDetails : BaseTest
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
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["HrMangerUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to HR Page => Employees";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.EMPLOYEES.GetDescription());

                Step = "Search for Employee in HR Records";
                pg_Hrms.SearchEmployeeInHrPage(TestDataNode["SearchEmpBy"].InnerText, TestDataNode["EmpName"].InnerText, TestDataNode["EmpName"].InnerText);

                Step = "Edit the Employee details in HR Page";
                pg_Hrms.EditEmployeeNameInEmployeePage(TestDataNode["EditedEmpFirstName"].InnerText, TestDataNode["EditedEmpName"].InnerText);

                Step = "View the employee Info in the HR Page";
                pg_Hrms.ViewEmployeeInformationInHrPage(TestDataNode["EditedEmpFirstName"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to HR Page => Employees";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.EMPLOYEES.GetDescription());

                Step = "Search for Employee in HR Records";
                pg_Hrms.SearchEmployeeInHrPage(TestDataNode["SearchEmpBy"].InnerText, TestDataNode["EditedEmpName"].InnerText, TestDataNode["EditedEmpName"].InnerText);

                Step = "Edit the Employee details in HR Page";
                pg_Hrms.EditEmployeeNameInEmployeePage(TestDataNode["EmpFirstName"].InnerText, TestDataNode["EmpName"].InnerText);

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
