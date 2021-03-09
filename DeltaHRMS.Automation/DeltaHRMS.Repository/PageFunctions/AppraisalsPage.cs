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
using System.IO;
using OpenQA.Selenium.Interactions;
using DeltaHRMS.Accelerators.UtilityClasses;
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

        public void InitilizationofAppraisalForm(string busUnit , string dept , string fromYear, string toYear, string date, string rating, string mode , string enableTo)
        {
            try
            {
                VerifyPageLoad();

                Reporter.Add(new Act(string.Format("Trying to configure appraisals details")));
                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPADDBTN.GetDescription()),
                            APPRAISALPAGEOBJECTS.APPADDBTN.GetDescription(), 5);
                VerifyPageLoad();

                DropdownSelectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPSELBUSSUNITDRPDWN.GetDescription()),
                    APPRAISALPAGEOBJECTS.APPSELBUSSUNITDRPDWN.GetDescription(), busUnit, "text", 5);
                
                VerifyPageLoad();

                // To click and select the department value
                DropdownSelectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPSELDEPDRPDWN.GetDescription()),
                    APPRAISALPAGEOBJECTS.APPSELDEPDRPDWN.GetDescription(), dept, "text", 5);
                
                VerifyPageLoad();

                DropdownSelectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPYRRNGFRMDRPDWN.GetDescription()),
                    APPRAISALPAGEOBJECTS.APPYRRNGFRMDRPDWN.GetDescription(), fromYear, "text", 5);

                VerifyPageLoad();
                
                DropdownSelectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPYRRNGTODRPDWN.GetDescription()),
                    APPRAISALPAGEOBJECTS.APPYRRNGTODRPDWN.GetDescription(), toYear, "text", 5);
                
                VerifyPageLoad();

                DropdownSelectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPENABLETODRPDWN.GetDescription()),
                    APPRAISALPAGEOBJECTS.APPENABLETODRPDWN.GetDescription(), enableTo, "text", 5);
                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPEMPLOYEERDUEDATE.GetDescription()),
                   APPRAISALPAGEOBJECTS.APPEMPLOYEERDUEDATE.GetDescription(), 5);

                SelectDateFromCalender(date);

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPELGIBLTYSELECTALL.GetDescription()),
                   APPRAISALPAGEOBJECTS.APPELGIBLTYSELECTALL.GetDescription(), 5);

                DropdownSelectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRATINDSDRPDWN.GetDescription()),
                    APPRAISALPAGEOBJECTS.APPRATINDSDRPDWN.GetDescription(), rating, "text", 5);

                DropdownSelectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPMODEDRPDWN.GetDescription()),
                    APPRAISALPAGEOBJECTS.APPMODEDRPDWN.GetDescription(), mode, "text", 5);

                VerifyPageLoad();
                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPSAVEBTN.GetDescription()),
                   APPRAISALPAGEOBJECTS.APPSAVEBTN.GetDescription());
                VerifyPageLoad();

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'ConfigureAppraisalDetails() function' {0}", ex.Message));
            }
        }

             

        /// <summary>
        /// Configure Line Mangers for Appraisals Step 2
        /// </summary>
        public void ConfigureLineManagerForm()
        {
            try
            {
                VerifyPageLoad();
                Reporter.Add(new Act(string.Format("Trying to configure Line Managers")));

                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.STEP2CONFLINMNGR.GetDescription()),
                APPRAISALPAGEOBJECTS.STEP2CONFLINMNGR.GetDescription());

                VerifyPageLoad();
                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.CHOSEBYORGHIERCHY.GetDescription()),
                APPRAISALPAGEOBJECTS.CHOSEBYORGHIERCHY.GetDescription());
                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.CONFPOPUP.GetDescription()),
                           APPRAISALPAGEOBJECTS.CONFPOPUP.GetDescription(), 5);
                VerifyPageLoad();

                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.SAVEBTN.GetDescription()),
                           APPRAISALPAGEOBJECTS.SAVEBTN.GetDescription());
                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.CONFPOPUP.GetDescription()),
                          APPRAISALPAGEOBJECTS.CONFPOPUP.GetDescription(), 5);

                VerifyPageLoad();
               
                // Configure Appraisal Parameters  Step 3

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.STEP3CONFGAPPPARM.GetDescription()),
                         APPRAISALPAGEOBJECTS.STEP3CONFGAPPPARM.GetDescription(), 5);
                VerifyPageLoad();

                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.ALLEMPLYESSBTN.GetDescription()),
                         APPRAISALPAGEOBJECTS.ALLEMPLYESSBTN.GetDescription());
                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.CONFPOPUP.GetDescription()),
                          APPRAISALPAGEOBJECTS.CONFPOPUP.GetDescription(), 5);
                VerifyPageLoad();

                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.SUBMITANDINITLIZEBTN.GetDescription()),
                         APPRAISALPAGEOBJECTS.SUBMITANDINITLIZEBTN.GetDescription());
                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.CONFPOPUP.GetDescription()),
                          APPRAISALPAGEOBJECTS.CONFPOPUP.GetDescription(), 5);

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw  new Exception(string.Format("Failed at 'ConfigureLineManagerForm() function' {0}", ex.Message));
            }
        }

        public void AddParameters()
        {

            try
            {
                VerifyPageLoad();

                Reporter.Add(new Act(string.Format("Trying to Add Parameters for Appraisals")));
                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSADD.GetDescription()),
                APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSADD.GetDescription(), 5);

                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSPARAMETER.GetDescription()),
                APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSPARAMETER.GetDescription(),"Automation Test" ,5);

                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSDESCRIPTION.GetDescription()),
               APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSDESCRIPTION.GetDescription(), "Adding weightage through automation script", 5);


                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSWEIGHTAGE.GetDescription()),
               APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSWEIGHTAGE.GetDescription(), "25", 5);

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSSAVE.GetDescription()),
                APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSSAVE.GetDescription(), 5);

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw  new Exception(string.Format("Failed at 'AddParameters() function' {0}", ex.Message)); 
            }
        }

         public void DeleteParameters()
        {
            try
            {
                
                Reporter.Add(new Act(string.Format("Trying to Delete Parameters for Appraisals")));
                var parametersTable = Driver.FindElements(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSTABLE.GetDescription()));

                for (int i = 0; i < parametersTable.Count; i++)
                {
                    if (parametersTable[i].FindElements(By.TagName("td"))[1].Text.Equals("Automation Test"))

                    {
                        parametersTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2].Click();

                        ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYLEAVECANCELPOPUPYES.GetDescription()),
                            SELFSERVICEOBJECTS.MYLEAVECANCELPOPUPYES.GetDescription(), 5);

                        VerifyPageLoad();

                        Reporter.Add(new Act(string.Format("Parameter Deleted Successfully " )));
                        break;
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'DeleteParameters() function' {0}", ex.Message));
            }
        }

        public void AddAppraisalsQuestions()
        {
            try
            {
                VerifyPageLoad();

                Reporter.Add(new Act(string.Format("Trying to Add Questions for Appraisals")));

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSADD.GetDescription()),
                APPRAISALPAGEOBJECTS.APPRAISALPARAMETERSADD.GetDescription(), 5);

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSADDSELECTPARAM.GetDescription()),
                APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSADDSELECTPARAM.GetDescription(), 5);

                var selectAddParameter = Driver.FindElements(By.XPath("//div[@class='select2-result-label']"));

                for (int i = 0; i < selectAddParameter.Count; i++)
                {
                    if (selectAddParameter[i].Text.Contains("Technical Skills"))
                    {
                        selectAddParameter[i].Click();
                        break;
                    }

                }

                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSQUESTION.GetDescription()),
               APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSQUESTION.GetDescription(), "Adding Question through Automation Script", 5);

                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSDESCRIPTION.GetDescription()),
               APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSDESCRIPTION.GetDescription(), "Adding Description through Automation Script", 5);

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSSAVE.GetDescription()),
                APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSSAVE.GetDescription(), 5);
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddAppraisalsQuestions() function' {0}", ex.Message));
            }
        }
          
        public void DeleteQuestions()
        {
            try
            {

                Reporter.Add(new Act(string.Format("Trying to Delete Questions for Appraisals")));
                var parametersTable = Driver.FindElements(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPRAISALQUESTIONSTABLE.GetDescription()));

                for (int i = 0; i < parametersTable.Count; i++)
                {
                    if (parametersTable[i].FindElements(By.TagName("td"))[1].Text.Equals("Technical Skills"))

                    {
                        parametersTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[2].HighLightObject(Driver).Click();

                        ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYLEAVECANCELPOPUPYES.GetDescription()),
                            SELFSERVICEOBJECTS.MYLEAVECANCELPOPUPYES.GetDescription(), 5);

                        VerifyPageLoad();

                        Reporter.Add(new Act(string.Format("Question Deleted Successfully ")));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'DeleteQuestions() function' {0}", ex.Message)); 
            }
        }

        public void AddAppraisalsSkills()
        {
            try
            {
                VerifyPageLoad();

                Reporter.Add(new Act(string.Format("Trying to Add Skills for Appraisals")));
                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.SKILLSADDBTN.GetDescription()),
                APPRAISALPAGEOBJECTS.SKILLSADDBTN.GetDescription(), 5);

                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.SKILLSADDSKILL.GetDescription()),
                APPRAISALPAGEOBJECTS.SKILLSADDSKILL.GetDescription(), "Adding Skills through Automation Script", 5);

                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.SKILLSADDDESCRIPTION.GetDescription()),
               APPRAISALPAGEOBJECTS.SKILLSADDDESCRIPTION.GetDescription(), "Adding skills description through automation script", 5);
                
                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.SKILLSADDSAVEBTN.GetDescription()),
                APPRAISALPAGEOBJECTS.SKILLSADDSAVEBTN.GetDescription(), 5);
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddAppraisalsSkills() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void EmployeeSelfAppraisals()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to perform employee self Appraisals Rating ")));
                int count = 3;
                int range = 2;
                
                var parameterlist = Driver.FindElements(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.EMPLOYEESELFAPPRAISALSPARAMETERSLIST.GetDescription()));

                for (int i = 0; i < parameterlist.Count; i++)
                {
                    parameterlist[i].Click();
                    var tablevalues = Driver.FindElements(By.XPath($"//div[@id ='tabs-{count}']/table/tbody/tr"));

                    if (tablevalues.Count > 0)
                    {
                        if (tablevalues[0].FindElements(By.TagName("td")).Count > 1)
                        {
                            for (int j = 0; j < tablevalues.Count; j++)

                            {

                                IWebElement resizeableElement = Driver.FindElement(By.XPath($"//div[@id = 'tabs-{count}']/table/tbody/tr[{j + 1}]/td[2]//div[@class = 'rateit-selected']"));

                                Actions action = new Actions(Driver);

                                action.ClickAndHold(resizeableElement).MoveByOffset(0, 95).Click().Build().Perform();
                                Driver.FindElement(By.XPath($"//div[@id ='rateit-range-{range}']")).HighLightObject(Driver).Click();


                                tablevalues[j].FindElements(By.TagName("td"))[2].FindElement(By.TagName("textarea")).SendKeys("Automation Test Comments " + range + "");
                                range = range + 1;

                            }
                            count = count + 1;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.EMPLOYEESELFAPPRAISALSSENDTOL1MANAGERBTN.GetDescription()),
                         APPRAISALPAGEOBJECTS.EMPLOYEESELFAPPRAISALSSENDTOL1MANAGERBTN.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'EmployeeSelfAppraisals() function' {0}", ex.Message));
            }
        }
        /// <summary>
        /// 
        /// </summary>

        public void ManagerEmployeeRating(string empname)
        {
            try
            {
                
                Reporter.Add(new Act(string.Format("Trying to perform Manager Rating to Employee")));

                

                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPMANGSEARCHEMP.GetDescription()),
                APPRAISALPAGEOBJECTS.APPMANGSEARCHEMP.GetDescription(),empname,5);

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.APPMANGSEARCHGOBTN.GetDescription()),
                APPRAISALPAGEOBJECTS.APPMANGSEARCHGOBTN.GetDescription(), 5);

                VerifyPageLoad();


                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.MANAGERAPPRAISALRATINGEXPANDBTN.GetDescription()),
                APPRAISALPAGEOBJECTS.MANAGERAPPRAISALRATINGEXPANDBTN.GetDescription(), 5);

                VerifyPageLoad();

                int count1 = 0;
                int range1 = 2;
                var parameterlist = Driver.FindElements(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.MANAGERAPPRAISALSPARAMETERSLIST.GetDescription()));

                for (int i = 0; i < parameterlist.Count; i++)
                {
                    parameterlist[i].Click();
                    var tablevalues = Driver.FindElements(By.XPath($"//div[@id ='tabs-{count1}']/table/tbody/tr"));

                    if (tablevalues.Count > 0)
                    {
                        if (tablevalues[0].FindElements(By.TagName("td")).Count > 1)
                        {
                            for (int j = 0; j < tablevalues.Count; j++)

                            {
                                IWebElement resizeableElement = Driver.FindElement(By.XPath($"//div[@id = 'tabs-{count1}']/table/tbody/tr[{j + 1}]/td[3]//div[@class = 'rateit-selected']"));

                                Actions action = new Actions(Driver);

                                action.ClickAndHold(resizeableElement).MoveByOffset(0, 57).Click().Build().Perform();
                                Driver.FindElement(By.XPath($"//table[@class='requisition-table employee_appraisal-table']//div[@class ='rateit']//div[@id ='rateit-range-{range1}']")).Click();


                                tablevalues[j].FindElements(By.TagName("td"))[3].FindElement(By.TagName("textarea")).SendKeys("Automation Test Comments " + range1 + "");
                                range1 = range1 + 1;
                            }
                            count1 = count1 + 1;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                SetObjectValue(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.MANAGERAPPRAISALSOVERALLRATING.GetDescription()),
                         APPRAISALPAGEOBJECTS.MANAGERAPPRAISALSOVERALLRATING.GetDescription(), "Automation Manager Overall Comments", 5);


                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.MANAGERAPPRAISALSSUBMITBTN.GetDescription()),
                         APPRAISALPAGEOBJECTS.MANAGERAPPRAISALSSUBMITBTN.GetDescription());

                VerifyPageLoad();

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'ManagerEmployeeRating() function' {0}", ex.Message));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ManagerPrintAppraisalsRating(string report)
        {
            try
            {
                VerifyPageLoad();

                Reporter.Add(new Act(string.Format("Trying to Print overall rating of the employee")));

                ObjectClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.MANAGERAPPRAISALRATINGEXPANDBTN.GetDescription()),
               APPRAISALPAGEOBJECTS.MANAGERAPPRAISALRATINGEXPANDBTN.GetDescription(), 5);

                VerifyPageLoad();

                JavaScriptClick(Locator.GetLocator(PAGE.APPRAISALS.GetDescription(), APPRAISALPAGEOBJECTS.MANAGERAPPRAISALSPRINTBTN.GetDescription()),
               APPRAISALPAGEOBJECTS.MANAGERAPPRAISALSPRINTBTN.GetDescription());

                VerifyPageLoad();

                string fileName = string.Format(@"C:\automationdownload\{0}.pdf", report);

                bool result = new FileReaderWriter().CopyFileToCurrentWorkingDirector(fileName);
                if (result)
                {
                    Reporter.Add(new Act(string.Format("Employee Appraisals Form {0} Downloaded successfully", fileName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Employee Appraisals Form {0} Did not Downloaded", fileName), false, Driver));
                    throw new Exception(string.Format("Employee Appraisals Form {0} Did not Downloaded", fileName));
                }
                FileReaderWriter fileRW = new FileReaderWriter();
                fileRW.DeleteFiles(@"C:\automationdownload", new string[] { report + ".pdf" });
                fileRW.DeleteFiles(Directory.GetCurrentDirectory(), new string[] { report + ".pdf" });

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'ManagerPrintAppraisalsRating() function' {0}", ex.Message));

                
            }
        }


        #endregion
    }
}