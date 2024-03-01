namespace PhiAi.Core;

public interface IAlgorithm<TAction>
{
    /// <summary>
    /// Return all possible actions sorted from best to worst
    /// </summary>
    IEnumerable<TAction> GetActions();

    /// <summary>
    /// Advance the state by taking an action
    /// </summary>
    void TakeAction(TAction action);
}