using System.Text.Json.Nodes;

namespace EzComs.Model.ActionContext.ConcreteActions
{
    public class JustSomeTestAction : IAction
    {
        public Guid Id { get; set; }
        public Dictionary<string, object> ActionOptions { get; set; } = new();
        public List<IAction> nextActions { get; set; } = new();
        public List<IAction> dependsOn { get; set; } = new();
        public ActionState State { get; set; }

        public async Task<ActionState> Execute()
        {
            return ActionState.DONE;
        }
    }
}
