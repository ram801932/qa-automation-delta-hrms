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
    [Script("", "", "DeltaHRMS", "HR", "Job Titles", "Create, View and Delete new Job Title", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class AddJobTitle : BaseTest
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

                Step = "Navigate to Employee Configuration => Job Titles";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.EMPLOYEECONFIGURATION.GetDescription(), SIDEBARSUBMENUNAMES.JOBTITLES.GetDescription());

                Step = "Add a New Job Title";
                pg_Hrms.AddJobTitle(TestDataNode["titleCode"].InnerText, TestDataNode["titleName"].InnerText, TestDataNode["jobDesc"].InnerText,
                                                   TestDataNode["minExpReq"].InnerText, TestDataNode["jobPayGrdCode"].InnerText, TestDataNode["jobPayFreq"].InnerText,
                                                   TestDataNode["comments"].InnerText);

                Step = "Verify the Job Title";
                pg_Hrms.VerifyJobTitles(TestDataNode["titleCode"].InnerText, TestDataNode["titleName"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to Employee Configuration => Job Titles";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.EMPLOYEECONFIGURATION.GetDescription(), SIDEBARSUBMENUNAMES.JOBTITLES.GetDescription());

                Step = "Delete the Job Title";
                pg_Hrms.DeleteJobTitles(TestDataNode["titleCode"].InnerText, TestDataNode["titleName"].InnerText);


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
