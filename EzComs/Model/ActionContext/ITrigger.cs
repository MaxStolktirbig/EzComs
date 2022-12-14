namespace EzComs.Model.ActionContext
{
    /// <summary>
    /// A trigger triggers an actionflow.
    /// </summary>
    public interface ITrigger
    {
        ActionFlow actionFlow { get; set; }
    }
}
