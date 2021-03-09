using DeltaHRMS.TestSuiteExecutor;
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
using System.Windows.Forms;
using System.Xml;

namespace DeltaHRMS.TestSuiteExecutor
{
    public partial class form_TestSuiteRunner : Form
    {
        
        List<string> lst_AvailableModules = new List<string>();
        List<string> lst_AvailableSubModules = new List<string>();
        List<string> lst_AvailableUserStories = new List<string>();
        List<string> lst_AvailableCategories = new List<string>();
        DataView dvTestCaseDetails = null;
        DataTable dtTestCaseDetails = new DataTable();
        ListBox.SelectedObjectCollection colSubModuleFilterCriteria;
        ListBox.SelectedObjectCollection colCategoryFilterCriteria;
        ListBox.SelectedObjectCollection colUserStoryFilterCriteria;
        public static List<Object[]> testCaseToExecute = new List<object[]>();
        public static Dictionary<String, String> qualifiedNames = new Dictionary<string, string>();
        public static List<Object[]> SingleThreadedTests = null;

        public form_TestSuiteRunner()
        {
            InitializeComponent();
        }

        private void form_TestSuiteRunner_Load(object sender, EventArgs e)
        {
            string assemblyFileName = ConfigurationManager.AppSettings.Get("TestsDLLName").ToString();
            string strDllPath = Directory.GetCurrentDirectory();
            string strDLLPath = string.Concat(strDllPath, "\\", assemblyFileName);
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
            int i_Counter = 0;
            Assembly assembly = Assembly.LoadFrom(strDLLPath);
            Array.ForEach(assembly.GetTypes(), type =>
            {
                if (type.GetCustomAttributes(typeof(ScriptAttribute), true).Length > 0)
                {
                    i_Counter++;

                    var objTestCase = (ScriptAttribute)type.GetCustomAttribute(typeof(ScriptAttribute));
                    ExecutionAttributes executionAttributes = new ExecutionAttributes();

                    executionAttributes.ModuleName = objTestCase.ModuleName.Trim();
                    executionAttributes.SubModuleName = objTestCase.SubModuleName.Trim();

                    if (!string.IsNullOrEmpty(objTestCase.UserStoryId))
                        executionAttributes.UserStoryId = objTestCase.UserStoryId.Trim();

                    if (!string.IsNullOrEmpty(objTestCase.TestCaseId))
                        executionAttributes.TCID = objTestCase.TestCaseId.Trim();
                    executionAttributes.TestCaseName = type.Name;
                    qualifiedNames.Add(executionAttributes.TestCaseName, type.AssemblyQualifiedName);
                    executionAttributes.TestCaseDescription = objTestCase.TestCaseDescription.Trim();
                    executionAttributes.ExecutionCategories = objTestCase.ExecutionCategories.Trim();

                    if (ConfigurationManager.AppSettings.Get("LoadTestCasesFromExternalFile").ToString().Equals("Yes"))
                    {
                        List<string> TestRuns = ReadFromTextFile();
                        if (TestRuns.Contains(executionAttributes.TestCaseName))
                        {
                            PopulateTestCases(executionAttributes, i_Counter);
                        }
                    }
                    else
                    {
                        PopulateTestCases(executionAttributes, i_Counter);
                    }
                }
            });

            lbl_TotalTCsResult.Text = i_Counter.ToString();
            lstbox_Module.Items.AddRange(lst_AvailableModules.ToArray());
            lstbox_SubModule.Items.AddRange(lst_AvailableSubModules.ToArray());
            lstbox_Criteria.Items.AddRange(lst_AvailableCategories.ToArray());
            lstbox_UserStory.Items.AddRange(lst_AvailableUserStories.ToArray());
            dvTestCaseDetails = new DataView(dtTestCaseDetails);
            dvTestCaseDetails.Sort = "Sl.No";
            dataGridView1.DataSource = dvTestCaseDetails;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        private void PopulateTestCases(ExecutionAttributes executeAttributes, int i_Counter)
        {
            dtTestCaseDetails.Rows.Add(false, i_Counter, executeAttributes.ModuleName, executeAttributes.SubModuleName, executeAttributes.UserStoryId, executeAttributes.TCID, executeAttributes.TestCaseName, executeAttributes.TestCaseDescription, executeAttributes.ExecutionCategories);

            if ((!lst_AvailableModules.Contains(executeAttributes.ModuleName)) && (executeAttributes.ModuleName.Length > 0))
            {
                lst_AvailableModules.Add(executeAttributes.ModuleName);
            }
            lst_AvailableModules.Sort();
            if ((!lst_AvailableSubModules.Contains(executeAttributes.SubModuleName)) && (executeAttributes.SubModuleName.Length > 0))
            {
                lst_AvailableSubModules.Add(executeAttributes.SubModuleName);
            }
            lst_AvailableSubModules.Sort();
            Array.ForEach(executeAttributes.ExecutionCategories.Split(','), x =>
            {
                if ((!lst_AvailableCategories.Contains(x)) && (x.Length > 0))
                {
                    lst_AvailableCategories.Add(x);
                }
            });
            lst_AvailableCategories.Sort();
            if ((!lst_AvailableUserStories.Contains(executeAttributes.UserStoryId)) && (executeAttributes.UserStoryId.Length > 0))
            {
                lst_AvailableUserStories.Add(executeAttributes.UserStoryId);
            }
            lst_AvailableUserStories.Sort();
        }

        private void btn_FilterData_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            string strSubModuleFilterCriteria = string.Empty;
            string strModuleFilterCriteria = string.Empty;
            string strCategoryFilterCriteria = string.Empty;
            string strUserStoryFilterCriteria = string.Empty;

            if (lstbox_Module.SelectedIndex != -1)
                strModuleFilterCriteria = lstbox_Module.SelectedItem.ToString().Trim();

            if (lstbox_SubModule.SelectedIndex != -1)
            {
                colSubModuleFilterCriteria = lstbox_SubModule.SelectedItems;
                if (colSubModuleFilterCriteria.Count > 1)
                {
                    for (int i = 0; i < colSubModuleFilterCriteria.Count - 1; i++)
                    {
                        strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'" + " OR SubModuleName LIKE ";
                    }
                    strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[colSubModuleFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
                }
                else
                    strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
            }
            else
                strSubModuleFilterCriteria = "'**'";

            if (lstbox_Criteria.SelectedIndex != -1)
            {
                colCategoryFilterCriteria = lstbox_Criteria.SelectedItems;
                if (colCategoryFilterCriteria.Count > 1)
                {
                    for (int i = 0; i < colCategoryFilterCriteria.Count - 1; i++)
                    {
                        strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'" + " OR ExecutionCategories LIKE ";
                    }
                    strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[colCategoryFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
                }
                else
                    strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
            }
            else
                strCategoryFilterCriteria = "'**'";

            if (lstbox_UserStory.SelectedIndex != -1)
            {
                colUserStoryFilterCriteria = lstbox_UserStory.SelectedItems;
                if (colUserStoryFilterCriteria.Count > 1)
                {
                    for (int i = 0; i < colUserStoryFilterCriteria.Count - 1; i++)
                    {
                        strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'" + " OR UserStory LIKE ";
                    }
                    strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[colUserStoryFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
                }
                else
                    strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
            }
            else
                strUserStoryFilterCriteria = "'**'";

            string strCondition = "SubModuleName LIKE " + strSubModuleFilterCriteria + "  AND ExecutionCategories LIKE " + strCategoryFilterCriteria + " AND UserStory LIKE " + strUserStoryFilterCriteria;

            DataView dvFilteredTestCaseDetails = new DataView(dtTestCaseDetails);
            dvFilteredTestCaseDetails.Sort = "ModuleName";
            dvFilteredTestCaseDetails.Sort = "SubModuleName";
            dvFilteredTestCaseDetails.Sort = "TestCaseID";
            dvFilteredTestCaseDetails.Sort = "TestCaseDescription";
            dvFilteredTestCaseDetails.Sort = "ExecutionCategories";
            dvFilteredTestCaseDetails.Sort = "UserStory";
            dvFilteredTestCaseDetails.RowFilter = strCondition;
            dataGridView1.DataSource = dvFilteredTestCaseDetails;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            lbl_TotalTCsResult.Text = dvFilteredTestCaseDetails.Count.ToString();
        }

        private void chkbox_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_SelectAll.Checked)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells[0].Value = true;
                }
            }
            else if (!chkbox_SelectAll.Checked)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells[0].Value = false;
                }
            }
        }

        public List<string> BuildCommand()
        {
            List<string> testCommand = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value.Equals(true))
                {
                    testCommand.Add(row.Cells[1].ToString());
                }
            }
            return testCommand;
        }

        private string GetUrl(string applicationName, string enviName)
        {
            string url = string.Empty;
            switch (applicationName.ToLower())
            {
                case "hrms":
                    url = GetEnvironment(applicationName + enviName);
                    break;
            }
            return url;
        }

        private string GetEnvironment(string appenviName)
        {
            string url = string.Empty;
            switch (appenviName.ToLower())
            {
                case "hrmsqa":
                    url = "https://staging.deltahrms.deltaintech.com/";
                    break;
                case "hrmsprod":
                    url = "https://staging.deltahrms.deltaintech.com/";
                    break;
            }
            return url;
        }

        private void btn_ExecuteTests_Click(object sender, EventArgs e)
        {
            testCaseToExecute = new List<object[]>();
            List<string> _TestRuns = BuildCommand();
            int NoOfTestCases = 0;

            if (lstbox_Module.SelectedItem == null)
            {
                MessageBox.Show("Please select the Module in the Module Filter");
                return;
            }

            if (_TestRuns.Count == 0)
            {
                MessageBox.Show("No test cases selected for execution");
                return;
            }

            try
            {
                string appConfigFilePath = string.Concat(Assembly.GetExecutingAssembly().Location, ".config");

                XmlDocumentHelper.ConfigModificatorSettings appConfigWriterSettings = new XmlDocumentHelper.ConfigModificatorSettings("//appSettings", "//add[@key='{0}']", appConfigFilePath);
                XmlDocumentHelper.ChangeValueByKey("Application", lstbox_Module.SelectedItem.ToString().Trim(), "value", appConfigWriterSettings);
                XmlDocumentHelper.ChangeValueByKey("ENVQA", GetUrl(lstbox_Module.SelectedItem.ToString().Trim(), ConfigurationManager.AppSettings.Get("TestDataFiles")), "value", appConfigWriterSettings);

                XmlDocumentHelper.RefreshAppSettings();

                Engine reportEngine = new Engine(ConfigurationManager.AppSettings.Get("ReportsPath").ToString(), ConfigurationManager.AppSettings.Get("ENVQA").ToString(), ConfigurationManager.AppSettings.Get("DefaultBrowser").ToString(), ConfigurationManager.AppSettings.Get("Application").ToString());
                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[0].Value.Equals(true))
                        {
                            string testCaseModuleName = string.Empty;
                            if (row.Cells[2].Value.ToString().Trim().Equals(AppSetter.ModuleName.HRMS.GetDescription()))
                            {
                                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TestDataFiles").ToString())
                                    && ConfigurationManager.AppSettings.Get("TestDataFiles").ToString().Equals(AppSetter.TestDataFile.DEV.ToString()))
                                {
                                    { testCaseModuleName = AppSetter.ModuleName.HRMSDEV.GetDescription(); }
                                }
                                else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TestDataFiles").ToString())
                                    && ConfigurationManager.AppSettings.Get("TestDataFiles").ToString().Equals(AppSetter.TestDataFile.QA.ToString()))
                                {
                                    { testCaseModuleName = AppSetter.ModuleName.HRMSQA.GetDescription(); }
                                }
                            }
                            else
                            { testCaseModuleName = row.Cells[2].Value.ToString().Trim(); }

                            string testCaseSubModuleName = row.Cells[3].Value.ToString().Trim();
                            string strBrowserName = ConfigurationManager.AppSettings.Get("DefaultBrowser").ToString();
                            string testCaseRequirementFeature = row.Cells[7].Value.ToString().Trim();
                            string usid = row.Cells[4].Value.ToString().Trim();
                            string tcid = row.Cells[5].Value.ToString().Trim();
                            NoOfTestCases = NoOfTestCases + tcid.Split(',').Length;
                            string testCaseName = row.Cells[6].Value.ToString().Trim();
                            string executionCategory = row.Cells[8].Value.ToString().Trim();

                            TestCase testCaseReporter = new TestCase(testCaseModuleName, testCaseSubModuleName, usid, tcid, testCaseName.Split('_')[0], testCaseName, testCaseRequirementFeature, executionCategory);
                            testCaseReporter.Summary = reportEngine.Reporter;
                            reportEngine.Reporter.TestCases.Add(testCaseReporter);
                            string strBrowserId = string.Empty;
                            // browsers 
                            foreach (String browserId in strBrowserName.ToString().Split(new char[] { ';' }))
                            {
                                strBrowserId = browserId != String.Empty ? browserId : ConfigurationManager.AppSettings.Get("DefaultBrowser").ToString();
                                Browser browserReporter = new Browser(strBrowserId);
                                browserReporter.ExeEnvironment = "Web";
                                browserReporter.TestCase = testCaseReporter;
                                testCaseReporter.Browsers.Add(browserReporter);

                                // Get the Test data details
                                XmlDocument xmlTestDataDoc = new XmlDocument();
                                xmlTestDataDoc.Load("TestData/" + testCaseModuleName + ".xml");

                                //Load the defectID xml file
                                //XmlDocument defectIdDoc = new XmlDocument();
                                //defectIdDoc.Load("TestData/" + "DefectID" + ".xml");
                                //string defectID;
                                XmlNodeList testdataNodeList = null;
                                //XmlNode defectIDNode = null;

                                int totalNodesFromSelectedFile = xmlTestDataDoc.DocumentElement.ChildNodes.Count;
                                if (xmlTestDataDoc.DocumentElement.SelectNodes("/TestData/" + testCaseName).Count >= 1)
                                    testdataNodeList = xmlTestDataDoc.DocumentElement.SelectNodes("/TestData/" + testCaseName);
                                else
                                    testdataNodeList = xmlTestDataDoc.DocumentElement.SelectNodes("/TestData/GenericData");

                                //Get the defect data node 
                                //if (defectIdDoc.DocumentElement.SelectNodes("/DefectData/" + testCaseName).Count >= 1)
                                //{
                                //    defectIDNode = defectIdDoc.DocumentElement.SelectSingleNode("/DefectData/" + testCaseName);
                                //    defectID = defectIDNode.SelectSingleNode("DefectID").InnerText;
                                //}
                                //else
                                string defectID = "";

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

                    Processor(Int32.Parse(ConfigurationManager.AppSettings.Get("MaxDegreeOfParallelism")));
                    reportEngine.NoOfTestCasesExecuted = NoOfTestCases.ToString();
                    reportEngine.TotalNoOfTestCases = dvTestCaseDetails.Count.ToString();
                    reportEngine.Summarize();
                    LinkLabel.Link link = new LinkLabel.Link();
                    String fileName = Path.Combine(reportEngine.ReportPath, "Summary.html");
                    link.LinkData = fileName;
                    Process.Start(link.LinkData as string);
                    reportEngine.CopyFilesToSharePoint();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                this.Activate();
                decimal dcl_TotalTestCasesExecuted = reportEngine.Reporter.TestCases.Count;
                decimal dcl_TotalTestCasesPassed = 0;
                decimal dcl_TotalTestCasesFailed = 0;

                string str_CSVFilePath = Path.Combine(reportEngine.ReportPath, "Summary.csv");

                using (var stream = File.CreateText(str_CSVFilePath))
                {
                    stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", "Module", "Sub-Module", "Category", "UserStory", "TC ID", "TestCase Name", "Browser", "Issue", "Result"));
                    foreach (TestCase testCase in reportEngine.Reporter.TestCases)
                    {
                        if (testCase.IsSuccess)
                            dcl_TotalTestCasesPassed++;
                        else
                            dcl_TotalTestCasesFailed++;
                        stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", testCase.ModuleName, testCase.SubModuleName, "\"" + testCase.ExecutionCategory + "\"", testCase.UserStory, "\"" + testCase.TCID + "\"", testCase.Title, String.Format("{0}-{1}", testCase.Browser.ExeEnvironment, testCase.Browser.BrowserName.ToUpper()), testCase.BugInfo, testCase.IsSuccess));
                    }
                    stream.Flush();
                }

                decimal dcl_PassPercentage = Math.Round((dcl_TotalTestCasesPassed / dcl_TotalTestCasesExecuted) * 100);
                lbl_TotalTCsExecutedResult.Text = dcl_TotalTestCasesExecuted.ToString();
                lbl_TotalTCsPassedResult.Text = dcl_TotalTestCasesPassed.ToString();
                lbl_TotalTCsFailedResult.Text = dcl_TotalTestCasesFailed.ToString();
                lbl_TotalTCsPassPerResult.Text = dcl_PassPercentage.ToString();

                if (ConfigurationManager.AppSettings.Get("SendEmail").ToString().Equals("Yes"))
                {
                    fn_SendEmail2(dcl_TotalTestCasesExecuted, dcl_TotalTestCasesPassed, dcl_TotalTestCasesFailed, dcl_PassPercentage, Path.Combine(reportEngine.ReportPath, "Summary.html"));
                    TimeSpan start = new TimeSpan(20, 0, 0);
                    TimeSpan end = new TimeSpan(6, 0, 0);
                    TimeSpan now = DateTime.Now.TimeOfDay;
                    if ((now > start) && (now < end))
                    {
                        //sendEMailThroughOUTLOOK(dcl_TotalTestCasesExecuted, dcl_TotalTestCasesPassed, dcl_TotalTestCasesFailed, dcl_PassPercentage, Path.Combine(reportEngine.ReportPath, "Summary.html"));
                    }
                }

                Console.WriteLine(dcl_PassPercentage.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                //Report.ReportPass("SendMail", returnString);
            }
            catch (Exception ex)
            {
                returnString = "Error: " + ex.ToString();
                //Report.ReportSoftFail("SendMail", returnString);
            }
            return returnString;
        }
        
        private void lnklbl_OverallExeStatResult_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Link.LinkData as string))
                    // Send the URL to the operating system.
                    Process.Start(e.Link.LinkData as string);
            }
            catch { }
        }

        private void lstbox_Module_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterSelection("Module", "Module_SubModule", "Module_Category", "Module_UserStory");
        }

        /// <summary>
        /// Filter Selection
        /// </summary>
        /// <param name="module"></param>
        /// <param name="subModule"></param>
        /// <param name="catogeryModule"></param>
        /// <param name="userStoryModule"></param>
        private void FilterSelection(string module, string subModule, string catogeryModule, string userStoryModule)
        {
            dataGridView1.DataSource = null;

            string strSubModuleFilterCriteria = string.Empty;
            string strModuleFilterCriteria = string.Empty;
            string strCategoryFilterCriteria = string.Empty;
            string strUserStoryFilterCriteria = string.Empty;

            if (lstbox_Module.SelectedIndex != -1)
                strModuleFilterCriteria = lstbox_Module.SelectedItem.ToString().Trim();
            Locator.ModuleName = strModuleFilterCriteria;
            if (lstbox_SubModule.SelectedIndex != -1)
            {
                colSubModuleFilterCriteria = lstbox_SubModule.SelectedItems;
                strSubModuleFilterCriteria = GetFilterCriteria(subModule, strModuleFilterCriteria);
            }
            else
                strSubModuleFilterCriteria = "'**'";

            if (lstbox_Criteria.SelectedIndex != -1)
            {
                colCategoryFilterCriteria = lstbox_Criteria.SelectedItems;

                strCategoryFilterCriteria = GetFilterCriteria(catogeryModule, strModuleFilterCriteria);
            }
            else
                strCategoryFilterCriteria = "'**'";

            if (lstbox_UserStory.SelectedIndex != -1)
            {
                colUserStoryFilterCriteria = lstbox_UserStory.SelectedItems;

                strUserStoryFilterCriteria = GetFilterCriteria(userStoryModule, strModuleFilterCriteria);
            }
            else
                strUserStoryFilterCriteria = "'**'";

            string strCondition = string.Empty;
            if (module.Equals("Module"))
            {
                strCondition = "ModuleName LIKE '*" + strModuleFilterCriteria + "*'" + " AND SubModuleName LIKE " + strSubModuleFilterCriteria + "  AND ExecutionCategories LIKE " + strCategoryFilterCriteria + " AND UserStory LIKE " + strUserStoryFilterCriteria;
            }
            else
            {
                strCondition = "SubModuleName LIKE " + strSubModuleFilterCriteria + "  AND ExecutionCategories LIKE " + strCategoryFilterCriteria + " AND UserStory LIKE " + strUserStoryFilterCriteria;
            }

            DataView dvFilteredTestCaseDetails = new DataView(dtTestCaseDetails);
            dvFilteredTestCaseDetails.Sort = "ModuleName";
            dvFilteredTestCaseDetails.Sort = "SubModuleName";
            dvFilteredTestCaseDetails.Sort = "TestCaseID";
            dvFilteredTestCaseDetails.Sort = "TestCaseDescription";
            dvFilteredTestCaseDetails.Sort = "ExecutionCategories";
            dvFilteredTestCaseDetails.Sort = "UserStory";
            dvFilteredTestCaseDetails.RowFilter = strCondition;
            dataGridView1.DataSource = dvFilteredTestCaseDetails;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            lbl_TotalTCsResult.Text = dvFilteredTestCaseDetails.Count.ToString();
        }

        private void lstbox_SubModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterSelection("SubModule", "Sub_SubModule", "Sub_Category", "Sub_UserStory");
        }

        /// <summary>
        /// Gets Filter Criteria as per user selection.
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="strModuleFilterCriteria"></param>
        /// <returns></returns>
        private string GetFilterCriteria(string moduleName, string strModuleFilterCriteria)
        {
            string filterCriteria = string.Empty;
            switch (moduleName)
            {
                case "Module_SubModule":
                    {
                        colSubModuleFilterCriteria = lstbox_SubModule.SelectedItems;
                        string strSubModuleFilterCriteria = string.Empty;
                        if (colSubModuleFilterCriteria.Count > 1)
                        {
                            for (int i = 0; i < colSubModuleFilterCriteria.Count - 1; i++)
                            {
                                strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'" + " OR SubModuleName LIKE ";
                            }
                            strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[colSubModuleFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'";
                        }
                        else
                            strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'";
                        filterCriteria = strSubModuleFilterCriteria;
                    }
                    break;

                case "Module_Category":
                    {
                        string strCategoryFilterCriteria = string.Empty;
                        if (colCategoryFilterCriteria.Count > 1)
                        {
                            for (int i = 0; i < colCategoryFilterCriteria.Count - 1; i++)
                            {
                                strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'" + " OR ExecutionCategories LIKE ";
                            }
                            strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[colCategoryFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'";
                        }
                        else
                            strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'";

                        filterCriteria = strCategoryFilterCriteria;
                    }
                    break;

                case "Module_UserStory":
                    {
                        colUserStoryFilterCriteria = lstbox_UserStory.SelectedItems;
                        string strUserStoryFilterCriteria = string.Empty;

                        if (colUserStoryFilterCriteria.Count > 1)
                        {
                            for (int i = 0; i < colUserStoryFilterCriteria.Count - 1; i++)
                            {
                                strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'" + " OR UserStory LIKE ";
                            }
                            strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[colUserStoryFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'";
                        }
                        else
                            strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '" + strModuleFilterCriteria + "'";

                        filterCriteria = strUserStoryFilterCriteria;
                    }
                    break;
                case "Sub_SubModule":
                case "Catego_SubModule":
                case "UserStory_SubModule":
                    {
                        string strSubModuleFilterCriteria = string.Empty;

                        if (colSubModuleFilterCriteria.Count > 1)
                        {
                            for (int i = 0; i < colSubModuleFilterCriteria.Count - 1; i++)
                            {
                                strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'" + " OR SubModuleName LIKE ";
                            }
                            strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[colSubModuleFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
                        }
                        else
                            strSubModuleFilterCriteria = strSubModuleFilterCriteria + "'*" + colSubModuleFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";

                        filterCriteria = strSubModuleFilterCriteria;
                    }
                    break;
                case "Sub_Category":
                case "Catego_Category":
                case "UserStory_Category":
                    {
                        string strCategoryFilterCriteria = string.Empty;
                        if (colCategoryFilterCriteria.Count > 1)
                        {
                            for (int i = 0; i < colCategoryFilterCriteria.Count - 1; i++)
                            {
                                strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'" + " OR ExecutionCategories LIKE ";
                            }
                            strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[colCategoryFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
                        }
                        else
                            strCategoryFilterCriteria = strCategoryFilterCriteria + "'*" + colCategoryFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";

                        filterCriteria = strCategoryFilterCriteria;
                    }
                    break;

                case "Sub_UserStory":
                case "Catego_UserStory":
                case "UserStory_UserStory":
                    {
                        string strUserStoryFilterCriteria = string.Empty;
                        if (colUserStoryFilterCriteria.Count > 1)
                        {
                            for (int i = 0; i < colUserStoryFilterCriteria.Count - 1; i++)
                            {
                                strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[i].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'" + " OR ExecutionCategories LIKE ";
                            }
                            strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[colUserStoryFilterCriteria.Count - 1] + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";
                        }
                        else
                            strUserStoryFilterCriteria = strUserStoryFilterCriteria + "'*" + colUserStoryFilterCriteria[0].ToString() + "*'" + " AND ModuleName LIKE '*" + strModuleFilterCriteria + "*'";

                        filterCriteria = strUserStoryFilterCriteria;
                    }
                    break;
            }

            return filterCriteria;
        }

        private void lstbox_Criteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterSelection("Catogery", "Catego_SubModule", "Catego_Category", "Catego_UserStory");
        }

        private void btn_ClearAllFilters_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            lstbox_Module.SelectedIndex = -1;
            lstbox_SubModule.SelectedIndex = -1;
            lstbox_Criteria.SelectedIndex = -1;
            lstbox_UserStory.SelectedIndex = -1;

            string strModuleFilterCriteria = string.Empty;
            string strSubModuleFilterCriteria = string.Empty;
            string strCategoryFilterCriteria = string.Empty;
            string strUserStoryFilterCriteria = string.Empty;

            string strCondition = "ModuleName LIKE '*" + strModuleFilterCriteria + "*' AND SubModuleName LIKE '*" + strSubModuleFilterCriteria + "*'  AND ExecutionCategories LIKE '*" + strCategoryFilterCriteria + "*'" + " AND UserStory LIKE '*" + strUserStoryFilterCriteria + "*'";

            DataView dvFilteredTestCaseDetails = new DataView(dtTestCaseDetails);
            dvFilteredTestCaseDetails.Sort = "Sl.No";
            dvFilteredTestCaseDetails.RowFilter = string.Empty;
            dataGridView1.DataSource = dvFilteredTestCaseDetails;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

            lbl_TotalTCsResult.Text = dvFilteredTestCaseDetails.Count.ToString();
            lbl_TotalTCsExecutedResult.Text = "0";
            lbl_TotalTCsPassedResult.Text = "0";
            lbl_TotalTCsFailedResult.Text = "0";
            lbl_TotalTCsPassPerResult.Text = "0";

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = false;
            }
            chkbox_SelectAll.Checked = false;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lstbox_UserStory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterSelection("UserStory", "UserStory_SubModule", "UserStory_Category", "UserStory_UserStory");
        }

        private void lbl_FilterByCategory_Click(object sender, EventArgs e)
        {

        }

        private List<string> ReadFromTextFile()
        {
            List<string> testCasesToLoad = new List<string>();

            testCasesToLoad = File.ReadLines("TestData/TestcasesToRun.txt").ToList<string>();
            return testCasesToLoad;
        }
        /// <summary>
        /// Initialize Fields
        /// </summary>
        public void InitializeFields()
        {
            lst_AvailableModules = new List<string>();
            lst_AvailableSubModules = new List<string>();
            lst_AvailableUserStories = new List<string>();
            lst_AvailableCategories = new List<string>();
            dvTestCaseDetails = new DataView();
            dtTestCaseDetails = new DataTable();
            testCaseToExecute = new List<object[]>();
            SingleThreadedTests = new List<object[]>();
            qualifiedNames = new Dictionary<string, string>();
            lstbox_Module.Items.Clear();
            lstbox_SubModule.Items.Clear();
            lstbox_Criteria.Items.Clear();
            lstbox_UserStory.Items.Clear();
        }

        /// <summary>
        /// button1_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AppSetter appSetter = new AppSetter();
                appSetter.ShowDialog();

                // Reload the Test cases.
                this.InitializeFields();
                this.form_TestSuiteRunner_Load(this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}