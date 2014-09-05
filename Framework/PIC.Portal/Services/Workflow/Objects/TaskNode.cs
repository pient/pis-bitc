using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Workflow
{
    public class TaskNode
    {
        #region Properties

        public string Title { get; set; }

        public string ActorsString { get; set; }

        #endregion

        #region Constructors

        public TaskNode()
        {
        }

        public TaskNode(TaskState tState)
        {

        }

        #endregion
    }
}
