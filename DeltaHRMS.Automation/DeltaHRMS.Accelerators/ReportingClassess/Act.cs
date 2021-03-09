using DeltaHRMS.Accelerators.UtilityClasses;
using DeltaHRMS.Accelerators.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace DeltaHRMS.Accelerators.Reporting
{
    public class Act
    {
        private bool isSuccess = true;

        /// <summary>
        /// Creates Action instance
        /// </summary>
        /// <param name="title"></param>
        public Act(String title)
        {
            try
            {
                this.Title = title;
                Console.WriteLine(this.Title);
                //this.TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                this.TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.Local);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Act(string actualValue, string expectedValue, string attributeName)
        {
            try
            {   
                if (actualValue.Equals(expectedValue))
                {   
                    this.Title = string.Format("Values are matching for <b>{0}</b>:" +
                    Environment.NewLine + "  Actual Value : <span style=\"color:Green;\">{1}</span>, Expected Value : <span style=\"color:Green;\">{2}</span>;",
                   attributeName, actualValue, expectedValue);
                }
                else
                {
                    this.IsSuccess = actualValue.Equals(expectedValue);
                    this.Title = string.Format("Values are mismatching for <b>{0}</b>: " +
                    Environment.NewLine + " Actual Value : <span style=\"color:Red;\">{1}</span>,  Expected Value : <span style=\"color:Red;\">{2}</span>;",
                    attributeName, actualValue, expectedValue, false);
                }
                this.TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.Local);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Act(String title, bool isPass)
        {
            try
            {
                this.Title = title;
                this.isSuccess = isPass;
                Console.WriteLine(this.Title);
                Extra = title + "<br/> ";
                //this.TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                this.TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.Local);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Act(string title, bool isPass, OpenQA.Selenium.Remote.RemoteWebDriver Driver)
        {
            try
            {
                this.Title = title;
                this.IsSuccess = isPass;               

                if (!isPass)
                {
                    this.TestActExtra(Driver);
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine(this.Title);
                Console.ResetColor();
                Extra = title + "<br/> ";
                this.TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.Local);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Gets or sets TimeStamp
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets Extra
        /// </summary>
        public String Extra { get; set; }

        /// <summary>
        /// Gets or sets isSuccess
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }
            set
            {
                isSuccess = value;
            }
        }

        public string Image { get; set; }
        public string ExternalFilePath { get; set; }

        public void TestActExtra(OpenQA.Selenium.Remote.RemoteWebDriver driver)
        {
            try
            {
                string relativePath = string.Concat("Screenshots", string.Format(@"\{0}_Error.png", Guid.NewGuid().ToString().Substring(0, 10)));
                System.Threading.Thread.Sleep(1000);
                ITakesScreenshot iTakeScreenshot = driver;
                string screenShot = iTakeScreenshot.GetScreenshot().AsBase64EncodedString;
                //this.Image = Path.Combine(Engine.pathOfReport, relativePath);
                this.Image = relativePath;
                File.WriteAllBytes(Path.Combine(Engine.pathOfReport, relativePath), Convert.FromBase64String(screenShot));
                this.Image = ConfigurationManager.AppSettings.Get("SharePointUrl").ToString() + Engine.pathOfReport.Split('\\')[2] + "/" + relativePath.Replace(@"\", @"/");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}