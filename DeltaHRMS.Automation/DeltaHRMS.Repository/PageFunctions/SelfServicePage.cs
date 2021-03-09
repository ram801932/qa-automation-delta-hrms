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
        /// Used to click on Apply Leave button on Self Service Page
        /// </summary>
        public void ClickApplyLeave()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to click on Apply Leave and Navigate from Self Service => Create Leave Request")));
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.APPLYLEAVEBUTTON.GetDescription()),
                            SELFSERVICEOBJECTS.APPLYLEAVEBUTTON.GetDescription(), 5);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'ClickApplyLeave() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to create a new Leave Request in Self Service Page
        /// </summary>
        /// <param name="leaveType">Type of Leave Ex: EL, SL</param>
        /// <param name="leavefor">Full day / Half day</param>
        public void CreateLeaveRequest(string leaveType, string date, string leavefor)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Create Leave Request by entering Required Details")));

                VerifyPageLoad();

                // Selecting the leave type

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.SELECTREQUESTLEAVETYPE.GetDescription()),
                            SELFSERVICEOBJECTS.SELECTREQUESTLEAVETYPE.GetDescription(), 5);

                var leaveTypeOptions = Driver.FindElements(By.XPath("//select[@id ='leavetypeid']/option"));
                for (int i = 0; i < leaveTypeOptions.Count; i++)
                {
                    if (leaveTypeOptions[i].Text.Contains(leaveType))
                    {
                        leaveTypeOptions[i].Click();
                        break;
                    }
                }

                VerifyPageLoad();
                // Selecting the from date from the calander

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEFROMDATE.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEFROMDATE.GetDescription(), 5);

                SelectDateFromCalender(date);

                VerifyPageLoad();
                // Selecting Leave To date from the calander
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVETODATE.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVETODATE.GetDescription(), 5);

                SelectDateFromCalender(date);

                VerifyPageLoad();
                // Selecting the leave for Full Day or Half Day
                DropdownSelectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEFORFIELD.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEFORFIELD.GetDescription(), leavefor, "text", 5);

                VerifyPageLoad();
                // Enter the reason for the leave
                SetObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEREASON.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEREASON.GetDescription(), APPLYLEAVECOMMENT, 5);

                // Click on Submit to apply leave
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.SUBMITBTN.GetDescription()),
                            SELFSERVICEOBJECTS.SUBMITBTN.GetDescription(), 5);

                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'CreateLeaveRequest() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to Verify weather leave is applied successfully
        /// </summary>
        public void VerifyLeaveApplication(string date)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to verify the applied leave in the My Leave page")));

                var leavesReqTable = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYLEAVEPENDINGLEAVESTABLE.GetDescription()));

                for (int i = 0; i < leavesReqTable.Count; i++)
                {
                    if (leavesReqTable[i].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(APPLYLEAVECOMMENT) &&
                        leavesReqTable[i].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Equals(date))
                    {
                        Reporter.Add(new Act(string.Format("Leave Applied Successfully for day {0}", date)));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'ClickApplyLeave() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to Delete the leave before Approval on My Leave Page
        /// </summary>
        public void DeleteAppliedLeave(string date)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Delete the applied leave in the My Leave page before Approval")));

                var leavesReqTable = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYLEAVEPENDINGLEAVESTABLE.GetDescription()));

                for (int i = 0; i < leavesReqTable.Count; i++)
                {
                    if (leavesReqTable[i].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Equals(APPLYLEAVECOMMENT) &&
                        leavesReqTable[i].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Equals(date))
                    {
                        leavesReqTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[1].HighLightObject(Driver).Click();

                        ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYLEAVECANCELPOPUPYES.GetDescription()),
                            SELFSERVICEOBJECTS.MYLEAVECANCELPOPUPYES.GetDescription(), 5);

                        VerifyPageLoad();

                        Reporter.Add(new Act(string.Format("Leave Deleted Successfully for day {0}", date)));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'DeleteAppliedLeaveBeforeApproval() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to create a leave request, failure scenario. Validate Error Message
        /// </summary>
        /// <param name="leaveType">Type of leave to be selected</param>
        /// <param name="leavefor">Leave for Full / Half day</param>
        /// <param name="expectedError">Expected Error Message</param>
        public void CreateLeaveRequestFailure(string leaveType, string leavefor, string date, string expectedError)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Verify Error by Creating Leave Request")));

                VerifyPageLoad();

                // Selecting the leave type
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.SELECTREQUESTLEAVETYPE.GetDescription()),
                            SELFSERVICEOBJECTS.SELECTREQUESTLEAVETYPE.GetDescription(), 5);

                var leaveTypeOptions = Driver.FindElements(By.XPath("//select[@id ='leavetypeid']/option"));
                for (int i = 0; i < leaveTypeOptions.Count; i++)
                {
                    if (leaveTypeOptions[i].Text.Contains(leaveType))
                    {
                        leaveTypeOptions[i].Click();
                        break;
                    }
                }

                VerifyPageLoad();

                // Selecting the from date from the calander
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEFROMDATE.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEFROMDATE.GetDescription(), 5);

                SelectDateFromCalender(date);

                VerifyPageLoad();
                // Selecting Leave To date from the calander
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVETODATE.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVETODATE.GetDescription(), 5);

                SelectDateFromCalender(date);

                VerifyPageLoad();

                // Selecting the leave for Full Day or Half Day
                DropdownSelectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEFORFIELD.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEFORFIELD.GetDescription(), leavefor, "text", 5);
                VerifyPageLoad();
                // Enter the reason for the leave
                SetObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEREASON.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEREASON.GetDescription(), APPLYLEAVECOMMENT, 5);

                // Click on Submit to apply leave
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.SUBMITBTN.GetDescription()),
                            SELFSERVICEOBJECTS.SUBMITBTN.GetDescription(), 5);

                VerifyPageLoad();

                if (RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVETYPEERROR.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVETYPEERROR.GetDescription(), "text").Contains(expectedError))
                {
                    Reporter.Add(new Act(string.Format("Leave Request unsucessful with error {0}", expectedError)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Leave Request unsucessful with error {0}", expectedError), false, Driver));
                    throw new Exception(string.Format("Leave Request unsucessful with error {0}", expectedError));
                }

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.CREATELEAVECLOSEBTN.GetDescription()),
                    SELFSERVICEOBJECTS.CREATELEAVECLOSEBTN.GetDescription(), 5);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'CreateLeaveRequest() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used for Approve / reject leave in Employee leave - Manager and above level
        /// </summary>
        /// <param name="empName"></param>
        /// <param name="status"></param>
        public void EmpLeaveManageLeaveRequest(string empName, string date, string status)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to {0} the employee Leave request in Leaves => employee Leaves page", status)));

                int pageCount = 0;
                bool found = false;
                if (CheckIfObjectExists(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLOYEELEAVELASTPAGEBTN.GetDescription()), 5))
                {
                    pageCount = Convert.ToInt32(Driver.FindElement(By.XPath("//div[@id = 'pagination_manageremployeevacations']/a[@class = 'last']")).GetAttribute("href").Split('=')[1]);
                }
                else
                {
                    pageCount = 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    var empLeaveTable = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLOYEELEAVETABLE.GetDescription()));

                    for (int j = 0; j < empLeaveTable.Count; j++)
                    {
                        if (empLeaveTable[j].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(empName) &&
                            empLeaveTable[j].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Equals(date))
                        {
                            empLeaveTable[j].FindElements(By.TagName("td"))[0].FindElements(By.TagName("div"))[0].FindElements(By.TagName("a"))[1].HighLightObject(Driver).Click();
                            found = true;
                            break;
                        }
                    }
                    if (pageCount > 1 && i < pageCount - 1 && found == false)
                    {
                        JavaScriptClick(By.XPath("//div[@id = 'pagination_manageremployeevacations']/a[@class = 'nextNew']"), SELFSERVICEOBJECTS.EMPLOYEELEAVENEXTBTN.GetDescription());
                        VerifyPageLoad();
                    }

                }

                VerifyPageLoad();

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLEAVEEDITSTATUS.GetDescription()),
                    SELFSERVICEOBJECTS.EMPLEAVEEDITSTATUS.GetDescription(), 5);

                var statusOptions = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLEAVEEDITSTATUSSELECTOPTIONS.GetDescription()));


                for (int i = 0; i < statusOptions.Count; i++)
                {
                    if (statusOptions[i].Text.Equals(status))
                    {
                        statusOptions[i].Click();
                        break;
                    }
                }

                SetObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLEAVEEDITCOMMENTSTEXT.GetDescription()),
                    SELFSERVICEOBJECTS.EMPLEAVEEDITCOMMENTSTEXT.GetDescription(), "Automation Test", 5);

                JavaScriptClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLEAVEEDITSAVE.GetDescription()),
                    SELFSERVICEOBJECTS.EMPLEAVEEDITSAVE.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'EmpLeaveManageLeaveRequest() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to Verify the employee leave status
        /// </summary>
        /// <param name="empName"></param>
        /// <param name="status"></param>
        public void EmpLeaveVerifyLeaveStatus(string empName, string date, string status)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to {0} the employee Leave request in Leaves => employee Leaves page", status)));

                var empLeaveTable = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLOYEELEAVETABLE.GetDescription()));

                for (int i = 0; i < empLeaveTable.Count; i++)
                {
                    if (empLeaveTable[i].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Equals(empName) &&
                        empLeaveTable[i].FindElements(By.TagName("td"))[3].HighLightObject(Driver).Text.Equals(date))
                    {
                        string leaveStatus = empLeaveTable[i].FindElements(By.TagName("td"))[6].HighLightObject(Driver).Text;
                        if (leaveStatus == status)
                        {
                            Reporter.Add(new Act(string.Format("Leave Status Updated to {0} Successfully", status)));
                        }
                        else
                        {
                            Reporter.Add(new Act(string.Format("Leave Status didnt Update to {0} Successfully", status), false, Driver));
                            throw new Exception(string.Format("Leave Status didnt Update to {0} Successfully Logged in", status));
                        }
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'EmpLeaveVerifyLeaveStatus() function' {0}", ex.Message));
            }
        }

        public void NavToMyLeavesApprovedLeaves()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Navigate to My Leave => Approved Leaves page")));

                Driver.FindElement(By.XPath("//div[@id ='filter_approvedleaves']/span")).Click();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'EmpLeaveVerifyLeaveStatus() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to create the Impersonate Leave by HR.
        /// </summary>
        /// <param name="leaveType">Type of leave. Casual / Comp or sick leave etc</param>
        /// <param name="fromDate">Leave from date</param>
        /// <param name="toDate">Leave to date</param>
        /// <param name="leavefor">Full / Half day</param>
        /// <param name="empName">Name of Employee</param>
        public void CreateImpersonateLeaveRequest(string leaveType, string fromDate, string toDate, string leavefor, string empName)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Create Leave Request by entering Required Details")));

                VerifyPageLoad();

                //Selecting Employee from list
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.IMPERSONATELEAVESELECTEMPLOYEE.GetDescription()),
                            SELFSERVICEOBJECTS.IMPERSONATELEAVESELECTEMPLOYEE.GetDescription(), 5);

                var empNames = Driver.FindElements(By.XPath("//select[@id='leaveapplyingemployeeid']/option"));
                for (int i = 0; i < empNames.Count; i++)
                {
                    if (empNames[i].Text.Contains(empName))
                    {
                        empNames[i].Click();
                        break;
                    }
                }

                VerifyPageLoad();

                // Selecting the leave type
                DropdownSelectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.REQUESTLEAVETYPE.GetDescription()),
                            SELFSERVICEOBJECTS.REQUESTLEAVETYPE.GetDescription(), leaveType, "text", 5);

                VerifyPageLoad();
                // Selecting the from date from the calander
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEFROMDATE.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEFROMDATE.GetDescription(), 5);

                SelectDateFromCalender(fromDate);

                VerifyPageLoad();

                // Selecting Leave To date from the calander
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVETODATE.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVETODATE.GetDescription(), 5);

                SelectDateFromCalender(toDate);

                VerifyPageLoad();

                if (leaveType == LEAVETYPES.COMPOFF.GetDescription() || leaveType == LEAVETYPES.MATERNITYLEAVE.GetDescription())
                {
                    // Upload the jpeg image for reason
                    IWebElement chooseFile = Driver.FindElement(By.XPath("//input[@name='myfile[]']"));
                    chooseFile.SendKeys(Directory.GetCurrentDirectory() + @"\TestData\CompOffReason.jpg");

                    VerifyPageLoad();
                }

                if (leaveType == LEAVETYPES.COMPOFF.GetDescription())
                {
                    // Selecting Comp Off date from the calander
                    ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.COMPOFFDATE.GetDescription()),
                                SELFSERVICEOBJECTS.COMPOFFDATE.GetDescription(), 5);

                    SelectDateFromCalender(toDate);

                    VerifyPageLoad();
                }

                DropdownSelectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEFORFIELD.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEFORFIELD.GetDescription(), leavefor, "text", 5);

                VerifyPageLoad();
                // Enter the reason for the leave
                SetObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEREASON.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEREASON.GetDescription(), APPLYLEAVECOMMENT, 5);

                // Click on Submit to apply leave
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.SUBMITBTN.GetDescription()),
                            SELFSERVICEOBJECTS.SUBMITBTN.GetDescription(), 5);

                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'CreateImpersonateLeaveRequest() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to create Maternity Leave Request
        /// </summary>
        /// <param name="leaveType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        public void CreateMaternityLeaveRequest(string leaveType, string fromDate, string toDate)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Create Maternity Leave Request")));

                VerifyPageLoad();

                // Selecting the leave type
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.REQUESTLEAVETYPE.GetDescription()),
                            SELFSERVICEOBJECTS.REQUESTLEAVETYPE.GetDescription(), 5);

                var leaveTypeOptions = Driver.FindElements(By.XPath("//select[@id ='leavetypeid']/option"));
                for (int i = 0; i < leaveTypeOptions.Count; i++)
                {
                    if (leaveTypeOptions[i].Text.Contains(leaveType))
                    {
                        leaveTypeOptions[i].Click();
                        break;
                    }
                }


                VerifyPageLoad();
                // Selecting the from date from the calander
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEFROMDATE.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEFROMDATE.GetDescription(), 5);

                SelectDateFromCalender(fromDate);

                VerifyPageLoad();

                // Selecting Leave To date from the calander
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVETODATE.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVETODATE.GetDescription(), 5);

                if (!(toDate.Split('-')[2] == DateTime.Now.ToString("yyyy")))
                {
                    toDate = new DateTime(DateTime.Now.Year, 12, 31).ToString("dd-MM-yyyy");
                    SelectDateFromCalender(toDate);
                    VerifyPageLoad();
                }
                else
                {
                    SelectDateFromCalender(toDate);
                }

                // Upload the jpeg image for reason
                IWebElement chooseFile = Driver.FindElement(By.XPath("//input[@name='myfile[]']"));
                chooseFile.SendKeys(Directory.GetCurrentDirectory() + @"\TestData\CompOffReason.jpg");


                VerifyPageLoad();



                VerifyPageLoad();
                // Enter the reason for the leave
                SetObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.LEAVEREASON.GetDescription()),
                            SELFSERVICEOBJECTS.LEAVEREASON.GetDescription(), APPLYLEAVECOMMENT, 5);

                // Click on Submit to apply leave
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.SUBMITBTN.GetDescription()),
                            SELFSERVICEOBJECTS.SUBMITBTN.GetDescription(), 5);

                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at CreateMaternityLeaveRequest() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to validate the detials in Self Service My Details Page
        /// </summary>
        /// <param name="empCode"></param>
        /// <param name="candidateName"></param>
        /// <param name="businessUnit"></param>
        /// <param name="reportingManager"></param>
        /// <param name="jobTitle"></param>
        /// <param name="doj"></param>
        /// <param name="email"></param>
        public void ValidateSelfServiceMyDetailsPage(string empCode, string candidateName, string businessUnit, string reportingManager, string jobTitle, string doj, string email)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Validate the Employee Details in Self Service => My Details Page")));

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYDETAILSOFFICIALTAB.GetDescription()),
                    SELFSERVICEOBJECTS.MYDETAILSOFFICIALTAB.GetDescription(), 5);

                if (RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYDETAILSEMPCODE.GetDescription()),
                    SELFSERVICEOBJECTS.MYDETAILSEMPCODE.GetDescription(), "text").Equals(empCode)
                    &&
                    RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYDETAILSCANDIDATENAME.GetDescription()),
                    SELFSERVICEOBJECTS.MYDETAILSCANDIDATENAME.GetDescription(), "text").Equals(candidateName)
                    &&
                    RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYDETAILSBUSINESSUNITNAME.GetDescription()),
                    SELFSERVICEOBJECTS.MYDETAILSBUSINESSUNITNAME.GetDescription(), "text").Equals(businessUnit)
                    &&
                    RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYDETAILSREPORTINGMANAGER.GetDescription()),
                    SELFSERVICEOBJECTS.MYDETAILSREPORTINGMANAGER.GetDescription(), "text").Equals(reportingManager)
                    &&
                    RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYDETAILSJOBTITLE.GetDescription()),
                    SELFSERVICEOBJECTS.MYDETAILSJOBTITLE.GetDescription(), "text").Equals(jobTitle)
                    &&
                    RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYDETAILSDATEOFJOINING.GetDescription()),
                    SELFSERVICEOBJECTS.MYDETAILSDATEOFJOINING.GetDescription(), "text").Equals(Convert.ToDateTime(doj).ToString("dd-MM-yyyy"))
                    &&
                    RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYDETAILSEMAIL.GetDescription()),
                    SELFSERVICEOBJECTS.MYDETAILSEMAIL.GetDescription(), "text").Equals(email))
                {
                    Reporter.Add(new Act(string.Format("Successfully Validate details for the user {0} in Self Service => My Details Page", candidateName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Values didnt match while Validating details for the user {0} in Self Service => My Details Page", candidateName), false, Driver));
                    throw new Exception(string.Format("Values didnt match while Validating details for the user {0} in Self Service => My Details Page", candidateName));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at ValidateSelfServiceMyDetailsPage() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to Web Check In for user Manually
        /// </summary>
        public void PerformWebCheckIn()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Perform the Web Check In")));

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.WEBCHECKINCHECKOUTBTN.GetDescription()),
                    SELFSERVICEOBJECTS.WEBCHECKINCHECKOUTBTN.GetDescription(), 5);

                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                VerifyPageLoad();

                Wait(3000);
                int time = Convert.ToInt32(RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.WEBCHECKINTIMER.GetDescription()), "text").Replace(":", ""));

                Wait(3000);

                int time2 = Convert.ToInt32(RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.WEBCHECKINTIMER.GetDescription()), "text").Replace(":", ""));

                if (time2 > time)
                {
                    Reporter.Add(new Act(string.Format("Web Check In Successful for the User")));

                    var checkInTable = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.WEBCHECKINTABLE.GetDescription()));

                    if (checkInTable[checkInTable.Count - 1].FindElements(By.TagName("td"))[0].Text.Contains(date))
                    {
                        Reporter.Add(new Act(string.Format("Web Check In Complete for the User at {0}", date)));
                    }
                    else
                    {
                        Reporter.Add(new Act(string.Format("Web Check In could not be Completed for the User at {0}", date), false, Driver));
                        throw new Exception(string.Format("Web Check In could not be Completed for the User at {0}", date));
                    }
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Web Check In is UnSuccessful for the User"), false, Driver));
                    throw new Exception(string.Format("Web Check In is UnSuccessful for the User"));
                }


            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at PerformWebCheckIn() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to perform Web Check Out Manually
        /// </summary>
        public void PerformWebCheckOut()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Perform the Web Check Out Manually")));

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.WEBCHECKINCHECKOUTBTN.GetDescription()),
                    SELFSERVICEOBJECTS.WEBCHECKINCHECKOUTBTN.GetDescription(), 5);

                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                VerifyPageLoad();
                Wait(3000);

                int time = Convert.ToInt32(RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.WEBCHECKINTIMER.GetDescription()), "text").Replace(":", ""));

                Wait(3000);

                int time2 = Convert.ToInt32(RetrieveObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.WEBCHECKINTIMER.GetDescription()), "text").Replace(":", ""));


                if (time == time2)
                {
                    Reporter.Add(new Act(string.Format("Web Check Out Successful for the User")));

                    var checkInTable = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.WEBCHECKINTABLE.GetDescription()));

                    if (checkInTable[checkInTable.Count - 1].FindElements(By.TagName("td"))[1].Text.Contains(date))
                    {
                        Reporter.Add(new Act(string.Format("Web Check Out Complete for the User at {0}", date)));
                    }
                    else
                    {
                        Reporter.Add(new Act(string.Format("Web Check Out could not be Completed for the User at {0}", date), false, Driver));
                        throw new Exception(string.Format("Web Check out could not be Completed for the User at {0}", date));
                    }
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Web Check In is UnSuccessful for the User"), false, Driver));
                    throw new Exception(string.Format("Web Check In is UnSuccessful for the User"));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at PerformWebCheckOut() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to create Attendance Regularization Request by Employee
        /// </summary>
        /// <param name="date"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        public void RegularizeAttendance(string date, string fromTime, string toTime)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Regularize the Attendance")));

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.REGULARIZATIONDAY.GetDescription()),
                    SELFSERVICEOBJECTS.REGULARIZATIONDAY.GetDescription(), 5);

                SelectDateFromCalender(date);
                VerifyPageLoad();

                // Select the Check in Time
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONCHECKINTIME.GetDescription()),
                    SELFSERVICEOBJECTS.MYREGULARIZATIONCHECKINTIME.GetDescription(), 5);

                string hrsXpath = String.Format(GetXpathString(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEHOURS.GetDescription()), fromTime.Split(':')[0]);

                JavaScriptClick(By.XPath(hrsXpath), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEHOURS.GetDescription());

                string minsXpath = String.Format(GetXpathString(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEMINUTES.GetDescription()), fromTime.Split(':')[1]);

                JavaScriptClick(By.XPath(minsXpath), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEMINUTES.GetDescription());

                VerifyPageLoad();

                // Regularizarion CheckOutdate
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONCHECKOUTDAY.GetDescription()),
                    SELFSERVICEOBJECTS.MYREGULARIZATIONCHECKOUTDAY.GetDescription(), 5);

                SelectDateFromCalender(date);

                // Select the Check Out Time
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONCHECKOUTTIME.GetDescription()),
                    SELFSERVICEOBJECTS.MYREGULARIZATIONCHECKOUTTIME.GetDescription(), 5);

                hrsXpath = String.Format(GetXpathString(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEHOURS.GetDescription()), toTime.Split(':')[0]);

                JavaScriptClick(By.XPath(hrsXpath), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEHOURS.GetDescription());

                minsXpath = String.Format(GetXpathString(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEMINUTES.GetDescription()), toTime.Split(':')[1]);

                JavaScriptClick(By.XPath(minsXpath), SELFSERVICEOBJECTS.MYREGULARIZATIONTIMEMINUTES.GetDescription());

                VerifyPageLoad();

                // Enter the Description
                SetObjectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONDESCRIPTION.GetDescription()),
                    SELFSERVICEOBJECTS.MYREGULARIZATIONDESCRIPTION.GetDescription(), "Regularize Time - Automation");

                // Click on Submit Button
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.MYREGULARIZATIONSUBMITBTN.GetDescription()),
                    SELFSERVICEOBJECTS.MYREGULARIZATIONSUBMITBTN.GetDescription(), 5);

                VerifyPageLoad();

                Wait(4000);
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at RegularizeAttendance() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to Approve / Reject / Cancel the Attendance Regulization Request
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="date"></param>
        /// <param name="status"></param>
        public void ManagerApproveRejectRegularizationRequest(string userName, string date, string status)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to {0} the Regularize Attendance request by {1}", status, userName)));

                var requestTable = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLOYEEREGULARIZATIONREQUESTTABLE.GetDescription()));

                for (int i = 0; i < requestTable.Count; i++)
                {
                    if (requestTable[i].FindElements(By.TagName("td"))[1].HighLightObject(Driver).Text.Contains(userName) && requestTable[i].FindElements(By.TagName("td"))[2].HighLightObject(Driver).Text.Contains(date))
                    {
                        requestTable[i].FindElements(By.TagName("td"))[0].FindElements(By.TagName("a"))[1].HighLightObject(Driver).Click();

                        VerifyPageLoad();

                        ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPREGULARIZATIONEDITSTATUSFIELD.GetDescription()),
                    SELFSERVICEOBJECTS.EMPREGULARIZATIONEDITSTATUSFIELD.GetDescription(), 5);

                        var statusOptions = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPREGULARIZATIONEDITSTATUSSELECTOPTIONS.GetDescription()));


                        for (int j = 0; j < statusOptions.Count; j++)
                        {
                            if (statusOptions[j].Text.Equals(status))
                            {
                                statusOptions[j].Click();
                                break;
                            }
                        }

                        JavaScriptClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPREGULARIZATIONSAVEBTN.GetDescription()),
                            SELFSERVICEOBJECTS.EMPREGULARIZATIONSAVEBTN.GetDescription());

                        VerifyPageLoad();
                        break;
                    }
                }

                // Validate the Regularization status after manager approve / reject it.
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                var element = Driver.FindElement(By.XPath("//div[@id='manageremployeeregularization']/table/tbody/tr[1]/td[10]"));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);

                requestTable = Driver.FindElements(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.EMPLOYEEREGULARIZATIONREQUESTTABLE.GetDescription()));

                for (int x = 0; x < requestTable.Count; x++)
                {
                    if (requestTable[x].FindElements(By.TagName("td"))[10].Text.Contains("Regularize Time - Automation"))
                    {
                        if (requestTable[x].FindElements(By.TagName("td"))[9].Text.Contains(status))
                        {
                            Reporter.Add(new Act(string.Format("The Regularize Attendance request by {0} is set to {1}", userName, status)));
                            break;
                        }
                        else
                        {
                            Reporter.Add(new Act(string.Format("Failed to set The Regularize Attendance request by {0} to {1}", userName, status), false, Driver));
                            throw new Exception(string.Format("Failed to set The Regularize Attendance request by {0} to {1}", userName, status));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at ManagerApproveRejectRegularizationRequest() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// used to change the status of Web Check for the user
        /// </summary>
        /// <param name="selectType"></param>
        /// <param name="userName"></param>
        /// <param name="status"></param>
        public void AssignWebCheckIn(string selectType, string userName, string status)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to set the Web Check In status: as {0} for user: {1}",status, userName)));

                DropdownSelectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.ASSIGNWEBCHECKINSELECTBY.GetDescription()),
                    SELFSERVICEOBJECTS.ASSIGNWEBCHECKINSELECTBY.GetDescription(), selectType, "text", 5);

                VerifyPageLoad();

                DropdownSelectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.ASSIGNWEBCHECKINSELECTEMPNAME.GetDescription()),
                    SELFSERVICEOBJECTS.ASSIGNWEBCHECKINSELECTEMPNAME.GetDescription(), userName, "text", 5);

                DropdownSelectValue(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.ASSIGNWEBCHECKINSTATUS.GetDescription()),
                    SELFSERVICEOBJECTS.ASSIGNWEBCHECKINSTATUS.GetDescription(), status, "text", 5);

                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.ASSIGNWEBCHECKINSUBMITBTN.GetDescription()),
                    SELFSERVICEOBJECTS.ASSIGNWEBCHECKINSUBMITBTN.GetDescription());

                VerifyPageLoad();
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at AssignWebCheckIn() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to verify if a leave type exists for Employee in Impersonate Leave
        /// </summary>
        /// <param name="empName"></param>
        /// <param name="leaveType"></param>
        /// <param name="expected"></param>
        public void VerifyLeaveTypesForEmployee(string empName, string leaveType, string expected)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to verify if Leave Type: {0} {1} for the user: {2}", leaveType, expected, empName)));

                string leaveExists = "Doesnt Exists";
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.IMPERSONATELEAVESELECTEMPLOYEE.GetDescription()),
                            SELFSERVICEOBJECTS.IMPERSONATELEAVESELECTEMPLOYEE.GetDescription(), 5);

                var empNames = Driver.FindElements(By.XPath("//select[@id='leaveapplyingemployeeid']/option"));
                for (int i = 0; i < empNames.Count; i++)
                {
                    if (empNames[i].Text.Contains(empName))
                    {
                        empNames[i].Click();
                        break;
                    }
                }

                VerifyPageLoad();

                // Selecting the leave type
                ObjectClick(Locator.GetLocator(PAGE.SELFSERVICE.GetDescription(), SELFSERVICEOBJECTS.SELECTREQUESTLEAVETYPE.GetDescription()),
                            SELFSERVICEOBJECTS.SELECTREQUESTLEAVETYPE.GetDescription(), 5);

                var leaveTypeOptions = Driver.FindElements(By.XPath("//select[@id ='leavetypeid']/option"));
                for (int i = 0; i < leaveTypeOptions.Count; i++)
                {
                    if (leaveTypeOptions[i].Text.Contains(leaveType))
                    {
                        leaveExists = "Exists";
                        break;
                    }
                }

                if (leaveExists.Equals(expected))
                {
                    Reporter.Add(new Act(string.Format("Leave Type: {0} {1} for the user: {2}", leaveType, leaveExists, empName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Leave Type: {0} {1} for the user: {2}", leaveType, leaveExists, empName), false, Driver));
                    throw new Exception(string.Format("Leave Type: {0} {1} for the user: {2}", leaveType, leaveExists, empName));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at VerifyLeaveTypesForEmployee() function' {0}", ex.Message));
            }
        }
        #endregion
    }
}