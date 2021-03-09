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
    [Script("", "", "DeltaHRMS", "Appraisals", "Configure Appraisals Parameters", "Configuration to be done by HR", "Regression")]
    /// <summary>
    ///  Appraisals - Config Appraisals Parameters
    /// </summary>
    class ManagerPrintAppraisalsRating : BaseTest
    {
        /// <summary>
        ///  Manager Prints the Overall rating of the Employee
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
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["UserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Appraisals page";
                pg_Hrms.NavigateToAppraisalsPage();

                Step = "Navigate to My Team Appraisal";
                pg_Hrms.SelectMenuFromSideBar("My Team Appraisal");

                /*These below steps are covered as part of the Appraisals End to End Scenario. To Run the individual script, add test data in DeltHRMS.xml 
                 with the tag name ManagerPrintAppraisalsRating
                Uncomment the below steps and execute*/

                //Step = "Print Overall Ratings of the Employee";
                //pg_Hrms.ManagerPrintAppraisalsRating(TestDataNode["docName"].InnerText);

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
