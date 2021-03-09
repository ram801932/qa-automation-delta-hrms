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
using iTextSharp.text;
using System.Windows.Forms;
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

        public string AddingOpenings(string busUnit, string dept, string reportingManager, string approver1, string date, string jobTitle, string position, string noOfPositions, string billingType, string posType, string minExpReq)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add new Openings in Recruitments Module")));

                ObjectClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENADDBTN.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITOPENADDBTN.GetDescription(), 5);
                VerifyPageLoad();

                string reqCode = GetElementValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITADDREQUISITIONCODE.GetDescription()),
                            "value", 5);
                
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENBUSINESSUNITDRPDWN.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENBUSINESSUNITDRPDWN.GetDescription(), busUnit, "text", 5);

                VerifyPageLoad();

                // To select the department value

                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENDEPDRPDWN.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENDEPDRPDWN.GetDescription(), dept, "text", 5);
                
                VerifyPageLoad();

                // To select the Reporting Manager 
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENREPMNGR.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENREPMNGR.GetDescription(), reportingManager, "text", 5);
                
                VerifyPageLoad();

                // To select Approver 1
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENAPPROVER1.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENAPPROVER1.GetDescription(), approver1, "text", 5);

                
                VerifyPageLoad();

                // Select date
                ObjectClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENDUEDATE.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENDUEDATE.GetDescription(), 5);

                SelectDateFromCalender(date);

                //Select Job Title
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENJOBTITLE.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENJOBTITLE.GetDescription(), jobTitle, "text", 5);

                VerifyPageLoad();

                // To select Position
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENPOSITION.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENPOSITION.GetDescription(), position, "text", 5);

                
                VerifyPageLoad();

                // Enter no of positions
                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENNOOPOSTN.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITOPENNOOPOSTN.GetDescription(), noOfPositions, 5);

                // Select Billing type
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENBILLINGTYPE.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENBILLINGTYPE.GetDescription(), billingType, "text", 5);

                VerifyPageLoad();

                // Select Position Type
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENPOSITIONTYPE.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITOPENPOSITIONTYPE.GetDescription(), posType, "text", 5);

                Driver.FindElement(By.XPath("//div[@id ='tabs']//a[@id='description-form']")).Click();

                Driver.FindElement(By.XPath("(//div[@class='jqte_editor'])[2]")).SendKeys(position);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENMINREQEXP.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITOPENMINREQEXP.GetDescription(), minExpReq, 5);

                VerifyPageLoad();

                JavaScriptClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITOPENSUBBTN.GetDescription()),
                   RECRUITMENTSPAGEOBJECTS.RECRUITOPENSUBBTN.GetDescription() );

                return reqCode;
            }
            catch (Exception ex)
            {

                throw  new Exception(string.Format("Failed at 'AddingOpenings() function' {0}", ex.Message)); 
            }
        }

       
        /// <summary>
        /// Used to add a candidate to the requisition
        /// </summary>
        /// <param name="requisitionCode"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="source"></param>
        /// <param name="email"></param>
        /// <param name="contactNo"></param>
        /// <param name="skillSet"></param>
        public void AddingCandidates(string requisitionCode, string firstName, string lastName, string source, string email, string contactNo, string skillSet)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add new Candidates in Recruitments Module")));

                ObjectClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDADDBTN.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITCANDADDBTN.GetDescription(), 5);
                VerifyPageLoad();

                // Select the Requisition Code
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDREQIDDRPDWN.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITCANDREQIDDRPDWN.GetDescription(), requisitionCode, "text", 5);


                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDFIRSTNAME.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITCANDFIRSTNAME.GetDescription(), firstName, 5);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDLASTNAME.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITCANDLASTNAME.GetDescription(), lastName, 5);

                // Select Source
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDSOURCE.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITCANDSOURCE.GetDescription(), source, "text", 5);


                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDEMAIL.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITCANDEMAIL.GetDescription(), email, 5);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDCONTNUM.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITCANDCONTNUM.GetDescription(), contactNo, 5);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDSKILLSET.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITCANDSKILLSET.GetDescription(), skillSet, 5);

                // Entering the candidate details manually
                ObjectClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILS.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILS.GetDescription(), 5);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSQUALIFICATION.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSQUALIFICATION.GetDescription(), "B.TECH", 5);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSEXPERINCE.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSEXPERINCE.GetDescription(), "3", 5);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCUSTLOCATION.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCUSTLOCATION.GetDescription(), "Hyderabad", 5);

                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCOUNTRY.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCOUNTRY.GetDescription(), "India", "text", 5);

                VerifyPageLoad();

                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSSTATE.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSSTATE.GetDescription(), "Telangana", "text", 5);

                VerifyPageLoad();

                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCITY.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSCITY.GetDescription(), "Hyderabad", "text", 5);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSPOSTALCODE.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.ADDCANDIDATEDETAILSENTERCANDIDATEDETAILSPOSTALCODE.GetDescription(), "501510", 5);

                VerifyPageLoad();

                JavaScriptClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITCANDSAV.GetDescription()),
                   RECRUITMENTSPAGEOBJECTS.RECRUITCANDSAV.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddingCandidates() function' {0}", ex.Message)); 
            }
        }

       
        /// <summary>
        /// Used to create interviews for the candidates
        /// </summary>
        /// <param name="reqCode"></param>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="interviewType"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="interviewName"></param>
        public void AddingInterviews(string reqCode, string name, string location, string interviewType, string date, string time, string interviewName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Add Interviwes in Recruitments Module")));

                ObjectClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWADDBTN.GetDescription()),
                            RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWADDBTN.GetDescription(), 5);
                VerifyPageLoad();

                // Select the Requisition Id
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWADDREQID.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWADDREQID.GetDescription(), reqCode, "text", 5);

                VerifyPageLoad();

                // Select the Candidate Name
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWCANDNAM.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWCANDNAM.GetDescription(), name, "text", 5);

                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWLOCATION.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWLOCATION.GetDescription(), location, 5);

                // Select the Candidate Name
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWTYPE.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWTYPE.GetDescription(), interviewType, "text", 5);

                // Selecting date
                ObjectClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWDATE.GetDescription()),
                    RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWDATE.GetDescription(), 5);

                SelectDateFromCalender(date);

                // Selecting Time
                string hrsXpath = String.Format(GetXpathString(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEHOURS.GetDescription()), time.Split(':')[0]);

                JavaScriptClick(By.XPath(hrsXpath), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEHOURS.GetDescription());

                string minsXpath = String.Format(GetXpathString(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEMINUTES.GetDescription()), time.Split(':')[1]);

                JavaScriptClick(By.XPath(minsXpath), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEMINUTES.GetDescription());
                
                // Enter the interview Name
                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWNAME.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWNAME.GetDescription(), interviewName, 5);

                ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SUBMITBUTTON.GetDescription()),
                GENERICOBJECTS.SUBMITBUTTON.GetDescription(), 5);

                VerifyPageLoad();

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'AddingInterviews() function' {0}", ex.Message));
            }
        }
        

        public void RecruitmentsInterviewRounds(string requisitionCode, string status, string candidateName, string interviewName, string feedbackDesc)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Set the status: {0} for the Requisition Code: {1}", status, requisitionCode)));

                var interviewsTable = Driver.FindElements(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITMENTINTERVIEWSTABLE.GetDescription()));

                for (int i = 0; i < interviewsTable.Count; i++)
                {
                    if (interviewsTable[i].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Contains(requisitionCode) &&
                        interviewsTable[i].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Contains(candidateName))
                    {
                        interviewsTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[1].HighLightObject(Driver).Click();
                        VerifyPageLoad();
                        break;
                    }
                }

                VerifyPageLoad();

                var interviewRoundsTable = Driver.FindElements(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITMENTINTERVIEWROUNDSTABLE.GetDescription()));

                for (int i = 0; i < interviewRoundsTable.Count; i++)
                {
                    if (interviewRoundsTable[i].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Contains(interviewName))
                    {
                        interviewRoundsTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[1].HighLightObject(Driver).Click();
                        VerifyPageLoad();
                        break;
                    }
                }

                VerifyPageLoad();
                
                // Switch to the interview rounds frame
                Driver.SwitchTo().Frame(Driver.FindElement(By.Id("interviewroundsCont")));

                // Enter the feedback description
                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWROUNDFEEDBACK.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWROUNDFEEDBACK.GetDescription(), feedbackDesc, 5);

                // Enter the feedback description
                SetObjectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWROUNDINTERVIEWERCOMMENTS.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWROUNDINTERVIEWERCOMMENTS.GetDescription(), feedbackDesc, 5);

                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWROUNDSTATUS.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWROUNDSTATUS.GetDescription(), status, "text",5);

                VerifyPageLoad();
                
                ObjectClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWUPDATEBTN.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWUPDATEBTN.GetDescription(), 5);

                VerifyPageLoad();

                Driver.SwitchTo().ParentFrame();

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'RecruitmentsInterviewRounds() function' {0}", ex.Message));
            }
        }


        /// <summary>
        /// Used to approve the requisiton created newly
        /// </summary>
        /// <param name="requisitionCode"></param>
        /// <param name="status"></param>
        public void ManagementApproveNewRequisition(string requisitionCode, string status)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Approve the new Requisition Code: {0}", requisitionCode)));

                var openingsTable = Driver.FindElements(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITMENTOPENINGSPOSITIONSTABLE.GetDescription()));

                for (int i = 0; i < openingsTable.Count; i++)
                {
                    if (openingsTable[i].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(requisitionCode))
                    {
                        openingsTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[1].HighLightObject(Driver).Click();
                        VerifyPageLoad();
                        break;
                    }
                }

                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITMENTSOPENINGSPOSITIONSAPPROVERSTATUS.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITMENTSOPENINGSPOSITIONSAPPROVERSTATUS.GetDescription(), status, "text", 5);

                JavaScriptClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SUBMITBUTTON.GetDescription()),
                GENERICOBJECTS.SUBMITBUTTON.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'ManagementApproveNewRequisition() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to change the status of the Interview
        /// </summary>
        /// <param name="interviewStatus"></param>
        /// <param name="requisitionCode"></param>
        /// <param name="candidateName"></param>
        /// <param name="candidateStatus"></param>
        public void HRChangeInterviewStatus(string interviewStatus, string requisitionCode, string candidateName, string candidateStatus)
        {
            try
            {
                Reporter.Add(new Act(string.Format("HR Trying to change the  Interview Status to: {0}", interviewStatus)));

                var interviewsTable = Driver.FindElements(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITMENTINTERVIEWSTABLE.GetDescription()));

                for (int i = 0; i < interviewsTable.Count; i++)
                {
                    if (interviewsTable[i].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Contains(requisitionCode) &&
                        interviewsTable[i].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Contains(candidateName))
                    {
                        interviewsTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[1].HighLightObject(Driver).Click();
                        VerifyPageLoad();
                        break;
                    }
                }

                // select interview and candidate status    
                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITMENTINTERVIEWDETAILSSTATUS.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITMENTINTERVIEWDETAILSSTATUS.GetDescription(), interviewStatus, "text", 5);

                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITMENTINTERVIEWDETAILSCANDIDATESTATUS.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITMENTINTERVIEWDETAILSCANDIDATESTATUS.GetDescription(), candidateStatus, "text", 5);

                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWUPDATEBTN.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.RECRUITINTERVIEWUPDATEBTN.GetDescription(), 5);

                VerifyPageLoad();

                if (CheckIfObjectExists(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.POPUPOKBUTTON.GetDescription()), 5))
                {
                    ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.POPUPOKBUTTON.GetDescription()),
                GENERICOBJECTS.POPUPOKBUTTON.GetDescription(), 5);
                }

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'HRChangeInterviewStatus() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to complete the selection process for shorlisted candidates
        /// </summary>
        /// <param name="requisitionCode"></param>
        /// <param name="candidateName"></param>
        /// <param name="status"></param>
        public void ManagementCompleteSelectionProcess(string requisitionCode, string candidateName, string status)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to complete the process for Requisition Code: {0}", requisitionCode)));

                var shortListedTable = Driver.FindElements(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.SHORTLISTEDSELECTEDCANDIDATESTABLE.GetDescription()));

                for (int i = 0; i < shortListedTable.Count; i++)
                {
                    if (shortListedTable[i].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Contains(requisitionCode) &&
                        shortListedTable[i].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Contains(candidateName))
                    {
                        shortListedTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[1].HighLightObject(Driver).Click();
                        VerifyPageLoad();
                        break;
                    }
                }

                DropdownSelectValue(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.SHORTLISTEDSELECTEDCANDIDATESCANDIDATESELCIONSTATUS.GetDescription()),
                RECRUITMENTSPAGEOBJECTS.SHORTLISTEDSELECTEDCANDIDATESCANDIDATESELCIONSTATUS.GetDescription(), status, "text", 5);

                ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SUBMITBUTTON.GetDescription()),
                GENERICOBJECTS.SUBMITBUTTON.GetDescription(), 5);

                VerifyPageLoad();

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'ManagementCompleteSelectionProcess() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to verify the status of the Shortlisted & Selected candidates
        /// </summary>
        /// <param name="requisitionCode"></param>
        /// <param name="candidateName"></param>
        /// <param name="status"></param>
        public void VerifyShortlistedCandidateStauts(string requisitionCode, string candidateName, string status)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to verify the status for the Requisition Code: {0}", requisitionCode)));

                string candidateStatus = string.Empty;

                var shortListedTable = Driver.FindElements(Locator.GetLocator(PAGE.RECRUITMENTS.GetDescription(), RECRUITMENTSPAGEOBJECTS.SHORTLISTEDSELECTEDCANDIDATESTABLE.GetDescription()));

                for (int i = 0; i < shortListedTable.Count; i++)
                {
                    if (shortListedTable[i].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Contains(requisitionCode) &&
                        shortListedTable[i].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Contains(candidateName))
                    {
                        candidateStatus = shortListedTable[i].FindElements(By.TagName("td"))[6].HighLightObject(Driver).Text;
                        break;
                    }
                }

                if (candidateStatus.Equals(status))
                {
                    Reporter.Add(new Act(string.Format("Status: {0} is set successfully for the Requisition Code: {1}", status, requisitionCode)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Stauts: {0} for the Requisition Code: {1} is not set", status, requisitionCode), false, Driver));
                    throw new Exception(string.Format("Stauts: {0} for the Requisition Code: {1} is not set", status, requisitionCode));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at 'VerifyShortlistedCandidateStauts() function' {0}", ex.Message));
            }
        }
        #endregion
    }
}