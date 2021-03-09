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
using AutoIt;
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
        /// Used to Click on Add New Employee Button
        /// </summary>
        public void AddNewEmployeeButton()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Click on Add New Employee Button")));
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDEMPLOYEEBTN.GetDescription()),
                            HRPAGEOBJECTS.ADDEMPLOYEEBTN.GetDescription(), 5);
                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'AddNewEmployeeButton() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EnterEmployeeOfficialDetails(string prefix, string firstName, string lastName, string modeEmp, string role, string email, string busUnit, string dept, string repManager, string emplStatus)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Enter the values in the Add New Employee Options Page")));

                string menuXPath = String.Format(GetXpathString(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription()), "Official");

                JavaScriptClick(By.XPath(menuXPath), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription());
                VerifyPageLoad();

                // Select Prefix
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFPREFIX.GetDescription()),
                    HRPAGEOBJECTS.OFFPREFIX.GetDescription(), prefix, "text", 5);

                // Set FirstName and Last Name
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFFIRSTNAME.GetDescription()),
                    HRPAGEOBJECTS.OFFFIRSTNAME.GetDescription(), firstName, 5);

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFLASTNAME.GetDescription()),
                    HRPAGEOBJECTS.OFFLASTNAME.GetDescription(), lastName, 5);

                // Select Mode of Employment
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFMODEOFEMPLOYMENT.GetDescription()),
                   HRPAGEOBJECTS.OFFMODEOFEMPLOYMENT.GetDescription(), modeEmp, "text", 5);

                // Select Role
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFROLE.GetDescription()),
                   HRPAGEOBJECTS.OFFROLE.GetDescription(), role, "text", 5);

                // Set the value of email
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFEMAIL.GetDescription()),
                    HRPAGEOBJECTS.OFFEMAIL.GetDescription(), email, 5);

                // Select Business Unit
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFBUSINESSUNIT.GetDescription()),
                   HRPAGEOBJECTS.OFFBUSINESSUNIT.GetDescription(), busUnit, "text", 5);

                Wait(2000);

                // Select Deptartment
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFDEPARTMENT.GetDescription()),
                   HRPAGEOBJECTS.OFFDEPARTMENT.GetDescription(), dept, "text", 5);

                Wait(2000);
                // Select Reporting Manager
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFREPORTINGMANAGER.GetDescription()),
                   HRPAGEOBJECTS.OFFREPORTINGMANAGER.GetDescription(), repManager, "text", 5);

                Wait(2000);
                // Select Employment Status
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFEMPSTATUS.GetDescription()),
                   HRPAGEOBJECTS.OFFEMPSTATUS.GetDescription(), emplStatus, "text", 5);

                // Select date of JOining

                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                var element = Driver.FindElement(By.XPath("//input[@id='date_of_joining' and @name = 'date_of_joining']"));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                element.Click();

                SelectDateFromCalender(DateTime.Now.ToString("dd-MM-yyyy"));

                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFSAVEBTN.GetDescription()),
                    HRPAGEOBJECTS.OFFSAVEBTN.GetDescription());

                VerifyPageLoad();
                string empId = RetrieveObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFEMPID.GetDescription()), "value");
                return empId;
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'EnterEmployeeOfficialDetails() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to select the holiday Group
        /// </summary>
        /// <param name="holidayGrp"></param>
        public void AddEmployeeSelectHolidayGroup(string holidayGrp)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Select Holiday group in Add Employee")));

                string menuXPath = String.Format(GetXpathString(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription()), "Holidays");

                JavaScriptClick(By.XPath(menuXPath), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription());
                VerifyPageLoad();

                // Select Holiday Group
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HDAYHOLIDAYGROUP.GetDescription()),
                    HRPAGEOBJECTS.HDAYHOLIDAYGROUP.GetDescription());

                var holidayGroup = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HDAYSELECTOPTIONS.GetDescription()));

                for (int i = 0; i < holidayGroup.Count; i++)
                {
                    if (holidayGroup[i].Text.Equals(holidayGrp))
                    {
                        holidayGroup[i].Click();
                        break;
                    }
                }

                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HDAYSAVEBTN.GetDescription()),
                    HRPAGEOBJECTS.HDAYSAVEBTN.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddEmployeeSelectHolidayGroup() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// used for entering personal details in add employee page
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="maritalStatus"></param>
        /// <param name="nationality"></param>
        /// <param name="ethinicCode"></param>
        /// <param name="raceCode"></param>
        /// <param name="language"></param>
        /// <param name="date"></param>
        /// <param name="BloodGrp"></param>
        public void AddEmployeeEnterPersonalDetails(string gender, string maritalStatus, string nationality, string ethinicCode, string raceCode, string language, string date, string BloodGrp)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Enter the Personal Details in Add Employee")));

                string menuXPath = String.Format(GetXpathString(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription()), "Personal");

                JavaScriptClick(By.XPath(menuXPath), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription());
                VerifyPageLoad();

                // Select Gender
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALGENDER.GetDescription()),
                    HRPAGEOBJECTS.PERSONALGENDER.GetDescription());

                var gen = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALGENDERSELECT.GetDescription()));

                for (int i = 0; i < gen.Count; i++)
                {
                    if (gen[i].Text.Equals(gender))
                    {
                        gen[i].Click();
                        break;
                    }
                }

                // Select Marital Status
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALMARTIALSTATUS.GetDescription()),
                    HRPAGEOBJECTS.PERSONALMARTIALSTATUS.GetDescription());

                var maritalSts = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALMARTIALSELECT.GetDescription()));

                for (int i = 0; i < maritalSts.Count; i++)
                {
                    if (maritalSts[i].Text.Equals(maritalStatus))
                    {
                        maritalSts[i].Click();
                        break;
                    }
                }

                // Select Nationality
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALNATIONALITY.GetDescription()),
                    HRPAGEOBJECTS.PERSONALNATIONALITY.GetDescription());

                var natOptions = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SELECTNATIONALITYOPTIONS.GetDescription()));

                for (int i = 0; i < natOptions.Count; i++)
                {
                    if (natOptions[i].Text.Equals(nationality))
                    {
                        natOptions[i].Click();
                        break;
                    }
                }

                // Select Ethnic Code
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALETHINICCODE.GetDescription()),
                    HRPAGEOBJECTS.PERSONALETHINICCODE.GetDescription());

                var ethinicOpt = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SELECTETHINICCODEOPTIONS.GetDescription()));

                for (int i = 0; i < ethinicOpt.Count; i++)
                {
                    if (ethinicOpt[i].Text.Equals(ethinicCode))
                    {
                        ethinicOpt[i].Click();
                        break;
                    }
                }

                // Select Race Code
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALRACECCODE.GetDescription()),
                    HRPAGEOBJECTS.PERSONALRACECCODE.GetDescription());

                var RaceCodeOpt = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALRACECODEOPTIONS.GetDescription()));

                for (int i = 0; i < RaceCodeOpt.Count; i++)
                {
                    if (RaceCodeOpt[i].Text.Equals(raceCode))
                    {
                        RaceCodeOpt[i].Click();
                        break;
                    }
                }

                // Select Language
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALLANGUAGE.GetDescription()),
                    HRPAGEOBJECTS.PERSONALLANGUAGE.GetDescription());

                var languageOpt = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALLANGUAGEOPTIONS.GetDescription()));

                for (int i = 0; i < languageOpt.Count; i++)
                {
                    if (languageOpt[i].Text.Equals(language))
                    {
                        languageOpt[i].Click();
                        break;
                    }
                }

                // Select Date of Birth

                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                var element = Driver.FindElement(By.XPath("//input[@id='dob']"));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                element.Click();

                SelectDateFromCalender(date);

                // Enter Blood Group
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.BLOODGROUP.GetDescription()),
                    HRPAGEOBJECTS.BLOODGROUP.GetDescription(), BloodGrp, 5);

                // Save the details
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.PERSONALSAVEBTN.GetDescription()),
                    HRPAGEOBJECTS.PERSONALSAVEBTN.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddEmployeeEnterPersonalDetails() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add contact details
        /// </summary>
        /// <param name="persEmail"></param>
        /// <param name="streetAdd"></param>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <param name="postalCode"></param>
        public void AddEmployeeContactDetails(string persEmail, string streetAdd, string country, string state, string city, string postalCode)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Enter the Contact Details in Add Employee")));

                string menuXPath = String.Format(GetXpathString(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription()), "Contact");

                JavaScriptClick(By.XPath(menuXPath), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription());
                VerifyPageLoad();

                // Enter Blood Group
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTPERSONALEMAIL.GetDescription()),
                    HRPAGEOBJECTS.CNTPERSONALEMAIL.GetDescription(), persEmail, 5);

                // Enter Blood Group
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTPERMANENTSTREETADDRESS.GetDescription()),
                    HRPAGEOBJECTS.CNTPERMANENTSTREETADDRESS.GetDescription(), streetAdd, 5);

                // Select Current Country
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTPERMANENTCOUNTRY.GetDescription()),
                    HRPAGEOBJECTS.CNTPERMANENTCOUNTRY.GetDescription());

                var permanentCountry = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTSELECTOPTIONS.GetDescription()));

                for (int i = 0; i < permanentCountry.Count; i++)
                {
                    if (permanentCountry[i].Text.Equals(country))
                    {
                        permanentCountry[i].Click();
                        break;
                    }
                }

                Wait(2000);
                // Select Current State
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTPERMANENTSTATE.GetDescription()),
                    HRPAGEOBJECTS.CNTPERMANENTSTATE.GetDescription());

                var permanentState = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTPERMANENTSTATEOPTIONS.GetDescription()));

                for (int i = 0; i < permanentState.Count; i++)
                {
                    if (permanentState[i].Text.Equals(state))
                    {
                        permanentState[i].Click();
                        break;
                    }
                }

                Wait(2000);
                // Select Current City
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTPERMANENTCITY.GetDescription()),
                    HRPAGEOBJECTS.CNTPERMANENTCITY.GetDescription());

                var permanentCity = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTPERMANENTCITYOPTIONS.GetDescription()));

                for (int i = 0; i < permanentCity.Count; i++)
                {
                    if (permanentCity[i].Text.Equals(city))
                    {
                        permanentCity[i].Click();
                        break;
                    }
                }

                // Enter Postal Code
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTPERMANENTPOSTALCODE.GetDescription()),
                    HRPAGEOBJECTS.CNTPERMANENTPOSTALCODE.GetDescription(), postalCode, 5);

                // Select Current City
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTCHECKBOX.GetDescription()),
                    HRPAGEOBJECTS.CNTCHECKBOX.GetDescription());

                // Select Current City
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.CNTSUBMITBTN.GetDescription()), HRPAGEOBJECTS.CNTSUBMITBTN.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddEmployeeContactDetails() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add new skill in add new employee page
        /// </summary>
        /// <param name="skillName"></param>
        /// <param name="expYears"></param>
        /// <param name="comptLevel"></param>
        /// <param name="skillLevel"></param>
        /// <param name="date"></param>
        public void AddSkillAddNewEmployee(string skillName, string expYears, string skillLevel, string date)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add the Skills in Add Employee")));
                VerifyPageLoad();
                string menuXPath = String.Format(GetXpathString(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription()), "Skills");

                JavaScriptClick(By.XPath(menuXPath), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription());
                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SKILLSADDBTN.GetDescription()),
                    HRPAGEOBJECTS.SKILLSADDBTN.GetDescription(), 5);

                VerifyPageLoad();

                Driver.SwitchTo().Frame(Driver.FindElement(By.Id("empskillsCont")));
                // Enter Skill Name
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SKLSKILL.GetDescription()),
                    HRPAGEOBJECTS.SKLSKILL.GetDescription(), skillName, 5);

                // Set years of Experince
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SKLYEARSOFEXPERIENCE.GetDescription()),
                    HRPAGEOBJECTS.SKLYEARSOFEXPERIENCE.GetDescription(), expYears, 5);

                //Select the Competency Level
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SKLCOMPETENCYLEVEL.GetDescription()),
                    HRPAGEOBJECTS.SKLCOMPETENCYLEVEL.GetDescription(), 5);

                var skllOptions = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SKLSELECTOPTIONS.GetDescription()));

                for (int i = 0; i < skllOptions.Count; i++)
                {
                    if (skllOptions[i].Text.Equals(skillLevel))
                    {
                        skllOptions[i].Click();
                        break;
                    }
                }

                //Select the skill last used year
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SKLSKILLLASTUSEDYEAR.GetDescription()),
                    HRPAGEOBJECTS.SKLSKILLLASTUSEDYEAR.GetDescription(), 5);

                SelectDateFromCalender(date);

                // Save Skill
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SKLSAVEBTN.GetDescription()),
                    HRPAGEOBJECTS.SKLSAVEBTN.GetDescription());

                VerifyPageLoad();
                Driver.SwitchTo().ParentFrame();
                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddSkillAddNewEmployee() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add Employee EXperiene in add employee page
        /// </summary>
        /// <param name="compName"></param>
        /// <param name="compWeb"></param>
        /// <param name="designation"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reasonForLeaveing"></param>
        /// <param name="refName"></param>
        /// <param name="refContact"></param>
        /// <param name="refEmail"></param>
        public void AddEmployeeExperince(string compName, string compWeb, string designation, string fromDate, string toDate, string reasonForLeaveing, string refName, string refContact, string refEmail)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add Eployee Experince in Add Employee")));
                VerifyPageLoad();
                string menuXPath = String.Format(GetXpathString(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription()), "Experience");

                JavaScriptClick(By.XPath(menuXPath), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription());
                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCEADDBTN.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCEADDBTN.GetDescription(), 5);

                VerifyPageLoad();
                Wait(2000);
                Driver.SwitchTo().Frame(Driver.FindElement(By.Id("experiencedetailsCont")));
                // Set Company Name
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCECOMPNAME.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCECOMPNAME.GetDescription(), compName, 5);

                // Set Company Website
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCECOMPWEBSITE.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCECOMPWEBSITE.GetDescription(), compWeb, 5);

                // Set designation
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCEDESIGNATION.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCEDESIGNATION.GetDescription(), designation, 5);

                //Select from date
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCEFROMDATE.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCEFROMDATE.GetDescription(), 5);

                SelectDateFromCalender(fromDate);

                //Select to date
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCETODATE.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCETODATE.GetDescription(), 5);

                SelectDateFromCalender(toDate);

                // Set Reason for leaving
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCEREASONFORLEAVING.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCEREASONFORLEAVING.GetDescription(), reasonForLeaveing, 5);

                // Set Referrer Name
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCEREFERRERNAME.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCEREFERRERNAME.GetDescription(), refName, 5);

                // Set Referrer Contact
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCEREFERENCECONTACT.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCEREFERENCECONTACT.GetDescription(), refContact, 5);

                // Set Reason for leaving
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCEFERERENCEEMAIL.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCEFERERENCEEMAIL.GetDescription(), refEmail, 5);

                // Save the exp Details
                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EXPERIENCESAVEBTN.GetDescription()),
                    HRPAGEOBJECTS.EXPERIENCESAVEBTN.GetDescription());

                VerifyPageLoad();

                Driver.SwitchTo().ParentFrame();
                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddEmployeeExperince() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// used to add educational details in add employee page
        /// </summary>
        /// <param name="eduLevel"></param>
        /// <param name="instName"></param>
        /// <param name="course"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="percentage"></param>
        public void AddEmployeeAddEducationDetails(string eduLevel, string instName, string course, string fromDate, string toDate, string percentage)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add Education Details in Add Employee")));

                VerifyPageLoad();
                string menuXPath = String.Format(GetXpathString(PAGE.HR.GetDescription(), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription()), "Education");

                JavaScriptClick(By.XPath(menuXPath), HRPAGEOBJECTS.HRADDEMPLOYEESUBFIELDSMENU.GetDescription());
                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONADDBTN.GetDescription()),
                    HRPAGEOBJECTS.EDUCATIONADDBTN.GetDescription(), 5);

                VerifyPageLoad();

                Driver.SwitchTo().Frame(Driver.FindElement(By.Id("educationdetailsCont")));

                //Select the Education Level
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONLEVEL.GetDescription()),
                    HRPAGEOBJECTS.EDUCATIONLEVEL.GetDescription(), 5);

                var eduLevelOptions = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONLEVELOPTIONS.GetDescription()));

                for (int i = 0; i < eduLevelOptions.Count; i++)
                {
                    if (eduLevelOptions[i].Text.Equals(eduLevel))
                    {
                        eduLevelOptions[i].Click();
                        break;
                    }
                }

                // Set Education Inst Name
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONINSTITITUTIONNAME.GetDescription()),
                    HRPAGEOBJECTS.EDUCATIONINSTITITUTIONNAME.GetDescription(), instName, 5);

                // Set Course Name
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONCOURSE.GetDescription()),
                    HRPAGEOBJECTS.EDUCATIONCOURSE.GetDescription(), course, 5);

                //Set from date for course
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONFROMDATE.GetDescription()),
                    HRPAGEOBJECTS.EDUCATIONFROMDATE.GetDescription(), 5);
                SelectDateFromCalender(fromDate);

                //Set to date for course
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONTODATE.GetDescription()),
                    HRPAGEOBJECTS.EDUCATIONTODATE.GetDescription(), 5);
                SelectDateFromCalender(toDate);

                // Set Percentage
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONPERCENTAGE.GetDescription()),
                    HRPAGEOBJECTS.EDUCATIONPERCENTAGE.GetDescription(), percentage, 5);

                //Save Details
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EDUCATIONSAVEBTN.GetDescription()),
                    HRPAGEOBJECTS.EDUCATIONSAVEBTN.GetDescription(), 5);

                VerifyPageLoad();
                Driver.SwitchTo().ParentFrame();
                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddEmployeeAddEducationDetails() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to verify Employee exists in HR
        /// </summary>
        /// <param name="category"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public void SearchEmployeeInHrPage(string category, string value, string name)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Search Employee in HR Page")));

                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SELECTSEARCHCATEGORY.GetDescription()),
                    HRPAGEOBJECTS.SELECTSEARCHCATEGORY.GetDescription(), category, "text", 5);

                switch (category)
                {
                    case "Employee Id":
                        SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SEARCHVALUEFIELD.GetDescription()),
                                        HRPAGEOBJECTS.SEARCHVALUEFIELD.GetDescription(), value, 5);
                        break;

                    case "Employee Name":
                        SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SEARCHVALUEFIELD.GetDescription()),
                                        HRPAGEOBJECTS.SEARCHVALUEFIELD.GetDescription(), value, 5);
                        break;

                    case "Employee Role":
                        DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SEARCHSELECTROLE.GetDescription()),
                    HRPAGEOBJECTS.SEARCHSELECTROLE.GetDescription(), value, "text", 5);
                        break;
                }

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.SEARCHBTN.GetDescription()),
                    HRPAGEOBJECTS.SEARCHBTN.GetDescription(), 5);
                VerifyPageLoad();

                int count = 0;
                var empNames = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EMPLOYEENAME.GetDescription()));

                for (int i = 0; i < empNames.Count; i++)
                {
                    if (empNames[i].Text.Contains(name))
                    {
                        count = count + 1;
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("User: {0} exists in the HR Database", name)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("User: {0} does not exists in the HR Database", name), false, Driver));
                    throw new Exception(string.Format("User: {0} does not exists in the HR Database", name));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'SearchEmployeeInHrPage() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to Download the Import Format in the HR Page
        /// </summary>
        public void DownloadImportFormat()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to download import format in HR Page")));

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.IMPORTFORMATBTN.GetDescription()),
                    HRPAGEOBJECTS.IMPORTFORMATBTN.GetDescription(), 5);

                VerifyPageLoad();

                string fileName = string.Format(@"C:\automationdownload\import_employees.xlsx");

                bool result = new FileReaderWriter().CopyFileToCurrentWorkingDirector(fileName);
                if (result)
                {
                    Reporter.Add(new Act(string.Format("Import Employees Format File Downloaded successfully")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Import Employees Format File didnt Download"), false, Driver));
                    throw new Exception(string.Format("Import Employees Format File didnt Download"));
                }

                FileReaderWriter fileRW = new FileReaderWriter();
                fileRW.DeleteFiles(@"C:\automationdownload", new string[] { "import_employees.xlsx" });
                fileRW.DeleteFiles(Directory.GetCurrentDirectory(), new string[] { "import_employees.xlsx" });

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'DownloadImportFormat() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to edit the employee name in hr page
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="name"></param>
        public void EditEmployeeNameInEmployeePage(string firstName, string name)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Edit Employee details in HR Page")));
                var empNames = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EMPLOYEENAME.GetDescription()));

                string prevName = empNames[0].Text;

                var actionBtns = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EMPLOYEEACTIONSBTN.GetDescription()));

                actionBtns[0].HighLightObject(Driver).Click();
                VerifyPageLoad();
                var editBtn = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EMPLOYEEACTIONSEDIT.GetDescription()));

                editBtn[0].HighLightObject(Driver).Click();

                VerifyPageLoad();
                
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFFIRSTNAME.GetDescription()),
                    HRPAGEOBJECTS.OFFFIRSTNAME.GetDescription(), firstName, 5);

                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.OFFSAVEBTN.GetDescription()),
                            HRPAGEOBJECTS.OFFSAVEBTN.GetDescription());

                VerifyPageLoad();

                NavigateToHrPage();
                SelectMenuFromSideBar(SIDEBARSUBMENUNAMES.EMPLOYEES.GetDescription());

                SearchEmployeeInHrPage("Employee Name", firstName, firstName);
                
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'EditEmployeeNameInEmployeePage() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to view the employee name in hr page
        /// </summary>
        /// <param name="firstName"></param>
        public void ViewEmployeeInformationInHrPage(string firstName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to view Employee Information in HR Page")));

                var actionBtns = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EMPLOYEEACTIONSBTN.GetDescription()));

                actionBtns[0].HighLightObject(Driver).Click();

                var viewBtn = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EMPLOYEEACTIONSVIEW.GetDescription()));

                viewBtn[0].HighLightObject(Driver).Click();

                VerifyPageLoad();

                if (RetrieveObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.EMPLOYEEVIEWFIRSTNAME.GetDescription()),
                            HRPAGEOBJECTS.EMPLOYEEVIEWFIRSTNAME.GetDescription(), "text").Contains(firstName))
                {
                    Reporter.Add(new Act(string.Format("Employee Name changed successfully")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Failed to update the Employee information"), false, Driver));
                    throw new Exception(string.Format("Failed to update the Employee information"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'ViewEmployeeInformationInHrPage() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to upload the Import Employee sheet with data
        /// </summary>
        public void UploadEmployeeImportEmployees()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Upload employees by Import Employees option in HR Page")));

                // Upload the jpeg image for reason
                IWebElement chooseFile = Driver.FindElement(By.XPath("//div[@class ='actions_div']/span[contains(.,'Import Employees')]"));
                chooseFile.HighLightObject(Driver).Click();

                string filelocation = Directory.GetCurrentDirectory() + @"\TestData\import_employees.xlsx";
                string newFileLocation = @"C:\automationdownload\import_employees.xlsx";

                File.Move(filelocation, newFileLocation);

                VerifyPageLoad();

                AutoItX.WinActivate("Open");
                AutoItX.Send(newFileLocation);
                Wait(1000);
                AutoItX.Send("{ENTER}");

                VerifyPageLoad();
                Wait(20000);

                File.Move(newFileLocation, filelocation);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'UploadEmployeeImportEmployees() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// used to validate if all the employees are successfully uploaded to the HR Database thru Import Mass Upload
        /// </summary>
        public void ValidateMassUplaodEmployees()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Validate the mass upload employees are successful in HR Page")));

                List<Tuple<string, string, string>> array = new List<Tuple<string, string, string>>();

                DataTable excel = ExcelHelper.ReadExcel(Directory.GetCurrentDirectory() + @"\TestData\import_employees.xlsx", "Sheet1");

                for (int i = 0; i < excel.Rows.Count; i++)
                {
                    array.Add(Tuple.Create(Convert.ToString(excel.Rows[i].ItemArray.GetValue(3)), Convert.ToString(excel.Rows[i].ItemArray.GetValue(1)), Convert.ToString(excel.Rows[i].ItemArray.GetValue(2))));
                }

                foreach (var tuple in array)
                {
                    SearchEmployeeInHrPage("Employee Id", tuple.Item1, tuple.Item2 + " " + tuple.Item3);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'UploadEmployeeImportEmployees() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add a new holiday Group 
        /// </summary>
        /// <param name="GroupName"></param>
        public void AddHolidayGroup(string GroupName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add the new Holiday Group in HR Page")));

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYGROUPBTN.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYGROUPBTN.GetDescription(), 5);

                VerifyPageLoad();

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYGRPGROUPNAME.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYGRPGROUPNAME.GetDescription(), GroupName, 5);

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYGRPDESCRIPTION.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYGRPDESCRIPTION.GetDescription(), "Automation - Add Holiday Group", 5);

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYGRPSAVEBTN.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYGRPSAVEBTN.GetDescription(), 5);

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddHolidayGroup() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to verify if holiday Group exists in the delta HRMS
        /// </summary>
        /// <param name="GroupName"></param>
        public void VerifyHolidayGroup(string GroupName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Holiday Group in HR Page")));
                int count = 0;
                var holidayGrps = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYGROUPTABLE.GetDescription()));

                for (int i = 0; i < holidayGrps.Count; i++)
                {
                    if (holidayGrps[i].FindElements(By.TagName("td"))[1].Text.Equals(GroupName))
                    {
                        count = count + 1;
                        break;
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Holiday Group exists in the Delta Holiday Groups")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Holiday Group doesnot exists in the Delta Holiday Groups"), false, Driver));
                    throw new Exception(string.Format("Holiday Group doesnot exists in the Delta Holiday Groups"));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'VerifyHolidayGroup() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to Delete the Holiday Group
        /// </summary>
        /// <param name="GroupName"></param>
        public void DeleteHolidayGroup(string GroupName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Holiday Group in HR Page")));
                int count = 0;
                var holidayGrps = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYGROUPTABLE.GetDescription()));

                for (int i = 0; i < holidayGrps.Count; i++)
                {
                    if (holidayGrps[i].FindElements(By.TagName("td"))[1].Text.Equals(GroupName))
                    {
                        holidayGrps[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2].HighLightObject(Driver).Click();

                        VerifyPageLoad();

                        ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription()),
                            HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription(), 5);
                        VerifyPageLoad();
                        count = count + 1;
                        break;
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Holiday Group Deleted from the Delta Holiday Groups")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Holiday Group not Deleted from the Delta Holiday Groups"), false, Driver));
                    throw new Exception(string.Format("Holiday Group not deleted from the Delta Holiday Groups"));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'DeleteHolidayGroup() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add a new holiday in the Manage Holidays Page
        /// </summary>
        /// <param name="holidayName"></param>
        /// <param name="groupName"></param>
        /// <param name="date"></param>
        public string ManageHoldiaysAddNewHoliday(string holidayName, string groupName, string date)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add the new Holiday in HR Page")));

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYGROUPBTN.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYGROUPBTN.GetDescription(), 5);

                VerifyPageLoad();

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYNAME.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYNAME.GetDescription(), holidayName, 5);

                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYGROUP.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYGROUP.GetDescription(), "1", "index", 5);

                string holGrp = RetrieveDropdownValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYGROUP.GetDescription()));

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYDATEBTN.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYDATEBTN.GetDescription(), 5);

                SelectDateFromCalender(date);

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYDESCRIPTION.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYDESCRIPTION.GetDescription(), "Automation - Add Holiday", 5);

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYSAVEBTN.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYSAVEBTN.GetDescription(), 5);

                VerifyPageLoad();

                return holGrp;
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddNewHoliday() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to verify if holiday exists in the delta HRMS
        /// </summary>
        /// <param name="holidayName"></param>
        /// <param name="holidayGrp"></param>
        /// <param name="date"></param>
        public void ManageHolidaysVerifyHoliday(string holidayName, string holidayGrp, string date)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Holiday in HR Page")));
                int count = 0;

                int pageCount = 0;
                bool found = false;
                if (CheckIfObjectExists(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYSLASTPAGEBTN.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYSLASTPAGEBTN.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var holidays = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYSTABLE.GetDescription()));

                    for (int j = 0; j < holidays.Count; j++)
                    {
                        if (holidays[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(holidayName)
                            && holidays[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(holidayGrp)
                            && holidays[j].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Equals(date))
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
                        JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLDIAYSNEXTPAGEBTN.GetDescription()),
                            HRPAGEOBJECTS.MANAGEHOLDIAYSNEXTPAGEBTN.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Holiday exists in the Delta Holiday List")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Holiday doesnot exists in the Delta Holiday List"), false, Driver));
                    throw new Exception(string.Format("Holiday doesnot exists in the Delta Holiday List"));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'ManageHolidaysVerifyHoliday() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to delete the holiday from Delta HRMS
        /// </summary>
        /// <param name="holidayName"></param>
        /// <param name="holidayGrp"></param>
        /// <param name="date"></param>
        public void ManageHolidaysDeleteHoliday(string holidayName, string holidayGrp, string date)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Delete the Holiday in HR Page")));
                int count = 0;

                int pageCount = 0;
                bool found = false;
                if (CheckIfObjectExists(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYSLASTPAGEBTN.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYSLASTPAGEBTN.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var holidays = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYSTABLE.GetDescription()));

                    for (int j = 0; j < holidays.Count; j++)
                    {
                        if (holidays[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(holidayName)
                            && holidays[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(holidayGrp)
                            && holidays[j].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Equals(date))
                        {
                            holidays[j].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2].HighLightObject(Driver).Click();

                            VerifyPageLoad();

                            ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription()),
                                HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription(), 5);

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
                        JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLDIAYSNEXTPAGEBTN.GetDescription()),
                            HRPAGEOBJECTS.MANAGEHOLDIAYSNEXTPAGEBTN.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Holiday deleted from successfully the Delta Holiday List")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Could not deleted the Holiday in the Delta Holiday List"), false, Driver));
                    throw new Exception(string.Format("Could not deleted the Holiday in the Delta Holiday List"));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'ManageHolidaysDeleteHoliday() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add New Leave Management Options
        /// </summary>
        /// <param name="businessUnit"></param>
        /// <param name="dept"></param>
        /// <param name="startMnth"></param>
        /// <param name="weekend1"></param>
        /// <param name="weekend2"></param>
        /// <param name="wrkingHrs"></param>
        /// <param name="hlfDayReq"></param>
        /// <param name="leaveTransfer"></param>
        /// <param name="skipHolidays"></param>
        /// <param name="hrManager"></param>
        /// <param name="description"></param>
        public void AddLeaveManagementOptions(string businessUnit, string dept, string startMnth, string weekend1, string weekend2, string wrkingHrs, string hlfDayReq, string leaveTransfer, string skipHolidays, string hrManager, string description)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add the new Leave Management Options")));

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ADDHOLIDAYGROUPBTN.GetDescription()),
                            HRPAGEOBJECTS.ADDHOLIDAYGROUPBTN.GetDescription(), 5);

                VerifyPageLoad();

                // Select Business unit name
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDBUSSUNITNAME.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDBUSSUNITNAME.GetDescription(), businessUnit, "text", 5);

                Wait(2000);
                // Select Department
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDDEPARTMENT.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDDEPARTMENT.GetDescription(), dept, "text", 5);

                // Select Calender start month
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDCALENDERSTARTMONTH.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDCALENDERSTARTMONTH.GetDescription(), startMnth, "text", 5);

                // Select Weekend Day 1
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDWEEKENDDAY1.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDWEEKENDDAY1.GetDescription(), weekend1, "text", 5);

                // Select Weekend Day 2
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDWEEKENDDAY2.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDWEEKENDDAY2.GetDescription(), weekend2, "text", 5);

                // Set Working Hours
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANGEMENTADDWORKINGHRS.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANGEMENTADDWORKINGHRS.GetDescription(), wrkingHrs, 5);

                // Select Half day Requests
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDHALFDAYREQUESTS.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDHALFDAYREQUESTS.GetDescription(), hlfDayReq, "text", 5);

                // Select Leave Transfer
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDALLOWLEAVETRANSFERS.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDALLOWLEAVETRANSFERS.GetDescription(), leaveTransfer, "text", 5);

                // Select skip Holidays
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDSKIPHOLIDAYS.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDSKIPHOLIDAYS.GetDescription(), skipHolidays, "text", 5);

                // Select HR Manager
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDHRMANAGER.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDHRMANAGER.GetDescription(), "1", "index", 5);

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDDESCRIPTION.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDDESCRIPTION.GetDescription(), description, 5);

                // Save the Values
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTADDSUBMITBTN.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTADDSUBMITBTN.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddLeaveManagementOptions() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// used to verify the Leave Manegement Options
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="weekend1"></param>
        /// <param name="weekend2"></param>
        public void VerifyLeaveManagementOptions(string dept, string weekend1, string weekend2)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Leave Management Options")));
                int count = 0;

                int pageCount = 0;
                bool found = false;
                if (CheckIfObjectExists(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTOPTINSLASTPAGE.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTOPTINSLASTPAGE.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var leaveMangementOptions = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTOPTIONSTABLE.GetDescription()));

                    for (int j = 0; j < leaveMangementOptions.Count; j++)
                    {
                        if (leaveMangementOptions[j].FindElements(By.TagName("td"))[4].HighLightObject(Driver).Text.Equals(dept)
                            && leaveMangementOptions[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(weekend1)
                            && leaveMangementOptions[j].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Equals(weekend2))
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
                        JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTOPTIONSNEXTPAGE.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTOPTIONSNEXTPAGE.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Leave Management Options exists for the Business Unit")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Leave Management Options doesnot exists for the Business Unit"), false, Driver));
                    throw new Exception(string.Format("Leave Management Options doesnot exists for the Business Unit"));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'VerifyLeaveManagementOptions() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to delete the Leave Management Options
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="weekend1"></param>
        /// <param name="weekend2"></param>
        public void DeleteLeaveManagementOptions(string dept, string weekend1, string weekend2)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Leave Management Options")));
                int count = 0;

                int pageCount = 0;
                bool found = false;
                if (CheckIfObjectExists(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTOPTINSLASTPAGE.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTOPTINSLASTPAGE.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var leaveMangementOptions = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTOPTIONSTABLE.GetDescription()));

                    for (int j = 0; j < leaveMangementOptions.Count; j++)
                    {
                        if (leaveMangementOptions[j].FindElements(By.TagName("td"))[4].HighLightObject(Driver).Text.Equals(dept)
                            && leaveMangementOptions[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(weekend1)
                            && leaveMangementOptions[j].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Equals(weekend2))
                        {
                            leaveMangementOptions[j].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2].HighLightObject(Driver).Click();

                            VerifyPageLoad();

                            ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription()),
                                HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription(), 5);

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
                        JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.LEAVEMANAGEMENTOPTIONSNEXTPAGE.GetDescription()),
                            HRPAGEOBJECTS.LEAVEMANAGEMENTOPTIONSNEXTPAGE.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Leave Management Options Deleted successfully for the Business Unit")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Could not delete the Leave Manegement Options"), false, Driver));
                    throw new Exception(string.Format("Could not delete the Leave Manegement Options"));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'DeleteLeaveManagementOptions() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add a new Job Title
        /// </summary>
        /// <param name="titleCode"></param>
        /// <param name="titleName"></param>
        /// <param name="jobDesc"></param>
        /// <param name="minExpReq"></param>
        /// <param name="jobPayGrdCode"></param>
        /// <param name="jobPayFreq"></param>
        /// <param name="comments"></param>
        public void AddJobTitle(string titleCode, string titleName, string jobDesc, string minExpReq, string jobPayGrdCode, string jobPayFreq, string comments)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add the new Job Title")));

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDBTN.GetDescription()),
                            HRPAGEOBJECTS.JOBTITLESADDBTN.GetDescription(), 5);

                VerifyPageLoad();

                //Enter Job Title Code
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDJOBTITLECODE.GetDescription()),
                             HRPAGEOBJECTS.JOBTITLESADDJOBTITLECODE.GetDescription(), titleCode, 5);

                //Enter Job Title
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDJOBTITLE.GetDescription()),
                             HRPAGEOBJECTS.JOBTITLESADDJOBTITLE.GetDescription(), titleName, 5);

                // Enter Job Description
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDJOBDESCRIPTION.GetDescription()),
                             HRPAGEOBJECTS.JOBTITLESADDJOBDESCRIPTION.GetDescription(), jobDesc, 5);

                // Enter Min Exp required
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDJOBMINEXPREQUIRED.GetDescription()),
                             HRPAGEOBJECTS.JOBTITLESADDJOBMINEXPREQUIRED.GetDescription(), minExpReq, 5);

                // Enter Job Pay Grade
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDJOBPAYGRADE.GetDescription()),
                             HRPAGEOBJECTS.JOBTITLESADDJOBPAYGRADE.GetDescription(), jobPayGrdCode, 5);

                // Select Job Pay Frequency
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDJOBPAYFREQUENCY.GetDescription()),
                            HRPAGEOBJECTS.JOBTITLESADDJOBPAYFREQUENCY.GetDescription(), jobPayFreq, "text", 5);

                // Enter comments
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDJOBCOMMENTS.GetDescription()),
                            HRPAGEOBJECTS.JOBTITLESADDJOBCOMMENTS.GetDescription(), comments, 5);

                // Save the Values
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESADDJOBSUBMITBTN.GetDescription()),
                            HRPAGEOBJECTS.JOBTITLESADDJOBSUBMITBTN.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddJobTitle() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// used to verify Job Title exists
        /// </summary>
        /// <param name="titleCode"></param>
        /// <param name="titleName"></param>
        public void VerifyJobTitles(string titleCode, string titleName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Job Title")));
                int count = 0;
                
                    var jobTilesTable = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESTABLE.GetDescription()));

                    for (int j = 0; j < jobTilesTable.Count; j++)
                    {
                        if (jobTilesTable[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(titleCode)
                            && jobTilesTable[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(titleName))
                        {
                            count = count + 1;
                            break;
                        }
                    }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Job Title exists for the Code: {0}", titleCode)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Job Title doesnot exists for the Code: {0}", titleCode), false, Driver));
                    throw new Exception(string.Format("Job Title doesnot exists for the Code: {0}", titleCode));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'VerifyJobTitles() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to delete the job title
        /// </summary>
        /// <param name="titleCode"></param>
        /// <param name="titleName"></param>
        public void DeleteJobTitles(string titleCode, string titleName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Job Title")));
                int count = 0;

                var jobTilesTable = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.JOBTITLESTABLE.GetDescription()));

                for (int j = 0; j < jobTilesTable.Count; j++)
                {
                    if (jobTilesTable[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(titleCode)
                        && jobTilesTable[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(titleName))
                    {
                        ObjectClickByPageScrolling(jobTilesTable[j].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2], "Delete Position Button", 100, 10);

                        VerifyPageLoad();

                        ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription()),
                            HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription(), 5);

                        VerifyPageLoad();
                        count = count + 1;
                        break;
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Job Title with the Code: {0} Delete successfully", titleCode)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Could not delete the Job Title with the Code: {0}", titleCode), false, Driver));
                    throw new Exception(string.Format("Could not delete the Job Title with the Code: {0}", titleCode));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'DeleteJobTitles() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add positions in HR page
        /// </summary>
        /// <param name="jobTitle"></param>
        /// <param name="positionName"></param>
        /// <param name="description"></param>
        public string AddPosition(string jobTitle, string positionName, string description)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add the new Position")));

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSADDBTN.GetDescription()),
                            HRPAGEOBJECTS.POSITIONSADDBTN.GetDescription(), 5);

                VerifyPageLoad();

                // Select Job Title
                DropdownSelectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSADDJOBTITLE.GetDescription()),
                            HRPAGEOBJECTS.POSITIONSADDJOBTITLE.GetDescription(), "1", "index", 5);

                string titleName = RetrieveDropdownValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSADDJOBTITLE.GetDescription()));
                
                // Enter Job Pay Grade
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSADDPOSITIONNAME.GetDescription()),
                             HRPAGEOBJECTS.POSITIONSADDPOSITIONNAME.GetDescription(), positionName, 5);

                // Enter comments
                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSADDDESCRIPTION.GetDescription()),
                            HRPAGEOBJECTS.POSITIONSADDDESCRIPTION.GetDescription(), description, 5);

                // Save the Values
                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSADDSUBMITBTN.GetDescription()),
                            HRPAGEOBJECTS.POSITIONSADDSUBMITBTN.GetDescription());

                VerifyPageLoad();

                return titleName;
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddPosition() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// used to verify the positions in HR page
        /// </summary>
        /// <param name="jobTitle"></param>
        /// <param name="positionName"></param>
        public void VerifyPositions(string jobTitle, string positionName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Positions")));
                int count = 0;

                int pageCount = 0;
                bool found = false;
                if (CheckIfObjectExists(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSTABLELASTPAGE.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSTABLELASTPAGE.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var positionsOptions = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSTABLE.GetDescription()));

                    for (int j = 0; j < positionsOptions.Count; j++)
                    {
                        if (positionsOptions[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(positionName)
                            && positionsOptions[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(jobTitle))
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
                        JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSTABLENEXTPAGE.GetDescription()),
                            HRPAGEOBJECTS.POSITIONSTABLENEXTPAGE.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Position {0} exists in HR Page", positionName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Position {0} doesnot exists in HR Page", positionName), false, Driver));
                    throw new Exception(string.Format("Position {0} doesnot exists in HR Page", positionName));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'VerifyPositions() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to delete the position
        /// </summary>
        /// <param name="jobTitle"></param>
        /// <param name="positionName"></param>
        public void DeletePositions(string jobTitle, string positionName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Delete the Positions")));
                int count = 0;

                int pageCount = 0;
                bool found = false;
                if (CheckIfObjectExists(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSTABLELASTPAGE.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSTABLELASTPAGE.GetDescription())).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var positionsOptions = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSTABLE.GetDescription()));

                    for (int j = 0; j < positionsOptions.Count; j++)
                    {
                        if (positionsOptions[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(positionName)
                            && positionsOptions[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(jobTitle))
                        {
                            ObjectClickByPageScrolling(positionsOptions[j].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2], "Delete Position Button", 100, 10);
                            
                            VerifyPageLoad();

                            ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription()),
                                HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription(), 5);

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
                        JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.POSITIONSTABLENEXTPAGE.GetDescription()),
                            HRPAGEOBJECTS.POSITIONSTABLENEXTPAGE.GetDescription());
                        VerifyPageLoad();
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Position {0} deleted successfully", positionName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Position {0} deleted successfully", positionName), false, Driver));
                    throw new Exception(string.Format("Position {0} deleted successfully", positionName));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'DeletePositions() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add new Role in the Roles and Privileges in HR tab
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleType"></param>
        /// <param name="description"></param>
        /// <param name="hirarchy"></param>
        public void AddNewRoleInRolesAndPrivileges(string roleName, string roleType, string description, string hirarchy)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add new Role: {0} in the Roles and Privileges", roleName)));

                ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESADDBTN.GetDescription()),
                                                HRPAGEOBJECTS.ROLESPRIVILEGESADDBTN.GetDescription(), 5);
                VerifyPageLoad();

                string xpath = String.Format(GetXpathString(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESSIDEBARSELECTION.GetDescription()), hirarchy);

                JavaScriptClick(By.XPath(xpath), HRPAGEOBJECTS.ROLESPRIVILEGESSIDEBARSELECTION.GetDescription());
                VerifyPageLoad();

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESADDROLENAME.GetDescription()),
                                                HRPAGEOBJECTS.ROLESPRIVILEGESADDROLENAME.GetDescription(), roleName, 5);

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESADDROLETYPE.GetDescription()),
                                                HRPAGEOBJECTS.ROLESPRIVILEGESADDROLETYPE.GetDescription(), roleType, 5);

                SetObjectValue(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESADDROLEDESCRIPTION.GetDescription()),
                                                HRPAGEOBJECTS.ROLESPRIVILEGESADDROLEDESCRIPTION.GetDescription(), description, 5);

                var checkBoxes = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESADDCHECKBOXES.GetDescription()));

                foreach (var item in checkBoxes)
                {   
                    item.Click();
                    PageScrollDown(item, 10);
                }

                JavaScriptClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESADDSUBMITBTN.GetDescription()),
                                                HRPAGEOBJECTS.ROLESPRIVILEGESADDSUBMITBTN.GetDescription());
                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddNewRoleInRolesAndPrivileges() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// used to verify the role exists in Roles and Privileges table
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleType"></param>
        public void VerifyRolesPriviligesTable(string roleName, string roleType)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify the Roles and Privileges Table")));
                int count = 0;

                var rolesPrivTable = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESTABLE.GetDescription()));

                for (int j = 0; j < rolesPrivTable.Count; j++)
                {
                    if (rolesPrivTable[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(roleName)
                        && rolesPrivTable[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(roleType))
                    {
                        count = count + 1;
                        break;
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Role: {0} exists in the Roles & Privileges Table", roleName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Role: {0} doesnot exists in the Roles & Privileges Table", roleName), false, Driver));
                    throw new Exception(string.Format("Role: {0} doesnot exists in the Roles & Privileges Table", roleName));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'VerifyRolesPriviligesTable() function' {0}", ex.Message));
            }
        }

        
        /// <summary>
        /// used to delete a role from the roles and privileges table
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleType"></param>
        public void DeleteRolesFromRolesAndPrivilegesTable(string roleName, string roleType)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Delete the Roles and Privileges Table")));
                int count = 0;

                var rolesPrivTable = Driver.FindElements(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.ROLESPRIVILEGESTABLE.GetDescription()));

                for (int j = 0; j < rolesPrivTable.Count; j++)
                {
                    if (rolesPrivTable[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(roleName)
                        && rolesPrivTable[j].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(roleType))
                    {
                        rolesPrivTable[j].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2].HighLightObject(Driver).Click();

                        VerifyPageLoad();

                        ObjectClick(Locator.GetLocator(PAGE.HR.GetDescription(), HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription()),
                            HRPAGEOBJECTS.MANAGEHOLIDAYCONFIRMATIONYES.GetDescription(), 5);

                        VerifyPageLoad();
                        count = count + 1;
                        break;
                    }
                }

                if (count > 0)
                {
                    Reporter.Add(new Act(string.Format("Role: {0} delete from the Roles & Privileges Table", roleName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Could not delete Role: {0} from the Roles & Privileges Table", roleName), false, Driver));
                    throw new Exception(string.Format("Could not delete Role: {0} from the Roles & Privileges Table", roleName));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'DeleteRolesFromRolesAndPrivilegesTable() function' {0}", ex.Message));
            }
        }
        #endregion
    }
}