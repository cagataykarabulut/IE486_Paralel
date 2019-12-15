using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Layout;

//IE486 Fall19
namespace FLOW.NET.Operational.Events
{
    public class EndLoadEvent : Event
    {
        private Transporter transporter;
        private Storage storage;
        private BinList binsToUnload;
        private BinList binsToCollect;

        public EndLoadEvent()
        {
        }

        public EndLoadEvent(double timeIn, BinList binsToUnloadIn, BinList binsToCollectIn, Storage storageIn, SimulationManager managerIn, Transporter transporterIn)
            : base(timeIn, managerIn)
        {
            this.transporter = transporterIn;
            this.storage = storageIn;
            this.binsToUnload = binsToUnloadIn;
            this.binsToCollect = binsToCollectIn;
        }

        public override EventState GetEventState()
        {
            throw new NotImplementedException();
        }

        protected override void TraceEvent()
        {
        }


        protected override void Operation()
        {
            foreach (Bin bin in binsToUnload)
            {
                ((Supermarket)transporter.Location).Release(this.Time, bin);
                transporter.Receive(this.Time, bin);
                bin.InTransfer = false;
            }
            if (transporter.Content.Count() != transporter.Capacity)
            {
                this.Manager.EventCalendar.ScheduleEndLoadEvent(this.Time, transporter);
                Statistics readyTransporterAtDockCount = transporter.Location.Statistics["readyTranporterAtDockCount"];
                readyTransporterAtDockCount.UpdateWeighted(this.Time, readyTransporterAtDock.Count);
            }
            else
            {
                
                if (((Node)transporter.Location).TransporterQueue.Count != 0)
                {
                    this.Manager.EventCalendar.ScheduleSeizeNodeEvent(this.Time, transporter.AssignedStorage.Node.TransporterQueueu[0], transporter.Location);
                }
                Node TargetNode = transporter.Route[0];
                transporter.InTransfer = false;
                transporter.OnRoad = true;
                ((Node)transporter.Location).Release(transporter);
                this.Manager.EventCalendar.ScheduleSeizeNodeEvent(this.Time + this.Manager.Distance(this.transporter.Location, TargetNode), transporter, TargetNode);
                this.Manager.OrderManager.TransportersToMatch.Remove(transporter);
                transporter.AssignedStorage = null;
                Statistics readyTransporterAtDockCount = transporter.Location.Statistics["readyTranporterAtDockCount"];
                readyTransporterAtDockCount.UpdateWeighted(this.Time, readyTransporterAtDock.Count);
            }
            return;
        }
    }
}
