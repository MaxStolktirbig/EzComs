using EzComs.Model.EventContext;

namespace EzComs.Model.ActionContext
{
    /// <summary>
    /// A trigger is subsribed to certain events happening and triggers an actionflow.
    /// </summary>
    public interface ITrigger
    {
        List<Event> subscribedEvents { get; set; }
        ActionFlow actionFlow { get; set; }

        public sealed void TiggerFlow()
        {
            actionFlow.Execute();
        }
    }
}
