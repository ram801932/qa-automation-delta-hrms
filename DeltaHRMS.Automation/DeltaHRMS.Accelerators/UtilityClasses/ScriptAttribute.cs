﻿using System;
using System.Collections.Generic;

namespace DeltaHRMS.Accelerators.Utilities
{
    /// <summary>
    /// Represents the attribute to define script properties for a test script/class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScriptAttribute : Attribute
    {
        /// <summary>
        /// The name of the Module
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// The name of the SubModule
        /// </summary>
        public string SubModuleName { get; set; }

        /// <summary>
        /// The name of the testcase
        /// </summary>
        public string TestCaseName { get; set; }

        /// <summary>
        /// The description of the test case
        /// </summary>
        public string TestCaseDescription { get; set; }

        /// <summary>
        /// Categories for selection or grouping of tests by the suite runner
        /// </summary>
        public string ExecutionCategories { get; set; }

        /// <summary>
        /// The Id of the test case
        /// </summary>
        public string TestCaseId { get; set; }

        /// <summary>
        /// The User Story of the test case
        /// </summary>
        public string UserStoryId { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>      
        public ScriptAttribute(string scriptModuleName = "", string scriptSubModuleName = "", string scriptTestCaseName = "", string scriptTestCaseDescription = "", string scriptExecutionCategories = "")
        {
            this.ModuleName = scriptModuleName;
            this.SubModuleName = scriptSubModuleName;
            this.TestCaseName = scriptTestCaseName;
            this.TestCaseDescription = scriptTestCaseDescription;
            this.ExecutionCategories = scriptExecutionCategories;
        }

        /// <summary>
        /// Default constructor
        /// </summary>      
        public ScriptAttribute(string testcaseId = "", string scriptModuleName = "", string scriptSubModuleName = "", string scriptTestCaseName = "", string scriptTestCaseDescription = "", string scriptExecutionCategories = "")
        {
            this.TestCaseId = testcaseId;
            this.ModuleName = scriptModuleName;
            this.SubModuleName = scriptSubModuleName;
            this.TestCaseName = scriptTestCaseName;
            this.TestCaseDescription = scriptTestCaseDescription;
            this.ExecutionCategories = scriptExecutionCategories;
        }

        /// <summary>
        /// Default constructor
        /// </summary>      
        public ScriptAttribute(string userStoryId = "", string testcaseId = "", string scriptModuleName = "", string scriptSubModuleName = "", string scriptTestCaseName = "", string scriptTestCaseDescription = "", string scriptExecutionCategories = "")
        {
            this.UserStoryId = userStoryId;
            this.TestCaseId = testcaseId;
            this.ModuleName = scriptModuleName;
            this.SubModuleName = scriptSubModuleName;
            this.TestCaseName = scriptTestCaseName;
            this.TestCaseDescription = scriptTestCaseDescription;
            this.ExecutionCategories = scriptExecutionCategories;
        }
    }
}

