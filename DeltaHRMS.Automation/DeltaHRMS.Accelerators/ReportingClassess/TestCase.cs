using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeltaHRMS.Accelerators.Reporting
{
    public class TestCase
    {
        private List<Browser> browsers = new List<Browser>();

        /// <summary>
        /// Creates a new Chapter
        /// </summary>
        /// <param name="title">Title</param>
        public TestCase(String strModuleName, String strSubModuleName, String id, String name, String requirementFeature)
        {
            this.ModuleName = strModuleName;
            this.SubModuleName = strSubModuleName;
            this.Title = id;
            this.Name = name;
            this.RequirementFeature = requirementFeature;
        }

        /// <summary>
        /// Creates a new Chapter
        /// </summary>
        /// <param name="title">Title</param>
        public TestCase(String strModuleName, String strSubModuleName, string tcid, String id, String name, String requirementFeature)
        {
            this.ModuleName = strModuleName;
            this.SubModuleName = strSubModuleName;
            this.TCID = tcid;
            this.Title = id;
            this.Name = name;
            this.RequirementFeature = requirementFeature;
        }

        /// <summary>
        /// Creates a new Chapter
        /// </summary>
        /// <param name="title">Title</param>
        public TestCase(String strModuleName, String strSubModuleName, string usid, string tcid, String id, String name, String requirementFeature, string executionCategory)
        {
            this.ModuleName = strModuleName;
            this.SubModuleName = strSubModuleName;
            this.UserStory = usid;
            this.TCID = tcid;
            this.Title = id;
            this.Name = name;
            this.RequirementFeature = requirementFeature;
            this.ExecutionCategory = executionCategory;
        }

        /// <summary>
        /// Gets or sets Module
        /// </summary>
        public String ModuleName { get; set; }


        /// <summary>
        /// Gets or sets SubModuleName
        /// </summary>
        public String SubModuleName { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Gets or sets Test Case ID
        /// </summary>
        public String TCID { get; set; }

        /// <summary>
        /// Gets or sets User Story ID
        /// </summary>
        public String UserStory { get; set; }

        /// <summary>
        /// Gets or sets Test Case Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets Requirement Feature
        /// </summary>
        public String RequirementFeature { get; set; }

        /// <summary>
        /// Gets or sets Test Case Execution Category
        /// </summary>
        public String ExecutionCategory { get; set; }

        /// <summary>
        /// Gets Browsers
        /// </summary>
        public List<Browser> Browsers
        {
            get
            {
                return browsers;
            }
        }

        /// <summary>
        /// Gets current Browser
        /// </summary>
        public Browser Browser
        {
            get
            {
                return Browsers.Last();
            }
        }

        /// <summary>
        /// Gets Passed Count
        /// </summary>
        public int PassedCount
        {
            get
            {
                return Browsers.FindAll(i => i.IsSuccess == true).Count;
            }
        }

        /// <summary>
        /// Gets Failed Count
        /// </summary>
        public int FailedCount
        {
            get
            {
                return Browsers.FindAll(i => i.IsSuccess == false).Count;
            }
        }

        /// <summary>
        /// Gets IsSuccess
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return Browsers.FindAll(i => i.IsSuccess == false).Count == 0;
            }
        }

        /// <summary>
        /// Gets or sets BugInfo
        /// </summary>
        public String BugInfo { get; set; }

        public Summary Summary { get; set; }
    }
}