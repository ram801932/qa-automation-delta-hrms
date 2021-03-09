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
    [Script("", "", "DeltaHRMS", "HR", "Roles And Privileges", "Add, view and delete New Role In Roles And Privileges", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class AddNewRoleInRolesAndPrivileges : BaseTest
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

                Step = "Navigate to HR Page => Roles & Privileges";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.ROLESANDPRIVILEGES.GetDescription());

                Step = "Add a New Role and selecting the privileges";
                pg_Hrms.AddNewRoleInRolesAndPrivileges(TestDataNode["roleName"].InnerText, TestDataNode["roleType"].InnerText, TestDataNode["description"].InnerText,
                                                   TestDataNode["hirarchy"].InnerText);

                Step = "Verify the Role in Roles and Privileges";
                pg_Hrms.VerifyRolesPriviligesTable(TestDataNode["roleName"].InnerText, TestDataNode["roleType"].InnerText);

                Step = "Delete the Role in Roles and Privileges";
                pg_Hrms.DeleteRolesFromRolesAndPrivilegesTable(TestDataNode["roleName"].InnerText, TestDataNode["roleType"].InnerText);


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
