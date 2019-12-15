
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Layout;

namespace FLOW.NET.Operational.Events
{
    public class EndUnloadEvent : Event
    {
        private Transporter transporter;
        private Supermarket supermarket;
        public EndUnloadEvent()
        {

        }
        public EndUnloadEvent(double timeIn, SimulationManager managerIn, Transporter transporterIn, Supermarket
            supermarketIn)
           : base(timeIn, managerIn)
        {
            this.transporter = transporterIn;
            this.supermarket = supermarketIn;
        }

        public override EventState GetEventState()
        {
            throw new NotImplementedException();
        }

        protected override void Operation()
        {
            foreach(Bin bin in this.transporter.Content)
            {
                this.transporter.Release(this.Time, bin);
                this.supermarket.LoadBin(this.Time, bin);
            }
            this.transporter.InTransfer = false;
            this.transporter.AssignedStorage = null;
            this.Manager.TriggerSupermarketControllerAlgorithm(this.supermarket);
            this.Manager.OrderManager.TransportersToMatch.Add(this.transporter);
            this.Manager.TriggerMatchingAlgorithm(this.transporter);
            this.transporter.Statistics["Utilization"].UpdateWeighted(this.Time, 0);
        }

        protected override void TraceEvent()
        {
        }


    }
}

