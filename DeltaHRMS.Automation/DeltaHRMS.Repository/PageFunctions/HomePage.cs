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
        /// User to Navigate to HR Page
        /// </summary>
        public void NavigateToHrPage()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Navigate from Home Page => HR Page on Delta Hrms Application")));
                ObjectClick(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.NAVTOHRBTN.GetDescription()),
                            HOMEOBJECTS.NAVTOHRBTN.GetDescription(), 5);
                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'NavigateToHrPage() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// User to Navigate To Self Service Page
        /// </summary>
        public void NavigateToSelfServicePage()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Navigate from Home Page => Self Service Page on Delta Hrms Application")));
                ObjectClick(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.NAVTOSELFSERVICEBTN.GetDescription()),
                            HOMEOBJECTS.NAVTOSELFSERVICEBTN.GetDescription(), 5);
                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'NavigateToSelfServicePage() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// use to navigate to appraisals page.
        /// </summary>
        public void NavigateToAppraisalsPage()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Navigate from Home Page => Appraisals Page on Delta Hrms Application")));
                ObjectClick(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.NAVTOAPPRAISALSBTN.GetDescription()),
                            HOMEOBJECTS.NAVTOAPPRAISALSBTN.GetDescription(), 5);
                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'NavigateToAppraisalsPage() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// use to navigate to Recruitments page.
        /// </summary>

        public void NavigateToRecruitmentsPage()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Navigate from Home Page => Recruitments Page on Delta Hrms Application")));
                ObjectClick(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.NAVTORECRUITMENTSBTN.GetDescription()),
                            HOMEOBJECTS.NAVTORECRUITMENTSBTN.GetDescription(), 5);
                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'NavigateToAppraisalsPage() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to navigate to Organization Page
        /// </summary>
        public void NavigateToOrganizationPage()
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to Navigate from Home Page => Organization Page on Delta Hrms Application")));
                ObjectClick(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.NAVTOORGANIZATIONBTN.GetDescription()),
                            HOMEOBJECTS.NAVTOORGANIZATIONBTN.GetDescription(), 5);
                VerifyPageLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'NavigateToOrganizationPage() function' {0}", ex.Message));
            }
        }

        #endregion
    }
}