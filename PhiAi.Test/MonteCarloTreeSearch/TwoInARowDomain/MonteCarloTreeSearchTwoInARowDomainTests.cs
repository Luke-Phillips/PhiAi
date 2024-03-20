using PhiAi.MonteCarloTreeSearch;
using PhiAi.Internal.MonteCarloTreeSearch;
using Examples;

namespace PhiAi.Test;

public class MonteCarloTreeSearchAlgorithmTwoInARowDomainTests
{
    private readonly MonteCarloTreeSearchAlgorithm<TwoInARowDomain, TwoInARowState, TwoInARowAction> _twoInARowMcts_default;
    private readonly MonteCarloTreeSearchAlgorithm<TwoInARowDomain, TwoInARowState, TwoInARowAction> _twoInARowMcts_random;
    
    public MonteCarloTreeSearchAlgorithmTwoInARowDomainTests()
    {
        _twoInARowMcts_default = new MonteCarloTreeSearchAlgorithm<TwoInARowDomain, TwoInARowState, TwoInARowAction>(RolloutPolicyType.Default);
        _twoInARowMcts_random = new MonteCarloTreeSearchAlgorithm<TwoInARowDomain, TwoInARowState, TwoInARowAction>();
    }

    // Unit Test Naming Scheme
    // <domain>_<primaryMethod>_<expectedResult>
    [Fact]
    public void TwoInARowDomain_CurrentState_EmptyGrid()
    {   
        TwoInARowState expectedState = new TwoInARowState();
        TwoInARowState actualState = _twoInARowMcts_default.CurrentState;
        Assert.True(expectedState.Equals(actualState));
    }

    [Fact]
    public void TwoInARowDomain_SelfPlay_1_Iterations_ReachTerminalState ()
    {   
        while (true)
        {
            _twoInARowMcts_random.SearchForIterations(1);
            var actions = _twoInARowMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _twoInARowMcts_random.TakeAction(actions.First());
        }
        var twoInARowDomain = new TwoInARowDomain();
        Assert.True(twoInARowDomain.IsStateTerminal(_twoInARowMcts_random.CurrentState));
    }

    [Fact]
    public void TwoInARowDomain_SelfPlay_10_Iterations_ReachTerminalState ()
    {   
        IEnumerable<TwoInARowAction> actions;
        while (true)
        {
            _twoInARowMcts_random.SearchForIterations(10);
            actions = _twoInARowMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _twoInARowMcts_random.TakeAction(actions.First());
        }
        var twoInARowDomain = new TwoInARowDomain();
        Assert.True(twoInARowDomain.IsStateTerminal(_twoInARowMcts_random.CurrentState));
    }

    [Fact]
    public void TwoInARowDomain_SelfPlay_100_Iterations_ReachTerminalState ()
    {   
        IEnumerable<TwoInARowAction> actions;
        while (true)
        {
            _twoInARowMcts_random.SearchForIterations(100);
            actions = _twoInARowMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _twoInARowMcts_random.TakeAction(actions.First());
        }
        var twoInARowDomain = new TwoInARowDomain();
        Assert.True(twoInARowDomain.IsStateTerminal(_twoInARowMcts_random.CurrentState));
    }

    [Fact]
    public void TwoInARowDomain_SelfPlay_1000_Iterations_ReachTerminalState ()
    {   
        IEnumerable<TwoInARowAction> actions;
        while (true)
        {
            _twoInARowMcts_random.SearchForIterations(1000);   
            actions = _twoInARowMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _twoInARowMcts_random.TakeAction(actions.First());
        }
        var twoInARowDomain = new TwoInARowDomain();
        Assert.True(twoInARowDomain.IsStateTerminal(_twoInARowMcts_random.CurrentState));
    }

    [Fact]
    public void TwoInARowDomain_SelfPlay_10000_Iterations_ReachTerminalState ()
    {   
        IEnumerable<TwoInARowAction> actions;
        while (true)
        {
            _twoInARowMcts_random.SearchForIterations(10000);
            actions = _twoInARowMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _twoInARowMcts_random.TakeAction(actions.First());
        }
        var twoInARowDomain = new TwoInARowDomain();
        Assert.True(twoInARowDomain.IsStateTerminal(_twoInARowMcts_random.CurrentState));
    }

    [Fact]
    public void TwoInARowDomain_SelfPlay_100000_Iterations_ReachTerminalState ()
    {   
        IEnumerable<TwoInARowAction> actions;
        while (true)
        {
            _twoInARowMcts_random.SearchForIterations(100000);
            actions = _twoInARowMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _twoInARowMcts_random.TakeAction(actions.First());
        }
        var twoInARowDomain = new TwoInARowDomain();
        Assert.True(twoInARowDomain.IsStateTerminal(_twoInARowMcts_random.CurrentState));
    }
}