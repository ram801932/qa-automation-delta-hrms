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
    [Script("", "", "DeltaHRMS", "SelfService", "Perform Web Check In & Check Out", "Employee Perform Web Check In & Check Out", "Regression")]
    /// <summary>
    ///  Employee - Create & Cancel Leave Request
    /// </summary>
    class SelfServiceWebCheckInCheckOut : BaseTest
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
                var activeEmp = pg_Hrms.ConnectToMySql(SQLQUIRIES.SQLQUERYFORACTIVEEMPLOYEES.GetDescription());

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(hrUserName[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Web Check In page";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.ASSIGNWEBCHECKIN.GetDescription());

                Step = "Assign Web Check In Access to the user";
                pg_Hrms.AssignWebCheckIn(TestDataNode["SelectByType"].InnerText, activeEmp[1] + " , " + activeEmp[3], TestDataNode["Status"].InnerText);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(activeEmp[0], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to Web Check In page";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.WEBCHECKIN.GetDescription());

                Step = "Perform Web Check in";
                pg_Hrms.PerformWebCheckIn();

                Step = "Perform Web Check Out";
                pg_Hrms.PerformWebCheckOut();

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
