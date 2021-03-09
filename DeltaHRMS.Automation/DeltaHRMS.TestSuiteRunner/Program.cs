using DeltaHRMS.Accelerators.BaseClasses;
using DeltaHRMS.Accelerators.Reporting;
using DeltaHRMS.Accelerators.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static DeltaHRMS.TestSuiteRunner.Constants;

namespace Maxim.eTrak.TestSuiteRunner
{
    class Program
    {
        #region MemberVariables
        static Dictionary<String, String> qualifiedNames = new Dictionary<string, string>();
        static List<Object[]> testCaseToExecute = new List<object[]>();
        public static List<Object[]> SingleThreadedTests = new List<object[]>();
        DataTable dtTestCaseDetails = new DataTable();
        #endregion

        static void Test()
        {
        }
        static void Main(string[] args)
        {
            int exitCode = 0;
            try
            {
                if (args.Length <= 0)
                {
                    exitCode = new Program().ExecuteTest();
                }
                else if ((args.Length == 1) && !string.IsNullOrEmpty(args[0]))
                {
                    exitCode = new Program().ExecuteTest(application: args[0]);
                }
                else if ((args.Length == 2) && !string.IsNullOrEmpty(args[1]))
                {
                    exitCode = new Program().ExecuteTest(application: args[0], subModule: args[1]);
                }
                else if ((args.Length == 3) && !string.IsNullOrEmpty(args[2]))
                {
                    exitCode = new Program().ExecuteTest(application: args[0], subModule: args[1], testCaseToBeExecuted: args[2]);
                }
                else if ((args.Length == 4) && !string.IsNullOrEmpty(args[3]))
                {
                    exitCode = new Program().ExecuteTest(application: args[0], subModule: args[1], testCaseToBeExecuted: args[2], executionCategory: args[3]);
                }
                else if ((args.Length == 5) && !string.IsNullOrEmpty(args[4]))
                {
                    exitCode = new Program().ExecuteTest(application: args[0], subModule: args[1], testCaseToBeExecuted: args[2], executionCategory: args[3], envi: args[4]);
                }
                else if ((args.Length == 6) && !string.IsNullOrEmpty(args[5]))
                {
                    exitCode = new Program().ExecuteTest(application: args[0], subModule: args[1], testCaseToBeExecuted: args[2], executionCategory: args[3], envi: args[4], maximParallel: args[5]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Execution terminated.. " + ex.Message);
                Console.WriteLine("Execution completed at " + DateTime.Now);
                throw new Exception(ex.Message);
            }
            finally
            {
                Environment.Exit(exitCode);
            }
        }

        /// <summary>
        /// Execute Test cases.
        /// </summary>
        /// <param name="ExecutionCategory"></param>
        /// <param name="Nunitresultsfilename"></param>
        /// <param name="concept"></param>
        /// <returns></returns>
        public int ExecuteTest(string application = "DeltaHRMS",
                               string subModule = "All",
                               string testCaseToBeExecuted = "All",
                               string executionCategory = "Regression",
                               string envi = "QA",
                               string maximParallel = "3")
        {
            int exitCodeValue = -1;
            int noOfTestCases = 0;
            try
            {
                Console.WriteLine("Execution started at " + DateTime.Now);
                dtTestCaseDetails.Columns.Clear();
                dtTestCaseDetails.Columns.Add("ToExecute", typeof(bool));
                dtTestCaseDetails.Columns.Add("Sl.No", typeof(int));
                dtTestCaseDetails.Columns.Add("ModuleName", typeof(string));
                dtTestCaseDetails.Columns.Add("SubModuleName", typeof(string));
                dtTestCaseDetails.Columns.Add("UserStory", typeof(string));
                dtTestCaseDetails.Columns.Add("TC ID", typeof(string));
                dtTestCaseDetails.Columns.Add("TestCaseID", typeof(string));
                dtTestCaseDetails.Columns.Add("TestCaseDescription", typeof(string));
                dtTestCaseDetails.Columns.Add("ExecutionCategories", typeof(string));
                string assemblyFileName = ConfigurationManager.AppSettings.Get("TestsDLLName").ToString();

                string strDllPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string strDLLPath = string.Concat(strDllPath, "\\", assemblyFileName);
                int i_Counter = 0;
                Assembly assembly = Assembly.LoadFrom(strDLLPath);

                List<string> listOfTestCases = null;
                if (testCaseToBeExecuted.Split(',').Count() > 0)
                {
                    listOfTestCases = testCaseToBeExecuted.Split(',').ToList();
                }

                List<string> listOfModules = null;
                if (subModule.Split(',').Count() > 0)
                {
                    listOfModules = subModule.Split(',').ToList();
                }

                Locator.ModuleName = application;
                Array.ForEach(assembly.GetTypes(), type =>
                {
                    if (type.GetCustomAttributes(typeof(ScriptAttribute), true).Length > 0)
                    {
                        var objTestCase = (ScriptAttribute)type.GetCustomAttribute(typeof(ScriptAttribute));
                        string strModuleName = objTestCase.ModuleName.Trim();
                        string strSubModuleName = objTestCase.SubModuleName.Trim();
                        string strUSID = string.Empty;
                        if (!string.IsNullOrEmpty(objTestCase.UserStoryId))
                            strUSID = objTestCase.UserStoryId.Trim();
                        string strTCID = string.Empty;
                        if (!string.IsNullOrEmpty(objTestCase.TestCaseId))
                            strTCID = objTestCase.TestCaseId.Trim();
                        string strTestCaseName = type.Name;
                        qualifiedNames.Add(strTestCaseName, type.AssemblyQualifiedName);
                        string strTestCaseDescription = objTestCase.TestCaseDescription.Trim();
                        string strExecutionCategories = objTestCase.ExecutionCategories.Trim();

                        if (strExecutionCategories.ToLower().Trim().Contains(executionCategory.ToLower().Trim()))
                        {
                            if (strModuleName.ToLower().Contains(application.ToLower()) && (listOfModules.Count > 0 && listOfModules.Contains(strSubModuleName)) &&
                               (listOfTestCases.Count > 0 && listOfTestCases.Contains(strTestCaseName)))
                            {
                                i_Counter++;
                                dtTestCaseDetails.Rows.Add(true, i_Counter, strModuleName, strSubModuleName, strUSID, strTCID, strTestCaseName, strTestCaseDescription, strExecutionCategories);
                            }
                            else if (strModuleName.ToLower().Contains(application.ToLower()) && (listOfModules.Count > 0 && listOfModules.Contains(strSubModuleName)) &&
                                testCaseToBeExecuted.ToLower().Contains("All".ToLower()))
                            {
                                i_Counter++;
                                dtTestCaseDetails.Rows.Add(true, i_Counter, strModuleName, strSubModuleName, strUSID, strTCID, strTestCaseName, strTestCaseDescription, strExecutionCategories);
                            }
                            else if (strModuleName.ToLower().Contains(application.ToLower()) && subModule.ToLower().Contains("All".ToLower()) &&
                                testCaseToBeExecuted.ToLower().Contains("All".ToLower()))
                            {
                                i_Counter++;
                                dtTestCaseDetails.Rows.Add(true, i_Counter, strModuleName, strSubModuleName, strUSID, strTCID, strTestCaseName, strTestCaseDescription, strExecutionCategories);
                            }
                        }
                    }
                });
                string appConfigFilePath = string.Concat(Assembly.GetExecutingAssembly().Location, ".config");

                XmlDocumentHelper.ConfigModificatorSettings appConfigWriterSettings = new XmlDocumentHelper.ConfigModificatorSettings("//appSettings", "//add[@key='{0}']", appConfigFilePath);
                XmlDocumentHelper.ChangeValueByKey("Application", application, "value", appConfigWriterSettings);
                XmlDocumentHelper.ChangeValueByKey("URL", GetUrl(application, envi), "value", appConfigWriterSettings);
                XmlDocumentHelper.ChangeValueByKey("MaxDegreeOfParallelism", maximParallel, "value", appConfigWriterSettings);
                XmlDocumentHelper.RefreshAppSettings();

                Engine reportEngine = new Engine(ConfigurationManager.AppSettings.Get("ReportsPath").ToString(), ConfigurationManager.AppSettings.Get("ENVQA").ToString(), ConfigurationManager.AppSettings.Get("DefaultBrowser").ToString(), ConfigurationManager.AppSettings.Get("Application").ToString());

                foreach (DataRow row in dtTestCaseDetails.Rows)
                {
                    if (row[0].Equals(true))
                    {
                        Console.WriteLine(row[0] + "-" + row[1] + "-" + row[2] + "-" + row[3] + "-" + row[4] + "-" + row[5] + "-" + row[6] + "-" + row[7]);
                        string testCaseModuleName = string.Empty;

                        if (application.Trim().Equals(ModuleName.DELTAHRMS.GetDescription()))
                        {
                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TestDataFiles").ToString())
                                && ConfigurationManager.AppSettings.Get("TestDataFiles").ToString().Equals(envi))
                            {
                                //if (row.Cells[2].Value.ToString().Trim().ToLower().Contains("etrakrc"))
                                { testCaseModuleName = ModuleName.DELTAHRMS.GetDescription(); }
                            }
                            else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TestDataFiles").ToString())
                                && ConfigurationManager.AppSettings.Get("TestDataFiles").ToString().Equals(envi))
                            {
                                { testCaseModuleName = ModuleName.DELTAHRMS.GetDescription(); }
                            }
                        }
                        else
                        { testCaseModuleName = application.Trim(); }

                        string testCaseSubModuleName = row[3].ToString().Trim();
                        string strBrowserName = ConfigurationManager.AppSettings.Get("DefaultBrowser").ToString();
                        string testCaseRequirementFeature = row[7].ToString().Trim();
                        string usid = row[4].ToString().Trim();
                        string tcid = row[5].ToString().Trim();
                        noOfTestCases = noOfTestCases + tcid.Split(',').Length;
                        string testCaseName = row[6].ToString().Trim();
                        // string executionCategory = row[8].ToString().Trim();
                        TestCase testCaseReporter = new TestCase(testCaseModuleName, testCaseSubModuleName, usid, tcid, testCaseName.Split('_')[0], testCaseName, testCaseRequirementFeature, executionCategory);
                        testCaseReporter.Summary = reportEngine.Reporter;
                        reportEngine.Reporter.TestCases.Add(testCaseReporter);
                        string strBrowserId = string.Empty;

                        // browsers
                        foreach (String browserId in strBrowserName.ToString().Split(new char[] { ';' }))
                        {
                            strBrowserId = browserId != String.Empty ? browserId : ConfigurationManager.AppSettings.Get("DefaultBrowser").ToString();
                            Browser browserReporter = new Browser(strBrowserId);
                            if (ConfigurationManager.AppSettings.Get(strBrowserId).Contains("Android"))
                                browserReporter.ExeEnvironment = "Mobile-Android";
                            else if (ConfigurationManager.AppSettings.Get(strBrowserId).Contains("iOS"))
                                browserReporter.ExeEnvironment = "Mobile-iOS";
                            else
                            {
                                browserReporter.ExeEnvironment = "Web";
                            }
                            browserReporter.TestCase = testCaseReporter;
                            testCaseReporter.Browsers.Add(browserReporter);

                            // Get the Test data details
                            XmlDocument xmlTestDataDoc = new XmlDocument();
                            xmlTestDataDoc.Load(strDllPath + "/TestData/" + testCaseModuleName + ".xml");

                            //Load the defectID xml file
                            XmlDocument defectIdDoc = new XmlDocument();
                            defectIdDoc.Load(strDllPath + "/TestData/" + "DefectID" + ".xml");
                            string defectID;
                            XmlNodeList testdataNodeList = null;
                            XmlNode defectIDNode = null;

                            int totalNodesFromSelectedFile = xmlTestDataDoc.DocumentElement.ChildNodes.Count;
                            if (xmlTestDataDoc.DocumentElement.SelectNodes("/TestData/" + testCaseName).Count >= 1)
                                testdataNodeList = xmlTestDataDoc.DocumentElement.SelectNodes("/TestData/" + testCaseName);
                            else
                                testdataNodeList = xmlTestDataDoc.DocumentElement.SelectNodes("/TestData/GenericData");

                            //Get the defect data node 
                            if (defectIdDoc.DocumentElement.SelectNodes("/DefectData/" + testCaseName).Count >= 1)
                            {
                                defectIDNode = defectIdDoc.DocumentElement.SelectSingleNode("/DefectData/" + testCaseName);
                                defectID = defectIDNode.SelectSingleNode("DefectID").InnerText;
                            }
                            else
                                defectID = "";

                            //Iterate for each data
                            foreach (XmlNode testDataNode in testdataNodeList)
                            {
                                Dictionary<String, String> browserConfig = Utility.GetBrowserConfig(strBrowserId);
                                string iterationId = testDataNode.SelectSingleNode("TDID").InnerText;

                                Iteration iterationReporter = new Iteration(iterationId, defectID);
                                iterationReporter.Browser = browserReporter;
                                browserReporter.Iterations.Add(iterationReporter);

                                testCaseToExecute.Add(new Object[] { testCaseReporter, browserConfig, testDataNode, iterationReporter, reportEngine });
                            }
                        }
                    }
                }
                Console.WriteLine("Total test cases - {0}", i_Counter);
                if (i_Counter.Equals(0))
                {
                    Console.WriteLine("***** No test cases found *****");
                    throw new Exception("No test cases found");
                }

                Processor(Int32.Parse(ConfigurationManager.AppSettings.Get("MaxDegreeOfParallelism")));
                reportEngine.NoOfTestCasesExecuted = noOfTestCases.ToString();
                reportEngine.TotalNoOfTestCases = i_Counter.ToString();

                reportEngine.Summarize();

                String fileName = Path.Combine(reportEngine.ReportPath, "Summary.html");
                Process.Start(fileName);
                reportEngine.CopyFilesToSharePoint();

                decimal dcl_TotalTestCasesExecuted = reportEngine.Reporter.TestCases.Count;
                decimal dcl_TotalTestCasesPassed = 0;
                decimal dcl_TotalTestCasesFailed = 0;
                string str_CSVFilePath = Path.Combine(reportEngine.ReportPath, "Summary.csv");

                using (var stream = File.CreateText(str_CSVFilePath))
                {
                    //stream.WriteLine(string.Format("{0},{1},{2},{3},{4}", "TestCase Name", "RequirementFeature", "Summary", "Title", "Passed?"));
                    stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", "Module", "Sub-Module", "Category", "UserStory", "TC ID", "TestCase Name", "Browser", "Issue", "Result"));
                    foreach (TestCase testCase in reportEngine.Reporter.TestCases)
                    {
                        if (testCase.IsSuccess)
                            dcl_TotalTestCasesPassed++;
                        else
                            dcl_TotalTestCasesFailed++;
                        //stream.WriteLine(string.Format("{0},{1},{2},{3},{4}", testCase.Name, testCase.RequirementFeature, testCase.Summary, testCase.Title, testCase.IsSuccess));
                        stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", testCase.ModuleName, testCase.SubModuleName, "\"" + testCase.ExecutionCategory + "\"", testCase.UserStory, "\"" + testCase.TCID + "\"", testCase.Title, String.Format("{0}-{1}", testCase.Browser.ExeEnvironment, testCase.Browser.BrowserName.ToUpper()), testCase.BugInfo, testCase.IsSuccess));
                    }
                    stream.Flush();
                }
                decimal dcl_PassPercentage = Math.Round((dcl_TotalTestCasesPassed / dcl_TotalTestCasesExecuted) * 100);

                if (dcl_PassPercentage <= 75)
                {
                    exitCodeValue = 3; Console.WriteLine("Pass percentage is less than or equal to 75");
                }
                else if (dcl_PassPercentage > 75 && dcl_PassPercentage <= 90)
                {
                    exitCodeValue = 2; Console.WriteLine("Pass percentage is greater 75 or less than or equal to 90");
                }
                else if (dcl_PassPercentage > 90 && dcl_PassPercentage < 100)
                {
                    exitCodeValue = 1; Console.WriteLine("Pass percentage is greater 90 or less than or equal to 100");
                }
                else if (dcl_PassPercentage == 100)
                {
                    exitCodeValue = 0; Console.WriteLine("Pass percentage is equal to 100");
                }

                Console.WriteLine("TotalTCs Executed - {0}", dcl_TotalTestCasesExecuted.ToString());
                Console.WriteLine("TotalTCs Passed - {0}", dcl_TotalTestCasesPassed.ToString());
                Console.WriteLine("TotalTCs Failed - {0}", dcl_TotalTestCasesFailed.ToString());
                Console.WriteLine("TotalTCs Pass Percentage - {0}", dcl_PassPercentage.ToString());

                Console.WriteLine("Final Summary Report - {0}", Path.Combine(reportEngine.ReportPath, "Summary.html"));
                if (ConfigurationManager.AppSettings.Get("SendEmail").ToString().Equals("Yes"))
                {
                    fn_SendEmail2(dcl_TotalTestCasesExecuted, dcl_TotalTestCasesPassed, dcl_TotalTestCasesFailed, dcl_PassPercentage, Path.Combine(reportEngine.ReportPath, "Summary.html"));
                    //sendEMailThroughOUTLOOK(dcl_TotalTestCasesExecuted, dcl_TotalTestCasesPassed, dcl_TotalTestCasesFailed, dcl_PassPercentage, Path.Combine(reportEngine.ReportPath, "Summary.html"));
                    TimeSpan start = new TimeSpan(20, 0, 0); //10 o'clock
                    TimeSpan end = new TimeSpan(10, 0, 0); //12 o'clock
                    TimeSpan now = DateTime.Now.TimeOfDay;
                    if ((now > start) && (now < end))
                    {
                        //sendEMailThroughOUTLOOK(dcl_TotalTestCasesExecuted, dcl_TotalTestCasesPassed, dcl_TotalTestCasesFailed, dcl_PassPercentage, Path.Combine(reportEngine.ReportPath, "Summary.html"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return exitCodeValue;
        }

        /// <summary>
        /// Get Url
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="enviName"></param>
        /// <returns></returns>
        private string GetUrl(string applicationName, string enviName)
        {
            string url = string.Empty;
            switch (applicationName.ToLower())
            {
                case "deltahrms":
                    url = GetEnvironment(applicationName + enviName);
                    break;
            }
            return url;
        }

        /// <summary>
        /// Get Environment
        /// </summary>
        /// <param name="appenviName"></param>
        /// <returns></returns>
        private string GetEnvironment(string appenviName)
        {
            string url = string.Empty;

            switch (appenviName.ToLower())
            {
                case "deltahrmsqa":
                    url = ConfigurationManager.AppSettings["HrmsQaUrl"];
                    break;
                case "deltahrmsdev":
                    url = ConfigurationManager.AppSettings["HrmsDevUrl"]; 
                    break;
            }

            return url;
        }

        /// <summary>
        /// Processor parallel execution.
        /// </summary>
        /// <param name="maxDegree"></param>
        static void Processor(int maxDegree)
        {
            try
            {
                if (ConfigurationManager.AppSettings.Get("ExecutionMode").ToLower().Equals("s"))
                {
                    ///Use this loop for sequential running of the test cases
                    foreach (object[] work in testCaseToExecute)
                    {
                        ProcessEachWork(work);
                    }
                }
                else if (ConfigurationManager.AppSettings.Get("ExecutionMode").ToLower().Equals("p"))
                {
                    /*Use this loop for parellel running of the test cases*/
                    Parallel.ForEach(testCaseToExecute,
                                     new ParallelOptions { MaxDegreeOfParallelism = maxDegree },
                                     work =>
                                     {
                                         ProcessEachWork(work);
                                     });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Processor sequential execution.
        /// </summary>
        static void Processor()
        {
            try
            {
                ///Use this loop for sequential running of the test cases
                foreach (object[] work in SingleThreadedTests)
                {
                    ProcessEachWork(work);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Copy directory from source path to destination path
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void CopyDirectory(string source, string destination)
        {
            try
            {
                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);

                DirectoryInfo dirInfo = new DirectoryInfo(source);
                FileInfo[] files = dirInfo.GetFiles();

                foreach (FileInfo file in files)
                {
                    string destFileName = Path.Combine(destination, file.Name);
                    file.CopyTo(destFileName);
                }

                DirectoryInfo[] directories = dirInfo.GetDirectories();

                foreach (DirectoryInfo directory in directories)
                {
                    string destDirName = Path.Combine(destination, directory.Name);
                    CopyDirectory(directory.FullName, destDirName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void ProcessEachWork(Object[] data)
        {
            try
            {
                TestCase objTestCase = (TestCase)data[0];
                string strTCName = objTestCase.Name.ToString().Trim();
                Type typeTestCase = Type.GetType(qualifiedNames[strTCName]);
                BaseTest baseCase = Activator.CreateInstance(typeTestCase) as BaseTest;
                try
                {

                    typeTestCase.GetMethod("Execute").Invoke(baseCase, data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(strTCName + " execution has caught exception " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string fn_SendEmail2(decimal _total, decimal _passed, decimal _failed, decimal _passpercent, string str_CSVFilePath)
        {
            string returnString = "";
            try
            {
                var encodedPassInByts = Convert.FromBase64String(ConfigurationManager.AppSettings.Get("EmailEncryptedPassword").ToString());
                string decodePass = Encoding.UTF8.GetString(encodedPassInByts);

                //string toEmail = Config.ToAddress;
                string addresses = ConfigurationManager.AppSettings.Get("SendEmailTo").ToString();
                string CCaddresses = ConfigurationManager.AppSettings.Get("SendEmailCC").ToString();

                MailMessage email = new MailMessage();
                //SmtpClient smtp = new SmtpClient("smtp.office365.com", 25);
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("SendMailFrom").ToString(), decodePass);

                String strReturn = String.Empty;
                strReturn = strReturn + "<table border = '1'; class='table table-striped table-bordered table-condensed default'> <tr> <th colspan='6' style='background-color: #1B3F73; color: white; border: 1px solid black;'> <center> Test Result Status </center> </th> </tr>";
                strReturn = strReturn + "<tr> <th style='background-color: #1B3F73; color: white'> Test Results </th> <th style='background-color: #1B3F73; color: #1BDE38'> <center> Passed </center> </th> <th style='background-color: #1B3F73; color: red'> <center> Failed </center> </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Tests Executed </center> </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Total Tests </center> </th> </th>  </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Pass% </center> </th> </tr>";
                strReturn = strReturn + "<tr> <td style='font-weight: bold'> Delta HRMS </td>";
                strReturn = strReturn + "<td style='font-weight: bold'> <center> " + _passed + " </center> </td> <td style='font-weight: bold'> <center> " + _failed + " </center> </td>";
                strReturn = strReturn + "<td style='font-weight: bold'> <center> " + (_passed + _failed) + " </center> </td> <td style='font-weight: bold'> <center> " + (_passed + _failed) + " </center> </td> <td style='font-weight: bold'> <center> " + _passpercent + " %" + " </center> </td> </tr>";
                strReturn = strReturn + "<tr style='font-weight: bold'> <td> Total </td> <td> <center> " + _passed + " </center> </td> <td> <center> " + _failed + " </center> </td> <td> <center> " + (_passed + _failed) + " </center> </td> <td> <center> " + (_passed + _failed) + " </center> </td> <td> <center> " + _passpercent + " %" + " </center> </td> </tr> </table>";

                string reportUrl = ConfigurationManager.AppSettings.Get("SharePointUrl").ToString() + str_CSVFilePath.Split('\\')[2];

                string concat = "<a href = '" + reportUrl + "'>Detailed Execution Reports</a>";

                StringBuilder str_Body = new StringBuilder();
                str_Body.Append("Hello Team,");
                str_Body.Append("<br />");
                str_Body.Append("<br />");
                str_Body.Append("Please find the Delta HRMS Automation Test Script Execution status on: " + DateTime.Now.ToString("dd-MM-yyyy"));
                str_Body.Append("<br />");
                str_Body.Append("<br />");
                str_Body.Append(strReturn);
                str_Body.Append("<br />");
                str_Body.Append("Please find the below SharePoint link to access snapshots and detailed reports:");
                str_Body.Append("<br />");
                str_Body.Append(concat);
                str_Body.Append("<br />");
                str_Body.Append("<br />");
                str_Body.Append("Please find the attached Summary.html Report");
                str_Body.Append("<br />");
                str_Body.Append("<br />");
                str_Body.Append("Thanks & Regards,");
                str_Body.Append("<br />");
                str_Body.Append("Automation Team");
                str_Body.Append("<br />");
                str_Body.Append("<br />");

                // draft the email
                MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings.Get("SendMailFrom").ToString());
                email.From = fromAddress;
                foreach (var address in addresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    email.To.Add(address);
                }

                foreach (var ccaddress in CCaddresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    email.CC.Add(ccaddress);
                }

                email.Subject = "Delta HRMS Automation Execution Results (Pass%=" + _passpercent + ") On: " + DateTime.Now.ToString("dd-MM-yyyy");
                email.Body = str_Body.ToString();
                email.IsBodyHtml = true;
                email.Attachments.Add(new Attachment(str_CSVFilePath));

                smtp.Send(email);

                returnString = "Success! Please check your e-mail.";
            }
            catch (Exception ex)
            {
                returnString = "Error: " + ex.ToString();
            }
            return returnString;
        }

        private List<string> ReadFromTextFile(string strDllPath)
        {
            List<string> testCasesToLoad = new List<string>();

            testCasesToLoad = File.ReadLines(strDllPath + "/TestData/TestcasesToRun.txt").ToList<string>();
            return testCasesToLoad;
        }
    }
}