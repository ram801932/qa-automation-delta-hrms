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
    [Script("", "", "DeltaHRMS", "SelfService", " Validate My Details Page", "Validate My Details in Self Service Tab", "Regression")]
    /// <summary>
    ///  Employee - Create & Cancel Leave Request
    /// </summary>
    class SelfServiceValidateMyDetails : BaseTest
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

                string userDetailsQuery = string.Format(SQLQUIRIES.SQLEMPDETAILS.GetDescription(), activeEmp[1]);
                var userDetails = pg_Hrms.ConnectToMySql(userDetailsQuery);

                Step = "Login to Delta HRMS with valid credentials";
                pg_Hrms.LoginToDeltaHRMS(userDetails[6], TestDataNode["Password"].InnerText);

                Step = "Navigate to Self Service page";
                pg_Hrms.NavigateToSelfServicePage();

                Step = "Navigate to My Details page";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.MYDETAILS.GetDescription());

                Step = "Validate the Values for Employee in My Details Page";
                pg_Hrms.ValidateSelfServiceMyDetailsPage(userDetails[0], userDetails[1], userDetails[2],
                                                            userDetails[3], userDetails[4], userDetails[5].Split(' ')[0],
                                                            userDetails[6]);

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
