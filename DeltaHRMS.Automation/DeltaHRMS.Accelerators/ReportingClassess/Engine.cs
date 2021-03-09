using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Collections;
using Microsoft.SharePoint.Client;
using System.Globalization;
using System.Security;
using System.Threading;

namespace DeltaHRMS.Accelerators.Reporting
{
    public class Engine
    {
        private String reportsPath = String.Empty;
        private String serverName = String.Empty;
        private String timestamp = String.Empty;
        private Object _provisionalSummaryLocker = new Object();

        Summary summary = new Summary();

        /// <summary>
        /// Gets Report Path
        /// </summary>
        public String ReportPath
        {
            get
            {
                return reportsPath;
            }
        }

        public static string pathOfReport { get; set; }

        /// <summary>
        /// Gets Reports TimeStamp
        /// </summary>
        public String Timestamp
        {
            get
            {
                return timestamp;
            }
        }

        /// <summary>
        /// Gets Server name
        /// </summary>
        public String ServerName
        {
            get
            {
                return serverName;
            }
        }



        /// <summary>
        /// Gets or sets Reporter
        /// </summary>
        public Summary Reporter
        {
            get
            {
                return summary;
            }
        }

        /// <summary>
        /// Gets or sets Reporter
        /// </summary>
        public string NoOfTestCasesExecuted { get; set; }

        public string TotalNoOfTestCases { get; set; }

        /// <summary>
        /// Creates Engine instance
        /// </summary>
        /// <param name="resultPath">Path to Report Results</param>
        public Engine(String resultPath, String serverName, String browserType = "", String concept = "")
        {
            try
            {
                this.serverName = serverName;
                this.timestamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.Local).ToString("MMddyyyyHHmmss");
                string browserConcept = (string.IsNullOrEmpty(browserType) == true ? "" : "-" + browserType) + (string.IsNullOrEmpty(concept) == true ? "" : "-" + concept);
                this.reportsPath = Path.Combine(resultPath, this.timestamp + browserConcept);
                pathOfReport = this.reportsPath;
                System.IO.Directory.CreateDirectory(this.reportsPath);
                System.IO.Directory.CreateDirectory(Path.Combine(this.reportsPath, "Screenshots"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CopyFilesToSharePoint()
        {
            var encodedPassInByts = Convert.FromBase64String(ConfigurationManager.AppSettings.Get("SharePointPassword").ToString());
            string pass = Encoding.UTF8.GetString(encodedPassInByts);
            SecureString password = new SecureString();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            foreach (char c in pass)
            {
                password.AppendChar(c);
            }

            string siteUrl = ConfigurationManager.AppSettings.Get("SharePointSite").ToString();
            ClientContext clientContext = new ClientContext(siteUrl);
            clientContext.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings.Get("SharePointUserName").ToString(), password);

            FileHelper.UploadFoldersRecursively(clientContext, this.reportsPath, "Documents");
        }

        public class FileHelper
        {
            public static void UploadDocument(ClientContext clientContext, string sourceFilePath, string serverRelativeDestinationPath)
            {
                using (var fs = new FileStream(sourceFilePath, FileMode.Open))
                {
                    var fi = new FileInfo(sourceFilePath);
                    Microsoft.SharePoint.Client.File.SaveBinaryDirect(clientContext, serverRelativeDestinationPath, fs, true);
                }
            }

            public static void UploadFolder(ClientContext clientContext, System.IO.DirectoryInfo folderInfo, Folder folder)
            {
                System.IO.FileInfo[] files = null;
                System.IO.DirectoryInfo[] subDirs = null;

                try
                {
                    files = folderInfo.GetFiles("*.*");
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }

                if (files != null)
                {
                    foreach (System.IO.FileInfo fi in files)
                    {
                        Console.WriteLine(fi.FullName);
                        clientContext.Load(folder);
                        clientContext.ExecuteQuery();
                        UploadDocument(clientContext, fi.FullName, folder.ServerRelativeUrl + "/" + fi.Name);
                    }

                    subDirs = folderInfo.GetDirectories();

                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        Folder subFolder = folder.Folders.Add(dirInfo.Name);
                        clientContext.ExecuteQuery();
                        UploadFolder(clientContext, dirInfo, subFolder);
                    }
                }
            }

            public static void UploadFoldersRecursively(ClientContext clientContext, string sourceFolder, string destinationLigraryTitle)
            {
                Web web = clientContext.Web;
                var query = clientContext.LoadQuery(web.Lists.Where(p => p.Title == destinationLigraryTitle));
                clientContext.ExecuteQuery();
                List documentsLibrary = query.FirstOrDefault();
                var folder = documentsLibrary.RootFolder;
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sourceFolder);

                clientContext.Load(documentsLibrary.RootFolder);
                clientContext.ExecuteQuery();

                folder = documentsLibrary.RootFolder.Folders.Add(di.Name);
                clientContext.ExecuteQuery();

                FileHelper.UploadFolder(clientContext, di, folder);
            }
        }

        public void CreateDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            Directory.CreateDirectory(directoryPath);
        }

        /// <summary>
        /// Publishes Summary Report of an iteration
        /// </summary>
        public void PublishIteration(Iteration iteration)
        {
            // If current iteration is a failure, get screenshot
            //if (!iteration.IsSuccess)
            //{
            //    try
            //    {
            //        File.WriteAllBytes(Path.Combine(this.reportsPath, "Screenshots", String.Format("{0} {1} {2} Error.png", iteration.Browser.TestCase.Title, iteration.Browser.Title, iteration.Title)),
            //                           Convert.FromBase64String(iteration.Screenshot));
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}

            #region Write HTML Content

            String template = @"
            <html>
            <head>
	        <link href='http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css' rel='stylesheet'>
	        <script src='http://code.jquery.com/jquery-1.11.0.min.js' type='text/javascript'></script>
            <script type='text / javascript' src='https://www.gstatic.com/charts/loader.js'></script>
            < script src='http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js'></script>
	        <style>
            html {
	        overflow: -moz-scrollbars-vertical; /* Vertical Scroll bar always visible, to avoid flicker while collapse/expand */
	        overflow-y: scroll;
	        }
            .bigger-icon {
		        transform:scale(3.0,3.0);
		        -ms-transform:scale(3.0,3.0); /* IE 9 */
		        -moz-transform:scale(3.0,3.0); /* Firefox */
		        -webkit-transform:scale(3.0,3.0); /* Safari and Chrome */
		        -o-transform:scale(3.0,3.0); /* Opera */}
             .default {
		
		            font-family: Courier New;
					font-size: 15px;
	            }
	        .Report-Chapter {
                padding:12px; margin-bottom: 5px;
		        background-color: #26466D; color: #fff;
		        font-size: 90%; font-weight:bold;
		        border: 1px solid #03242C; border-radius: 4px;
		        font-family: Menlo,Monaco,Consolas,'Courier New',monospace; cursor: pointer; }
	        .Report-Step {
		        padding:12px; margin-bottom: 5px;
		        background-color: #ddd; color: #000;
		        font-size: 90%; font-weight:bold;
		        border: 1px solid #bebebe; border-radius: 4px;
		        font-family: Menlo,Monaco,Consolas,'Courier New',monospace; cursor: pointer;}
	        .Report-Action {
		        padding:12px; margin-bottom: 5px;
		        background-color: #f7f7f9; color: #000; font-size: 90%;
		        border: 1px solid #e1e1e8; border-radius: 4px;
		        font-family: Menlo,Monaco,Consolas,'Courier New',monospace;}
	        .green { color:green; }
	        .red {color: red; }
	        .normal {color: black; }
            .brightgreen {color:lime;}
	        .brightred {color: orangered;}
            .darkbg {background-image:url('https://passportplus2btraining.freemanco.com/Content/img/freeman_header_bkg.jpg');}
            .timestamp {color:#555;}
	        </style>
            <script language='javascript'>
            	$(function() {
		            $('.Report-Chapter').click(function(){
			            $(this).parent().children('.wrapper').slideToggle();
		            });
		
		            $('.Report-Step').click(function(){
			            $(this).parent().children('.Report-Action').slideToggle();
		            });
	            });
            </script>
            </head>
            <body>
	            <div class='container'>
		            <div style='padding-top: 5px; padding-bottom:5px;'>
			            <div class='pull-right'><img src='https://deltaintech.com/wp-content/uploads/2020/04/logo.png'/></div>
		            </div>
	            </div>
	            <div class='container default'>
                    <div class='darkbg' style='background-color:#26466D; color:#fff; min-height:100px; padding:20px; margin-bottom:5px; margin-top:5px; top:-40px;'>
		                <div class='row'>
		                  <div class='col-md-6' > <b> Server: </b>{{SERVER}}<br/> <b> Browser: </b>{{BROWSER}}<br/> </div>
		                  <div class='col-md-6' > <b> Start: </b>{{EXECUTION_BEGIN}}<br/> <b> End: </b>{{EXECUTION_END}}<br/><b> Duration: </b>{{EXECUTION_DURATION}}</div>
		                </div>
	                </div>
                </div>
                <div class='container default'>
                    <div class='darkbg' style='background-color:#26466D; color:#fff; min-height:60px; padding:20px; margin-bottom:5px; margin-top:5px; top:-20px;'>
		                <div class='row'>
                          <div class='col-md-3' > <b> {{TCID}} </b> </div>
                          <div class='col-md-8' > <b> {{TC_NAME}} </b> </div>
                          <div class='col-md-1' > <span class='glyphicon glyphicon-{{STATUS_ICON}} bigger-icon' style='padding-left:10px;'></span>  </div>
                        </div>
	                </div>
                </div>
                <div class='container'>
                    {{CONTENT}}
                </div>
            </body>
            </html>";
            #endregion

            StringBuilder builder = new StringBuilder();

            foreach (Chapter chapter in iteration.Chapters)
            {
                builder.AppendFormat("<div><p class='Report-Chapter'>Chapter: {0}<span class='pull-right'><span class='glyphicon glyphicon-{1}'></span></span></p>", chapter.Title, chapter.IsSuccess ? "ok brightgreen" : "remove brightred");

                foreach (Step step in chapter.Steps)
                {
                    builder.AppendFormat("<div class='wrapper'><p class='Report-Step'>Step: {0}<span class='pull-right'><span class='glyphicon glyphicon-{1}'></span></span></p>", step.Title, step.IsSuccess ? "ok green" : "remove red");

                    if (step.Actions != null)
                    {
                        foreach (Act action in step.Actions)
                        {
                            builder.AppendFormat("<p class='Report-Action' style='display:none;'>{0}<span class='pull-right'><span class='timestamp'>{1}</span>&nbsp;&nbsp; ", action.Title, action.TimeStamp.ToString("H:mm:ss"));
                            if (action.IsSuccess)
                            {
                                if (!string.IsNullOrEmpty(action.ExternalFilePath))
                                    builder.Append("<a href='" + action.ExternalFilePath + "'><span class='glyphicon glyphicon-open'></span></a>&nbsp;");
                                builder.Append("<span class='glyphicon glyphicon-ok green'></span>");
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(action.Image))
                                    builder.Append("<a href='" + action.Image + "'><span class='glyphicon glyphicon-screenshot'></span></a>&nbsp;");
                                if (!string.IsNullOrEmpty(action.ExternalFilePath))
                                    builder.Append("<a href='" + action.ExternalFilePath + "'><span class='glyphicon glyphicon-open'></span></a>&nbsp;");
                                builder.Append("<span class='glyphicon glyphicon-remove red'></span>");
                            }

                            builder.Append("</span></p>");
                        }
                    }

                    builder.Append("</div>");
                }

                builder.Append("</div>");
            }

            if (!iteration.IsSuccess)
            {
                builder.AppendFormat("<div class='default'><p>{0}</p></div>", iteration.Chapter.Step.Action.Extra);
            }

            template = template.Replace("{{STATUS_ICON}}", iteration.IsSuccess ? "ok brightgreen" : "remove brightred");
            template = template.Replace("{{TCID}}", iteration.Browser.TestCase.TCID);
            template = template.Replace("{{TC_NAME}}", iteration.Browser.TestCase.Name);
            if (iteration.Browser.TestCase.ModuleName.Contains("OUTBACK") || iteration.Browser.TestCase.ModuleName.Contains("OBS"))
                template = template.Replace("{{SERVER}}", ConfigurationManager.AppSettings["ENVQA"]);
            else
                template = template.Replace("{{SERVER}}", this.ServerName);
            template = template.Replace("{{BROWSER}}", String.Format("{0}-{1} {2}", iteration.Browser.ExeEnvironment, iteration.Browser.BrowserName.ToUpper(), iteration.Browser.BrowserVersion));
            template = template.Replace("{{EXECUTION_BEGIN}}", iteration.StartTime.ToString("MM-dd-yyyy HH:mm:ss"));
            template = template.Replace("{{EXECUTION_END}}", iteration.EndTime.ToString("MM-dd-yyyy HH:mm:ss"));
            template = template.Replace("{{EXECUTION_DURATION}}", iteration.EndTime.Subtract(iteration.StartTime).ToString().StartsWith("-") ? "00:00:00.00" : iteration.EndTime.Subtract(iteration.StartTime).ToString());

            String fileName = Path.Combine(this.reportsPath, String.Format("{0} {1} {2}.html", iteration.Browser.TestCase.Title, iteration.Browser.Title, iteration.Title));

            using (StreamWriter output = new StreamWriter(fileName))
            {
                output.Write(template.Replace("{{CONTENT}}", builder.ToString()));
            }
        }

        /// <summary>
        /// Publishes Summary Report
        /// </summary>
        public void Summarize(bool isFinal = true, string Nunitresultfilename = @"C:\Reports\UItestResults.xml")
        {
            #region HTML Template

            string htmlFilePath = Directory.GetCurrentDirectory() + "\\ReportingClassess\\SummaryReport.html";

            string template = string.Empty;
            template = System.IO.File.ReadAllText(htmlFilePath);

            #endregion

            Int16 caseCounter = 1;
            StringBuilder builder = new StringBuilder();
            DateTime FirstCaseBeginTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.Local);
            DateTime LastCaseEndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.Local);
            TimeSpan ExecutionTimeCumulative = TimeSpan.Zero;

            foreach (TestCase testCase in Reporter.TestCases)
            {
                foreach (Browser browser in testCase.Browsers)
                {
                    foreach (Iteration iteration in browser.Iterations.FindAll(itr => itr.IsCompleted == true))
                    {
                        string fileExten = ConfigurationManager.AppSettings.Get("SharePointUrl").ToString() + this.reportsPath.Split('\\')[2] + "/";
                        string strConfluenceURL = string.Concat("", testCase.Name);
                        builder.Append("<tr>");
                        builder.AppendFormat("<td> <table> <tr> <td> {0} </td>  </tr> </table> </td>", caseCounter.ToString());
                        builder.AppendFormat("<td> <table> <tr> <td> {0} </td>  </tr> </table> </td>", testCase.ModuleName.Trim());
                        builder.AppendFormat("<td> <table> <tr> <td> {0} </td>  </tr> </table> </td>", testCase.ExecutionCategory.Trim());
                        builder.AppendFormat("<td> <table> <tr> <td> {0} </td>  </tr> </table> </td>", testCase.UserStory);
                        builder.AppendFormat("<td> <table> <tr> <td> {0} </td>  </tr> </table> </td>", testCase.TCID);
                        builder.AppendFormat("<td> <table> <tr> <td> {0} </td>  </tr> </table>  </td>", strConfluenceURL);
                        builder.AppendFormat("<td>{0}</td>", String.Format("{0}-{1}", iteration.Browser.ExeEnvironment, iteration.Browser.BrowserName.ToUpper()));
                        builder.AppendFormat("<td> <table> <tr> <td> {0} </td>  </tr> </table> </td>", iteration.EndTime.Subtract(iteration.StartTime).ToString(@"hh\:mm\:ss"));
                        builder.AppendFormat("<td> <table style='width:200px;'> <tr> <td> {0} </td>  </tr> </table> </td>", iteration.BugInfo);
                        builder.AppendFormat("<td> <table> <tr> <td><a href='{0}' target='_blank'><span class='glyphicon glyphicon-{1}'></span></a></td>  </tr> </table> </td>", String.Format("{0}{1} {2} {3}.html", fileExten, testCase.Title, browser.Title, iteration.Title), iteration.IsSuccess == true ? "ok green" : "remove red");

                        builder.Append("</tr>");
                        caseCounter++;

                        if (iteration.StartTime < FirstCaseBeginTime) FirstCaseBeginTime = iteration.StartTime;
                        if (iteration.EndTime > LastCaseEndTime) LastCaseEndTime = iteration.EndTime;
                        ExecutionTimeCumulative = ExecutionTimeCumulative.Add(iteration.EndTime.Subtract(iteration.StartTime));
                    }
                }
            }

            Dictionary<String, Dictionary<String, long>> getStatusByBrowser = summary.GetStatusByBrowser();

            List<string> lstDisinctModuleNames = (from m in Reporter.TestCases select m.ModuleName).Distinct().ToList();
            StringBuilder serverName = new StringBuilder();
            foreach (string s in lstDisinctModuleNames)
            {
                {
                    serverName.Append(ConfigurationManager.AppSettings["URL"]);
                    serverName.Append("\r\n");
                }
            }
            template = template.Replace("{{TESTCOUNT}}", NoOfTestCasesExecuted);
            template = template.Replace("{{SERVER}}", serverName.ToString());
            template = template.Replace("{{MAX_PARALLEL}}", ConfigurationManager.AppSettings.Get("MaxDegreeOfParallelism"));
            template = template.Replace("{{EXECUTION_BEGIN}}", FirstCaseBeginTime.ToString("MM-dd-yyyy HH:mm:ss"));
            template = template.Replace("{{EXECUTION_END}}", LastCaseEndTime.ToString("MM-dd-yyyy HH:mm:ss"));
            template = template.Replace("{{EXECUTION_DURATION}}", LastCaseEndTime.Subtract(FirstCaseBeginTime).ToString());
            template = template.Replace("{{EXECUTION_DURATION_CUM}}", ExecutionTimeCumulative.ToString());
            template = template.Replace("{{BARCHARTDATA}}", BuildBarChartData(getStatusByBrowser));
            template = template.Replace("{{BARCHARTDATAPER}}", BuildBarChartDataPer(getStatusByBrowser));
            template = template.Replace("{{BARCHART_TABLE}}", BuildBarChartTable(getStatusByBrowser));

            String fileName = Path.Combine(this.reportsPath, isFinal ? "Summary.html" : "Summary_Provisional.html");
            lock (_provisionalSummaryLocker)
            {
                using (StreamWriter output = new StreamWriter(fileName))
                {
                    output.Write(template.Replace("{{CONTENT}}", builder.ToString()));
                }
            }

            if (isFinal)
            {
                GenerateNunitOutput(FirstCaseBeginTime, LastCaseEndTime, Nunitresultfilename);
            }
        }

        public void GenerateNunitOutput(DateTime startTime, DateTime endTime, string Nunitresultfilename)
        {
            string DirectoryPath = Path.GetDirectoryName(Nunitresultfilename);
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            using (XmlWriter writer = XmlWriter.Create(Nunitresultfilename))
            {
                var testAssembly = ConfigurationManager.AppSettings.Get("TestsDLLName").ToString();

                writer.WriteStartDocument();

                //Header
                writer.WriteStartElement("test-results");
                writer.WriteAttributeString("id", Guid.NewGuid().ToString());
                writer.WriteAttributeString("name", testAssembly);
                writer.WriteAttributeString("total", Reporter.TestCases.Count.ToString());
                writer.WriteAttributeString("passed", Reporter.PassedCount.ToString());
                writer.WriteAttributeString("failed", Reporter.FailedCount.ToString());
                writer.WriteAttributeString("date", startTime.ToUniversalTime().ToString("yyyy-MM-dd"));
                writer.WriteAttributeString("time", startTime.ToUniversalTime().ToString("HH:mm:ss"));

                //Environment Info
                writer.WriteStartElement("environment");
                writer.WriteAttributeString("cwd", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                writer.WriteAttributeString("machine-name", Environment.MachineName);
                writer.WriteAttributeString("user", Environment.UserName);
                writer.WriteAttributeString("user-domain", Environment.UserDomainName);
                writer.WriteEndElement();

                var groupedTests = Reporter.TestCases.GroupBy(g => new { g.ModuleName });

                //container Test suite
                writer.WriteStartElement("test-suite");
                writer.WriteAttributeString("id", testAssembly);
                writer.WriteAttributeString("name", testAssembly);
                writer.WriteAttributeString("executed", "True");
                writer.WriteAttributeString("success", (Reporter.FailedCount == 0).ToString());
                writer.WriteAttributeString("start-time", startTime.ToString());
                writer.WriteAttributeString("end-time", endTime.ToString());
                writer.WriteAttributeString("time", (endTime - startTime).TotalSeconds.ToString());
                writer.WriteStartElement("results");

                foreach (var moduleGroup in groupedTests)
                {
                    writer.WriteStartElement("test-suite");
                    writer.WriteAttributeString("id", testAssembly + "." + moduleGroup.Key.ModuleName);
                    writer.WriteAttributeString("name", moduleGroup.Key.ModuleName);
                    writer.WriteAttributeString("total", moduleGroup.Count().ToString());
                    writer.WriteAttributeString("passed", moduleGroup.Sum(t => t.PassedCount).ToString());
                    writer.WriteAttributeString("failed", moduleGroup.Sum(t => t.FailedCount).ToString());
                    writer.WriteStartElement("results");


                    foreach (var testCase in moduleGroup)
                    {
                        foreach (Browser browser in testCase.Browsers)
                        {
                            foreach (Iteration iteration in browser.Iterations.FindAll(itr => itr.IsCompleted == true))
                            {

                                writer.WriteStartElement("test-case");
                                writer.WriteAttributeString("id", String.Format("{0}.{1}.{2}", testCase.Name, browser.BrowserName, iteration.Title).Trim());
                                writer.WriteAttributeString("name", String.Format("{0}.{1}.{2}", testCase.Name, browser.BrowserName, iteration.Title).Trim());
                                writer.WriteAttributeString("results", (iteration.IsSuccess ? "Passed" : "Failed"));
                                writer.WriteAttributeString("start-time", iteration.StartTime.ToUniversalTime().ToString());
                                writer.WriteAttributeString("end-time", iteration.EndTime.ToUniversalTime().ToString());
                                writer.WriteAttributeString("time", (iteration.EndTime - iteration.StartTime).TotalSeconds.ToString());

                                if (!iteration.IsSuccess)
                                {

                                    var errorSplit = iteration.Chapter.Step.Action.Extra.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                    writer.WriteStartElement("failure");

                                    writer.WriteStartElement("message");
                                    if (!string.IsNullOrEmpty(iteration.BugInfo))
                                        writer.WriteCData(iteration.BugInfo + " - " + errorSplit[0]);
                                    else
                                        writer.WriteCData(errorSplit[0]);
                                    writer.WriteEndElement();

                                    writer.WriteStartElement("stack-trace");
                                    writer.WriteCData(errorSplit[1]);
                                    writer.WriteEndElement();

                                    writer.WriteEndElement();
                                }

                                writer.WriteEndElement();
                            }
                        }
                    }

                    writer.WriteEndElement();//results
                    writer.WriteEndElement();//module suite
                }


                writer.WriteEndElement();//results
                writer.WriteEndElement();//containersuite

                writer.WriteEndElement();//header
                writer.WriteEndDocument();
                writer.Close();
            }
        }

        /// <summary>
        /// Build Bar Chart Data
        /// </summary>
        public string BuildBarChartData(Dictionary<String, Dictionary<String, long>> browserStatus)
        {
            String strReturn = String.Empty;
            int temp;
            int passedTotal = 0;
            int failedTotal = 0;

            strReturn = strReturn + "[ ['Application', 'Passed',  { role: 'style' }, { role: 'annotation' }, 'Failed',  { role: 'style' }, { role: 'annotation' } ],";

            foreach (String browserName in browserStatus.Keys)
            {
                string appName = "Delta HRMS";
                strReturn = strReturn + "['" + appName + "',";

                temp = 1;
                Dictionary<String, long> status = browserStatus[browserName];
                foreach (long statusCount in status.Values)
                {
                    if (temp == 1)
                    {
                        passedTotal = passedTotal + Convert.ToInt32(statusCount);
                        //strReturn = strReturn + statusCount + ", 'green','" + Convert.ToInt32(Math.Round((double)passedTotal / (passedTotal + failedTotal) * 100, 0)) + "%',";
                    }
                    else
                    {
                        failedTotal = failedTotal + Convert.ToInt32(statusCount);
                        //strReturn = strReturn + statusCount + ", 'red','" + Convert.ToInt32(Math.Round((double)failedTotal / (passedTotal + failedTotal) * 100, 0)) + "%',";
                    }
                    temp++;
                }
                strReturn = strReturn + Convert.ToInt32(Math.Round((double)passedTotal / (passedTotal + failedTotal) * 100, 0)) + ", 'green','" + Convert.ToInt32(Math.Round((double)passedTotal / (passedTotal + failedTotal) * 100, 0)) + "%', " + Convert.ToInt32(Math.Round((double)failedTotal / (passedTotal + failedTotal) * 100, 0)) + ", 'red','" + Convert.ToInt32(Math.Round((double)failedTotal / (passedTotal + failedTotal) * 100, 0)) + "%',";
                strReturn = strReturn.TrimEnd(',') + " ],";
            }

            strReturn = strReturn.TrimEnd(',');
            strReturn = strReturn + " ]";
            return strReturn;
        }

        public string BuildBarChartDataPer(Dictionary<String, Dictionary<String, long>> browserStatus)
        {
            String strReturn = String.Empty;
            int temp;
            long _passPercentage = 0;
            long _failPercentage = 0;

            strReturn = strReturn + "[ ['Status', 'Pass %', { role: 'annotation' }, 'Fail %', { role: 'annotation' } ],";

            List<string> lstDisinctModuleNames = (from m in Reporter.TestCases select m.SubModuleName).Distinct().ToList();
            foreach (string SubModuleName in lstDisinctModuleNames)
            {
                int _totalExecuted = (from val in Reporter.TestCases 
                                      where val.SubModuleName == SubModuleName
                                      select val).Count();

                int _passCount = (from val in Reporter.TestCases
                                  where val.SubModuleName == SubModuleName && val.IsSuccess == true
                                  select val).Count();

                int _FailCount = (from val in Reporter.TestCases
                                  where val.SubModuleName == SubModuleName && val.IsSuccess == false
                                  select val).Count();


                _passPercentage = Convert.ToInt64(Math.Round(((double)_passCount / _totalExecuted) * 100, 0));
                _failPercentage = Convert.ToInt64(Math.Round(((double)_FailCount / _totalExecuted) * 100, 0));
                strReturn = strReturn + "['" + SubModuleName + "',";

                foreach (String browserName in browserStatus.Keys)
                {


                    temp = 1;
                    Dictionary<String, long> status = browserStatus[browserName];
                    foreach (long statusCount in status.Values)
                    {
                        if (temp == 1)
                            strReturn = strReturn + _passPercentage + ", '" + _passPercentage + "%',";
                        else
                            strReturn = strReturn + _failPercentage + ", ' ',"; //" + _failPercentage + "%

                        temp++;
                    }

                    strReturn = strReturn.TrimEnd(',') + " ],";
                }
            }

            strReturn = strReturn.TrimEnd(',');
            strReturn = strReturn + " ]";

            return strReturn;
        }

        /// <summary>
        /// Build Bar Chart Table
        /// </summary>
        public string BuildBarChartTable(Dictionary<String, Dictionary<String, long>> browserStatus)
        {
            String strReturn = String.Empty;
            int total = 0;
            int passedTotal = 0;
            int failedTotal = 0;
            int temp;
            int count = 0;
            strReturn = strReturn + "<table class='table table-striped table-bordered table-condensed default'> <tr> <th colspan='6' style='background-color: #1B3F73; color: white'> <center> Test Result Status </center> </th> </tr>";
            strReturn = strReturn + "<tr> <th style='background-color: #1B3F73; color: white'> Test Results </th> <th style='background-color: #1B3F73; color: #1BDE38'> <center> Passed </center> </th> <th style='background-color: #1B3F73; color: red'> <center> Failed </center> </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Tests Executed </center> </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Total Tests </center> </th> </th>  </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Pass% </center> </th> </tr>";

            foreach (String browserName in browserStatus.Keys)
            {
                strReturn = strReturn + "<tr> <td> Delta HRMS </td>";

                Dictionary<String, long> status = browserStatus[browserName];

                total = 0;
                temp = 1;

                foreach (long statusCount in status.Values)
                {
                    strReturn = strReturn + "<td> <center> " + statusCount + " </center> </td>";
                    total = total + Convert.ToInt32(statusCount);

                    if (temp == 1)
                    {
                        passedTotal = passedTotal + Convert.ToInt32(statusCount);
                    }
                    else
                    {
                        failedTotal = failedTotal + Convert.ToInt32(statusCount);
                    }

                    temp++;
                }

                strReturn = strReturn + "<td style='font-weight: bold'> <center> " + total + " </center> </td> <td style='font-weight: bold'> <center> " + TotalNoOfTestCases + " </center> </td> <td style='font-weight: bold'> <center> " + (Convert.ToInt32(Math.Round(((double)passedTotal / (passedTotal + failedTotal)) * 100, 0))) + " %" + " </center> </td> </tr>";
            }
            strReturn = strReturn + "<tr style='font-weight: bold'> <td> Total </td> <td> <center> " + passedTotal + " </center> </td> <td> <center> " + failedTotal + " </center> </td> <td> <center> " + (passedTotal + failedTotal) + " </center> </td> <td> <center> " + TotalNoOfTestCases + " </center> </td> <td> <center> " + (Convert.ToInt32(Math.Round(((double)passedTotal / (passedTotal + failedTotal)) * 100, 0))) + " %" + " </center> </td> </tr> </table>";

            strReturn = strReturn + "<table class='table table-striped table-bordered table-condensed default'> <tr> <th colspan='6' style='background-color: #1B3F73; color: white'> <center> Module Wise - Test Result Status </center> </th> </tr>";
            strReturn = strReturn + "<tr> <th style='background-color: #1B3F73; color: white'> Application </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Module Name </center> </th> </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Tests Executed </center> </th> <th style='background-color: #1B3F73; color: #1BDE38'> <center> Passed </center> </th> <th style='background-color: #1B3F73; color: red'> <center> Failed </center> </th>  </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Pass% </center> </th> </tr>";

            List<string> lstDisinctModuleNames = (from m in Reporter.TestCases select m.SubModuleName).Distinct().ToList();
            foreach (string SubModuleName in lstDisinctModuleNames)
            {
                int _totalExecuted = (from val in Reporter.TestCases
                                      where val.SubModuleName == SubModuleName
                                      select val).Count();

                int _passCount = (from val in Reporter.TestCases
                                  where val.SubModuleName == SubModuleName && val.IsSuccess == true
                                  select val).Count();

                int _FailCount = (from val in Reporter.TestCases
                                  where val.SubModuleName == SubModuleName && val.IsSuccess == false
                                  select val).Count();

                var value = ((double)_passCount / _totalExecuted) * 100;
                int _passPercentage = Convert.ToInt32(Math.Round(value, 0));

                strReturn = strReturn + "<tr style='font-weight: bold'> <td> Delta HRMS </td> <td> <center> " + SubModuleName + " </center> </td> <td> <center> " + _totalExecuted + " </center> </td> <td> <center> " + _passCount + " </center> </td> <td> <center> " + _FailCount + " </center> </td> <td> <center> " + _passPercentage + " %" + " </center> </td> </tr>";
                count = count + 1;
            }

            strReturn = strReturn + "<tr style='font-weight: bold'> <td> Total </td> <td> <center> " + count + " </center> </td> <td> <center> " + (passedTotal + failedTotal) + " </center> </td> <td> <center> " + passedTotal + " </center> </td> <td> <center> " + failedTotal + " </center> </td> <td> <center> " + (Convert.ToInt32(Math.Round(((double)passedTotal / (passedTotal + failedTotal)) * 100, 0))) + " %" + " </center> </td> </tr> </table>";
            return strReturn;
        }

    }
}