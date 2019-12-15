using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Collections;

namespace FLOW.NET.Operational
{
    //IE486 Fall19
    public class OrderManager : FLOWObject
    {
        private TransporterList transportersToMatch;
        private TransferTaskList transferTasksToMatch;

        public OrderManager(string nameIn, FLOWObject parentIn)
            : base(nameIn, parentIn)
        {
            this.transferTasksToMatch = new TransferTaskList();
            this.transportersToMatch = new TransporterList();
        }

        public TransporterList TransportersToMatch
        {
            get { return this.transportersToMatch; }
            set { this.transportersToMatch = value; }
        }

        public TransferTaskList TransferTasksToMatch
        {
            get { return this.transferTasksToMatch; }
            set { this.transferTasksToMatch = value; }
        }
    }
}
