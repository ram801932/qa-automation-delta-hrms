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
    [Script("", "", "DeltaHRMS", "Organization", "Business Unit & Departments", "Add a new Business Unit & Departments", "Regression")]
    /// <summary>
    ///  Recruitments - Create Requsition
    /// </summary>
    class AddNewBusinessUnitAndDepartment : BaseTest
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
                pg_Hrms.LoginToDeltaHRMS(TestDataNode["UserName"].InnerText, TestDataNode["Password"].InnerText);

                Step = "Navigate from Home Page => Organiztion Page";
                pg_Hrms.NavigateToOrganizationPage();

                Step = "Navigate to Business Units Sub Menu in Organization Tab";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.BUSINESSUNITS.GetDescription());

                Step = "Create a new Business Unit";
                pg_Hrms.AddNewBusinessUnitInOrganization(TestDataNode["busUnit"].InnerText);

                Step = "Navigate from Home Page => Organiztion Page";
                pg_Hrms.NavigateToOrganizationPage();

                Step = "Navigate to Departments Sub Menu in Organization Tab";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.DEPARTMENTS.GetDescription());

                Step = "Create a new Department with the Business Unit";
                pg_Hrms.AddNewDepartmentInOrganization(TestDataNode["dept"].InnerText, TestDataNode["busUnit"].InnerText);

                Step = "Delete the newly created Department";
                pg_Hrms.DeleteDepartmentInOrganization(TestDataNode["dept"].InnerText, TestDataNode["busUnit"].InnerText);

                Step = "Navigate from Home Page => Organiztion Page";
                pg_Hrms.NavigateToOrganizationPage();

                Step = "Navigate to Business Units Sub Menu in Organization Tab";
                pg_Hrms.SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.BUSINESSUNITS.GetDescription());

                Step = "Delete the newly created Business Unit in Organization tab";
                pg_Hrms.DeleteBusinessUnitInOrganization(TestDataNode["busUnit"].InnerText);

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
