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
    class ConfigureAppraisalsQuestions : BaseTest
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
                pg_Hrms.LoginToDeltaHRMS("skoppineni@deltaintech.com", TestDataNode["Password"].InnerText);

                Step = "Navigate to Appraisals page";
                pg_Hrms.NavigateToAppraisalsPage();

                Step = "Navigate to Configuration -> Questions";
                pg_Hrms.SelectMenuSubMenuFromSideBar(SIDEBARMENUNAMES.CONFIGURATION.GetDescription(),SIDEBARSUBMENUNAMES.QUESTIONS.GetDescription());

                Step = "Adding Appraisals Questions";
                pg_Hrms.AddAppraisalsQuestions();

                Step = "Deleting Appraisals Questions";
                pg_Hrms.DeleteQuestions();

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
