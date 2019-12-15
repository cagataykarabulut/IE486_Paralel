using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Layout;

namespace FLOW.NET.Operational.Events
{
    public class BypassEvent : Event
    {
        private Supermarket supermarket;
        private BinList binsToUnload;
  
          
        public BypassEvent()
        {
        }
        public BypassEvent(double timeIn, SimulationManager managerIn, Supermarket supermarketIn, BinList binsToUnloadIn):base(timeIn, managerIn)
        {
            this.supermarket = supermarketIn;
            this.binsToUnload = binsToUnloadIn;
        }

        public Supermarket Supermarket
        {
            get { return this.supermarket; }
            set { this.supermarket = value; }
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
            foreach(Bin bin in binsToUnload)
            {
                Bin binToCollect = supermarket.GetBinWithMinimumCount(bin.ComponentType);
                supermarket.Release(this.Time, binToCollect);
                Statistics EmptyBinCount = supermarket.Statistics["EmptyBinCount"];
                //EmptyBinCount.UpdateAverage(this.Time, binToCollect.Content)
                supermarket.Content.Remove(binToCollect);
                supermarket.LoadBin(this.Time, bin);
            }
            this.Manager.TriggerSupermarketControllerAlgorithm(supermarket);
        }
    }
}

