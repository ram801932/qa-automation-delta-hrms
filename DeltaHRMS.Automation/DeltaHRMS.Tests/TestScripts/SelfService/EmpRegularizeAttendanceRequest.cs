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
    [Script("", "", "DeltaHRMS", "SelfService", "Regularize Attendance Request", "Employee raise a request for Regularize Attendance", "Regression")]
    /// <summary>
    ///  Employee - Create & Cancel Leave Request
    /// </summary>
    class EmpRegularizeAttendanceRequest : BaseTest
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

                string managerUseIdQuery = string.Format(SQLQUIRIES.SQLFETCHEMPIDWITHFULLNAME.GetDescription(), activeEmp[2]);
                var managerName = pg_Hrms.ConnectToMySql(managerUseIdQuery);

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(activeEmp[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Self Service => My Regularization In page";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.MYREGULARIZATION.GetDescription());

                Step = "Create a request to Regularize the attendance";
                pg_Hrms.RegularizeAttendance(DateTime.Now.ToString("dd-MM-yyyy"), TestDataNode["CheckIn"].InnerText, TestDataNode["CheckOut"].InnerText);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(managerName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Self Service => Employee Regularization In page";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.EMPLOYEEREGULARIZATION.GetDescription());

                Step = "Cancel the Attendance Regularization Request raised by Employee";
                pg_Hrms.ManagerApproveRejectRegularizationRequest(activeEmp[1], DateTime.Now.ToString("yyyy-MM-dd"), "Reject");

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
