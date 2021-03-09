using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaHRMS.TestSuiteExecutor
{
    public class ExecutionAttributes
    {
        private string mModuleName = string.Empty;
        private string mSubModuleName = string.Empty;
        private string mUserStoryId = string.Empty;
        private string mTCID = string.Empty;
        private string mTestCaseName = string.Empty;
        private string mTestCaseDescription = string.Empty;
        private string mExecutionCategories = string.Empty;

        public string ExecutionCategories
        {
            get { return mExecutionCategories; }
            set { mExecutionCategories = value; }
        }
        public string TestCaseDescription
        {
            get { return mTestCaseDescription; }
            set { mTestCaseDescription = value; }
        }
        public string TestCaseName
        {
            get { return mTestCaseName; }
            set { mTestCaseName = value; }
        }
        public string TCID
        {
            get { return mTCID; }
            set { mTCID = value; }
        }


        public string UserStoryId
        {
            get { return mUserStoryId; }
            set { mUserStoryId = value; }
        }

        public string SubModuleName
        {
            get { return mSubModuleName; }
            set { mSubModuleName = value; }
        }

        public string ModuleName
        {
            get { return mModuleName; }
            set { mModuleName = value; }
        }

    }
}
