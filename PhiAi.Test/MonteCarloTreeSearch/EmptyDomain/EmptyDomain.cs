using PhiAi.Core;

namespace PhiAi.Test;

public class EmptyState : IState
{
    public bool IsTerminal { get; set; } = true;
    public string Agent { get; set; } = "";
    public bool Equals(IState state) =>  Agent == state.Agent && IsTerminal == state.IsTerminal;
}

public class EmptyAction : IAction
{
    public bool Equals(IAction action) => true;
}

public class EmptyDomain : IDomain<EmptyState, EmptyAction>
{
    public IEnumerable<EmptyAction> GetActionsFromState(EmptyState state) => new List<EmptyAction> {};
    public EmptyState GetStateFromStateAndAction(EmptyState state, EmptyAction action) => new EmptyState();
    public bool IsStateTerminal(EmptyState state) => true;
    public double GetTerminalStateValue(EmptyState state) => 0;
    public EmptyState GetInitialState() => new EmptyState();
}