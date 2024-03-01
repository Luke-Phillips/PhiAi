namespace PhiAi.Core;

/// <summary>
/// Interface <c>IAction</c> represents a transition between two states.
/// </summary>
public interface IAction
{
    /// <summary>
    /// <c>Equals</c> Compares another <c>IAction</c> with itself and should return true if and only if they represent the same action. 
    /// </summary>
    /// <param name="action">The action to compare against</param>
    /// <returns>A boolean value that represents whether or not two actions represent the same action</returns>
    bool Equals(IAction action);
}