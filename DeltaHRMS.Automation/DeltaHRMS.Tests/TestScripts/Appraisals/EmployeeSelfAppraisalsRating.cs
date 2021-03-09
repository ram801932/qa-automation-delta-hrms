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
    [Script("", "", "DeltaHRMS", "Appraisals", "Employee Self Appraisals Ratings", "Ratings given by Employee", "Regression")]
    /// <summary>
    ///  Appraisals - Employee Self Appraisals Ratings
    /// </summary>
    class EmployeeSelfAppraisals : BaseTest
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

                Step = "Navigate to Appraisals page";
                pg_Hrms.NavigateToAppraisalsPage();

                /*These below steps are covered as part of the Appraisals End to End Scenario. To Run the individual script, add test data in DeltHRMS.xml 
                 with the tag name EmployeeSelfAppraisals
                Uncomment the below steps and execute*/

                //Step = "Employee Self Appraisals Completion";
                //pg_Hrms.EmployeeSelfAppraisals();

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
