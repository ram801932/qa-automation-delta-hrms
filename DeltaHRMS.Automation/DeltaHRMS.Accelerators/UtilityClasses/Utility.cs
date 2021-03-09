using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using WindowsInput;
using WindowsInput.Native;

namespace DeltaHRMS.Accelerators.Utilities
{
    /// <summary>
    ///This class has function to return a particular RemoteWebDriver instance based on the settings in configuration file
    ///This class functions will be called by classes from TestSuiteRunner and SuiteRunner projects to get the RemoteWebDriver befor the test is started
    /// </summary>
    public class Utility
    {

        public static void PressKeyBoardTabKey()
        {
            InputSimulator inputSimulator = new InputSimulator();
                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.TAB);
        }
        public static void PressKeyBoardEnterKey()
        {
            InputSimulator inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
        }
        private static Dictionary<string, string> environmentSettings = new Dictionary<string, string>();
        public static Dictionary<string, string> BrowserConfig = null;
        /// <summary>
        /// Gets settings for current environment
        /// </summary>
        public static Dictionary<string, string> EnvironmentSettings
        {
            get
            {
                string environment = ConfigurationManager.AppSettings.Get("Environment");
                if (environmentSettings.Count > 0) return environmentSettings;
                String[] KeyValue = null;

                lock (environmentSettings)
                {
                    foreach (String setting in ConfigurationManager.AppSettings.Get(environment).Split(new Char[] { ';' }))
                    {
                        KeyValue = setting.Split(new Char[] { '=' }, 2);
                        if (KeyValue.Length > 1)
                        {
                            environmentSettings.Add(KeyValue[0].Trim(), KeyValue[1].Trim());
                        }
                    }
                }
                return environmentSettings;
            }
        }

        /// <summary>
        /// Prepares RemoteWebDriver basing on configuration supplied
        /// </summary>
        /// <param name="browserConfig"></param>
        /// <returns></returns>
        public static RemoteWebDriver GetDriver(Dictionary<string, string> browserConfig)
        {
            RemoteWebDriver driver = null;
            try
            {
                if (browserConfig["target"] == "local")
                {
                    switch (browserConfig["browser"])
                    {
                        case "Firefox":
                            FirefoxProfile p = new FirefoxProfile();
                            FirefoxBinary path = new FirefoxBinary(@"C:\\Program Files\\Mozilla Firefox\\firefox.exe");
                            break;

                        case "IE":
                            var options = new InternetExplorerOptions();
                            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                            options.EnsureCleanSession = true;
                            options.EnableNativeEvents = true;
                            driver = new InternetExplorerDriver(Directory.GetParent(Assembly.GetEntryAssembly().Location).ToString(), options);
                            break;

                        case "Chrome":
                            ChromeOptions chrOpts = new ChromeOptions();
                            chrOpts.AddArguments("test-type");
                            //chrOpts.AddArguments("--headless");
                            chrOpts.AddArguments("--disable-extensions");
                            chrOpts.AddArgument("no-sandbox");
                            chrOpts.AddUserProfilePreference("download.default_directory", "C:\\automationdownload");
                            driver = new ChromeDriver(Directory.GetParent(Assembly.GetEntryAssembly().Location).ToString(), chrOpts, TimeSpan.FromMinutes(3));
                            break;
                    }
                    driver.Manage().Window.Maximize();
                    driver.Manage().Cookies.DeleteAllCookies();
                    return driver;
                }
                return null;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets Browser related configuration data from App.Config
        /// </summary>
        /// <param name="browserId">Identity of Browser</param>
        /// <returns><see cref="Dictionary<String, String>"/></returns>
        public static Dictionary<String, String> GetBrowserConfig(String browserId)
        {
            browserId = ConfigurationManager.AppSettings.Get(browserId).ToString();
            Dictionary<String, String> config = new Dictionary<string, string>();
            String[] KeyValue = null;

            foreach (String attribute in browserId.Split(new Char[] { ';' }))
            {
                if (attribute != "")
                {
                    KeyValue = attribute.Split(new Char[] { ':' });
                    config.Add(KeyValue[0].Trim(), KeyValue[1].Trim());
                }
            }
            BrowserConfig = config;
            return config;
        }

        /// <summary>
        /// Converts currency string value to Integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Integer</returns>
        public static int ConverCurrentToInteger(string value)
        {
            return int.Parse(value, System.Globalization.NumberStyles.AllowCurrencySymbol | System.Globalization.NumberStyles.Number);
        }

        /// <summary>
        /// ExecuteExternalProgram
        /// </summary>
        /// <param name="externalPath"> path of external program</param>
        /// <param name="arguments">program arguments</param>
        public static int ExecuteExternalProgram(string externalPath, string arguments)
        {
            using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            {
                pProcess.StartInfo.FileName = externalPath;
                pProcess.StartInfo.Arguments = arguments;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pProcess.StartInfo.CreateNoWindow = true;
                pProcess.Start();
                string output = pProcess.StandardOutput.ReadToEnd(); //The output result
                pProcess.WaitForExit();
                return pProcess.ExitCode;
            }
        }
    }
}