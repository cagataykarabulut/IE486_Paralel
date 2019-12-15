using System;
using FLOW.NET.Layout;
using FLOW.NET.Operational;

namespace FLOW.NET.Decision.Matching
{
    public class NearestVehicle : MatchingAlgorithm
    {

        protected override TransferTaskDecision MakeMatching(TransporterList transportersToMatchIn, TransferTaskList transferTasksToMatchIn)
        {
            TransferTask transferTask = transferTasksToMatchIn[0];
            for (int i = 1; i < transferTasksToMatchIn.Count; i++)
            {
                if (transferTask.DueDate > transferTasksToMatchIn[i].DueDate)
                {
                    transferTask = transferTasksToMatchIn[i];
                }
            }
            transferTasksToMatchIn.Remove(transferTask); //The TransferTask with the earliest DueDate is choosen and removed from the transferTasksToMatchIn list.

            TransferTaskDecision decision = new TransferTaskDecision();
            decision.transferTask = transferTask;

            TransporterList AlreadyAtThatSupermarket = new TransporterList();
            TransporterList JustEndUnloaded = new TransporterList();
            TransporterList AtPark = new TransporterList();
            TransporterList OnRoadToPark = new TransporterList();

            foreach (Transporter transporter in transportersToMatchIn)
            {
                if (transporter.AssignedStorage == (Supermarket)transferTask.Location)
                    AlreadyAtThatSupermarket.Add(transporter);

                else if (transporter.AssignedStorage == null)
                {
                    if (transporter.Location == transporter.Park)
                    {
                        AtPark.Add(transporter);
                    }

                    else if (transporter.OnRoad == true)
                    {
                        OnRoadToPark.Add(transporter);
                    }

                    else
                    {
                        JustEndUnloaded.Add(transporter);

                    }
                }
            }
            foreach (Transporter transporter in AlreadyAtThatSupermarket)
            {
                if (transporter.AvailableStorage(transferTask))
                {
                    decision.transporter = transporter;
                    return decision;
                }
            }

            if (JustEndUnloaded.Count > 0)
            {
                decision.transporter = JustEndUnloaded[0];
                return decision;

            }
            if (AtPark.Count != 0)
            {
                decision.transporter = AtPark[0];
                return decision;
            }

            if (OnRoadToPark.Count != 0)
            {
                decision.transporter = OnRoadToPark[0];
                return decision;
            }
            return null;
        }
    }

}