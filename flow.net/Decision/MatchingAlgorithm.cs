using System;
using FLOW.NET.Layout;
using FLOW.NET.Operational;

namespace FLOW.NET.Decision
{
    public abstract class MatchingAlgorithm : DecisionAlgorithm
    {


        //Transporter<list> FindCandidateTransporters();

        public MatchingAlgorithm()
        {

        }

        public void Execute()
        {
            TransporterList transportersToMatch = this.Manager.OrderManager.TransportersToMatch;
            TransferTaskList transferTasksToMatch = this.Manager.OrderManager.TransferTasksToMatch;
            //Do I copy these lists or just point them? I should copy

            //this.Manager.TransferTaskRankingAlgorithm.Execute(TransferTasksToMatch)

            //Is this how you copy the lists? :
            TransferTaskList transferTasksToMatchIn = new TransferTaskList();
            transferTasksToMatchIn = transferTasksToMatch;
            TransporterList transportersToMatchIn = new TransporterList();
            transportersToMatchIn = transportersToMatch;

            //The original lists must not be modified. The copied lists should be used as input for MakeMatchingAlgorihm
            while (transferTasksToMatchIn.Count != 0 && transportersToMatchIn.Count != 0)
            {
                //MakeMatchingAlgorithm must empty one of the lists so that the loop ends
                TransferTaskDecision decision = this.MakeMatching(transportersToMatchIn, transferTasksToMatchIn);
                if (decision != null)
                {
                    TransferTask transferTask = decision.transferTask;
                    Transporter transporter = decision.transporter;
                    transferTasksToMatch.Remove(transferTask);
                    transporter.AssignedTasks.Add(transferTask);
                    transporter.AssignedStorage = transferTask.Location;

                    if (transporter.Location == transferTask.Location)
                    {
                        this.Manager.TriggerSupermarketControllerAlgorithm(transferTask.Location);
                        if (!transferTask.Location.ListOfReadyTransportersAtDock.Contains(transporter))
                        {
                            transferTask.Location.ListOfReadyTransportersAtDock.Add(transporter);
                        }
                    }

                    else
                    {
                        transporter.Route.Add(transferTask.Location);
                        this.Manager.EventCalendar.ScheduleSeizeNodeEvent(this.Manager.Time, transporter);
                        ((Node)transporter.Location).Release(this.Manager.Time, transporter);
                        //transporter.OnRoad = true;

                    }
                }
            }

            foreach (Transporter transporter in transportersToMatch)
            {
                if (transporter.AssignedStorage == null && transporter.Location != transporter.Park && transporter.OnRoad == false)
                {
                    transporter.Route.Add(transporter.Park.Node);
                    this.Manager.EventCalendar.ScheduleSeizeNodeEvent(this.Manager.Time, transporter);
                    ((Node)transporter.Location).Release(this.Manager.Time, transporter);
                    //transporter.OnRoad = true;
                }
            }


        }

        protected abstract TransferTaskDecision MakeMatching(TransporterList transportersToMatchIn, TransferTaskList transferTasksToMatchIn);
    }


    public class TransferTaskDecision
    {
        public Transporter transporter;
        public TransferTask transferTask;
    }
}
