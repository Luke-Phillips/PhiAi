namespace PhiAi.Core;

public interface IState
{
    /// <summary>
    /// A terminal state is one in which no further actions (and by extension, no further state) can take place.
    /// <c>IsTerminal</c> should be set to <c>true</c> in the implementation of <c>IDomain.GetStateFromStateAndAction</c>
    /// if the given state is a terminal state.
    /// It should be kept <c>false</c> otherwise.
    /// </summary>
    bool IsTerminal { get; set; }
    
     /// <summary>
     /// The agent (actor, player, user, ai, etc) that can take an action from this state
     /// </summary>
    string Agent { get; set; }
    
    /// <summary>
    /// <c>Equals</c> Compares another <c>IState</c> with itself and should return true if and only if they represent the same state. 
    /// </summary>
    /// <param name="state">The state to compare against</param>
    /// <returns>A boolean value that represents whether or not two states represent the same state</returns>
    bool Equals(IState state);
}