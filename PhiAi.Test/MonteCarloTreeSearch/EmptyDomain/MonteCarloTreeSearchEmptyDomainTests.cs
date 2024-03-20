using PhiAi.MonteCarloTreeSearch;

namespace PhiAi.Test;

public class MonteCarloTreeSearchAlgorithmEmptyDomainTests
{
    private readonly MonteCarloTreeSearchAlgorithm<EmptyDomain, EmptyState, EmptyAction> _emptyMcts_default;
    
    public MonteCarloTreeSearchAlgorithmEmptyDomainTests()
    {
        _emptyMcts_default = new MonteCarloTreeSearchAlgorithm<EmptyDomain, EmptyState, EmptyAction>();
    }

    // Unit Test Naming Scheme
    // <domain>_<primaryMethod>_<expectedResult>
    [Fact]
    public void EmptyDomain_GetActions_ActionCount0()
    {
        int expectedActionCount = 0;
        int actualActionCount = _emptyMcts_default.GetActions().Count();
        Assert.Equal(expectedActionCount, actualActionCount);
    }

    [Fact]
    public void EmptyDomain_TakeActionFromTerminalState_ThrowsException()
    {
        EmptyState expectedState = new EmptyState();
        _emptyMcts_default.TakeAction(new EmptyAction());
        EmptyState actualState = _emptyMcts_default.CurrentState;
        Assert.Equal(expectedState, actualState);
    }
}