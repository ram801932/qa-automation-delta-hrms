#region namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Xml;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Data;
using System.Text;
using DeltaHRMS.Accelerators.Utilities;
using DeltaHRMS.Accelerators.Reporting;

#endregion

namespace DeltaHRMS.Accelerators
{
    /// <summary>
    /// This is the Super class for all pages
    /// </summary>
    /// 
    public abstract class BasePage
    {
        /// <summary>
        /// Get the browser configuration details
        /// </summary>
        // public Dictionary<string, string> BrowserConfig = Util.GetBrowserConfig(ConfigurationManager.AppSettings.Get("DefaultBrowser"));
        public Dictionary<string, string> BrowserConfig = Utility.BrowserConfig;

        /// <summary>
        /// Gets or Sets Driver
        /// </summary>
        public RemoteWebDriver Driver { get; set; }

        /// <summary>
        /// Gets or Sets Test Data as XMLNode
        /// </summary>
        public XmlNode TestDataNode { get; set; }

        /// <summary>
        /// Gets or Sets Reporter
        /// </summary>
        public Iteration Reporter { get; set; }

        public Dictionary<string, Dictionary<string, Dictionary<string, List<BrowserFinder>>>> PageObjects { get; set; }

        #region Constructor
        public BasePage()
        {

        }
        public BasePage(RemoteWebDriver _Driver)
        {
            this.Driver = _Driver;
        }

        public BasePage(XmlNode _testNode)
        {
            this.TestDataNode = _testNode;
        }

        public BasePage(RemoteWebDriver _Driver, XmlNode _testNode)
        {
            this.Driver = _Driver;
            this.TestDataNode = _testNode;
        }

        public BasePage(RemoteWebDriver _Driver, XmlNode _testNode, Iteration iteration, string moduleName)
        {
            this.Driver = _Driver;
            this.TestDataNode = _testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region ObjectRetrievalFunctions
        internal IWebElement GetNativeElement(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    try
                    {
                        element = Driver.FindElement(lookupBy);
                        if (element != null)
                        {
                            try
                            {
                                string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                                jsExecutor.ExecuteScript(script, new object[] { element });
                                jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { element });
                            }
                            catch { }
                            break;
                        }
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return element;
        }

        internal IList<IWebElement> GetNativeElements(By lookupBy, int maxWaitTime = 60)
        {
            IList<IWebElement> element = null;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    try
                    {
                        element = Driver.FindElements(lookupBy);
                        if (element.Count > 0) break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return element;
        }

        internal IWebElement GetNativeElementInElement(IWebElement parentElement, By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    try
                    {
                        element = parentElement.FindElement(lookupBy);
                        if (element != null)
                        {
                            try
                            {
                                string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                                jsExecutor.ExecuteScript(script, new object[] { element });
                                jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { element });
                            }
                            catch { }
                        }
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return element;
        }
        
        public IWebElement WaitForElementVisible(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitTime)).Until(condition =>
                {
                    IWebElement elementToBeDisplayed = null;
                    try
                    {
                        elementToBeDisplayed = Driver.FindElement(lookupBy);
                    }
                    catch { }

                    return elementToBeDisplayed;
                });
                if (element != null)
                {
                    try
                    {
                        string script = String.Format(@"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: {0}"";", "orange");
                        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                        jsExecutor.ExecuteScript(script, new object[] { element });
                        jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(false));"), new object[] { element });
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                element = null;
                Console.WriteLine(ex.Message);
            }
            return element;
        }

        /// <summary>
        /// Verifies weather the element is present on the UI or not.
        /// </summary>
        /// <param name="lookupBy"></param>
        /// <param name="maxWaitTime"></param>
        /// <returns>TRUE if element displayed</returns>
        public bool IsWebElementDisplayed(By lookupBy, int maxWaitTime = 30)
        {
            bool isElementDisplayed = false;
            try
            {
                Wait(maxWaitTime);
                if (Driver.FindElement(lookupBy).Displayed)
                    isElementDisplayed = true;
            }
            catch (Exception)
            {
                //isElementDisplayed = false;
            }

            return isElementDisplayed;
        }

        /// <summary>
        /// Gets the Element value
        /// </summary>
        /// <param name="lookupBy"></param>
        /// <param name="attributeName"></param>
        /// <param name="maxWaitTime"></param>
        /// <returns>string of the element</returns>
        public string GetElementValue(By lookupBy, string attributeName = "", int maxWaitTime = 30)
        {
            string elementValue = string.Empty;
            try
            {
                Wait(maxWaitTime);
                if (attributeName == string.Empty)
                {
                    elementValue = Driver.FindElement(lookupBy).Text;
                }
                else if (elementValue == null || elementValue == string.Empty)
                {
                    elementValue = Driver.FindElement(lookupBy).GetAttribute(attributeName);
                }

            }
            catch (Exception)
            {
                //elementValue = string.Empty;
            }

            return elementValue;
        }
        
        internal IWebElement WaitForElementVisibleWithoutHighLight(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitTime)).Until(ExpectedConditions.ElementIsVisible(lookupBy));
                if (element != null)
                {
                    try
                    {
                        //string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                        //IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                        //jsExecutor.ExecuteScript(script, new object[] { element });
                        //jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { element });
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return element;
        }


        public void ElementFocus(By lookupBy, int maxWaitTime = 60)
        {
            try
            {
                IWebElement element = WaitForElementVisible(lookupBy, maxWaitTime);
                new Actions(Driver).MoveToElement(element).Perform();
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                jsExecutor.ExecuteScript(String.Format("javascript:window.scrollBy({0},{1})", 0, (element.Location.Y - 200)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        
        internal IWebElement WaitForElementVisible(IWebElement WebElement, int maxWaitTime = 60)
        {

            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitTime));
                wait.Until(webEle => WebElement);
                if (WebElement != null)
                {
                    try
                    {
                        string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                        jsExecutor.ExecuteScript(script, new object[] { WebElement });
                        jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { WebElement });
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return WebElement;
        }

        public void WaitForElementNotDisplayed(By by)
        {
            try
            {
                for (int i = 0; i < 60; i++)
                {
                    if (Driver.FindElement(by).Displayed == true)
                        System.Threading.Thread.Sleep(1000);
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal IList<IWebElement> GetNativeElementsInElement(IWebElement parentElement, By lookupBy, int maxWaitTime = 60)
        {
            IList<IWebElement> element = null;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    try
                    {
                        element = parentElement.FindElements(lookupBy);
                        if (element.Count > 0) break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return element;
        }

        #endregion

        #region ObjectOperationFunctions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupBy"></param>
        /// <param name="strInputValue"></param>
        /// <param name="maxWaitTime"></param>
        public void SetValueToObjectWithoutHighLight(By lookupBy, string strInputValue, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisibleWithoutHighLight(lookupBy, maxWaitTime);
                if (element != null)
                {
                    element.Clear();
                    element.SendKeys(strInputValue);
                }
                else throw new Exception("Object not found to Set value");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GetDropdownListItems(By lookupBy, string[] expData, int maxWaitTime = 60)
        {
            try
            {
                Reporter.Add(new Act(string.Format("Pick the time from application and match against time taken from SAT")));
                string[] data = new string[expData.Length];
                IWebElement element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    SelectElement dropDownElement = new SelectElement(element);
                    IList<IWebElement> options = dropDownElement.Options;
                    for (int i = 0; i < options.Count; i++)
                    {

                        //data.Add(options[i].Text);
                        data[i] = options[i].Text;
                    }
                }
                else throw new Exception("Object not found to Select value");

                for (int i = 0; i < expData.Length; i++)
                {
                    if (expData[i] != data[i])
                        throw new Exception("Expected Time doesn't match with the Time on Application");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<string> GetDropdownListItems(By lookupBy, int maxWaitTime = 60)
        {
            //Reporter.Add(new Act(string.Format("Pick the time from application and match against time taken from SAT")));
            List<string> expData = new List<string>();
            IWebElement element = null;
            try
            {
                //string[] data = new string[expData.Length];

                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    SelectElement dropDownElement = new SelectElement(element);
                    IList<IWebElement> options = dropDownElement.Options;
                    for (int i = 0; i < options.Count; i++)
                    {

                        //data.Add(options[i].Text);
                        //data[i] = options[i].Text;
                        expData.Add(options[i].Text);
                    }
                    return expData;
                }
                else throw new Exception("Object not found to Select value");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<int> GetDropdownListItemsByInt(By lookupBy, int maxWaitTime = 60)
        {

            List<int> expData = new List<int>();
            IWebElement element = null;
            try
            {
                Reporter.Add(new Act(string.Format("Pick the time from application and match against time taken from SAT")));
                //string[] data = new string[expData.Length];

                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    SelectElement dropDownElement = new SelectElement(element);
                    IList<IWebElement> options = dropDownElement.Options;
                    for (int i = 1; i < options.Count; i++)
                    {
                        //data.Add(options[i].Text);
                        //data[i] = options[i].Text;
                        expData.Add(Convert.ToInt32(options[i].Text));
                    }
                    return expData;
                }
                else throw new Exception("Object not found to Select value");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void clearObjectValue(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    element.Clear();
                }
                else throw new Exception("Object not found to Clear value");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        public bool validateElementAttributeValue(By lookupBy, string strAttributeName, string strExpectedValue, int maxWaitTime = 60)
        {
            bool valueEquals = false;
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    valueEquals = string.Equals(element.GetAttribute(strAttributeName).Trim().ToLower(), strExpectedValue.ToLower());
                }
                else throw new Exception("Object not found to validate Element attribute value");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return valueEquals;
        }
        
        public bool IsElementSelected(By lookupBy, int maxWaitTime = 60)
        {
            bool valueEquals = false;
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    valueEquals = element.Selected;
                }
                else throw new Exception("Object not found to validate Is Element selected");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return valueEquals;
        }

        public void MouseOverOnObject(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    new Actions(Driver).MoveToElement(element).MoveByOffset(element.Size.Height / 2, element.Size.Height / 2).Perform();
                    Thread.Sleep(1000);
                }
                else throw new Exception("Object not found to MouseOver");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void DragAndDropObject(By lookupBy, By lookupBy1, int maxWaitTime = 60)
        {
            IWebElement element1 = null, element2 = null;
            try
            {
                element1 = WaitForElementVisible(lookupBy, maxWaitTime);
                element2 = WaitForElementVisible(lookupBy1, maxWaitTime);
                if (element1 != null && element2 != null)
                {
                    Actions builder = new Actions(Driver);

                    IAction dragAndDrop = builder.ClickAndHold(element1)
                       .MoveToElement(element2)
                       .Release(element1)
                       .Build();

                    dragAndDrop.Perform();
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Object not found to drag and drop" + ex.Message);
            }
        }

        public void MoveToElement(By lookupBy, int int_offSetX, int int_offSetY, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    new Actions(Driver).MoveToElement(element, int_offSetX, int_offSetY).Perform();
                    Thread.Sleep(500);
                }
                else throw new Exception("Object not found to Move To Element");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void MoveToElementAndClick(By lookupBy, int int_offSetX, int int_offSetY, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    MoveToElement(lookupBy, int_offSetX, int_offSetY);
                    new Actions(Driver).Click().Perform();
                    Thread.Sleep(500);
                }
                else throw new Exception("Object not found to Move To Element");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CheckIfObjectExists(By lookupBy, int maxWaitTime = 30)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        
        public bool ValidateIfExists(By lookupBy, string objectName, int maxWaitTime = 30, bool Condition = true)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (Condition)
                {
                    if (element != null)
                    {
                        Reporter.Add(new Act(String.Format("Control '{0}' exist on the page", objectName)));
                        return true;
                    }
                    else
                    {
                        Reporter.Add(new Act(String.Format("Control '{0}' doesn't exist on the page", objectName), false, Driver));
                        return false;
                    }
                }
                else
                {
                    if (element == null)
                    {
                        Reporter.Add(new Act(String.Format("Control '{0}' doesn't exist on the page", objectName)));
                        return false;
                    }
                    else
                    {
                        Reporter.Add(new Act(String.Format("Control '{0}' exist on the page", objectName), false, Driver));
                        return true;
                    }
                }
            }
            catch
            {
                Reporter.Add(new Act(String.Format("Control '{0}' doesn't exist on the page", objectName), false, Driver));
                return false;
            }
        }

        /// <summary>
        /// Validate If Exists
        /// </summary>
        /// <param name="maxWaitTime"></param>
        /// <param name="Condition"></param>
        /// <param name="objectName"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public bool ValidateIfExists(string[] objectName, string[] objects, int maxWaitTime = 30, bool Condition = true)
        {
            try
            {
                List<bool> resultList = new List<bool>();
                for (int i = 0; i < objects.Length; i++)
                {
                    resultList.Add(ValidateIfExists(By.XPath(objects[i]), objectName[i], maxWaitTime, Condition));
                }
                return resultList.Contains(false) ? false : true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Validate control Value Or Text Or Enabled/Disabled
        /// </summary>
        /// <param name="lookupBy">Object to be verified</param>
        /// <param name="objectName">Name of the object</param>
        /// <param name="expectedTextOrValue">expected text/value. true for Enabled/Disabled state verification</param>
        /// <param name="attributeName">by default text. User can pass value, enabled or disabled</param>
        /// <param name="maxWaitTime">time to wait for the object visible</param>
        public void ValidateControlValueOrTextOrState(By lookupBy, string objectName, string expectedTextOrValue, string attributeName = "text", int maxWaitTime = 30)
        {
            IWebElement element = null;

            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    if (attributeName.ToLower().Equals("text"))
                    {
                        if (element.Text.Equals(expectedTextOrValue))
                            Reporter.Add(new Act(string.Format("control '{0}' text is shown as expected. The expected text is: '{1}'", objectName, expectedTextOrValue)));
                        else
                            Reporter.Add(new Act(string.Format("control '{0}' text is not shown as expected. The actual text is: '{1}'", objectName, element.Text), false, Driver));
                    }
                    else if (attributeName.ToLower().Equals("value"))
                    {
                        if (element.GetAttribute("value").Equals(expectedTextOrValue))
                            Reporter.Add(new Act(string.Format("control '{0}' value is shown as expected", objectName)));
                        else
                            Reporter.Add(new Act(string.Format("control '{0}' value is not shown as expected. The actual value is: '{1}'", objectName, element.GetAttribute("value")), false, Driver));
                    }
                    else if (attributeName.ToLower().Equals("enabled"))
                    {
                        if (element.Enabled.ToString().ToLower().Equals(expectedTextOrValue))
                            Reporter.Add(new Act(string.Format("control '{0}' is enabled as expected", objectName)));
                        else
                            Reporter.Add(new Act(string.Format("control '{0}' is disabled", objectName), false, Driver));
                    }
                    else if (attributeName.ToLower().Equals("disabled"))
                    {
                        if (element.GetAttribute(attributeName).Equals(expectedTextOrValue))
                            Reporter.Add(new Act(string.Format("control {0} is disabled as expected", objectName)));
                        else
                            Reporter.Add(new Act(string.Format("control {0} is enabled", objectName), false, Driver));
                    }
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Object '{0}' is not found to Verify TextOrValueOrState'", objectName), false, Driver));
                    throw new Exception(string.Format("Object '{0}' is not found to Verify TextOrValueOrState", objectName));

                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Exception occurred in in retreiving TextOrValueOrState from object : {0}. The exception is: {1}", objectName, ex.Message), false, Driver));
                throw new Exception(ex.Message);
            }
        }

        public void ValidateDropdownValue(By lookupBy, string objectName, string expectedTextOrValue, int maxWaitTime = 30)
        {
            IWebElement element = null;

            try
            {

                element = WaitForElementVisible(lookupBy, maxWaitTime);

                //switch (selectBy.ToLower())
                //{
                //    case "text": dropDownElement.SelectByText(inputValue); break;
                //    case "index": dropDownElement.SelectByIndex(Convert.ToInt32(inputValue)); break;
                //    case "value": dropDownElement.SelectByValue(inputValue); break;
                //    default: dropDownElement.SelectByText(inputValue); break;
                //}
                //Reporter.Add(new Act(String.Format("control '{0}' is set with value '{1}'", objectName, inputValue)));

                if (element != null)
                {
                    SelectElement dropDownElement = new SelectElement(element);
                    if (dropDownElement.SelectedOption.Text.Equals(expectedTextOrValue))
                        Reporter.Add(new Act(string.Format("control '{0}' value is shown as expected. The expected text is: " + expectedTextOrValue, objectName)));
                    else
                        Reporter.Add(new Act(string.Format("control '{0}' value is not shown as expected. The actual value is: '{1}'", objectName, element.Text), false, Driver));
                }
                else
                {
                    Reporter.Add(new Act(string.Format("Dropdown Object '{0}' is not found to Verify Value'", objectName), false, Driver));
                    throw new Exception(string.Format("Dropdown Object '{0}' is not found to Verify TextOrValueOrState", objectName));

                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Exception occurred in in retreiving Value from Dropdown object : {0}. The exception is: {1}", objectName, ex.Message), false, Driver));
                throw new Exception(ex.Message);
            }
        }


        public void ObjectClick(By lookupBy, string objectName, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    Actions action = new Actions(Driver);
                    element.Click();
                    Reporter.Add(new Act(String.Format("Clicked on '{0}'", objectName)));
                }
                else
                {
                    Reporter.Add(new Act(String.Format("Object '{0}' not found to click", objectName), false, Driver));
                    throw new Exception(String.Format("Object '{0}' not found to click", objectName));
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Exception occured in clicking '{0}'. The exception is: {1}", objectName, ex.Message), false, Driver));
                throw new Exception(ex.Message);
            }
            WaitForPageLoad();
        }




        public void ObjectClickByPageScrolling(By lookupBy, string objectName, int YValue = 100, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                PageScrollDown(lookupBy, YValue);
                if (element != null)
                {
                    Actions action = new Actions(Driver);
                    element.Click();
                    Reporter.Add(new Act(String.Format("Clicked on '{0}'", objectName)));
                }
                else
                {
                    Reporter.Add(new Act(String.Format("Object '{0}' not found to click", objectName), false, Driver));
                    throw new Exception("Object not found to click");
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Exception occured in clicking '{0}'. The exception is: {1}", objectName, ex.Message), false, Driver));
                throw new Exception(ex.Message);
            }
        }


        public void ObjectClickByPageScrolling(IWebElement element, string objectName, int YValue = 100, int maxWaitTime = 60)
        {
            try
            {
                PageScrollDown(element, YValue);
                if (element != null)
                {
                    Actions action = new Actions(Driver);
                    element.Click();
                    Reporter.Add(new Act(String.Format("Clicked on '{0}'", objectName)));
                }
                else
                {
                    Reporter.Add(new Act(String.Format("Object '{0}' not found to click", objectName), false, Driver));
                    throw new Exception("Object not found to click");
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Exception occured in clicking '{0}'. The exception is: {1}", objectName, ex.Message), false, Driver));
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Method to Set Object with Selected Input Value
        /// </summary>
        /// <param name="lookupBy">Object to loop for</param>
        /// <param name="objectName">Name of the object</param>
        /// <param name="inputValue">Input Value</param>
        /// <param name="maxWaitTime">Maximum Wait Time</param>
        public void SetObjectValue(By lookupBy, string objectName, string inputValue, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    element.Clear();
                    element.SendKeys(inputValue);
                    Reporter.Add(new Act(String.Format("control '{0}' is set with value '{1}'", objectName, objectName.ToLower().Equals("password") ? "*****" : inputValue)));
                }
                else
                {
                    Reporter.Add(new Act(String.Format("control '{0}' is not found to set value '{1}'", objectName, inputValue), false, Driver));
                    throw new Exception(String.Format("Object '{0}' not found to Set value", objectName));
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Exception occurred in setting value '{0}' for object '{1}. The exception is: {2}", inputValue, objectName, ex.Message), false, Driver));
                throw new Exception(ex.Message);
            }
        }

        public void DropdownSelectValue(By lookupBy, string objectName, string inputValue, string selectBy = "text", int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    SelectElement dropDownElement = new SelectElement(element);
                    if (dropDownElement.Options.Count > 1)
                    {
                        switch (selectBy.ToLower())
                        {
                            case "text": dropDownElement.SelectByText(inputValue); break;
                            case "index": dropDownElement.SelectByIndex(Convert.ToInt32(inputValue)); break;
                            case "value": dropDownElement.SelectByValue(inputValue); break;
                            default: dropDownElement.SelectByText(inputValue); break;
                        }

                        Reporter.Add(new Act(String.Format("control '{0}' is set with value '{1}'", objectName, inputValue)));
                    }
                }
                else
                {
                    Reporter.Add(new Act(String.Format("dropdown control '{0}' is not found to set value '{1}'", objectName, inputValue), false, Driver));
                    throw new Exception(String.Format("Object '{0}' not found to Select value", objectName));
                }

            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Exception occurred in setting value '{0}' for dropdown object '{1}. The exception is: {2}", inputValue, objectName, ex.Message), false, Driver));
                throw new Exception(ex.Message);
            }
        }

        public string RetrieveObjectValue(By lookupBy, string objectName, string attributeName = "text")
        {
            string returnValue = string.Empty;
            IWebElement element = null;
            int maxWaitTime = 30;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    if (attributeName.ToLower().Equals("text"))
                    {
                        returnValue = element.Text;
                    }
                    else
                    {
                        returnValue = element.GetAttribute(attributeName);
                    }
                }
                else
                {
                    Reporter.Add(new Act(String.Format("object '{0}' is not found to retrieve text'", objectName), false, Driver));
                    throw new Exception(String.Format("Object '{0}' not found to Get Element attribute value", objectName));

                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(String.Format("Exception occurred in in retreiving text from object : {0}. The exception is: {1}", objectName, ex.Message), false, Driver));
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        public string RetrieveObjectValue(By lookupBy, string attributeName = "text")
        {
            string returnValue = string.Empty;
            IWebElement element = null;
            int maxWaitTime = 30;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    if (attributeName.ToLower().Equals("text"))
                    {
                        returnValue = element.Text;
                    }
                    else
                    {
                        returnValue = element.GetAttribute(attributeName);
                    }
                }
                else
                {
                    Reporter.Add(new Act(String.Format("object not found to get element attribute value'"), false, Driver));
                    throw new Exception("Object not found to Get Element attribute value");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        public string RetrieveDropdownValue(By lookupBy)
        {
            string returnValue = string.Empty;
            IWebElement element = null;
            int maxWaitTime = 30;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    SelectElement selectedValue = new SelectElement(element);
                    returnValue = selectedValue.SelectedOption.Text;
                }
                else throw new Exception("Object not found to Get Element attribute value");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// Compare Lists
        /// </summary>
        /// <param name="ActualList">ActualList</param>
        /// <param name="ExpectedList">ExpectedList</param>
        /// <returns>true, if validated successfully</returns>
        public bool CompareLists(List<string> ActualList, List<string> ExpectedList, string nameOftheList = "")
        {
            bool flag = true;
            try
            {
                List<string> unmatchedList = ActualList.Where(x => !ExpectedList.Contains(x)).ToList();

                if (unmatchedList.Count == 0)
                {
                    if (string.IsNullOrEmpty(nameOftheList))
                        Reporter.Add(new Act(String.Format("The Actual and Expected Lists are matched.")));
                    else
                        Reporter.Add(new Act(String.Format("The Actual and Expected List of {0} are matched.", nameOftheList)));

                    flag = true;
                }
                else
                {
                    Reporter.Add(new Act(String.Format("Following are the mismatches"), false, Driver));
                    foreach (string item in unmatchedList)
                    {
                        Reporter.Add(new Act(item));
                    }

                    flag = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Performs Click with JavaScript
        /// </summary>
        /// <param name="locator"></param>
        public void JavaScriptClick(By by, string objectName)
        {
            Reporter.Add(new Act(String.Format("Clicked on '{0}'", objectName)));
            IWebElement element = Driver.FindElement(by);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            jsExecutor.ExecuteScript(@"arguments[0].click();", new object[] { element });
        }

        #endregion

        #region GenericFunctions

        public bool ValidateDriverTitle(string strExpectedValue, int maxWaitTime = 60)
        {
            bool titleMatches = false;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    if (Driver.Title.ToString().Contains(strExpectedValue)) return true;
                    else Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return titleMatches;
        }
        public bool ValidateDriverTitleContains(string strExpectedValue, int maxWaitTime = 60)
        {
            bool titleMatches = false;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    if (Driver.Title.ToString().Contains(strExpectedValue)) return true;
                    else Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return titleMatches;
        }

        public void NavigateToURL(string strURL, string page = "")
        {
            try
            {
                Driver.Navigate().GoToUrl(strURL);
                //Driver.Manage().Window.Maximize();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string retrieveCurrentBrowserURL(int maxWaitTime = 0)
        {
            try
            {
                Thread.Sleep(maxWaitTime * 1000);
                return Driver.Url;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DoubleClickOnObject(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    Actions action = new Actions(Driver);
                    action.DoubleClick(element).Build().Perform();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SwitchToElement(By lookupBy, int maxWaitTime)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    Driver.SwitchTo().Frame(element);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SwitchToIFrames(By lookupBy)
        {
            try
            {
                IReadOnlyCollection<IWebElement> frames = Driver.FindElements(By.TagName("iframe"));
                foreach (IWebElement frame in frames)
                {
                    Driver.SwitchTo().Frame(frame);
                    IReadOnlyCollection<IWebElement> chframes = Driver.FindElements(By.TagName("iframe"));
                    if (chframes.Count.Equals(0))
                    {
                        Driver.SwitchTo().ParentFrame();
                    }
                    else
                    {
                        foreach (IWebElement chframe in chframes)
                        {
                            Driver.SwitchTo().Frame(chframe);
                            if (CheckIfObjectExists(lookupBy))
                            {
                                break;
                            }
                            else
                            {
                                Driver.SwitchTo().ParentFrame();
                            }
                        }
                    }
                }
            }
            catch (StaleElementReferenceException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SwitchToiFrame(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {

                element = GetNativeElement(lookupBy, maxWaitTime);
                if (element != null)
                {
                    IWebElement webElement = GetNativeElementInElement(element, By.TagName("iframe"));

                    Driver.SwitchTo().Frame(webElement);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SwitchToDefaultContent()
        {
            try
            {
                Driver.SwitchTo().DefaultContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void SwitchToBaseWindow()
        {
            try
            {

                Driver.SwitchTo().Window(Driver.WindowHandles[0]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Wait time
        /// </summary>
        /// <param name="maxWaitTime"></param>
        public void Wait(int maxWaitTime = 800)
        {
            Thread.Sleep(maxWaitTime);
        }

        public void WaitForPageLoad(int maxWaitTime = 800)
        {
            bool objAvailable = false;
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitTime));
            IJavaScriptExecutor javascript = Driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("Driver", "Driver not supports javascript execution");
            objAvailable = wait.Until((d) =>
            {
                try
                {
                    string readyState = javascript.ExecuteScript(
                        "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (Exception ex)
                {
                    Reporter.Add(new Act(string.Format("Stopped execution after waiting for page to load for 10 sec"), false));
                    Console.WriteLine(string.Format("Stopped execution after waiting for page to load for 10 sec. Message {0}", ex.Message));
                    return false;
                }
            });
        }

        public void SwitchToWindow(string strWindowTitleName, string urlContent, int maxWaitTime = 60)
        {
            try
            {
                bool blnwindowFound = false;
                for (int i = 0; i < maxWaitTime; i++)
                {
                    var currentWindow = Driver.CurrentWindowHandle;
                    List<string> availableWindows = new List<string>(Driver.WindowHandles);
                    foreach (string w in availableWindows)
                    {
                        if (w != currentWindow)
                        {
                            Driver.SwitchTo().Window(w);
                            if (Driver.Title == strWindowTitleName || Driver.Url.ToLower().Contains(urlContent.ToLower()))
                            {
                                blnwindowFound = true;
                                break;
                            }
                        }

                    }
                    if (blnwindowFound == true) break;
                    else Thread.Sleep(1000);
                }
                if (blnwindowFound == false) throw new Exception(strWindowTitleName + " not found to switch");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SwitchToWindow(string strWindowTitleName, int maxWaitTime = 15)
        {
            try
            {
                Reporter.Add(new Act("Switching To Window"));
                Reporter.Add(new Act("Getting current windowhandle"));
                bool blnwindowFound = false;
                for (int i = 0; i < maxWaitTime; i++)
                {
                    var currentWindow = Driver.CurrentWindowHandle;
                    List<string> availableWindows = new List<string>(Driver.WindowHandles);
                    foreach (string w in availableWindows)
                    {
                        if (w != currentWindow)
                        {
                            Driver.SwitchTo().Window(w);
                            if (Driver.Title == strWindowTitleName)
                            {
                                Reporter.Add(new Act(strWindowTitleName + "found"));
                                blnwindowFound = true;
                                break;
                            }
                        }

                    }
                    if (blnwindowFound == true) break;
                    else Thread.Sleep(1000);
                }
                if (blnwindowFound == false)
                {
                    Reporter.Add(new Act(strWindowTitleName + " not found to switch"));
                    throw new Exception(strWindowTitleName + " not found to switch");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SwitchToWindowContains(string strWindowTitleName, int maxWaitTime = 60)
        {
            try
            {
                bool blnwindowFound = false;
                for (int i = 0; i < maxWaitTime; i++)
                {
                    var currentWindow = Driver.CurrentWindowHandle;
                    List<string> availableWindows = new List<string>(Driver.WindowHandles);
                    foreach (string w in availableWindows)
                    {
                        if (w != currentWindow)
                        {
                            Driver.SwitchTo().Window(w);
                            if (Driver.Title.Contains(strWindowTitleName))
                            {
                                blnwindowFound = true;
                                break;
                            }
                            else
                            {
                                Driver.SwitchTo().Window(currentWindow);
                            }
                        }
                    }
                    if (blnwindowFound == true) break;
                    else Thread.Sleep(1000);
                }
                if (blnwindowFound == false) throw new Exception(strWindowTitleName + " not found to switch");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CloseWindow(int maxWaitTime = 60)
        {
            try
            {
                Driver.ExecuteScript("close()");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PageScrollUp()
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                js.ExecuteScript("window.scrollTo(0, 0)");
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PageScrollUp(By by, int YValue = 100)
        {
            try
            {
                IWebElement element = Driver.FindElement(by);
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                js.ExecuteScript("arguments[0].scrollTop = arguments[1];", element, YValue);
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PageScrollDown(By by, int YValue = 100)
        {
            try
            {
                IWebElement element = Driver.FindElement(by);
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                js.ExecuteScript(string.Format("window.scrollTo(0, {0})", (element.Location.Y - YValue)));
                System.Threading.Thread.Sleep(1000);
                //JavascriptExecutor jse = (JavascriptExecutor)driver;
                //jse.executeScript("window.scrollTo(0, document.body.scrollHeight)");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PageScrollDown(IWebElement element, int YValue = 100)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                js.ExecuteScript(string.Format("window.scrollTo(0, {0})", (element.Location.Y - YValue)));
                System.Threading.Thread.Sleep(1000);
                //JavascriptExecutor jse = (JavascriptExecutor)driver;
                //jse.executeScript("window.scrollTo(0, document.body.scrollHeight)");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            try
            {
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return builder.ToString();
        }

        public string GenerateRandomNumber(int size)
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 0; i < size; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }
        #endregion

        /// <summary>
        /// Switch Between Windows & returns parent window handle
        /// </summary>
        public string SwitchToChildWindow()
        {
            String currentWindow = null;
            try
            {
                currentWindow = Driver.CurrentWindowHandle;
                IReadOnlyCollection<String> windowHandles = Driver.WindowHandles;

                foreach (string handle in windowHandles)
                {
                    if (handle != currentWindow)
                    {
                        Driver.SwitchTo().Window(handle);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return currentWindow;
        }

        /// <summary>
        /// Switch to window by taking window handle as parameter
        /// </summary>
        public void SwitchToWindowUsingWindowHandle(string WindowHandle)
        {
            try
            {
                IReadOnlyCollection<String> windowHandles = Driver.WindowHandles;

                foreach (string handle in windowHandles)
                {
                    if (handle == WindowHandle)
                    {
                        Driver.SwitchTo().Window(handle);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LeaveCurrentWebSite()
        {
            Driver.SwitchTo().Alert().Accept();
        }

        public string GenerateUniqueEmailId(string prefixToUse = "Test")
        {
            return prefixToUse + GenerateDateTimeString() + "@Bloominbrands.com";
        }

        /// <summary>
        /// Generates the date time string.
        /// </summary>
        /// <returns>Date Time stamp string</returns>
        public string GenerateDateTimeString()
        {
            //return DateTime.Now.ToString("MMddyyyyhhmmssfffftt");
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }

        #region AlertPopUpFunctions
        internal IAlert GetAlertHandle(int maxWaitTime = 10)
        {
            IAlert AlertHandle = null;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    try
                    {
                        AlertHandle = Driver.SwitchTo().Alert();
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return AlertHandle;
        }

        public bool IsAlertPresent()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(3000) /*timeout in seconds*/);
                if (wait.Until(ExpectedConditions.AlertIsPresent()) == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IWebElement WaitForElementExist(By by)
        {
            IWebElement element = null;
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(3000) /*timeout in seconds*/);
                element = wait.Until(ExpectedConditions.ElementExists(by));

                if (element == null)
                    return element;
                else
                    return element;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return element;
            }

        }

        public void AcceptAlert(int maxWaitTime = 10)
        {
            IAlert AlertHandle = null;
            try
            {
                AlertHandle = GetAlertHandle(maxWaitTime);
                if (AlertHandle != null)
                {
                    AlertHandle.Accept();
                }
                else
                {
                    throw new Exception("Alert handle not available");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DismissAlert(int maxWaitTime = 10)
        {
            IAlert AlertHandle = null;
            try
            {
                AlertHandle = GetAlertHandle(maxWaitTime);
                if (AlertHandle != null)
                {
                    AlertHandle.Dismiss();
                }
                else
                {
                    throw new Exception("Alert handle not available");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetAlertText(int maxWaitTime = 10)
        {
            IAlert AlertHandle = null;
            string strAlertText = string.Empty;
            try
            {
                AlertHandle = GetAlertHandle(maxWaitTime);
                if (AlertHandle != null)
                {
                    strAlertText = AlertHandle.Text;
                }
                else
                {
                    throw new Exception("Alert handle not available");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strAlertText;
        }
        #endregion

        #region AssertionFunctions
        public void CheckStringEqual(String actual, String expected)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Verify '{0}' Equals '{1}'", expected, actual)));

                if (!String.Equals(actual, expected))
                {
                    Reporter.Add(new Act(String.Format("Not Equal {0} : {1}", actual, expected), false, Driver));
                }
                else
                {
                    Reporter.Add(new Act(String.Format(" {0} : {1} are equal", actual, expected)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CheckStringContains(String StrText, String Token)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Verify '{0}' contains '{1}'", StrText, Token)));
                if (!StrText.Contains(Token))
                {
                    Reporter.Add(new Act(String.Format("Does not Contan {0} : {1}", StrText, Token), false, Driver));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void Equal(DateTime actual, DateTime expected, String name = "")
        {
            try
            {
                Reporter.Add(new Act(String.Format("Verify '{0}' Equals '{1}'", expected, actual)));

                if (!String.Equals(actual, expected))
                {
                    throw new Exception(String.Format("Not Equal {0} : {1}", actual, expected));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void NullOrEmpty(String data)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Verify Null or Empty '{0}'", data)));

                if (!String.IsNullOrEmpty(data) || !String.IsNullOrWhiteSpace(data))
                {
                    throw new Exception(String.Format("Data is not Null or Empty"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void NotNullOrEmpty(String data)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Verify Null or Empty '{0}'", data)));

                if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                {
                    throw new Exception(String.Format("Data is Null or Empty"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Equal(Int64 first, Int64 second)
        {
            try
            {
                if (!(first == second))
                {
                    Reporter.Add(new Act(String.Format("Not Equal {0} : {1}", first, second), false, Driver));
                }
                else
                {
                    Reporter.Add(new Act(String.Format("Verified '{0}' Equals '{1}'", first, second)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CheckEqualityOfObjects(bool first, bool second, string strStepDesc = "")
        {
            try
            {
                if (string.IsNullOrEmpty(strStepDesc))
                {
                    Reporter.Add(new Act(String.Format("Verify '{0}' Equals '{1}'", first, second)));
                }
                else
                {
                    Reporter.Add(new Act(strStepDesc));
                }

                if (!(first == second))
                {
                    throw new Exception(String.Format("Not Equal {0} : {1}", first, second));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Equal(Decimal first, Decimal second, string strObject = "")
        {
            try
            {
                if (!first.Equals(second))
                {
                    Reporter.Add(new Act(String.Format("Expected {0} is not equal to Actual {1}", first, second), false, Driver));
                }
                else
                {
                    Reporter.Add(new Act(String.Format("Expected '{0}' is equals to Actual '{1}'", first, second)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GreaterThan(Decimal first, Decimal second, string strObject = "")
        {
            try
            {
                if (first != 0)
                {
                    if (first > second)
                    {
                        Reporter.Add(new Act(String.Format("'{0}' Is Greater Than '{1}'", first, second)));
                    }
                    else
                    {
                        Reporter.Add(new Act(String.Format("'{0}' Is Less Than '{1}'", first, second), false, Driver));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AlphabeticalOrder(IList<String> items)
        {
            try
            {
                String lastItem = String.Empty;
                foreach (String item in items)
                {
                    if (lastItem == String.Empty)
                    {
                        lastItem = item;
                    }

                    if (lastItem.CompareTo(item) > 0)
                    {
                        throw new Exception(String.Format("Item {0} not in alphabetical order", item));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void NotEqual(String first, String second)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Verify '{0}' Not Equals '{1}'", first, second)));

                if (String.Equals(first, second))
                {
                    throw new Exception(String.Format("Equal {0} : {1}", first, second));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void NotEqual(Int64 first, Int64 second)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Verify '{0}' Not Equals '{1}'", first, second)));

                if (first == second)
                {
                    throw new Exception(String.Format("Not Equal {0} : {1}", first, second));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region TableFunctions

        public IList<IWebElement> GetTableDataRowWise(By locater)
        {
            try
            {
                IList<IWebElement> data = GetNativeElements(locater);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public DataTable GetDataTable(int columnCount)
        {
            DataTable dt = new DataTable();
            try
            {
                if (columnCount > 0)
                {
                    for (int i = 0; i < columnCount; i++)
                    {
                        dt.Columns.Add(i.ToString(), typeof(object));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public IWebElement GetFirstElementFromTable(ICollection<IWebElement> cells)
        {
            IWebElement currentData = null;
            try
            {
                IEnumerator<IWebElement> tableCells = cells.GetEnumerator();
                while (tableCells.MoveNext())
                {
                    currentData = tableCells.Current;
                    break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return currentData;
        }
        public DataRow GetSpecifiedRow(DataTable table, string columnValue)
        {
            DataRow dRow = null;
            object[] rowData;
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    int length = row.ItemArray.Length;
                    rowData = row.ItemArray;
                    for (int count = 0; count < length; count++)
                    {
                        if (rowData[count].Equals(columnValue))
                        {
                            dRow = row;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dRow;
        }
        public DataTable GetCompleteTableDetails(By locateBy, int maxWaitTime = 60)
        {
            ICollection<IWebElement> rows = null;
            ICollection<IWebElement> cells = null;
            DataTable dtTableDetails = null;
            IWebElement table = null;
            try
            {
                table = GetNativeElement(locateBy);
                rows = GetNativeElementsInElement(table, By.TagName("tr"));
                int columnCount = GetFirstElementFromTable(rows).FindElements(By.TagName("td")).Count;
                dtTableDetails = GetDataTable(columnCount);
                DataRow dRow;
                int columns = 0;

                foreach (var row in rows)
                {
                    cells = row.FindElements(By.TagName("td"));
                    dRow = dtTableDetails.NewRow();
                    IEnumerator<IWebElement> tableCells = cells.GetEnumerator();
                    while (tableCells.MoveNext())
                    {

                        dRow[columns] = tableCells.Current.Text.ToString();
                        columns++;
                    }
                    dtTableDetails.Rows.Add(dRow);
                    columns = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dtTableDetails;
        }

        public bool VerifyElementExistsUnderRow(string expRowName, string expValue, By locator, int maxWaitTime = 60)
        {
            bool objFound = false;
            try
            {
                IWebElement table = GetNativeElement(locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                foreach (var row in rows)
                {
                    string name = row.Text;
                    if (name.Contains(expRowName))
                    {

                        if (name.Contains(expValue))
                        {
                            objFound = true;
                            Reporter.Add(new Act(expRowName + " contains " + expValue));
                            break;
                        }
                        else
                        {
                            Reporter.Add(new Act(expRowName + " does not contains " + expValue));
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return objFound;
        }

        public int GetRowCount(By locator, int maxWaitTime = 60)
        {
            int count;
            try
            {
                IWebElement table = GetNativeElement(locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                count = rows.Count;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return count;
        }

        public string GetElementDataUnderRow(string expRowName, By locator, int maxWaitTime = 60)
        {
            string strNameValue = null;
            try
            {
                IWebElement table = GetNativeElement(locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                foreach (var row in rows)
                {
                    string name = row.Text;
                    if (name.Contains(expRowName))
                    {
                        int stIndex = name.LastIndexOf(expRowName);
                        strNameValue = name.Substring(stIndex + expRowName.Length).Trim();
                        if (strNameValue.Length > 1)
                        {
                            Reporter.Add(new Act(expRowName + " does have a value: " + strNameValue));

                        }

                        else
                        {
                            throw new Exception("No value for the Data " + expRowName);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return strNameValue;
        }

        public IWebElement GetRowWithExpectedTextInTable(string expRowName, By locator, int maxWaitTime = 60)
        {
            IWebElement elementWithText = null;
            try
            {
                IWebElement table = GetNativeElement(locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                foreach (var row in rows)
                {
                    string name = row.Text;
                    if (name.Contains(expRowName))
                    {
                        int stIndex = name.LastIndexOf(expRowName);
                        string strNameValue = name.Substring(stIndex + expRowName.Length).Trim();
                        elementWithText = row;
                        if (strNameValue.Length > 1)
                        {
                            Reporter.Add(new Act(expRowName + " does have a value: " + strNameValue));

                        }

                        else
                        {
                            throw new Exception("No value for the Data " + expRowName);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return elementWithText;
        }

        public void VerifyColumnValueonRowValueinSearchresult(string rowvalue, string columnvalue, By locator)
        {
            Reporter.Add(new Act(String.Format("Validating search results")));
            try
            {
                int i = 1;
                string val = string.Empty;
                IList<string> DisplayNames = new List<string>();

                IWebElement table = GetNativeElement(locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                string[] words = locator.ToString().Split(':');
                string path = words[1].Trim();
                foreach (var row in rows)
                {

                    IWebElement architecture = row.FindElement(By.XPath(path + "/tbody/" + "tr[" + i + "]" + "/td[6]"));
                    string value = architecture.Text.ToString();
                    if (value == columnvalue)
                    {
                        IWebElement hostName = row.FindElement(By.XPath(path + "/tbody/" + "tr[" + i + "]" + "/td[4]/a"));
                        if (hostName.Text.ToString() == rowvalue)
                        {
                            Reporter.Add(new Act("Architecture is displayed as " + columnvalue + "in search results "));
                            return;
                        }
                        else
                        {
                            i++;
                        }


                    }
                    else
                    {
                        i++;
                    }

                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(ex.Message));
                throw new Exception(ex.Message);
            }

        }
        public void VerifyResultSortedByColumnName(string expcolumnName, By locator)
        {


            try
            {
                int i = 1;
                string val = string.Empty;
                IList<string> DisplayNames = new List<string>();
                string value = "";

                IWebElement table = GetNativeElement(locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                ICollection<IWebElement> columns = GetNativeElementsInElement(table, By.TagName("td"));
                string[] words = locator.ToString().Split(':');
                string path = words[1].Trim();
                foreach (var row in rows)
                {
                    IWebElement displayName = row.FindElement(By.XPath(path + "/tbody/" + "tr[" + i + "]" + "/td[5]/a"));
                    value = displayName.Text.ToString();
                    DisplayNames.Add(value);
                    i++;
                    if (i == rows.Count)
                    {
                        //Sort elements by Alphabets
                        List<string> list = DisplayNames.ToList<string>();
                        list.Sort();
                        for (int k = 0; k <= DisplayNames.Count - 1; k++)
                        {
                            if (list[k] == DisplayNames[k])
                            {
                                if (k == DisplayNames.Count - 1)
                                {
                                    Reporter.Add(new Act("Elements are displayed in the alphabetical order"));
                                    return;
                                }

                            }
                            else
                            {
                                Reporter.Add(new Act("Elements are NOT displayed in the alphabetical order"));
                                throw new Exception("Elements are NOT displayed in the alphabetical order at " + DisplayNames[k]);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public void IsColumnSortedInTable(string strColumn, By Locator)
        {
            try
            {

                IWebElement table = GetNativeElement(Locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                int intColIndex = 0;
                int i = 0;
                IWebElement firstRow = rows.ElementAt(0);
                ICollection<IWebElement> ColValue = null;
                ICollection<IWebElement> headersAvai = GetNativeElementsInElement(firstRow, By.TagName("th"));
                foreach (IWebElement colHeader in headersAvai)
                {
                    if (colHeader.Text == strColumn)
                    {
                        intColIndex = i;
                        //click to sort
                        int itr = 0;
                        IWebElement srtLink = GetNativeElementInElement(colHeader, By.TagName("i"));
                        //IWebElement srtLink = waitForElementVisible(colHeader.FindElement(By.TagName("i")));

                        while (srtLink.GetAttribute("className").ToString() != ("tablesorter-icon icon-sort-down"))
                        {

                            itr++;
                            srtLink.Click();
                            srtLink = WaitForElementVisible(srtLink);
                            if (itr > 2)
                                throw new Exception(String.Format("Unable to click <b><u>{0}</u></b> link to sort the list", srtLink.Text));
                        }
                        break;
                    }
                    i++;

                }
                List<string> lstValue = new List<string>();
                rows = GetNativeElementsInElement(table, By.TagName("tr"));
                foreach (IWebElement row in rows)
                {
                    ColValue = GetNativeElementsInElement(row, By.TagName("td"));
                    if (ColValue.Count >= intColIndex)
                    {
                        lstValue.Add(ColValue.ElementAt(intColIndex).Text.ToLower());
                    }
                }

                List<String> sList = new List<string>();
                sList.AddRange(lstValue);

                sList.Sort();

                for (int ctr = 0; ctr <= lstValue.Count; ctr++)
                {
                    if (lstValue[ctr] != sList[ctr])
                        throw new Exception(String.Format("<p>List is not sorted: <b><u>{0}</u></b> should be before <b><u>{1}</u></b> </p>", sList[ctr], lstValue[ctr]));

                }
            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public void ClickValueintable(string columnName, string expvalue, By locator)
        {
            try
            {
                Reporter.Add(new Act(String.Format("Clcik on Distribution List Item")));
                int i = 1;
                string val = string.Empty;
                IList<string> DisplayNames = new List<string>();
                IWebElement table = GetNativeElement(locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                string[] words = locator.ToString().Split(':');
                string path = words[1].Trim();
                foreach (var row in rows)
                {

                    IWebElement value = row.FindElement(By.XPath(path + "/tbody/" + "tr[" + i + "]" + "/td[4]/a"));
                    if (value.Text.ToString() == expvalue)
                    {
                        value.Click();
                        return;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            catch (Exception ex)
            {
                Reporter.Add(new Act(ex.Message));
                throw new Exception(ex.Message);
            }

        }

        public void VerifyColumnHeaderInTable(String tableID, String columnName)
        {
            try
            {
                string[] columnNames = TestDataNode.SelectSingleNode(columnName).InnerText.ToString().Split(',');
                string tableName = TestDataNode.SelectSingleNode(tableID).InnerText.ToString();
                SwitchToIFrames(By.Id(tableName));
                for (int i = 0; i < columnNames.Length; i++)
                {
                    VerifyValueForCorrespondingDataInTable("Name", By.Id(tableName), columnNames[i].ToString());
                    Reporter.Add(new Act("Column Name Displyed As - " + columnNames[i].ToString()));
                }
                SwitchToDefaultContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void VerifyColumnHeaderInTable(String refName, String tableID, String columnName)
        {
            try
            {
                string[] columnNames = TestDataNode.SelectSingleNode(columnName).InnerText.ToString().Split(',');
                string tableName = TestDataNode.SelectSingleNode(tableID).InnerText.ToString();
                SwitchToIFrames(By.Id(tableName));
                for (int i = 0; i < columnNames.Length; i++)
                {
                    VerifyValueForCorrespondingDataInTable(refName, By.Id(tableName), columnNames[i].ToString());
                    Reporter.Add(new Act("Column Name Displyed As - " + columnNames[i].ToString()));
                }
                SwitchToDefaultContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void VerifyValueForCorrespondingDataInTable(string refName, By locator, string compareValue)
        {
            try
            {
                string val = string.Empty;
                IList<string> DisplayNames = new List<string>();

                IWebElement table = GetNativeElement(locator);
                ICollection<IWebElement> rows = GetNativeElementsInElement(table, By.TagName("tr"));
                string[] words = locator.ToString().Split(':');
                string path = words[1].Trim();
                bool usrRoleAvbl = false;
                foreach (var row in rows)
                {
                    if (row.Text.Contains(refName))
                    {
                        usrRoleAvbl = row.Text.Contains(compareValue.ToString().Trim()) ? true : false;
                        if (usrRoleAvbl)
                            return;
                        else
                            throw new Exception(String.Format("Invalid: <b>{0}</b> for Value <b>{1}</b>", compareValue, refName));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PageReload()
        {
            try
            {
                Driver.Navigate().Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PageBack()
        {
            try
            {
                Driver.Navigate().Back();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PressEscapeKey()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                Actions action = new Actions(Driver);
                action.SendKeys(OpenQA.Selenium.Keys.Escape);
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void pressArrowDownKey()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                Actions action = new Actions(Driver);
                action.SendKeys(OpenQA.Selenium.Keys.ArrowDown).Build().Perform();
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void pressEnterKey()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                Actions action = new Actions(Driver);
                action.SendKeys(OpenQA.Selenium.Keys.Enter).Build().Perform();
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void pressArrowUpKey()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                Actions action = new Actions(Driver);
                action.SendKeys(OpenQA.Selenium.Keys.ArrowUp).Build().Perform();
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void pressTabKey()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                Actions action = new Actions(Driver);
                action.SendKeys(OpenQA.Selenium.Keys.Tab).Build().Perform();
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }

    public static class SeleniumExtention
    {
        /// <summary>
        /// HighLightObject
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="lookupBy"></param>
        public static IWebElement HighLightObject(this IWebElement element, RemoteWebDriver driver)
        {
            try
            {
                if (element != null)
                {
                    try
                    {
                        string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                        jsExecutor.ExecuteScript(script, new object[] { element });
                        jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { element });
                    }
                    catch { }
                }
            }
            catch
            {
                Thread.Sleep(1000);
            }
            return element;
        }
    }
}