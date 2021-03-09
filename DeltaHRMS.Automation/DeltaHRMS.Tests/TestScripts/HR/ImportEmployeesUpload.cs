﻿#region Microsoft references

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
    [Script("", "", "DeltaHRMS", "HR", "Import Employees Upload", "Login as HR and Import employees in bulk", "Regression")]
    /// <summary>
    ///  HR - Add a new Employee.
    /// </summary>
    class ImportEmployeesUpload : BaseTest
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

                /* Load the desired data in the import_employees.xlsx in DeltaHRMS.Tests => Test Data folder, 
                Make sure in properties of import_employees.xlsx(Right click on doc and select properties) Set Copy to Output directory as Copy Always 
                Uncomment the below steps 48,49 to validate the uploaded data*/

                //Step = "Import the Employees by uploading excel";
                //pg_Hrms.UploadEmployeeImportEmployees();

                //Step = "Validate the Employees are Added to the HR Database";
                //pg_Hrms.ValidateMassUplaodEmployees();

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
