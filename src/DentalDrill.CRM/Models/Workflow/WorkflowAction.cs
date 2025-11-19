using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Workflow
{
    public class WorkflowAction
    {
        public WorkflowAction(String title, String url, Boolean enabled)
        {
            this.Title = title;
            this.Url = url;
            this.Enabled = enabled;
        }

        public String Title { get; }

        public String Url { get; }

        public Boolean Enabled { get; }
    }
}
