using PhiAi.Core;

public class TestState : IState
{
    public bool IsTerminal { get; set; } = true;
    public string State { get; set; } = "";
    public bool Equals(IState state)
    {
        return State == ((TestState) state).State;
    }
}

public class TestAction : IAction
{
    public string Action { get; set; } = "";
    public bool Equals(IAction action)
    {
        return Action == ((TestAction) action).Action;
    }
}

public class TestDomain : IDomain<TestState, TestAction>
{
    public IEnumerable<TestAction> GetActionsFromState(TestState state)
    {
        return new List<TestAction> {};
    }
    public TestState GetStateFromStateAndAction(TestState state, TestAction action)
    {
        return new TestState();
    }
    public bool IsStateTerminal(TestState state)
    {
        return true;
    }
    public double GetTerminalStateValue(TestState state)
    {
        return 0;
    }
    public TestState GetInitialState()
    {
        return new TestState();
    }
}