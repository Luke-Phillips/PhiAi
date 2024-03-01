namespace PhiAi.Core;

// TODO use tick tack toe as an example

/// <summary>
/// PhiAi library provides algorithms for implementing AI within your chosen domain.
/// IDomain is the interface you must implement to define this domain and make the provided algorithms usable.
/// All methods must be referentially transparent (always return output <c>y</c> for input <c>x</c>)
/// </summary>
/// <typeparam name="TState">Represents your domain's state</typeparam>
/// <typeparam name="TAction">Represents your domain's action (transition between states)</typeparam>
public interface IDomain<TState, TAction>
    where TState : IState
    where TAction : IAction
{
    /// <summary>
    /// Transition between states happens via actions, but not all actions are legal from a given state.
    /// <c>GetActionsFromState</c> should return a list of all legal actions
    /// </summary>
    /// <param name="state">The state from which to derive the actions</param>
    /// <returns>A list of legal actions from the given state</returns> 
    IEnumerable<TAction> GetActionsFromState(TState state);
 
    /// <summary>
    /// Transition between states happens via actions.
    /// <c>GetStateFromStateAndAction</c> should return a new state that is the result of transitioning from <paramref name="state"/> via <paramref name="action"/>.
    /// </summary>
    /// <param name="state">The state from which to derive the actions</param>
    /// <returns>A list of legal actions from the given state</returns> 
    TState GetStateFromStateAndAction(TState state, TAction action);

    /// <summary>
    /// A terminal state is one in which no further actions (and by extension, no further state) can take place. <c>IsStateTerminal</c> should return true if the given state is a terminal state. 
    /// </summary>
    /// <param name="state"></param>
    /// <returns>A <c>bool<c> representing whether or not the supplied state (<paramref name="state"/>) is a terminal state</returns>
    bool IsStateTerminal(TState state);
    
    /// <summary>
    /// A value must be assigned once a terminal state is reached. The value helps the algorithm decide which action is best to take.
    /// <c>GetTerminalStateValue<c/> should analyze a terminal state and return a value.
    /// <example>
    /// If implementing a board game AI, these might be common terminal state values:
    /// <list type="table">
    ///   <listheader>
    ///     <term>Terminal State</term>
    ///     <description>Value</description>
    ///   </listheader>
    ///   <item>
    ///     <term>winning/term>
    ///     <description>1</description>
    ///   <item>
    ///     <term>losing</term>
    ///     <description>-1</description>
    ///   <item>
    ///     <term>draw/term>
    ///     <description>0</description>
    ///   </item>
    /// </list>
    /// </exmaple>
    /// </summary>
    /// <param name="state">A terminal state</param>
    /// <returns>A <c>double</c> representing the supplied terminal state's value</returns>
    double GetTerminalStateValue(TState state);

    /// <summary>
    /// We must start somewhere. <c>GetInitialState</c> should return the starting state
    /// </summary>
    /// <returns>A <c>TState</c> representing the initial starting state</returns>
    TState GetInitialState();
}