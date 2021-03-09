using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeltaHRMS.Accelerators.Reporting
{
    public class Chapter
    {
        private List<Step> steps = new List<Step>();

        /// <summary>
        /// Creates a new Chapter
        /// </summary>
        /// <param name="title">Title</param>
        public Chapter(String title)
        {
            this.Title = title;
            Console.WriteLine(string.Format("***** {0} *****",this.Title));
        }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Gets Steps
        /// </summary>
        public List<Step> Steps
        {
            get
            {
                return steps;
            }
        }

        /// <summary>
        /// Get current Step
        /// </summary>
        public Step Step
        {
            get
            {
                if (Steps.Count() == 0)
                    Steps.Add(new Step("UNKNOWN STEP"));
                return Steps.Last();
            }
        }

        /// <summary>
        /// Gets or sets IsSuccess
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                //if (Steps.Count() > 0)
                //{
                //    return Step.IsSuccess;
                //}
                //return true;
                return Steps.Where(x => x.IsSuccess == false).Count() == 0 ? true : false;
            }

        }

    }
}
