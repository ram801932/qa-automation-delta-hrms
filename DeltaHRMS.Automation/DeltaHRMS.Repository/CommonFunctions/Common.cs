#region Microsoft Reference
using System;
using System.Xml;
using System.Configuration;
#endregion

#region Selenium Reference
using OpenQA.Selenium.Remote;
#endregion

#region Delta Automation Reference
using System.Collections.Generic;
using OpenQA.Selenium;
using static DeltaHRMS.Repository.PageFunctions.Constants;
using DeltaHRMS.Accelerators;
using DeltaHRMS.Accelerators.Reporting;
using DeltaHRMS.Accelerators.Utilities;
using DeltaHRMS.Accelerators.UtilityClasses;
using DeltaHRMS.Repository.PageFunctions;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.Metadata.Edm;
using System.Data;
#endregion

namespace DeltaHRMS.Repository.CommonFunctions
{
    /// <summary>
    ///  Represents WebPage . Inherats from BasePage.
    /// </summary>
    public class Common : BasePage
    {
        #region Fields
        /// <summary>
        ///  Memeber variables should start with 'm'followed by camaline method
        /// </summary>
        private string mName = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        private static List<string> mVINNumbers = null;

        /// <summary>
        ///  static member variables should start with 's' followed by camaline method
        /// </summary>
        private static string sField = string.Empty;

        /// <summary>
        /// locker for synchronization
        /// </summary>
        public static object mLocker = new object();
        #endregion

        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        /// <summary>
        /// VIN List
        /// </summary>       
        public static List<string> VINList
        {
            set
            {
                mVINNumbers = value;
            }
            get
            {
                return mVINNumbers;
            }
        }

        /// <summary>
        /// Locker for synchronization
        /// </summary>
        public static object Locker
        {
            set
            {
                mLocker = value;
            }
            get
            {
                return mLocker;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Common class.
        /// </summary>
        public Common()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common class with specified RemoteWebDriver
        /// </summary>
        /// <param name="_Driver">RemoteWebDriver</param>
        public Common(RemoteWebDriver _Driver)
        {
            this.Driver = _Driver;
        }

        /// <summary>
        /// Initializes a new instance of the Common class with specified xmlNode
        /// </summary>
        /// <param name="_testNode">XmlNode</param>
        public Common(XmlNode _testNode)
        {
            this.TestDataNode = _testNode;
        }

        /// <summary>
        /// Initializes a new instance of the Common class with specified RemoteWebDriver and  XmlNode
        /// </summary>
        /// <param name="_Driver">RemoteWebDriver</param>
        /// <param name="_testNode">XmlNode</param>
        public Common(RemoteWebDriver _Driver, XmlNode _testNode)
        {
            this.Driver = _Driver;
            this.TestDataNode = _testNode;
        }

        /// <summary>
        /// Initializes a new instance of the Common class with specified RemoteWebDriver, XmlNode, Iteration and loads page objects from PageObject.xml file
        /// </summary>
        /// <param name="_Driver">RemoteWebDriver</param>
        /// <param name="_testNode">XmlNode</param>
        /// <param name="iteration">Iteration</param>
        public Common(RemoteWebDriver _Driver, XmlNode _testNode, Iteration iteration)
        {
            try
            {
                this.Driver = _Driver;
                this.TestDataNode = _testNode;
                this.Reporter = iteration;

                this.PageObjects = Locator.LoadPageObjects(Locator.ModuleName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Common(RemoteWebDriver _Driver, XmlNode _testNode, Iteration iteration, string moduleName)
        {
            this.Driver = _Driver;
            this.TestDataNode = _testNode;
            this.Reporter = iteration;
            this.PageObjects = Locator.LoadPageObjects(moduleName);
            Locator.ModuleName = moduleName;
        }
        #endregion

        #region ObjectGenericFunction
        /// <summary>
        /// Navigates to Delta HRMS Login page.
        /// </summary>
        /// <returns>HRMS class object.</returns>
        /// <exception cref="">throws exception.</exception>
        public DeltaHRMSApplication NavigateToDeltaHRMSLoginPage()
        {
            try
            {
                string menuUrl = ConfigurationManager.AppSettings["URL"];
                Reporter.Add(new Act("Navigating to Delta HRMS page " + menuUrl));
                NavigateToURL(menuUrl);
                WaitForPageLoad();
                return new DeltaHRMSApplication(Driver, TestDataNode, Reporter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used to verify if page is loaded successfully
        /// </summary>
        /// <param name="MaxTime"></param>
        public void VerifyPageLoad(int MaxTime = 120)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Wait for the current page to load fully")));



                for (int i = 0; i < MaxTime; i++)
                {
                    Wait(2000);
                    if ((!IsWebElementDisplayed(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.LOADINGCIRCLE.GetDescription()), 1)))
                    {
                        Reporter.Add(new Act(String.Format("Page is loaded successfully")));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Failed at VerifyPageLoad(): {0}", ex.Message)));
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used to expand the side menu bar
        /// </summary>
        public void ExpandSideBar()
        {
            try
            {
                Reporter.Add(new Act(String.Format("Expading the side bar in Delta HRMS")));

                if(CheckIfObjectExists(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SIDEBARMENUACTIVE.GetDescription()), 5))
                {
                    Reporter.Add(new Act(String.Format("Side bar is Expanded")));
                }
                else
                {
                    ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SIDEBARMENUHIDESHOW.GetDescription()),
                        GENERICOBJECTS.SIDEBARMENUHIDESHOW.GetDescription(), 5);
                }
                
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Failed at ExpandSideBar(): {0}", ex.Message)));
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used to logout from Delta HRMS application
        /// </summary>
        public void LogoutFromDeltaHRMS()
        {
            try
            {
                Reporter.Add(new Act(String.Format("Trying to Logout from the Delta HRMS application")));

                ObjectClick(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.HOMEPAGEUSERPROFILEBTN.GetDescription()),
                        HOMEOBJECTS.HOMEPAGEUSERPROFILEBTN.GetDescription(), 5);

                var userProfSubMenu = Driver.FindElements(Locator.GetLocator(PAGE.HOME.GetDescription(), HOMEOBJECTS.HOMEPAGEUSERPROFILESUBMENU.GetDescription()));

                for (int i = 0; i < userProfSubMenu.Count; i++)
                {
                    if (userProfSubMenu[i].Text.Equals("Logout"))
                    {
                        userProfSubMenu[i].Click();
                        VerifyPageLoad();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Failed at LogoutFromDeltaHRMS(): {0}", ex.Message)));
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used to select Menu and Sub Menu from the sidebar 
        /// </summary>
        /// <param name="menuName"> Menu Name</param>
        /// <param name="subMenu">Sub Menu Name</param>
        public void SelectMenuSubMenuFromSideBar(string menuName, string subMenu)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Trying to select {0} => {1}", menuName, subMenu)));

                string menuXPath = String.Format(GetXpathString(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SIDEBARMENUHIDDEN.GetDescription()), menuName);
                
                string subMenuXpath = String.Format(GetXpathString(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SIDEBARSUBMENUBTN.GetDescription()),menuName, subMenu);

                if (CheckIfObjectExists(By.XPath(menuXPath),5))
                {
                    JavaScriptClick(By.XPath(menuXPath), GENERICOBJECTS.SIDEBARMENUBTN.GetDescription());
                    VerifyPageLoad();

                    JavaScriptClick(By.XPath(subMenuXpath), GENERICOBJECTS.SIDEBARSUBMENUBTN.GetDescription());
                    VerifyPageLoad();
                }
                else
                {
                    JavaScriptClick(By.XPath(subMenuXpath), GENERICOBJECTS.SIDEBARSUBMENUBTN.GetDescription());
                    VerifyPageLoad();
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Failed at SelectMenuSubMenuFromSideBar(): {0}", ex.Message)));
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used to select when only menu exists with out any submenu in Side bar
        /// </summary>
        /// <param name="menuName"></param>
        public void SelectMenuFromSideBar(string menuName)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Trying to select {0} from Side Bar", menuName)));

                string menuXPath = String.Format(GetXpathString(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SIDEBARMENUBTN.GetDescription()), menuName);

                if (CheckIfObjectExists(By.XPath(menuXPath),5))
                {
                    JavaScriptClick(By.XPath(menuXPath), GENERICOBJECTS.SIDEBARMENUBTN.GetDescription());
                    VerifyPageLoad();
                }
                else
                {
                    Reporter.Add(new Act(string.Format("User: failed to Select menu {0} from Side Bar", menuName), false, Driver));
                    throw new Exception(string.Format("User: failed to Select menu {0} from Side Bar", menuName));
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Failed at SelectMenuFromSideBar(): {0}", ex.Message)));
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used to select a date from the Calender
        /// </summary>
        /// <param name="date"></param>
        public void SelectDateFromCalender(string date)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Trying to select Date: {0} from Calender", date)));

                if (date.Split('-')[1].Equals(DateTime.Now.ToString("MM")) && date.Split('-')[2].Equals(DateTime.Now.ToString("yyyy")))
                {
                    var toDates = Driver.FindElements(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.CALANDERDATES.GetDescription()));

                    for (int i = 0; i < toDates.Count; i++)
                    {
                        if (toDates[i].Text == date.Split('-')[0].TrimStart('0'))
                        {
                            toDates[i].Click();
                            break;
                        }
                    }
                }
                else
                {
                    DateTime dtDate = new DateTime(Convert.ToInt32(date.Split('-')[2]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[0]));
                    string sMonthName = dtDate.ToString("MMM");

                    DropdownSelectValue(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.CALANDERSELECTYEAR.GetDescription()),
                        GENERICOBJECTS.CALANDERSELECTYEAR.GetDescription(), date.Split('-')[2], "text", 5);

                    DropdownSelectValue(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.CALANDERSELECTMONTH.GetDescription()),
                        GENERICOBJECTS.CALANDERSELECTMONTH.GetDescription(), sMonthName, "text", 5);
                    
                    var toDates = Driver.FindElements(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.CALANDERDATES.GetDescription()));

                    for (int i = 0; i < toDates.Count; i++)
                    {
                        if (toDates[i].Text == date.Split('-')[0].TrimStart('0'))
                        {
                            toDates[i].Click();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Failed at SelectDateFromCalender(): {0}", ex.Message)));
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used to Add or Remove a shortcut from Side Bar
        /// </summary>
        /// <param name="shortcutName"></param>
        /// <param name="addOrRemove"></param>
        public void AddRemoveShortcut(string shortcutName, string addOrRemove)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Trying to {0} shortcut option exists for {1} in side bar", addOrRemove, shortcutName)));

                string xpath = String.Format(GetXpathString(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SHORTCUTBUTTONS.GetDescription()), shortcutName);

                if (addOrRemove.Equals("Add"))
                {
                    if (CheckIfObjectExists(By.XPath(xpath), 5))
                    {
                        Reporter.Add(new Act(string.Format("Shortcut Options exists for {0} in Side bar", shortcutName)));
                    }
                    else
                    {
                        ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.CREATESHORTCUTBTN.GetDescription()),
                                    GENERICOBJECTS.CREATESHORTCUTBTN.GetDescription(), 5);
                        Reporter.Add(new Act(string.Format("Shortcut Options Added for {0} in Side bar", shortcutName)));
                    }
                }

                if (addOrRemove.Equals("Remove"))
                {
                    if (!CheckIfObjectExists(By.XPath(xpath), 5))
                    {
                        Reporter.Add(new Act(string.Format("Shortcut Options Removed for option {0} in Side bar", shortcutName)));
                    }
                    else
                    {
                        ObjectClick(Locator.GetLocator(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.CREATESHORTCUTBTN.GetDescription()),
                                    GENERICOBJECTS.CREATESHORTCUTBTN.GetDescription(), 5);
                        Reporter.Add(new Act(string.Format("Shortcut Options Removed sucessully for option {0} in Side bar", shortcutName)));
                    }
                }

                VerifyPageLoad();

            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at AddRemoveShortcut() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to verify if the shortcut option exists in side bar
        /// </summary>
        /// <param name="shortcutName"></param>
        public void VerifyShortCutExists(string shortcutName)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Trying to Verify if shortcut option exists for {0} in side bar", shortcutName)));

                string xpath = String.Format(GetXpathString(PAGE.GENERIC.GetDescription(), GENERICOBJECTS.SHORTCUTBUTTONS.GetDescription()), shortcutName);

                if (CheckIfObjectExists(By.XPath(xpath), 5))
                {
                    Reporter.Add(new Act(string.Format("Shortcut Options exists for {0} in Side bar", shortcutName)));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Shortcut Options doesnot exists for {0} in Side bar", shortcutName)));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Failed at VerifyShortCutExists() function' {0}", ex.Message));
            }
        }

        /// <summary>
        /// Used to add the business days from the given date
        /// </summary>
        /// <param name="current"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public DateTime AddBusinessDays(DateTime current, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                }
                while (current.DayOfWeek == DayOfWeek.Saturday
                    || current.DayOfWeek == DayOfWeek.Sunday);
            }
            return current;
        }

        /// <summary>
        /// Used to connect to the delta HRMS QA Database and fetch the details
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public List<string> ConnectToMySql(string sqlQuery)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Trying to connect to the deltahrmsqa Database")));

                MySqlConnection connection = new MySqlConnection();

                connection.ConnectionString = SQLDATABASECONNECTION;
                connection.Open();

                if (connection.State.ToString().Equals("Open"))
                {
                    Reporter.Add(new Act(string.Format("Connected to the deltahrmsqa database successfully")));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Could not connect to the to the deltahrmsqa database"), false, Driver));
                }
                
                MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);
                MySqlDataReader dataReader = sqlCommand.ExecuteReader();
                List<string> datalist = new List<string>();

                if (dataReader.FieldCount > 0)
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        dataReader.Read();
                        datalist.Add(dataReader.GetValue(i).ToString());
                    }
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Could not find the data for the Query in the deltahrmsqa database"), false, Driver));
                }

                connection.Close();
                return datalist;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Used for calling a stored procedure n MySQL
        /// </summary>
        /// <param name="storeProcedureName"></param>
        public void CallAStoredProcedure(string storeProcedureName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection();

                connection.ConnectionString = SQLDATABASECONNECTION;
                connection.Open();

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = storeProcedureName;
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region PageFunction

        /// <summary>
        /// Get Xpath String
        /// </summary>
        /// <param name="locatorName"></param>
        /// <returns>returns xpath as a string</returns>
        public string GetXpathString(string locatorName, string elementName)
        {
            string xpath = string.Empty;

            for (int i = 1; i < Locator.GetLocator(locatorName, elementName).ToString().Split(':').Length; i++)
            {

                if (string.IsNullOrEmpty(Locator.GetLocator(locatorName, elementName).ToString().Split(':')[i]))
                {
                    xpath += "::";
                }
                else
                {
                    xpath += Locator.GetLocator(locatorName, elementName).ToString().Split(':')[i];
                }
            }

            return xpath;
        }

        /// <summary>
        /// Generic method to Wait for the loader to disappear from page.
        /// </summary>
        public void WaitForObjectToDisappear(By lookupBy, string objName, int maxTime = 60)
        {
            Reporter.Add(new Act(string.Format("Waiting for the object {0} to disappear", objName)));
            try
            {
                int count = 1;
                bool result = true;
                result = CheckIfObjectExists(lookupBy, 1);
                while (result)
                {
                    Wait(1000);
                    count = count + 1;
                    result = CheckIfObjectExists(lookupBy, 1);
                    if (count == maxTime)
                    {
                        break;
                    }
                }
                if (result == true)
                {
                    Reporter.Add(new Act(string.Format("Page took long time to load"), false, Driver));
                }
                else Reporter.Add(new Act(string.Format("Page Loaded Successfully")));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}