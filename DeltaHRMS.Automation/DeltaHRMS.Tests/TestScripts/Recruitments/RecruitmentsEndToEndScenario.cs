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
    [Script("", "", "DeltaHRMS", "Recruitments", "Recruitments End To End Scenario", "End to End flow of creating and closing requestion", "Regression")]
    /// <summary>
    ///  Recruitments - Create Requsition
    /// </summary>
    class RecruitmentsEndToEndScenario : BaseTest
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

                Step = "Login to Delta HRMS with HR credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["HrUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Recuruitments page";
                pg_Hrms.NavigateToRecruitmentsPage();

                Step = "Navigate to Openings/Positions";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.OPENINGSANDPOSITIONS.GetDescription());

                Step = "Create a new Requisition";
                string requisitionCode = pg_Hrms.AddingOpenings(TestDataNode["busUnit"].InnerText, TestDataNode["dept"].InnerText, TestDataNode["reportingManager"].InnerText,
                                                TestDataNode["approver1"].InnerText,DateTime.Now.AddDays(1).ToString("dd-MM-yyyy"), TestDataNode["jobTitle"].InnerText,
                                                TestDataNode["position"].InnerText, TestDataNode["noOfPositions"].InnerText, TestDataNode["billingType"].InnerText,
                                                TestDataNode["posType"].InnerText, TestDataNode["minExpReq"].InnerText);

                
                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with Approver 1 credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["Approver1"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Recuruitments page";
                pg_Hrms.NavigateToRecruitmentsPage();

                Step = "Navigate to Openings/Positions";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.OPENINGSANDPOSITIONS.GetDescription());

                Step = "Approve1 to Approve the Requisition Created";
                pg_Hrms.ManagementApproveNewRequisition(requisitionCode, TestDataNode["Approver1Status"].InnerText);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with HR credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["HrUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Recuruitments page";
                pg_Hrms.NavigateToRecruitmentsPage();

                Step = "Navigate to Candidates Menu";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.CANDIDATES.GetDescription());

                Step = "Adding Candidates to the Requisition";
                pg_Hrms.AddingCandidates(requisitionCode + " - " + TestDataNode["jobTitle"].InnerText, TestDataNode["firstName"].InnerText, TestDataNode["lastName"].InnerText,
                                         TestDataNode["source"].InnerText, DateTime.Now.ToString("MMddHHmmss") + "tester@test.com", DateTime.Now.ToString("MMddHHmmss"), 
                                         TestDataNode["skillSet"].InnerText);

                Step = "Navigate to Interviews Menu";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.INTERVIEWS.GetDescription());

                Step = "Scheduling the Interviews for the Requisition and the Candidates";
                pg_Hrms.AddingInterviews(requisitionCode + " - " + TestDataNode["jobTitle"].InnerText, TestDataNode["candidateName"].InnerText,
                                           TestDataNode["location"].InnerText, TestDataNode["interviewType"].InnerText, DateTime.Now.AddDays(1).ToString("dd-MM-yyyy"), TestDataNode["time"].InnerText,
                                           TestDataNode["interviewName"].InnerText);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with Interviewer credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["ManagerUserId"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Recuruitments page";
                pg_Hrms.NavigateToRecruitmentsPage();

                Step = "Navigate to Openings/Positions";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.INTERVIEWS.GetDescription());

                Step = "Interviewer - Complete the Interview Process";
                pg_Hrms.RecruitmentsInterviewRounds(requisitionCode, TestDataNode["Interviewr1status"].InnerText, TestDataNode["candidateName"].InnerText,
                                                    TestDataNode["interviewName"].InnerText, TestDataNode["feedbackDesc"].InnerText);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with HR credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["HrUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Recuruitments page";
                pg_Hrms.NavigateToRecruitmentsPage();

                Step = "Navigate to Interviews Menu";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.INTERVIEWS.GetDescription());

                Step = "HR - Complete the Interview Status";
                pg_Hrms.HRChangeInterviewStatus(TestDataNode["interviewStatus"].InnerText, requisitionCode, TestDataNode["candidateName"].InnerText,
                                                TestDataNode["candidateStatus"].InnerText);

                Step = "Logout from Delta HRMS Application";
                pg_Hrms.LogoutFromDeltaHRMS();

                Step = "Login to Delta HRMS with Approver 1 credentials";
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["Approver1"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to Recuruitments page";
                pg_Hrms.NavigateToRecruitmentsPage();

                Step = "Navigate to Shortlisted & Selected Candidates Menu";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.SHORTLISTEDANDSELECTEDCANDIDATES.GetDescription());

                Step = "Management - Complete the Selection Process";
                pg_Hrms.ManagementCompleteSelectionProcess(requisitionCode, TestDataNode["candidateName"].InnerText, TestDataNode["Interviewr1status"].InnerText);

                Step = "Navigate to Recuruitments page";
                pg_Hrms.NavigateToRecruitmentsPage();

                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.INTERVIEWS.GetDescription());

                Step = "Navigate to Recuruitments page";
                pg_Hrms.NavigateToRecruitmentsPage();

                Step = "Navigate to Shortlisted & Selected Candidates Menu";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.SHORTLISTEDANDSELECTEDCANDIDATES.GetDescription());

                Step = "Verify the Candidate Status in Shortlisted & Selected Candidates";
                pg_Hrms.VerifyShortlistedCandidateStauts(requisitionCode, TestDataNode["candidateName"].InnerText, TestDataNode["Interviewr1status"].InnerText);

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
