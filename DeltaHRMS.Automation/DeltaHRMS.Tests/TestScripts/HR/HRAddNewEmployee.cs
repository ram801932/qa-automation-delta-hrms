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
    [Script("", "", "DeltaHRMS", "HR", "Add Employee", "Login as HR and Create a new Employee", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class HRAddNewEmployee : BaseTest
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
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["HrMangerUserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to HR Page => Employees";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.EMPLOYEES.GetDescription());

                Step = "Navigate to Add New Employee Page";
                pg_Hrms.AddNewEmployeeButton();

                Step = "Enter details in Add Employee Options page";
                string empId = pg_Hrms.EnterEmployeeOfficialDetails(TestDataNode["Prefix"].InnerText, TestDataNode["firstName"].InnerText, 
                                                        TestDataNode["lastName"].InnerText, TestDataNode["modeEmp"].InnerText, 
                                                        TestDataNode["role"].InnerText, "testautomation" + DateTime.Now.ToString("ddmmyy") + "@deltaintech.com",
                                                        TestDataNode["busUnit"].InnerText, TestDataNode["dept"].InnerText,
                                                        TestDataNode["repManager"].InnerText, TestDataNode["emplStatus"].InnerText);

                Step = "Select the Holiday Group";
                pg_Hrms.AddEmployeeSelectHolidayGroup(TestDataNode["holidayGrp"].InnerText);

                Step = "Enter Employee Personal Details";
                pg_Hrms.AddEmployeeEnterPersonalDetails(TestDataNode["gender"].InnerText, TestDataNode["maritalStatus"].InnerText, TestDataNode["nationality"].InnerText,
                                                        TestDataNode["ethinicCode"].InnerText, TestDataNode["raceCode"].InnerText, TestDataNode["language"].InnerText,
                                                        TestDataNode["date"].InnerText, TestDataNode["BloodGrp"].InnerText);

                Step = "Add Contact Details in Add Employee Page";
                pg_Hrms.AddEmployeeContactDetails(TestDataNode["persEmail"].InnerText, TestDataNode["streetAdd"].InnerText, TestDataNode["country"].InnerText,
                                                    TestDataNode["state"].InnerText, TestDataNode["city"].InnerText, TestDataNode["postalCode"].InnerText);

                Step = "Add new Skills for newly added employee";
                pg_Hrms.AddSkillAddNewEmployee(TestDataNode["skillName"].InnerText, TestDataNode["expYears"].InnerText, 
                                                TestDataNode["skillLevel"].InnerText, DateTime.Now.AddMonths(-1).ToString("dd-MM-yyyy"));

                Step = "Add Employement details in Add New Employee page";
                pg_Hrms.AddEmployeeExperince(TestDataNode["compName"].InnerText, TestDataNode["compWeb"].InnerText, TestDataNode["designation"].InnerText,
                                            TestDataNode["fromDate"].InnerText, TestDataNode["toDate"].InnerText, TestDataNode["reasonForLeaveing"].InnerText,
                                            TestDataNode["refName"].InnerText, TestDataNode["refContact"].InnerText, TestDataNode["refEmail"].InnerText);

                Step = "Add Education Details for Employee";
                pg_Hrms.AddEmployeeAddEducationDetails(TestDataNode["eduLevel"].InnerText, TestDataNode["instName"].InnerText, TestDataNode["course"].InnerText,
                                                        TestDataNode["edufromDate"].InnerText, TestDataNode["edutoDate"].InnerText, TestDataNode["percentage"].InnerText);

                Step = "Navigate to HR page";
                pg_Hrms.NavigateToHrPage();

                Step = "Navigate to HR Page => Employees";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.EMPLOYEES.GetDescription());

                Step = "Verify if Employee Exists in HR Records";
                pg_Hrms.SearchEmployeeInHrPage("Employee Id", empId, TestDataNode["firstName"].InnerText + " " + TestDataNode["lastName"].InnerText);

                pg_Hrms.CallAStoredProcedure("deleteemployee");

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
