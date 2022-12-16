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
            List<Task<ActionState>> actionsInExecution = new();
            InitialActions.ForEach(action => { actionsInExecution.Add(action.Start()); });
            actionsInExecution.ForEach(task => task.Start());
            Task.WaitAll(actionsInExecution.ToArray());
            
            //if there are any actions that have not been completed (are not done) then set the isCompleted to false
            isCompleted = actionsInExecution.Where(action => action.Result != ActionState.DONE).Count() > 0 ? false : true;
        }
    }
}
