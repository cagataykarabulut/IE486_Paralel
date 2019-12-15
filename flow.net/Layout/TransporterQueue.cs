using System;
using System.IO;
using System.Xml.Serialization;
using FLOW.NET.Operational;
using FLOW.NET;
using FLOW.NET.Layout;

namespace FLOW.NET.Layout
{
    [XmlType("TransporterQueue")]
    public class TransporterQueue : StaticObject
    {
        private TransporterList content;

        public TransporterQueue()
        {
            this.CreateStatistics();
        }

        public TransporterQueue(string nameIn, FLOWObject parentIn, int capacityIn, Node nodeIn)
            : base(nameIn, parentIn, capacityIn)
        {
            this.content = new TransporterList();
        }

        [XmlIgnore()]
        public TransporterList TransporterQueueContent
        {
            get { return this.content; }
            set { this.content = value; }
        }

        public void ClearStatistics(double timeIn)
        {
            Statistics length = this.Statistics["Length"];
            length.Clear(timeIn, this.Content.Count);
            Statistics time = this.Statistics["Time"];
            time.Clear(timeIn, 0);
        }

        public void CreateStatistics()
        {
            this.Statistics.Add("Length", new Statistics(0));
            this.Statistics.Add("Time", new Statistics(0));
            this.Statistics.Add("Reserved", new Statistics(0));
        }

        public void FinalizeStatistics(double timeIn)
        {
            Statistics length = this.Statistics["Length"];
            length.UpdateWeighted(timeIn, this.Content.Count);
        }

        public override string GetInformation()
        {
            StringWriter writer = new StringWriter();
            writer.WriteLine(base.GetInformation());
            writer.WriteLine("Capacity : {0}", this.Capacity.ToString());
            return writer.ToString().Trim();
        }

        public void Receive(double timeIn, Transporter transporterIn)
        {
            this.Content.Add(transporterIn);
            Statistics length = this.Statistics["Length"];
            length.UpdateWeighted(timeIn, this.Content.Count);
            transporterIn.ChangeLocation(timeIn, this);
            this.Reserved--; //Do we use this? check statistics:
            Statistics reserved = this.Statistics["Reserved"];
            reserved.UpdateWeighted(timeIn, this.Reserved);
        }

        public void Release(double timeIn, Transporter transporterIn)
        {
            this.Content.Remove(transporterIn);
            Statistics length = this.Statistics["Length"];
            length.UpdateWeighted(timeIn, this.Content.Count);
            Statistics time = this.Statistics["Time"];
            time.UpdateAverage(timeIn, (timeIn - transporterIn.EntryTime));
            transporterIn.Location = null;
        }
    }

    [XmlType("TransporterQueueState")]
    public class TransporterQueueState : StaticObjectState
    {
        public TransporterQueueState()
        {
        }

        public void GetState(TransporterQueue transporterQueueIn)
        {
            base.GetState(transporterQueueIn);
        }


        //public void SetState(TransporterQueue transporterQueueIn, SimulationManager managerIn)
        //{
        //    JobManager jobManager = managerIn.JobManager;
        //    transporterQueueIn.Content.Clear();
        //    foreach (string transporterName in this.Content)
        //    {
        //        Transporter transporter = jobManager.Transporters[transporterName];
        //        transporterQueueIn.Receive(transporter.EntryTime, transporter);
        //    }
        //    base.SetState(transporterQueueIn);
        //}
    }

}
