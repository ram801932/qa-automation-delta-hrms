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

        #region Constructor
        /// <summary>
        /// Constructor without parameters
        /// </summary>
        public DeltaHRMSApplication()
        {
        }

        public DeltaHRMSApplication(RemoteWebDriver _Driver)
        {
            this.Driver = _Driver;
        }

        public DeltaHRMSApplication(XmlNode _testNode)
        {
            this.TestDataNode = _testNode;
        }

        public DeltaHRMSApplication(RemoteWebDriver _Driver, XmlNode _testNode)
        {
            this.Driver = _Driver;
            this.TestDataNode = _testNode;
        }

        public DeltaHRMSApplication(RemoteWebDriver _Driver, XmlNode _testNode, Iteration iteration)
        {
            try
            {
                this.Driver = _Driver;
                this.TestDataNode = _testNode;
                this.Reporter = iteration;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DeltaHRMSApplication(RemoteWebDriver _Driver, XmlNode _testNode, Iteration iteration, string moduleName)
            : base(_Driver, _testNode, iteration, moduleName)
        {
            try
            {
                this.Driver = _Driver;
                this.TestDataNode = _testNode;
                this.Reporter = iteration;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region PageFunction

        /// <summary>
        /// Login to the Delta HRMS
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void LoginToDeltaHRMS(string userName, string password)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Login to Delta HRMS with UserName: {0} in textbox", userName)));

                SetObjectValue(Locator.GetLocator(PAGE.LOGIN.GetDescription(), LOGINOBJECTS.USERNAME.GetDescription()),
                               LOGINOBJECTS.USERNAME.GetDescription(),
                               userName,
                               5);

                SetObjectValue(Locator.GetLocator(PAGE.LOGIN.GetDescription(), LOGINOBJECTS.PASSWORD.GetDescription()),
                               LOGINOBJECTS.PASSWORD.GetDescription(),
                               password,
                               5);

                ObjectClick(Locator.GetLocator(PAGE.LOGIN.GetDescription(), LOGINOBJECTS.LOGINSUBMIT.GetDescription()),
                            LOGINOBJECTS.LOGINSUBMIT.GetDescription(), 5);

                VerifyPageLoad();

                //if(CheckIfObjectExists(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.NOIFICATIONALERTPOPUP.GetDescription()), 10))
                //{
                //    if (Driver.FindElement(By.XPath("//div[@class ='ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-draggable ui-resizable']")).GetCssValue("display") == "block")
                //    {
                //        ObjectClick(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.NOIFICATIONALERTPOPUPCLOSEBTN.GetDescription()),
                //            HOMEOBJECTS.NOIFICATIONALERTPOPUPCLOSEBTN.GetDescription(), 5);
                //    }
                //}
                

                if (ValidateIfExists(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.HOMEMENUBAR.GetDescription()), HOMEOBJECTS.HOMEMENUBAR.GetDescription(), 5))
                {
                    Reporter.Add(new Act(string.Format("User: {0} Successfully Logged in", userName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("User: {0} failed to log-in", userName), false, Driver));
                    throw new Exception(string.Format("User: {0} failed to log-in", userName));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed at 'LoginToDeltaHRMS() function' {0}", ex.Message));
            }
        }
        #endregion
    }
}