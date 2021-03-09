using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Xml;
using DeltaHRMS.Accelerators.Reporting;
using DeltaHRMS.Accelerators.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;


namespace DeltaHRMS.Accelerators.BaseClasses
{
    /// <summary>
    /// Description of BaseTest.
    /// </summary>
    public abstract class BaseTest
    {

        /// <summary>
        /// Gets or Sets Driver
        /// </summary>
        public RemoteWebDriver Driver { get; set; }

        private Iteration mReporter = null;
        /// <summary>
        /// Gets or Sets Reporter
        /// </summary>
        public Iteration Reporter
        {
            get
            {
                return mReporter;
            }
            set
            {
                mReporter = value;
            }
        }

        /// <summary>
        /// Gets or Sets Step
        /// </summary>
        protected string Step
        {
            get
            {
                //TODO: Get should go away
                return Reporter.Chapter.Step.Title;
            }
            set
            {
                Reporter.Add(new Step(value));
            }
        }

        /// <summary>
        /// Gets or Sets Identity of Test Case
        /// </summary>
        public string TestCaseId { get; set; }

        /// <summary>
        /// Gets or Sets Identity of Test Data
        /// </summary>
        public string TestDataId { get; set; }

        /// <summary>
        /// Gets or Sets Test Data as XMLNode
        /// </summary>
        public XmlNode TestDataNode { get; set; }

        /// <summary>
        /// Gets or Sets Test Case as XMLNode
        /// </summary>
        public XmlNode TestCaseNode { get; set; }       


        #region Instance
        private static BaseTest instance;
        public static BaseTest Instance
        {
            get
            {
                if (instance == null)
                {
                    if (instance == null)
                    {
                        instance = Activator.CreateInstance<BaseTest>();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Constructor
        public BaseTest()
        {
        }

        public BaseTest(RemoteWebDriver _Driver)
        {
            this.Driver = _Driver;
        }

        public BaseTest(XmlNode _testNode)
        {
            this.TestCaseNode = _testNode;
        }
        #endregion

        #region PageInstance
        protected T Page<T>() where T : BasePage
        {
            Type pageType = typeof(T);
            if (pageType != null)
            {
                T ob = Activator.CreateInstance<T>();
                return ob;
            }
            else
            {
                return null;
            }
        }

        protected T Page<T>(RemoteWebDriver driver) where T : BasePage, new()
        {
            Type pageType = typeof(T);
            if (pageType != null)
            {
                T ob = (T)Activator.CreateInstance(pageType, new object[] { driver });
                return ob;
            }
            else
            {
                return null;
            }
        }

        protected T Page<T>(XmlNode _testNode) where T : BasePage, new()
        {
            Type pageType = typeof(T);
            if (pageType != null)
            {
                T ob = (T)Activator.CreateInstance(pageType, new object[] { _testNode });
                return ob;
            }
            else
            {
                return null;
            }
        }

        protected T Page<T>(RemoteWebDriver driver, XmlNode _testNode) where T : BasePage, new()
        {
            Type pageType = typeof(T);
            if (pageType != null)
            {
                T ob = (T)Activator.CreateInstance(pageType, new object[] { driver, _testNode });
                return ob;
            }
            else
            {
                return null;
            }
        }

        protected T Page<T>(RemoteWebDriver driver, XmlNode _testNode, Iteration iteration) where T : BasePage, new()
        {
            try
            {
                Type pageType = typeof(T);
                if (pageType != null)
                {
                    T ob = (T)Activator.CreateInstance(pageType, new object[] { driver, _testNode, iteration });
                    return ob;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        protected T Page<T>(RemoteWebDriver driver, XmlNode _testNode, Iteration iteration, string moduleName) where T : BasePage, new()
        {
            Type pageType = typeof(T);
            if (pageType != null)
            {
                T ob = (T)Activator.CreateInstance(pageType, new object[] { driver, _testNode, iteration, moduleName });
                return ob;
            }
            else
            {
                return null;
            }
        }

        #endregion

        public void Execute(TestCase testCaseObject, Dictionary<String, String> browserConfig, XmlNode testDataNode, Iteration iteration, Engine reportEngine)
        {
            try
            {
                this.Driver = Utility.GetDriver(browserConfig);
                this.mReporter = iteration;
                this.TestCaseId = testCaseObject.Title;
                this.TestDataId = testDataNode.SelectSingleNode("TDID").InnerText;
                this.TestDataNode = testDataNode;
                if (browserConfig["target"] == "local")
                {
                    this.Reporter.Browser.BrowserName = ((RemoteWebDriver)Driver).Capabilities.GetCapability("browserName").ToString();
                    this.Reporter.Browser.BrowserVersion = ((RemoteWebDriver)Driver).Capabilities.GetCapability("version").ToString();
                }
                // Does Seed having anything?
                if (this.Reporter.Chapter.Steps.Count == 0)
                    this.Reporter.Chapters.RemoveAt(0);

                ExecuteTestCase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //if (this.Reporter.Chapter.Step.Action.Extra == null)
                //    this.Reporter.Chapter.Step.Action.Extra = "User defined error <br/> ";
                //else
                //    this.Reporter.Chapter.Step.Action.Extra = ex.Message + "<br/>" + ex.StackTrace;
                this.Reporter.Chapter.Step.Action.IsSuccess = false;
                this.Reporter.Chapter.Step.Action.TestActExtra(Driver);
            }
            finally
            {
                try
                {
                    this.Reporter.IsCompleted = true;
                    if (this.Reporter.Chapter.Step.Action.Extra == null)
                        this.Reporter.Chapter.Step.Action.Extra = "User defined error <br/> ";
                    //Screenshot is taken in previous step, below steps are not requireds
                    //ITakesScreenshot iTakeScreenshot = Driver;
                    //this.Reporter.Screenshot = iTakeScreenshot.GetScreenshot().AsBase64EncodedString;                    
                    this.Reporter.EndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.Local);

                    lock (reportEngine)
                    {
                        reportEngine.PublishIteration(this.Reporter);
                        reportEngine.Summarize(false);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                finally
                {
                    Driver.Quit();
                }
            }
        }

        /// <summary>	
        /// Executes Test Case, should be overriden by derived
        /// </summary>
        protected virtual void ExecuteTestCase()
        {
            Reporter.Add(new Chapter("Execute Test Case"));
        }

        /// <summary>
        /// Prepares Seed Data, should be overriden by derived
        /// </summary>
        protected virtual void PrepareSeed()
        {
            Reporter.Add(new Chapter("Prepare Seed Data"));
        }
    }
}