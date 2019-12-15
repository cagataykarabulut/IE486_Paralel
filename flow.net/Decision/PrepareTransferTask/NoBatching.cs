using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Layout;
using FLOW.NET.Operational;

namespace FLOW.NET.Decision.PrepareTransferTask
{
    public class NoBatching : PrepareTransferTaskAlgorithm
    {
        protected override TransferTaskList CreateNewTasks(BinList binListIn)
        {
            TransferTaskList newTransferTasks = new TransferTaskList();

            foreach (Bin bin in binListIn)
            {
                newTransferTasks.Add(bin);
            }

            readyBinCount.UpdateWeighted(this.Manager.Time, binListIn.Count); //StatisticsUpdate
            transferTaskCount.UpdateWeighted(this.Manager.Time, newTransferTasks.Count); //StatisticsUpdate

            return 

        }
    }
}
