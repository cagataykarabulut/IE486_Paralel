
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Layout;

namespace FLOW.NET.Operational.Events
{
    public class EndLoadUnloadEvent : Event
    {
        private Transporter transporter;
        private BinMagazine binMagazine;
        private BinList binsToUnload;
        private BinList binsToCollect;
        public EndLoadUnloadEvent()
        {

        }
        public EndLoadUnloadEvent(double timeIn, BinList binsToUnloadIn, SimulationManager managerIn, Transporter transporterIn, BinMagazine
            binMagazineIn)  
           : base(timeIn, managerIn)
        {
            this.transporter = transporterIn;
            this.binMagazine = binMagazineIn;
            this.binsToUnload = binsToUnloadIn;
        }
        public override EventState GetEventState()
        {
            throw new NotImplementedException();
        }


        protected override void Operation()   
        {
            foreach(Bin bin in binsToUnload)
            {
                this.transporter.Release(this.Time, bin);
                ((Node)this.transporter.Location).BinMagazine.LoadBin(this.Time, bin);
            }

            foreach(Bin bin in binsToCollect)
            {
                ((Node)this.transporter.Location).BinMagazine.Release(this.Time, bin);
                this.transporter.Receive(this.Time, bin);
            }

            if(((Node)this.transporter.Location).transporterQueue[0]!= null)
            {
                this.Manager.EventCalendar.ScheduleSeizeNodeEvent(this.Time+1, ((Node)this.transporter.Location).transporterQueue[0]);
            }

            this.Manager.TriggerStationControllerAlgorithm((Station)this.binMagazine.Parent);

            this.Manager.EventCalendar.ScheduleSeizeNodeEvent(this.Time + transporter.TravelTime.GenerateValue(), this.transporter);

            this.Manager.OrderManager.CloseOrder(binsToUnload);

            ((Node)this.transporter.Location).Release(this.Time, this.transporter);
        }

        protected override void TraceEvent()
        {
        }
    }
}

