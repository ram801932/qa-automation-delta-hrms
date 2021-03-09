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

namespace DeltaHRMS.Tests.TestScripts.Appraisals
{
    [Script("", "", "DeltaHRMS", "Appraisals", "Configure Appraisals by Manager", "Configuration to be done by HR", "Regression")]
    /// <summary>
    ///  Appraisals - Config Appraisals
    /// </summary>
    class AppraisalsEndtoEnd : BaseTest
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

                //Step = "Clearing the Appraisals data in MySql DataBase";
                //pg_CommonPage.CallAStoredProcedure("clearappraisal");

                Step = "Launch 'Delta HRMS' application";
                var pg_Hrms = pg_CommonPage.NavigateToDeltaHRMSLoginPage();

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["HrMangerUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Appraisals page";
                pg_Hrms.NavigateToAppraisalsPage();

                Step = "Navigate to Initialize Appraisal";
                pg_Hrms.SelectMenuFromSideBar("Initialize Appraisal");

                Step = "Configure Appraisals";
                pg_Hrms.InitilizationofAppraisalForm(TestDataNode["BusinessUnit"].InnerText, TestDataNode["Department"].InnerText, 
                TestDataNode["FromYear"].InnerText, TestDataNode["ToYear"].InnerText, DateTime.Now.AddDays(3).ToString("dd-MM-yyyy"), 
                TestDataNode["Rating"].InnerText, TestDataNode["Mode"].InnerText, TestDataNode["EnableTo"].InnerText); 

                 Step = "Configure Line Managers L1";
                pg_Hrms.ConfigureLineManagerForm();

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["EmpUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Appraisals page";
                pg_Hrms.NavigateToAppraisalsPage();

                Step = "Navigate to Self Appraisal";
                pg_Hrms.SelectMenuFromSideBar("Self Appraisal");

                Step = "Employee Self Appraisals Completion";
                pg_Hrms.EmployeeSelfAppraisals();

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["ManagerUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to My Team Appraisal";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.MYTEAMAPPRAISAL.GetDescription());

                Step = "Manager Employee Rating";
                pg_Hrms.ManagerEmployeeRating(TestDataNode["employeename"].InnerText);

                string filename = TestDataNode["employeename"].InnerText.Replace(' ', '_') + "_" + TestDataNode["FromYear"].InnerText + "_" + TestDataNode["ToYear"].InnerText + "_Y1";

                Step = "Print Overall Ratings of the Employee";
                pg_Hrms.ManagerPrintAppraisalsRating(filename);

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
