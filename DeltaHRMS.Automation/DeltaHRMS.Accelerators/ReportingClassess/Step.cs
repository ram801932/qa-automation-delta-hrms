﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeltaHRMS.Accelerators.Reporting
{
    public class Step
    {
        private List<Act> actions = new List<Act>();

        /// <summary>
        /// Creates a new Chapter
        /// </summary>
        /// <param name="title">Title</param>
        public Step(String title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Gets Actions
        /// </summary>
        public List<Act> Actions
        {
            get
            {
                return actions;
            }
        }

        /// <summary>
        /// Gets current Action
        /// </summary>
        public Act Action
        {
            get
            {
                if (Actions != null && Actions.Count > 0)
                    return Actions.Last();
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets or sets IsSuccess
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                //if (Actions.Count() > 0)
                //{
                //    return Action.IsSuccess;
                //}
                //return true;
                return Actions.Where(x => x.IsSuccess == false).Count() == 0 ? true : false;
            }

        }
    }
}
