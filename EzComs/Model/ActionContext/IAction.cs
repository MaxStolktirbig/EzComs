using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EzComs.Model.ActionContext
{
    /// <summary>
    /// Represent any action, this has to be implemented to be added to an actionflow
    /// </summary>
    public interface IAction
    {
        Guid Id { get; set; }
        ActionState State { get; set; }

        //Contains all the variables necessary to complete the action
        Dictionary<string, object> ActionOptions { get; set; }

        //the action depends on the  these actions 
        public List<IAction> dependsOn { get; set; }

        public List<IAction> nextActions { get; set; }



        /// <summary>
        /// Kicks of an action. It is sealed which makes it so that the actions are always done in a standardized manner
        /// </summary>
        /// <returns>
        /// An ActionState that represents the state of the current action
        /// </returns>
        public sealed async Task<ActionState> Start() {

            //fail the action if one of the actions that are required failed or cannot start
            if (dependsOn.Where(action => action.State == ActionState.FAILED || action.State == ActionState.COULD_NOT_START).Count() > 0)
            {
                State = ActionState.COULD_NOT_START;
            }
            //keep retrying to start when one of the actions that it depends on is either pending or still in execution
            if (dependsOn.Where(action => action.State == ActionState.IN_EXECUTION || action.State == ActionState.PENDING).Count() > 0)
            {
                State = Start().Result;
            }
            //only start the action if we're still pending, otherwise return the current state
            if (State == ActionState.PENDING)
            {
                //try to execute this action
                try
                {
                    State = ActionState.IN_EXECUTION;
                    State = Execute().Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    State = ActionState.FAILED;
                }
                //try executing next actions regarless
                ExecuteNextActions();
            }
            return State;

        } 

        //execute should implement the execution type of action
        protected Task<ActionState> Execute();

        /// <summary>
        /// Execute all next actions with multithreading
        /// </summary>
        private void ExecuteNextActions() {

            List<Task<ActionState>> actionsInExecution = new();
            nextActions.ForEach(action => { actionsInExecution.Add(action.Execute()); });
            actionsInExecution.ForEach(async actionInExecution =>
            {
                Console.WriteLine($"Awaiting action execution of action {actionInExecution.Id} on thread {Thread.CurrentThread.ManagedThreadId}");
                _ = await actionInExecution;
                Console.WriteLine($"Finished action execution of action {actionInExecution.Id}");
            });
        }
    }


    public enum ActionState
    {
        PENDING,
        COULD_NOT_START,
        IN_EXECUTION,
        DONE,
        FAILED
    }
}
