namespace PhiAi.Core;

public class NoEntryActionException : Exception
{
    public NoEntryActionException() : base("NoEntryActionException: State node has no references to action from previous state")
    {

    }
    public NoEntryActionException(string message) : base($"NoEntryActionException: State node has no reference to the action taken from previous state: {message}")
    {

    }
}