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

namespace DeltaHRMS.Tests.TestScripts.Recruitments
{
    [Script("", "", "DeltaHRMS", "Recruitments", "Create Requistion", "Create Position/Opening in Delta", "Regression")]
    /// <summary>
    ///  Recruitments - Create Requsition
    /// </summary>
    class CreateRequisition : BaseTest
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
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["HrUserName"].InnerText, TestDataNode["Password"].InnerText);

                /*These below steps are covered as part of the Recruitments End to End Scenario. To Run the individual script, add test data in DeltHRMS.xml
                 with the tag name CreateRequisition
                Uncomment the below steps and execute*/

                //Step = "Navigate to Recuruitments page";
                //pg_Hrms.NavigateToRecruitmentsPage();

                //Step = "Navigate to Openings/Positions";
                //pg_Hrms.AddingOpenings(TestDataNode["busUnit"].InnerText, TestDataNode["dept"].InnerText, TestDataNode["reportingManager"].InnerText,
                //                                TestDataNode["approver1"].InnerText, DateTime.Now.ToString("dd-MM-yyyy"), TestDataNode["jobTitle"].InnerText,
                //                                TestDataNode["position"].InnerText, TestDataNode["noOfPositions"].InnerText, TestDataNode["billingType"].InnerText,
                //                                TestDataNode["posType"].InnerText, TestDataNode["minExpReq"].InnerText);

                
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
