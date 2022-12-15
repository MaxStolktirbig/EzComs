using System.Text.Json;
using System.Text.Json.Nodes;

namespace EzComs.Model.ActionContext
{
    /// <summary>
    /// An action flow is a representation of a collection of actions that should be done in a certain order
    /// </summary>
    public class ActionFlow
    {
        public Guid? id { get { if (id is null) return new Guid(); else return id; } set { if (id is null) id = new Guid(); } }

        /// <summary>
        /// used to check wether or not the action flow is completed
        /// </summary>
        public Boolean isCompleted { get; set; } = false;
        public List<IAction> InitialActions { get; set; } = new();

        /// <summary>
        /// Starts the execution of the action flow. This function will start all initial actions. 
        /// </summary>
        public void Execute()
        {
            List<IAction> startedActions = new();
            InitialActions.ForEach(action => { action.Start(); startedActions.Add(action); });

            //keep checking if all actions have been completed and remove all actions from execution that are no longer pending or executing 
            while (startedActions.Count() > 0) { foreach (IAction action in startedActions.Where(action => action.State is not ActionState.PENDING or ActionState.IN_EXECUTION){
                    startedActions.Remove(action);
                }; 
            
            }
            //if there are any actions that have not been completed (are not done) then set the isCompleted to false
            isCompleted = InitialActions.Where(step => step.State != ActionState.DONE).Count() > 0;
        }
    }
}
