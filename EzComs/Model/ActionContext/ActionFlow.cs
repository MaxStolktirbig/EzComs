using System.Text.Json;
using System.Text.Json.Nodes;

namespace EzComs.Model.ActionContext
{
    /// <summary>
    /// An action flow is a representation of a collection of actions that should be done in a certain order
    /// </summary>
    public class ActionFlow
    {
        //used to check wether or not the action flow is completed
        public Boolean isCompleted { get; set; } = false;
        public List<IAction> Steps { get; set; } = new();

        public void Execute()
        {
            
            foreach (var step in Steps) {

                _ = step.Start().Result;
            }
            isCompleted = Steps.Where(step => step.State != ActionState.DONE).Count() > 0;
        }
    }
}
