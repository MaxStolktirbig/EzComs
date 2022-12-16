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
        bool NextActionsCompleted { get; set; }

        //Contains all the variables necessary to complete the action
        object ActionOptions { get; set; }

        //the action depends on the  these actions 
        public List<IAction> dependsOn { get; set; }

        public List<IAction> nextActions { get; set; }



        /// <summary>
        /// Kicks of an action. It is sealed which makes it so that the actions are always done in a standardized manner
        /// </summary>
        /// <returns>
        /// Returns an ActionState that represents the state of the current action
        /// </returns>
        public sealed async Task<ActionState> Start() {

            NextActionsCompleted = false;
            //fail the action if one of the actions that are required failed or cannot start
            if (dependsOn.Where(action => action.State == ActionState.FAILED || action.State == ActionState.COULD_NOT_START).Count() > 0)
            {
                State = ActionState.COULD_NOT_START;
                Console.WriteLine($"Action(id={Id}) could not be started because it's dependencies were not resolved");
            }
            //keep retrying to start when one of the actions that it depends on is either pending or still in execution
            if (dependsOn.Where(action => action.State == ActionState.IN_EXECUTION || action.State == ActionState.PENDING).Count() > 0)
            {
                State = await Start();
            }
            //only start the action if we're still pending, otherwise return the current state
            if (State == ActionState.PENDING)
            {
                //try to execute this action
                try
                {
                    State = ActionState.IN_EXECUTION;
                    State = await Execute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    State = ActionState.FAILED;
                }
                //try executing next actions regarless
                NextActionsCompleted = ExecuteNextActions();
            }
            return State;

        } 

        /// <summary>
        /// Executes an implementation which differs for every action type
        /// </summary>
        /// <returns>
        /// An ActionState which represents whether or not the task has been succeeded
        /// </returns>
        protected Task<ActionState> Execute();

        /// <summary>
        /// Execute all next actions asynchronously
        /// </summary>
        /// <returns>
        /// Boolean that represents whether or not all followup tasks could be done
        /// </returns>
        private bool ExecuteNextActions() {
            bool returnState = true;
            List<Task<ActionState>> actionsInExecution = new();
            nextActions.ForEach(action => { actionsInExecution.Add(action.Execute()); });
            actionsInExecution.ForEach(task => task.Start());
            Task.WaitAll(actionsInExecution.ToArray());
            actionsInExecution.ForEach(actionInExecution =>
            {
               ActionState currentActionState = actionInExecution.Result;
               returnState = returnState == true ? currentActionState == ActionState.DONE : returnState;
            });
            return returnState;
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
