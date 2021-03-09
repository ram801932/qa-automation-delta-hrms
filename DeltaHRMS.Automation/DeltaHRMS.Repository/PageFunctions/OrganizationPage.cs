#region Microsoft Reference
using System;
using System.Xml;
#endregion

#region Selenium Reference
using OpenQA.Selenium.Remote;
#endregion

#region Delta Automation Reference
using DeltaHRMS.Repository.CommonFunctions;
using static DeltaHRMS.Repository.PageFunctions.Constants;
using OpenQA.Selenium;
using DeltaHRMS.Accelerators.Utilities;
using DeltaHRMS.Accelerators.Reporting;
using DeltaHRMS.Accelerators.UtilityClasses;
using System.IO;
using System.Collections.Generic;
using System.Data;
using DeltaHRMS.Accelerators;
#endregion

namespace DeltaHRMS.Repository.PageFunctions
{
    /// <summary>
    ///  Represents PageFunction. Inherates from BasePage.
    /// </summary>
    public partial class DeltaHRMSApplication : Common
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region PageFunction
        
        /// <summary>
        /// used to add a new department in the Organization
        /// </summary>
        /// <param name="departmentName"></param>
        /// <param name="businessUnit"></param>
        public void AddNewDepartmentInOrganization(string departmentName, string businessUnit)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add Department: {0} in Organization Page", departmentName)));

                bool result = VerifyIfDepartmentExistsInOrganization(departmentName, businessUnit);

                if (result)
                {
                    Reporter.Add(new Act(string.Format("Department: {0} Added successfully in the Organization Page", departmentName)));
                }
                else
                {
                    ObjectClick(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSADDDEPARTMENTBTN.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.DEPARTMENTSADDDEPARTMENTBTN.GetDescription(), 5);

                    VerifyPageLoad();

                    SetObjectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSDEPNAME.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSDEPNAME.GetDescription(), departmentName, 5);

                    DropdownSelectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSBUSINESSUNIT.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSBUSINESSUNIT.GetDescription(), businessUnit, "text", 5);

                    SetObjectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSDEPCODE.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSDEPCODE.GetDescription(), departmentName, 5);

                    if(RetrieveObjectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSSTREETADD1.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSSTREETADD1.GetDescription(), "text").Equals(""))
                    {
                        SetObjectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSSTREETADD1.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADDDEPARTMENTSSTREETADD1.GetDescription(), departmentName, 5);
                    }

                    ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SUBMITBUTTON.GetDescription()),
                        GENERICOBJECTS.SUBMITBUTTON.GetDescription(), 5);

                    VerifyPageLoad();

                    result  = VerifyIfDepartmentExistsInOrganization(departmentName, businessUnit);

                    if (result)
                    {
                        Reporter.Add(new Act(string.Format("Department: {0} Added successfully in the Organization Page", departmentName)));
                    }
                    else
                    {
                        Reporter.Add(new Act(string.Format("Could not add Department: {0} in the Organization Page", departmentName), false, Driver));
                        throw new Exception(string.Format("Could not add Department: {0} in the Organization Page", departmentName));
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddNewDepartmentInOrganization() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to verify if Department exists in Organization
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        public bool VerifyIfDepartmentExistsInOrganization(string departmentName, string businessUnit)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify Department: {0}, exists in Organization Page", departmentName)));
                int count = 0;

                int pageCount = 0;
                bool found = false;

                if (CheckIfObjectExists(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLELASTPAGEBTN.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLELASTPAGEBTN.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var departments = Driver.FindElements(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLE.GetDescription()));

                    for (int j = 0; j < departments.Count; j++)
                    {
                        if (departments[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(departmentName) &&
                            departments[j].FindElements(By.TagName("td"))[6].HighLightObject(Driver).Text.Equals(businessUnit))
                        {
                            count = count + 1;
                            found = true;
                            break;
                        }
                    }

                    if (found == true)
                    {
                        break;
                    }
                    if (pageCount > 1 && i < pageCount - 1 && found == false)
                    {
                        JavaScriptClick(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLENEXTPAGEBTN.GetDescription()),
                            ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLENEXTPAGEBTN.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Department: {0} exists in the Organization Page", departmentName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Department: {0} doesnot exists in the Organization Page", departmentName)));
                }

                return found;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'VerifyIfDepartmentExistsInOrganization() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add new Business Unit in Organization
        /// </summary>
        /// <param name="businessUnit"></param>
        public void AddNewBusinessUnitInOrganization(string businessUnit)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add new Business Unit: {0} in Organization Page", businessUnit)));

                bool result = VerifyIfBusinessUnitExistsInOrganization(businessUnit);

                if (result)
                {
                    Reporter.Add(new Act(string.Format("Business Unit: {0} Added successfully in the Organization Page", businessUnit)));
                }
                else
                {
                    ObjectClick(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADDBUSINESSUNITBTN.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADDBUSINESSUNITBTN.GetDescription(), 5);

                    VerifyPageLoad();

                    SetObjectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADDBUSINESSUNITNAME.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADDBUSINESSUNITNAME.GetDescription(), businessUnit, 5);

                    SetObjectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADDBUSINESSUNITUNITCODE.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADDBUSINESSUNITUNITCODE.GetDescription(), businessUnit, 5);

                    if (RetrieveObjectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADBUSINESSUNTISTREETADD1.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADBUSINESSUNTISTREETADD1.GetDescription(), "text").Equals(""))
                    {
                        SetObjectValue(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.ADBUSINESSUNTISTREETADD1.GetDescription()),
                        ORGANIZATIONPAGEOBJECTS.ADBUSINESSUNTISTREETADD1.GetDescription(), businessUnit, 5);
                    }

                    ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SUBMITBUTTON.GetDescription()),
                        GENERICOBJECTS.SUBMITBUTTON.GetDescription(), 5);

                    VerifyPageLoad();

                    result = VerifyIfBusinessUnitExistsInOrganization(businessUnit);

                    if (result)
                    {
                        Reporter.Add(new Act(string.Format("Business Unit: {0} Added successfully in the Organization Page", businessUnit)));
                    }
                    else
                    {
                        Reporter.Add(new Act(string.Format("Could not add Business Unit: {0} in the Organization Page", businessUnit), false, Driver));
                        throw new Exception(string.Format("Could not add Business Unit: {0} in the Organization Page", businessUnit));
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddNewBusinessUnitInOrganization() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to verify if business unit exists in organization
        /// </summary>
        /// <param name="businessUnit"></param>
        /// <returns></returns>
        public bool VerifyIfBusinessUnitExistsInOrganization(string businessUnit)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify Business Unit: {0}, exists in Organization Page", businessUnit)));
                int count = 0;

                int pageCount = 0;
                bool found = false;

                if (CheckIfObjectExists(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLELASTPAGEBTN.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLELASTPAGEBTN.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var businessUnits = Driver.FindElements(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLE.GetDescription()));

                    for (int j = 0; j < businessUnits.Count; j++)
                    {
                        if (businessUnits[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(businessUnit))
                        {
                            count = count + 1;
                            found = true;
                            break;
                        }
                    }

                    if (found == true)
                    {
                        break;
                    }
                    if (pageCount > 1 && i < pageCount - 1 && found == false)
                    {
                        JavaScriptClick(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLENEXTPAGEBTN.GetDescription()),
                            ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLENEXTPAGEBTN.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Business Unit: {0} exists in the Organization Page", businessUnit)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Business Unit: {0} doesnot exists in the Organization Page", businessUnit)));
                }

                return found;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'VerifyIfBusinessUnitExistsInOrganization() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to delete the department from the organization page
        /// </summary>
        /// <param name="departmentName"></param>
        /// <param name="businessUnit"></param>
        public void DeleteDepartmentInOrganization(string departmentName, string businessUnit)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Delete the Department: {0}, in Organization Page", departmentName)));
                int count = 0;

                int pageCount = 0;
                bool found = false;

                if (CheckIfObjectExists(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLELASTPAGEBTN.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLELASTPAGEBTN.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var departments = Driver.FindElements(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLE.GetDescription()));

                    for (int j = 0; j < departments.Count; j++)
                    {
                        if (departments[j].FindElements(By.TagName("td"))[1].Text.Equals(departmentName) &&
                            departments[j].FindElements(By.TagName("td"))[6].Text.Equals(businessUnit))
                        {
                            departments[j].FindElements(By.TagName("td"))[0].HighLightObject(Driver).FindElements(By.TagName("a"))[2].Click();

                            ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.POPUPOKBUTTON.GetDescription()),
                                GENERICOBJECTS.POPUPOKBUTTON.GetDescription(), 5);

                            VerifyPageLoad();
                            count = count + 1;
                            found = true;
                            break;
                        }
                    }

                    if (found == true)
                    {
                        break;
                    }
                    if (pageCount > 1 && i < pageCount - 1 && found == false)
                    {
                        JavaScriptClick(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLENEXTPAGEBTN.GetDescription()),
                            ORGANIZATIONPAGEOBJECTS.DEPARTMENTSTABLENEXTPAGEBTN.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Department: {0} deleted successfully in the Organization Page", departmentName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Could not delete the Department: {0} in the Organization Page", departmentName)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'DeleteDepartmentInOrganization() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to delete the business unit in the Organizations page
        /// </summary>
        /// <param name="businessUnit"></param>
        public void DeleteBusinessUnitInOrganization(string businessUnit)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Delete Business Unit: {0}, in Organization Page", businessUnit)));
                int count = 0;

                int pageCount = 0;
                bool found = false;

                if (CheckIfObjectExists(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLELASTPAGEBTN.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLELASTPAGEBTN.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var businessUnits = Driver.FindElements(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLE.GetDescription()));

                    for (int j = 0; j < businessUnits.Count; j++)
                    {
                        if (businessUnits[j].FindElements(By.TagName("td"))[1].Text.Equals(businessUnit))
                        {
                            PageScrollDown(businessUnits[j].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2], 10);

                            businessUnits[j].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2].HighLightObject(Driver).Click();

                            ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.POPUPOKBUTTON.GetDescription()),
                                GENERICOBJECTS.POPUPOKBUTTON.GetDescription(), 5);

                            VerifyPageLoad();

                            count = count + 1;
                            found = true;
                            break;
                        }
                    }

                    if (found == true)
                    {
                        break;
                    }
                    if (pageCount > 1 && i < pageCount - 1 && found == false)
                    {
                        JavaScriptClick(Locator.GetLocator(PAGE.ORGANIZATION.GetDescription(), ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLENEXTPAGEBTN.GetDescription()),
                            ORGANIZATIONPAGEOBJECTS.BUSINESSUNITSTABLENEXTPAGEBTN.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Business Unit: {0} deleted successfully in the Organization Page", businessUnit)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Could not delete the Business Unit: {0} in the Organization Page", businessUnit)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'DeleteBusinessUnitInOrganization() function' {0}", ex.Message));
            }
        }

        #endregion
    }
}