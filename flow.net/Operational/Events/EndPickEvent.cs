<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Layout;

namespace FLOW.NET.Operational.Events
{
    public class EndPickEvent : Event
    {
        private Bin bin;
        private Supermarket supermarket;

        public EndPickEvent()
        {

        }

        public EndPickEvent(double timeIn, Bin binIn, Supermarket supermarketIn, SimulationManager managerIn)
            :base(timeIn, managerIn)
        {
            this.bin = binIn;
            this.supermarket = supermarketIn;
        }
        public override EventState GetEventState()
        {
            throw new NotImplementedException();
        }
        protected override void Operation()
        {
            this.supermarket.AvailablePicker += 1;
            this.supermarket.ReadyBinList.Add(bin);
            this.Manager.TriggerSupermarketControllerAlgorithm(supermarket);

        }
        protected override void TraceEvent()
        {
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Layout;

namespace FLOW.NET.Operational.Events
{
    public class EndPickEvent : Event
    {
        private Bin bin;
        private Supermarket supermarket;

        public EndPickEvent()
        {

        }

        public EndPickEvent(double timeIn, Bin binIn, Supermarket supermarketIn, SimulationManager managerIn)
            :base(timeIn, managerIn)
        {
            this.bin = binIn;
            this.supermarket = supermarketIn;
        }
        public override EventState GetEventState()
        {
            throw new NotImplementedException();
        }
        protected override void Operation()
        {
            this.supermarket.AvailablePicker += 1;
            this.supermarket.ReadyBinList.Add(bin);
            this.Manager.TriggerSupermarketControllerAlgorithm(supermarket);

        }
        protected override void TraceEvent()
        {
        }
    }
}
>>>>>>> 760da83299df0ba0df576f6701afc427ac7f0095
